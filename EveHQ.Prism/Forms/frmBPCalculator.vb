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
Imports EveHQ.Core
Imports EveHQ.EveData
Imports EveHQ.Prism.BPCalc
Imports EveHQ.Prism.Classes
Imports EveHQ.Prism.Controls
Imports DevComponents.DotNetBar

Namespace Forms

    Public Class FrmBPCalculator

#Region "Class Variables"
        Dim _bpPilot As EveHQPilot
        Dim _updateBPInfo As Boolean = False
        Dim _startUp As Boolean = False
        Dim _inventionStartUp As Boolean = True
        Dim _currentBP As New OwnedBlueprint
        Dim _currentInventionBP As New OwnedBlueprint
        Dim _ownedBP As BPAssetComboboxItem
        Dim _currentJob As New Job
        Dim _productionArray As EveData.AssemblyArray
        Dim _copyTimeMod As Double = 1.0

        ReadOnly _startMode As BPCalcStartMode = BPCalcStartMode.None
        Dim _initialJob As Job = Nothing

        ' Invention Specific Variables
        Dim _inventionBpid As Integer = 0
        Dim _inventionBaseChance As Double = 20
        Dim _inventionSkill1 As Integer = 0
        Dim _inventionSkill2 As Integer = 0
        Dim _inventionSkill3 As Integer = 0
        Dim _inventionMetaLevel As Integer = 0
        Dim _inventionMetaItemID As Integer = 0
        Dim _inventionDecryptorID As Integer = 0
        Dim _inventionDecryptorName As String = ""
        Dim _inventionChance As Double = 20
        Dim _inventedBP As New OwnedBlueprint
        Dim _inventionAttempts As Double = 0
        Dim _inventionSuccessCost As Double = 0
        Dim _resetInventedBP As Boolean = False

#End Region

#Region "Old Property Variables"
        ReadOnly _bpName As String = ""
        Dim _bpOwnerName As String = ""
        ReadOnly _usingOwnedBPs As Boolean = False
        ReadOnly _ownedBpid As String = ""
#End Region

#Region "Internal Properties"
        Dim _productionChanged As Boolean = False
        Private Property ProductionChanged() As Boolean
            Get
                Return _productionChanged
            End Get
            Set(ByVal value As Boolean)
                If _startUp = False Then
                    _productionChanged = value
                    If _initialJob IsNot Nothing Then
                        btnSaveProductionJob.Enabled = value
                    End If
                End If
            End Set
        End Property
#End Region

#Region "Constructors"

        Public Sub New()

            ' This call is required by the designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            ' This is for a default blank BPCalc
            _usingOwnedBPs = False
            _startMode = BPCalcStartMode.None
            _bpOwnerName = PrismSettings.UserSettings.DefaultBPOwner

        End Sub

        Public Sub New(ByVal usingOwnedBPs As Boolean)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' This is for a default blank BPCalc
            _startUp = True
            _usingOwnedBPs = usingOwnedBPs
            _startMode = BPCalcStartMode.None
            _bpOwnerName = PrismSettings.UserSettings.DefaultBPOwner

        End Sub

        Public Sub New(ByVal bpName As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' This is for a non-owned BP
            _bpName = bpName
            _usingOwnedBPs = False
            _bpOwnerName = PrismSettings.UserSettings.DefaultBPOwner
            _startMode = BPCalcStartMode.StandardBP

        End Sub

        Public Sub New(ByVal bpOwner As String, ByVal bpAssetID As Long)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' This is for a non-owned BP
            _bpOwnerName = bpOwner
            _ownedBpid = CStr(bpAssetID)
            _usingOwnedBPs = True
            _startMode = BPCalcStartMode.OwnerBP

        End Sub

        Public Sub New(ByVal existingJob As Job, forInvention As Boolean)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _startUp = True
            _initialJob = existingJob
            _currentJob = existingJob.Clone
            _currentBP = _currentJob.CurrentBlueprint
            If forInvention = False Then
                _startMode = BPCalcStartMode.ProductionJob
            Else
                _startMode = BPCalcStartMode.InventionJob
            End If
            _bpOwnerName = existingJob.BlueprintOwner
            If _currentBP IsNot Nothing Then
                _ownedBpid = CStr(_currentBP.AssetId)
            End If
            Text = "BPCalc - Production Job: " & _currentJob.JobName

        End Sub

#End Region

#Region "Form Loading Routines"

        Private Sub frmBPCalculator_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing

            ' Check for changed production values
            If ProductionChanged = True And PrismSettings.UserSettings.HideSaveJobDialog = False Then

                TaskDialog.AntiAlias = True
                TaskDialog.EnableGlass = False
                Dim tdi As New TaskDialogInfo
                tdi.TaskDialogIcon = eTaskDialogIcon.Information
                tdi.DialogButtons = eTaskDialogButton.Yes Or eTaskDialogButton.No Or eTaskDialogButton.Cancel
                tdi.DefaultButton = eTaskDialogButton.Yes
                tdi.Title = "Save Job Changes?"
                tdi.Header = "Save Job Changes?"
                tdi.Text = "There are unsaved changes to this Production Job. Would you like to save these now?"
                tdi.DialogColor = eTaskDialogBackgroundColor.DarkBlue
                tdi.CheckBoxCommand = SaveJobDialogCheckBox
                Dim reply As eTaskDialogResult = TaskDialog.Show(Me, tdi)

                If reply = eTaskDialogResult.Cancel Then
                    e.Cancel = True
                Else
                    If reply = eTaskDialogResult.Yes Then
                        ' Save the current job before exiting
                        Call SaveCurrentProductionJob()
                    End If

                    ' Kill the event handlers from the PrismProductionResources controls
                    RemoveHandler PPRInvention.ProductionResourcesChanged, AddressOf InventionResourcesChanged
                    RemoveHandler PPRProduction.ProductionResourcesChanged, AddressOf ProductionResourcesChanged

                    ' Remove handlers for the price modification controls
                    RemoveHandler PACUnitValue.PriceUpdated, AddressOf CalculateInvention
                    RemoveHandler PACDecryptor.PriceUpdated, AddressOf CalculateInvention
                    RemoveHandler PACMetaItem.PriceUpdated, AddressOf CalculateInvention
                    RemoveHandler PACSalesPrice.PriceUpdated, AddressOf CalculateInvention

                    RemoveHandler PACUnitValue.PriceUpdated, AddressOf UpdateBlueprintInformation
                    RemoveHandler PACSalesPrice.PriceUpdated, AddressOf UpdateBlueprintInformation

                End If
            End If
        End Sub

        Private Sub SaveJobDialogCheckBox_Executed(ByVal sender As Object, ByVal e As EventArgs) Handles SaveJobDialogCheckBox.Executed
            PrismSettings.UserSettings.HideSaveJobDialog = SaveJobDialogCheckBox.Checked
        End Sub

        Private Sub frmBPCalculator_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            '_startUp = True

            ' Set up the event handlers from the PrismProductionResources controls
            AddHandler PPRInvention.ProductionResourcesChanged, AddressOf InventionResourcesChanged
            AddHandler PPRProduction.ProductionResourcesChanged, AddressOf ProductionResourcesChanged

            ' Set up handlers for the price modification controls
            AddHandler PACUnitValue.PriceUpdated, AddressOf CalculateInvention
            AddHandler PACDecryptor.PriceUpdated, AddressOf CalculateInvention
            AddHandler PACMetaItem.PriceUpdated, AddressOf CalculateInvention
            AddHandler PACSalesPrice.PriceUpdated, AddressOf CalculateInvention

            AddHandler PACUnitValue.PriceUpdated, AddressOf UpdateBlueprintInformation
            AddHandler PACSalesPrice.PriceUpdated, AddressOf UpdateBlueprintInformation

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
            For Each cPilot As EveHQPilot In HQ.Settings.Pilots.Values
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

            If cboPilot.Items.Count = 0 Then
                MessageBox.Show("There are no active pilots available, create or activate a pilot in EveHQ Settings before using Blueprint Calculator.", "No Pilots Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Close()
                Return
            End If

            If _bpOwnerName IsNot Nothing Then
                If cboOwner.Items.Contains(_bpOwnerName) = True Then
                    cboOwner.SelectedItem = _bpOwnerName
                End If
            End If

            If CType(PPRProduction.cboAssetSelection.DropDownControl, PrismSelectionControl).lvwItems.Items.ContainsKey(PrismSettings.UserSettings.DefaultBPCalcAssetOwner) = True Then
                CType(PPRProduction.cboAssetSelection.DropDownControl, PrismSelectionControl).lvwItems.Items(PrismSettings.UserSettings.DefaultBPCalcAssetOwner).Checked = True
            End If
            If CType(PPRInvention.cboAssetSelection.DropDownControl, PrismSelectionControl).lvwItems.Items.ContainsKey(PrismSettings.UserSettings.DefaultBPCalcAssetOwner) = True Then
                CType(PPRInvention.cboAssetSelection.DropDownControl, PrismSelectionControl).lvwItems.Items(PrismSettings.UserSettings.DefaultBPCalcAssetOwner).Checked = True
            End If

            ' Select data depending on our startup routine
            Select Case _startMode

                Case BPCalcStartMode.None

                    ' Set the Prism pilot as selected owner
                    If cboPilot.Items.Contains(PrismSettings.UserSettings.DefaultBPCalcManufacturer) = True Then
                        cboPilot.SelectedItem = PrismSettings.UserSettings.DefaultBPCalcManufacturer
                    Else
                        cboPilot.SelectedIndex = 0
                    End If

                    ' Update the list of Blueprints
                    If _usingOwnedBPs = True Then
                        chkOwnedBPOs.Checked = True
                    Else
                        Call DisplayAllBlueprints()
                    End If

                    ' Check if we have anything in the BPName Property
                    If _bpName <> "" Then
                        If cboBPs.Items.Contains(_bpName) Then
                            cboBPs.SelectedItem = _bpName
                        End If
                    End If

                    If _usingOwnedBPs = True Then
                        ' Check if we have anything in the OwnedBP Property
                        If _ownedBP IsNot Nothing Then
                            ' Set the details of the main Blueprint
                            cboBPs.SelectedItem = _ownedBP
                        End If
                    End If


                Case BPCalcStartMode.StandardBP

                    ' Set the Prism pilot as selected owner
                    If cboPilot.Items.Contains(PrismSettings.UserSettings.DefaultBPCalcManufacturer) = True Then
                        cboPilot.SelectedItem = PrismSettings.UserSettings.DefaultBPCalcManufacturer
                    Else
                        cboPilot.SelectedIndex = 0
                    End If

                    ' Update the list of Blueprints
                    Call DisplayAllBlueprints()

                    ' Check if we have anything in the BPName Property
                    If _bpName <> "" Then
                        If cboBPs.Items.Contains(_bpName) Then
                            cboBPs.SelectedItem = _bpName
                        End If
                    End If

                Case BPCalcStartMode.OwnerBP

                    ' Set the Prism pilot as selected owner
                    If cboPilot.Items.Contains(PrismSettings.UserSettings.DefaultBPCalcManufacturer) = True Then
                        cboPilot.SelectedItem = PrismSettings.UserSettings.DefaultBPCalcManufacturer
                    Else
                        cboPilot.SelectedIndex = 0
                    End If

                    ' Update the list of Blueprints
                    chkOwnedBPOs.Checked = True

                    If _usingOwnedBPs = True Then
                        ' Check if we have anything in the OwnedBP Property
                        If _ownedBP IsNot Nothing Then
                            ' Set the details of the main Blueprint
                            cboBPs.SelectedItem = _ownedBP
                        End If
                    End If

                Case BPCalcStartMode.ProductionJob

                    Call DisplayProductionJobDetails()
                    tabBPCalcFunctions.SelectedTab = tiProduction

                Case BPCalcStartMode.InventionJob

                    Call DisplayProductionJobDetails()
                    tabBPCalcFunctions.SelectedTab = tiInvention
            End Select

            ' Reset the changed flag - nothing has really changed as we've just finished loading the form!
            _productionChanged = False

        End Sub

        Private Sub frmBPCalculator_Shown(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Shown
            _startUp = False
        End Sub

        Private Sub DisplayProductionJobDetails()
            ' Set Manufacturer
            If cboPilot.Items.Contains(_currentJob.Manufacturer) Then
                cboPilot.SelectedItem = _currentJob.Manufacturer
            Else
                If cboPilot.Items.Contains(PrismSettings.UserSettings.DefaultBPCalcManufacturer) = True Then
                    cboPilot.SelectedItem = PrismSettings.UserSettings.DefaultBPCalcManufacturer
                Else
                    cboPilot.SelectedIndex = 0
                End If
            End If

            ' Set BP values
            If _currentJob.OverridingME <> "" Then
                nudMELevel.Value = CInt(_currentJob.OverridingME)
            End If
            If _currentJob.OverridingPE <> "" Then
                nudPELevel.Value = CInt(_currentJob.OverridingPE)
            End If

            If _currentBP IsNot Nothing Then
                If _currentBP.AssetId <= Integer.MaxValue AndAlso StaticData.Blueprints.ContainsKey(CInt(_currentBP.AssetId)) Then
                    ' This is a standard BP, not an owned one
                    Call DisplayAllBlueprints()
                    cboBPs.SelectedItem = StaticData.Types(CInt(_currentBP.AssetId)).Name
                Else
                    ' This is an owned BP
                    chkOwnedBPOs.Checked = True
                    cboBPs.SelectedItem = _ownedBP
                End If
            End If

            nudRuns.Value = _currentJob.Runs

            PPRProduction.ProductionJob = _currentJob
        End Sub

        Private Sub DisplayInventionDetails()

            If _currentJob.InventionJob Is Nothing Then
                _currentJob.InventionJob = New BPCalc.InventionJob
            End If

            ' Set InventionBP
            cboInventions.SelectedItem = StaticData.Types(_currentJob.InventionJob.InventedBpid).Name

            ' Set Decryptor
            If _currentJob.InventionJob.DecryptorUsed IsNot Nothing Then
                cboDecryptor.SelectedItem = _currentJob.InventionJob.DecryptorUsed.Name & " (" & _currentJob.InventionJob.DecryptorUsed.ProbMod.ToString & "x, " & _currentJob.InventionJob.DecryptorUsed.MEMod.ToString & "ME, " & _currentJob.InventionJob.DecryptorUsed.PEMod.ToString & "PE, " & _currentJob.InventionJob.DecryptorUsed.RunMod.ToString & "r)"
            End If

            ' Set MetaItem
            If _currentJob.InventionJob.MetaItemId <> 0 Then
                cboMetaItem.Items.Add(StaticData.Types(_currentJob.InventionJob.MetaItemId).MetaLevel & ": " & StaticData.Types(_currentJob.InventionJob.MetaItemId).Name)
            End If

            ' Set Runs
            nudInventionBPCRuns.LockUpdateChecked = _currentJob.InventionJob.OverrideBpcRuns
            'If currentJob.InventionJob.OverrideBPCRuns = True Then
            '    nudInventionBPCRuns.Value = currentJob.InventionJob.BPCRuns
            'End If

            ' Set Skills
            nudInventionSkill1.LockUpdateChecked = _currentJob.InventionJob.OverrideEncSkill
            If _currentJob.InventionJob.OverrideEncSkill = True Then
                nudInventionSkill1.Value = _currentJob.InventionJob.EncryptionSkill
            End If
            nudInventionSkill2.LockUpdateChecked = _currentJob.InventionJob.OverrideDcSkill1
            If _currentJob.InventionJob.OverrideDcSkill1 = True Then
                nudInventionSkill2.Value = _currentJob.InventionJob.DatacoreSkill1
            End If
            nudInventionSkill3.LockUpdateChecked = _currentJob.InventionJob.OverrideDcSkill2
            If _currentJob.InventionJob.OverrideDcSkill2 = True Then
                nudInventionSkill3.Value = _currentJob.InventionJob.DatacoreSkill2
            End If

            ' Set InventionFlag
            chkInventionFlag.Checked = _currentJob.HasInventionJob

            Call CalculateInvention()


        End Sub

        Private Sub DisplayAllBlueprints()
            ' Load the Blueprints into the combo box
            cboBPs.BeginUpdate()
            cboBPs.Items.Clear()
            cboBPs.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            cboBPs.AutoCompleteSource = AutoCompleteSource.ListItems
            For Each newBP As EveData.Blueprint In StaticData.Blueprints.Values
                Dim bpName As String = StaticData.Types(newBP.Id).Name
                If chkInventBPOs.Checked = True Then
                    If btnToggleInvention.Value = True Then
                        ' Use T1 data
                        If newBP.Inventions.Count > 0 Then
                            cboBPs.Items.Add(bpName)
                        End If
                    Else
                        ' Use T2 data
                        If newBP.InventFrom.Count > 0 Then
                            cboBPs.Items.Add(bpName)
                        End If
                    End If
                Else
                    cboBPs.Items.Add(bpName)
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
            Dim ownerBPs As New SortedList(Of Long, BlueprintAsset)
            If PlugInData.BlueprintAssets.ContainsKey(_bpOwnerName) = True Then
                ownerBPs = PlugInData.BlueprintAssets(_bpOwnerName)
            End If
            For Each bp As BlueprintAsset In ownerBPs.Values
                If bp.Runs <> 0 Then
                    If StaticData.Blueprints.ContainsKey(CInt(bp.TypeID)) Then
                        Dim bpacbi As New BPAssetComboboxItem(StaticData.Types(CInt(bp.TypeID)).Name, bp.AssetID, bp.MELevel, bp.PELevel, bp.Runs)

                        'Basic filter if inventable item filtering is on
                        If chkInventBPOs.Checked = True Then
                            If btnToggleInvention.Value = True Then
                                ' Use T1 data
                                If StaticData.Blueprints(CInt(bp.TypeID)).Inventions.Count > 0 Then
                                    cboBPs.Items.Add(bpacbi)
                                End If
                            Else
                                ' Use T2 data
                                If StaticData.Blueprints(CInt(bp.TypeID)).InventFrom.Count > 0 Then
                                    cboBPs.Items.Add(bpacbi)
                                End If
                            End If
                        Else
                            cboBPs.Items.Add(bpacbi)
                        End If

                        ' Check if this matches the ownedBPID
                        If bpacbi.AssetID = _ownedBpid Then
                            _ownedBP = bpacbi
                        End If
                    End If
                End If
            Next
            cboBPs.Sorted = True
            cboBPs.EndUpdate()
        End Sub

#End Region

#Region "Pilot & Owner Selection Routines"

        Private Sub cboOwner_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboOwner.SelectedIndexChanged
            If cboOwner.SelectedItem IsNot Nothing Then
                _bpOwnerName = cboOwner.SelectedItem.ToString
                Call DisplayOwnedBlueprints()
            End If
        End Sub

        Private Sub cboPilot_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPilot.SelectedIndexChanged
            ' Set the pilot
            If HQ.Settings.Pilots.ContainsKey(cboPilot.SelectedItem.ToString) Then
                _bpPilot = HQ.Settings.Pilots(cboPilot.SelectedItem.ToString)
                ' Update skills and stuff
                _currentJob.UpdateManufacturer(_bpPilot.Name)
                Call UpdatePilotSkills()
            End If
            If _inventionStartUp = False Then
                Call UpdateInventionUi()
            End If
        End Sub

        Private Sub chkOverrideSkills_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkOverrideSkills.CheckedChanged

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
                Call UpdatePilotSkills()
                ' Set change flag
                ProductionChanged = True
            Else
                ' Don't do anything here at present as we shall just use the default values for the last selected pilot
            End If

        End Sub

        Private Sub UpdatePilotSkills()
            ' Delay updating the BP Info until we have completed the update to the pilot
            _updateBPInfo = False
            ' Update Research Skill
            If _bpPilot.PilotSkills.ContainsKey("Research") = True Then
                cboResearchSkill.SelectedIndex = _bpPilot.PilotSkills("Research").Level
            Else
                cboResearchSkill.SelectedIndex = 0
            End If
            ' Update Metallurgy Skill
            If _bpPilot.PilotSkills.ContainsKey("Metallurgy") = True Then
                cboMetallurgySkill.SelectedIndex = _bpPilot.PilotSkills("Metallurgy").Level
            Else
                cboMetallurgySkill.SelectedIndex = 0
            End If
            ' Update Science Skill
            If _bpPilot.PilotSkills.ContainsKey("Science") = True Then
                cboScienceSkill.SelectedIndex = _bpPilot.PilotSkills("Science").Level
            Else
                cboScienceSkill.SelectedIndex = 0
            End If
            ' Update Industry Skill
            If _bpPilot.PilotSkills.ContainsKey("Industry") = True Then
                cboIndustrySkill.SelectedIndex = _bpPilot.PilotSkills("Industry").Level
            Else
                cboIndustrySkill.SelectedIndex = 0
            End If
            ' Update PE Skill
            If _bpPilot.PilotSkills.ContainsKey("Material Efficiency") = True Then
                cboProdEffSkill.SelectedIndex = _bpPilot.PilotSkills("Material Efficiency").Level
            Else
                cboProdEffSkill.SelectedIndex = 0
            End If
            ' Allow updating again
            _updateBPInfo = True
            If _startUp = False Then
                ' Set skills for job etc
                Dim prodImplant As Integer = CInt(cboIndustryImplant.SelectedItem.ToString.TrimEnd(CChar("%")))
                ' Update the job skills
                _currentJob.UpdateJobSkills(cboProdEffSkill.SelectedIndex, cboIndustrySkill.SelectedIndex, prodImplant)
                ' Update the Blueprint information
                PPRProduction.ProductionJob = _currentJob
                ' Set change flag
                ProductionChanged = True
            End If
        End Sub


#End Region

#Region "Blueprint Selection & Calculation Routines"

        Private Sub cboBPs_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboBPs.SelectedIndexChanged
            If cboBPs.SelectedItem IsNot Nothing Then
                If _startUp = False Then
                    ' Enable the various parts
                    gpPilotSkills.Enabled = True
                    _updateBPInfo = False
                    If TypeOf (cboBPs.SelectedItem) Is BPAssetComboboxItem Then
                        ' This is an owner blueprint!
                        Dim selBP As BPAssetComboboxItem = CType(cboBPs.SelectedItem, BPAssetComboboxItem)
                        Dim bpID As Integer = StaticData.TypeNames(selBP.Name)
                        _currentBP = OwnedBlueprint.CopyFromBlueprint(StaticData.Blueprints(CInt(bpID)))
                        _currentBP.MELevel = selBP.MELevel
                        _currentBP.PELevel = selBP.PELevel
                        _currentBP.Runs = selBP.Runs
                        _currentBP.AssetId = CLng(selBP.AssetID)
                        ' Update the research boxes
                        nudMELevel.MinValue = _currentBP.MELevel : nudMELevel.Value = _currentBP.MELevel
                        nudPELevel.MinValue = _currentBP.PELevel : nudPELevel.Value = _currentBP.PELevel
                    Else
                        ' This is a standard blueprint
                        Dim bpID As Integer = StaticData.TypeNames(cboBPs.SelectedItem.ToString.Trim)
                        _currentBP = OwnedBlueprint.CopyFromBlueprint(StaticData.Blueprints(CInt(bpID)))
                        _currentBP.MELevel = 0
                        _currentBP.PELevel = 0
                        _currentBP.Runs = -1
                        _currentBP.AssetId = CLng(bpID)
                        ' Update the research boxes
                        nudMELevel.MinValue = -10 : nudMELevel.Value = _currentBP.MELevel
                        nudPELevel.MinValue = -10 : nudPELevel.Value = _currentBP.PELevel
                    End If
                    ' Set change flag
                    ProductionChanged = True
                End If
                ' Reset the invention flags for a new item
                _currentJob.InventionJob = Nothing
                chkInventionFlag.Checked = False
                _inventionBpid = 0
                _resetInventedBP = True
                ' Check if all the invention data is present
                Call OwnedBlueprint.CheckForInventionItems(_currentBP)
                ' Disable the invention tab if we have no inventable items
                If _currentBP.InventFrom.Count > 0 Then
                    ' Populate invention data
                    Call UpdateInventionUi()
                Else
                    If _currentBP.Inventions.Count > 0 Then
                        ' Populate invention data
                        Call UpdateInventionUi()
                    Else
                        tiInvention.Visible = False
                    End If
                End If
                ' Update the form title
                If _startMode <> BPCalcStartMode.ProductionJob Then
                    Text = "BPCalc - " & cboBPs.SelectedItem.ToString
                End If
                ' First get the image
                pbBP.ImageLocation = ImageHandler.GetImageLocation(_currentBP.Id)
                ' Update the standard BP Info
                lblBPME.Text = _currentBP.MELevel.ToString
                lblBPPE.Text = _currentBP.PELevel.ToString
                lblBPRuns.Text = _currentBP.Runs.ToString
                lblBPMaxRuns.Text = _currentBP.MaxProductionLimit.ToString("N0")
                lblBPWF.Text = _currentBP.WasteFactor.ToString("N2") & "%"
                ' Update the prices
                lblBPOMarketValue.Text = (CDbl(StaticData.Types(_currentBP.Id).BasePrice) * 0.9).ToString("N2") & " Isk"
                ' Update the limits on the Runs
                nudCopyRuns.MaxValue = _currentBP.MaxProductionLimit
                If _currentBP.Runs = -1 Then
                    nudRuns.MaxValue = 1000000
                Else
                    nudRuns.MaxValue = Math.Min(_currentBP.MaxProductionLimit, _currentBP.Runs)
                End If
                ToolTip1.SetToolTip(nudCopyRuns, "Limited to " & _currentBP.MaxProductionLimit.ToString & " runs by the Blueprint data")
                ToolTip1.SetToolTip(lblRunsPerCopy, "Limited to " & _currentBP.MaxProductionLimit.ToString & " runs by the Blueprint data")
                _updateBPInfo = True
                ' Calculate what arrays we can use to manufacture this
                Call CalculateAssemblyLocations()
                ' Calculate the remaining blueprint information
                Call SetBlueprintInformation(_startUp)
            End If
        End Sub

        Private Sub UpdateInventionUi()

            _inventionStartUp = True

            ' Set the InventionBP based on selection
            If _currentBP.InventFrom.Count > 0 Then
                _currentInventionBP = OwnedBlueprint.CopyFromBlueprint(StaticData.Blueprints(_currentBP.InventFrom(0)))
            Else
                _currentInventionBP = OwnedBlueprint.CopyFromBlueprint(_currentBP)
            End If

            ' Update the available inventions
            cboInventions.BeginUpdate()
            cboInventions.Items.Clear()
            For Each bpid As Integer In _currentInventionBP.Inventions
                cboInventions.Items.Add(StaticData.Types(bpid).Name)
            Next
            cboInventions.Sorted = True
            cboInventions.EndUpdate()

            Dim inventionSkills As New LinkedList(Of DictionaryEntry)

            ' Update the decryptors and get skills by looking at the resources and determining the type of interface used
            Dim decryptorGroupID As String = ""
            If _currentInventionBP.Resources.ContainsKey(BlueprintActivity.Invention) = False Then
                Dim msg As New System.Text.StringBuilder
                msg.AppendLine("There are invention resources missing from the " & StaticData.Types(_currentInventionBP.Id).Name & ".")
                msg.AppendLine()
                msg.AppendLine("This could be caused by a corrupt database. Please report this to the developers so it can be investigated.")
                MessageBox.Show(msg.ToString, "Missing Invention Resources", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                tiInvention.Visible = False
                Exit Sub
            Else
                tiInvention.Visible = True
                For Each resource As EveData.BlueprintResource In _currentInventionBP.Resources(BlueprintActivity.Invention).Values
                    ' Add the resource to the list
                    Dim resName As String = StaticData.Types(resource.TypeId).Name
                    If resName.EndsWith("Interface", StringComparison.Ordinal) = True Then
                        Select Case resName.Substring(0, 1)
                            Case "O"
                                ' Amarr
                                decryptorGroupID = "728"
                                Dim skillLevel As Integer = 0
                                If _bpPilot.PilotSkills.ContainsKey("Amarr Encryption Methods") = True Then
                                    skillLevel = _bpPilot.PilotSkills("Amarr Encryption Methods").Level
                                End If
                                inventionSkills.AddFirst(New DictionaryEntry("Amarr Encryption Methods", skillLevel))
                            Case "C"
                                ' Minmatar
                                decryptorGroupID = "729"
                                Dim skillLevel As Integer = 0
                                If _bpPilot.PilotSkills.ContainsKey("Minmatar Encryption Methods") = True Then
                                    skillLevel = _bpPilot.PilotSkills("Minmatar Encryption Methods").Level
                                End If
                                inventionSkills.AddFirst(New DictionaryEntry("Minmatar Encryption Methods", skillLevel))
                            Case "I"
                                ' Gallente
                                decryptorGroupID = "730"
                                Dim skillLevel As Integer = 0
                                If _bpPilot.PilotSkills.ContainsKey("Gallente Encryption Methods") = True Then
                                    skillLevel = _bpPilot.PilotSkills("Gallente Encryption Methods").Level
                                End If
                                inventionSkills.AddFirst(New DictionaryEntry("Gallente Encryption Methods", skillLevel))
                            Case "E"
                                ' Caldari
                                decryptorGroupID = "731"
                                Dim skillLevel As Integer = 0
                                If _bpPilot.PilotSkills.ContainsKey("Caldari Encryption Methods") = True Then
                                    skillLevel = _bpPilot.PilotSkills("Caldari Encryption Methods").Level
                                End If
                                inventionSkills.AddFirst(New DictionaryEntry("Caldari Encryption Methods", skillLevel))
                        End Select
                        ' Terminate early once we know
                    ElseIf resName.StartsWith("Datacore", StringComparison.Ordinal) = True Then
                        Dim skillName As String = resName.TrimStart("Datacore - ".ToCharArray)
                        Dim skillLevel As Integer = 0
                        If _bpPilot.PilotSkills.ContainsKey(skillName) = True Then
                            skillLevel = _bpPilot.PilotSkills(skillName).Level
                        End If
                        inventionSkills.AddLast(New DictionaryEntry(skillName, skillLevel))
                    End If
                Next
            End If
            ' Update the invention resources with this BP
            PPRInvention.InventionBP = _currentInventionBP

            ' Load the decryptors
            cboDecryptor.BeginUpdate()
            cboDecryptor.Items.Clear()
            cboDecryptor.Items.Add("<None>")
            For Each decrypt As BPCalc.Decryptor In PlugInData.Decryptors.Values
                If decrypt.GroupID = decryptorGroupID Then
                    cboDecryptor.Items.Add(decrypt.Name & " (" & decrypt.ProbMod.ToString & "x, " & decrypt.MEMod.ToString & "ME, " & decrypt.PEMod.ToString & "PE, " & decrypt.RunMod.ToString & "r)")
                End If
            Next
            cboDecryptor.SelectedIndex = 0
            cboDecryptor.EndUpdate()

            ' Display the skills - hopefully should be 3 :)
            If inventionSkills.Count = 3 Then
                lblInvSkill1.Text = "Skill 1: " & CStr(inventionSkills(0).Key)
                lblInvSkill1.Tag = CStr(inventionSkills(0).Key)
                nudInventionSkill1.Value = CInt(inventionSkills(0).Value)
                _inventionSkill1 = CInt(inventionSkills(0).Value)
                lblInvSkill2.Text = "Skill 2: " & CStr(inventionSkills(1).Key)
                lblInvSkill2.Tag = CStr(inventionSkills(1).Key)
                nudInventionSkill2.Value = CInt(inventionSkills(1).Value)
                _inventionSkill2 = CInt(inventionSkills(1).Value)
                lblInvSkill3.Text = "Skill 3: " & CStr(inventionSkills(2).Key)
                lblInvSkill3.Tag = CStr(inventionSkills(2).Key)
                nudInventionSkill3.Value = CInt(inventionSkills(2).Value)
                _inventionSkill3 = CInt(inventionSkills(2).Value)
            Else
                MessageBox.Show("Ooops! Seems to be more invention skills here than what we can use in the calculation!", "Invention Skills Issue", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

            ' Load the meta items
            cboMetaItem.BeginUpdate()
            cboMetaItem.Items.Clear()
            cboMetaItem.Items.Add("<None>")
            For Each metaItem As Integer In _currentInventionBP.InventionMetaItems
                cboMetaItem.Items.Add(StaticData.Types(metaItem).MetaLevel & ": " & StaticData.Types(metaItem).Name)
            Next
            cboMetaItem.SelectedIndex = 0
            cboMetaItem.EndUpdate()

            ' Work out Base Chance
            Select Case StaticData.Types(_currentInventionBP.ProductId).Group
                Case 27, 419
                    _inventionBaseChance = 20
                Case 26, 28
                    _inventionBaseChance = 25
                Case 25, 420, 513
                    _inventionBaseChance = 30
                Case Else
                    Select Case _currentInventionBP.Id
                        Case 17477
                            _inventionBaseChance = 20
                        Case 17479
                            _inventionBaseChance = 25
                        Case 17481
                            _inventionBaseChance = 30
                        Case Else
                            _inventionBaseChance = 40
                    End Select
            End Select
            lblBaseChance.Text = "Base Invention Chance: " & _inventionBaseChance.ToString & "%"

            ' Update the BPC Override Values
            nudInventionBPCRuns.MaxValue = _currentInventionBP.MaxProductionLimit
            nudInventionBPCRuns.Value = _currentInventionBP.MaxProductionLimit

            Call DisplayInventionDetails()

            If cboInventions.Items.Count > 0 Then
                If cboInventions.Items.Contains(StaticData.Types(_currentBP.Id).Name) Then
                    cboInventions.SelectedItem = StaticData.Types(_currentBP.Id).Name
                Else
                    cboInventions.SelectedIndex = 0
                End If
            End If

            _inventionStartUp = False
            Call CalculateInvention()

        End Sub

        Private Sub SetBlueprintInformation(ByVal recalcOnly As Boolean)
            ' Recalculate the required resources
            If recalcOnly = False Then
                Call CalculateProductionResources()
            Else
                Call _currentJob.RecalculateResourceRequirements()
                Call DisplayProductionInformation()
            End If
        End Sub

        Private Sub ProductionResourcesChanged()
            If _startUp = False Then
                ' Set change flag
                ProductionChanged = True
                Call UpdateBlueprintInformation()
            End If
        End Sub

        Private Sub UpdateBlueprintInformation()
            If StaticData.Types(_currentBP.Id).Name <> "" Then
                ' Calculate and display the waste factor
                Call CalculateWasteFactor()
                ' Display the research times
                Call CalculateBlueprintTimes()
                ' Display production info
                Call DisplayProductionInformation()
            End If
        End Sub

        Private Sub CalculateAssemblyLocations()
            Dim productID As Integer = _currentBP.ProductId
            Dim product As EveType = StaticData.Types(productID)
            ' Load the Assembly Array Data
            cboPOSArrays.BeginUpdate()
            cboPOSArrays.Items.Clear()
            For Each newArray As EveData.AssemblyArray In StaticData.AssemblyArrays.Values
                If newArray.AllowableCategories.Contains(product.Category) Or newArray.AllowableGroups.Contains(product.Group) Then
                    If newArray.Name.EndsWith("Array", StringComparison.Ordinal) = True Then
                        cboPOSArrays.Items.Add(newArray.Name)
                    End If
                End If
            Next
            cboPOSArrays.Sorted = True
            cboPOSArrays.EndUpdate()
            chkPOSProduction.Enabled = True
        End Sub

        Private Sub CalculateWasteFactor()
            Dim currentBpwf As Double = 0
            If _currentBP.WasteFactor <> 0 Then
                If nudMELevel.Value < 0 Then
                    ' This is for negative ME
                    currentBpwf = ((_currentBP.WasteFactor / 100) * (1 - nudMELevel.Value)) + (0.25 - (0.05 * cboProdEffSkill.SelectedIndex))
                Else
                    ' This is for zero and positive ME
                    currentBpwf = ((_currentBP.WasteFactor / 100) / (1 + nudMELevel.Value)) + (0.25 - (0.05 * cboProdEffSkill.SelectedIndex))
                End If
            End If
            txtNewWasteFactor.Text = (currentBpwf * 100).ToString("N6") & "%"
        End Sub

        Private Sub CalculateBlueprintTimes()
            Dim meImplant As Double = 1 - (CDbl(cboMetallurgyImplant.SelectedItem.ToString.TrimEnd(CChar("%"))) / 100)
            Dim peImplant As Double = 1 - (CDbl(cboResearchImplant.SelectedItem.ToString.TrimEnd(CChar("%"))) / 100)
            Dim copyImplant As Double = 1 - (CDbl(cboScienceImplant.SelectedItem.ToString.TrimEnd(CChar("%"))) / 100)
            Dim meTime As Double = _currentBP.ResearchMaterialLevelTime * (1 - (0.05 * cboMetallurgySkill.SelectedIndex)) * meImplant
            Dim peTime As Double = _currentBP.ResearchProductionLevelTime * (1 - (0.05 * cboResearchSkill.SelectedIndex)) * peImplant
            Dim copyTime As Double = 0
            If _currentBP.MaxProductionLimit <> 0 Then
                copyTime = _currentBP.ResearchCopyTime / _currentBP.MaxProductionLimit * 2 * (1 - (0.05 * cboScienceSkill.SelectedIndex)) * copyImplant
            End If
            If chkResearchAtPOS.Checked = True Then
                meTime *= 0.75
                peTime *= 0.75
                copyTime *= _copyTimeMod
            End If
            ' Display the ME Time
            If nudMELevel.Value > 0 Then
                lblMETime.Text = SkillFunctions.TimeToString(meTime * (nudMELevel.Value - _currentBP.MELevel), False)
            Else
                lblMETime.Text = SkillFunctions.TimeToString(0, False)
            End If
            ' Display the PE Time
            If nudPELevel.Value > 0 Then
                lblPETime.Text = SkillFunctions.TimeToString(peTime * (nudPELevel.Value - _currentBP.PELevel), False)
            Else
                lblPETime.Text = SkillFunctions.TimeToString(0, False)
            End If
            ' Display the Copy Time
            lblCopyTime.Text = SkillFunctions.TimeToString(copyTime * nudCopyRuns.Value, False)
        End Sub

        Private Sub CalculateProductionResources()

            ' Check for production array
            If chkPOSProduction.Checked = True Then
                If _productionArray IsNot Nothing Then
                End If
            Else
                _productionArray = Nothing
            End If

            ' Set blueprint runs
            Dim runs As Integer = CInt(nudRuns.Value)

            ' Get resources
            Dim prodImplant As Integer = CInt(cboIndustryImplant.SelectedItem.ToString.TrimEnd(CChar("%")))
            ' Save stuff for copying
            Dim oldJobName As String = ""
            If _currentJob.JobName <> "" Then
                oldJobName = _currentJob.JobName
            End If
            Dim tij As BPCalc.InventionJob = _currentJob.InventionJob
            _currentJob = _currentBP.CreateProductionJob(_bpOwnerName, cboPilot.SelectedItem.ToString, cboProdEffSkill.SelectedIndex, cboIndustrySkill.SelectedIndex, prodImplant, nudMELevel.Value.ToString, nudPELevel.Value.ToString, runs, _productionArray, False)
            _currentJob.InventionJob = tij
            If oldJobName <> "" Then
                _currentJob.JobName = oldJobName
            End If
            PPRProduction.ProductionJob = _currentJob

        End Sub

#End Region

#Region "Production UI Routines"

        Private Sub chkOwnedBPOs_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkOwnedBPOs.CheckedChanged
            cboOwner.Enabled = chkOwnedBPOs.Checked
            If chkOwnedBPOs.Checked = True Then
                If cboOwner.SelectedItem IsNot Nothing Then
                    _bpOwnerName = cboOwner.SelectedItem.ToString
                End If
                Call DisplayOwnedBlueprints()
            Else
                _bpOwnerName = ""
                Call DisplayAllBlueprints()
            End If
        End Sub

        Private Sub chkPOSProduction_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkPOSProduction.CheckedChanged
            cboPOSArrays.Enabled = chkPOSProduction.Checked
            If _updateBPInfo = True Then
                If chkPOSProduction.Checked = True Then
                    If cboPOSArrays.SelectedItem IsNot Nothing Then
                        _productionArray = StaticData.AssemblyArrays(cboPOSArrays.SelectedItem.ToString)
                    Else
                        _productionArray = Nothing
                    End If
                Else
                    _productionArray = Nothing
                End If
                _currentJob.AssemblyArray = _productionArray
                PPRProduction.ProductionJob = _currentJob
                ' Set change flag
                ProductionChanged = True
            End If
        End Sub

        Private Sub cboPOSArrays_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPOSArrays.SelectedIndexChanged
            If _updateBPInfo = True Then
                _productionArray = StaticData.AssemblyArrays(cboPOSArrays.SelectedItem.ToString)
                _currentJob.AssemblyArray = _productionArray
                PPRProduction.ProductionJob = _currentJob
                ' Set change flag
                ProductionChanged = True
            End If
        End Sub

        Private Sub nudMELevel_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudMELevel.ValueChanged
            If _startUp = False And _updateBPInfo = True Then
                _currentJob.OverridingME = CStr(nudMELevel.Value)
                PPRProduction.ProductionJob = _currentJob
                ' Set change flag
                ProductionChanged = True
            End If
        End Sub

        Private Sub nudPELevel_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudPELevel.ValueChanged
            If _startUp = False And _updateBPInfo = True Then
                _currentJob.OverridingPE = CStr(nudPELevel.Value)
                PPRProduction.ProductionJob = _currentJob
                ' Set change flag
                ProductionChanged = True
            End If
        End Sub

        Private Sub nudCopyRuns_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudCopyRuns.ValueChanged
            If _startUp = False And _updateBPInfo = True Then
                ' Update the Blueprint information
                Call UpdateBlueprintInformation()
                ' Set change flag
                ProductionChanged = True
            End If
        End Sub

        Private Sub chkResearchAtPOS_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkResearchAtPOS.CheckedChanged
            chkAdvancedLab.Enabled = chkResearchAtPOS.Checked
            If chkResearchAtPOS.Checked = True Then
                If chkAdvancedLab.Checked = True Then
                    _copyTimeMod = 0.65
                Else
                    _copyTimeMod = 0.75
                End If
            Else
                _copyTimeMod = 1.0
            End If
            If _updateBPInfo = True Then
                ' Update the Blueprint information
                Call UpdateBlueprintInformation()
            End If
        End Sub

        Private Sub nudRuns_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudRuns.ValueChanged
            If _startUp = False And _updateBPInfo = True Then
                _currentJob.Runs = CInt(nudRuns.Value)
                PPRProduction.ProductionJob = _currentJob
                ' Set change flag
                ProductionChanged = True
            End If
        End Sub

        Private Sub cboResearchSkill_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboResearchSkill.SelectedIndexChanged
            If _startUp = False And _updateBPInfo = True Then
                ' Update the Blueprint information
                Call UpdateBlueprintInformation()
            End If
        End Sub

        Private Sub cboMetallurgySkill_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboMetallurgySkill.SelectedIndexChanged
            If _startUp = False And _updateBPInfo = True Then
                ' Update the Blueprint information
                Call UpdateBlueprintInformation()
            End If
        End Sub

        Private Sub cboScienceSkill_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboScienceSkill.SelectedIndexChanged
            If _startUp = False And _updateBPInfo = True Then
                ' Update the Blueprint information
                Call UpdateBlueprintInformation()
            End If
        End Sub

        Private Sub cboIndustrySkill_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboIndustrySkill.SelectedIndexChanged
            If _startUp = False And _updateBPInfo = True Then
                ' Update the Blueprint information
                _currentJob.UpdateJobIndSkill(cboIndustrySkill.SelectedIndex)
                PPRProduction.ProductionJob = _currentJob
                ' Set change flag
                ProductionChanged = True
            End If
        End Sub

        Private Sub cboProdEffSkill_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboProdEffSkill.SelectedIndexChanged
            If _startUp = False And _updateBPInfo = True Then
                _currentJob.UpdateJobPESkill(cboProdEffSkill.SelectedIndex)
                PPRProduction.ProductionJob = _currentJob
                ' Set change flag
                ProductionChanged = True
            End If
        End Sub

        Private Sub cboResearchImplant_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboResearchImplant.SelectedIndexChanged
            If _startUp = False And _updateBPInfo = True Then
                ' Update the Blueprint information
                Call UpdateBlueprintInformation()
            End If
        End Sub

        Private Sub cboMetallurgyImplant_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboMetallurgyImplant.SelectedIndexChanged
            If _startUp = False And _updateBPInfo = True Then
                ' Update the Blueprint information
                Call UpdateBlueprintInformation()
            End If
        End Sub

        Private Sub cboScienceImplant_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboScienceImplant.SelectedIndexChanged
            If _startUp = False And _updateBPInfo = True Then
                ' Update the Blueprint information
                Call UpdateBlueprintInformation()
            End If
        End Sub

        Private Sub cboIndustryImplant_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboIndustryImplant.SelectedIndexChanged
            If _startUp = False And _updateBPInfo = True Then
                ' Update the Blueprint information
                Dim prodImplant As Integer = CInt(cboIndustryImplant.SelectedItem.ToString.TrimEnd(CChar("%")))
                _currentJob.UpdateJobProdImplant(prodImplant)
                PPRProduction.ProductionJob = _currentJob
                ' Set change flag
                ProductionChanged = True
            End If
        End Sub

        Private Sub chkAdvancedLab_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkAdvancedLab.CheckedChanged
            If chkAdvancedLab.Checked = True Then
                _copyTimeMod = 0.65
            Else
                _copyTimeMod = 0.75
            End If
            If _updateBPInfo = True Then
                ' Update the Blueprint information
                Call UpdateBlueprintInformation()
            End If
        End Sub

        Private Sub DisplayProductionInformation()
            ' Calculate the batch size
            Dim productID As Integer = _currentBP.ProductId
            Dim product As EveType = StaticData.Types(productID)
            lblBatchSize.Text = product.PortionSize.ToString("N0")
            lblProdQuantity.Text = (product.PortionSize * _currentJob.Runs).ToString("N0")
            ' Calculate the factory costs
            Dim factoryCosts As Double = Math.Round((PrismSettings.UserSettings.FactoryRunningCost / 3600 * _currentJob.RunTime) + PrismSettings.UserSettings.FactoryInstallCost, 2, MidpointRounding.AwayFromZero)
            ' Display Build Time Information
            lblUnitBuildTime.Text = SkillFunctions.TimeToString(_currentJob.RunTime / _currentJob.Runs, False)
            lblTotalBuildTime.Text = SkillFunctions.TimeToString(_currentJob.RunTime, False)
            ' Display Materials costs
            lblUnitBuildCost.Text = (_currentJob.Cost / _currentJob.Runs).ToString("N2") & " isk"
            lblTotalBuildCost.Text = _currentJob.Cost.ToString("N2") & " isk"
            ' Display Factory costs
            lblFactoryCosts.Text = factoryCosts.ToString("N2") & " isk"
            Dim totalCosts As Double = _currentJob.Cost + factoryCosts
            Dim unitcosts As Double = Math.Round(totalCosts / (_currentJob.Runs * product.PortionSize), 2, MidpointRounding.AwayFromZero)
            Dim value As Double = DataFunctions.GetPrice(productID)
            Dim profit As Double = value - unitcosts
            PACUnitValue.TypeID = productID
            lblTotalCosts.Text = totalCosts.ToString("N2") & " isk"
            lblUnitCost.Text = unitcosts.ToString("N2") & " isk"
            lblUnitValue.Text = value.ToString("N2") & " isk"
            lblUnitProfit.Text = profit.ToString("N2") & " isk"
            lblProfitRate.Text = (profit * product.PortionSize / ((_currentJob.RunTime / _currentJob.Runs) / 3600)).ToString("N2") & " isk"
            lblProfitMargin.Text = CDbl(profit / value * 100).ToString("N2") & " %"
            lblProfitMarkup.Text = CDbl(profit / unitcosts * 100).ToString("N2") & " %"
            If profit > 0 Then
                lblUnitProfit.ForeColor = Color.Green
                lblProfitRate.ForeColor = Color.Green
            Else
                If profit < 0 Then
                    lblUnitProfit.ForeColor = Color.Red
                    lblProfitRate.ForeColor = Color.Red
                Else
                    lblUnitProfit.ForeColor = Color.Black
                    lblProfitRate.ForeColor = Color.Black
                End If
            End If
        End Sub

        Private Sub btnSaveProductionJob_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveProductionJob.Click
            Call SaveCurrentProductionJob()
        End Sub

        Private Sub SaveCurrentProductionJob()
            If _initialJob IsNot Nothing And _currentJob IsNot Nothing Then
                Jobs.JobList(_currentJob.JobName) = _currentJob.Clone
                ProductionChanged = False
                _initialJob = _currentJob.Clone
                PrismEvents.StartUpdateProductionJobs()
            Else
                Using newJobName As New FrmAddProductionJob
                    newJobName.ShowDialog()
                    If newJobName.DialogResult = DialogResult.OK Then
                        _currentJob.JobName = newJobName.JobName
                        Jobs.JobList.Add(newJobName.JobName, _currentJob.Clone)
                        ProductionChanged = False
                        _initialJob = _currentJob.Clone
                        Text = "BPCalc - Production Job: " & _currentJob.JobName
                    End If
                End Using
                PrismEvents.StartUpdateProductionJobs()
            End If
        End Sub

        Private Sub btnSaveProductionJobAs_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveProductionJobAs.Click
            Using newJobName As New FrmAddProductionJob
                newJobName.ShowDialog()
                If newJobName.DialogResult = DialogResult.OK Then
                    _currentJob.JobName = newJobName.JobName
                    Jobs.JobList.Add(newJobName.JobName, _currentJob.Clone)
                    ProductionChanged = False
                    _initialJob = _currentJob.Clone
                    Text = "BPCalc - Production Job: " & _currentJob.JobName
                End If
            End Using
            PrismEvents.StartUpdateProductionJobs()
        End Sub

#End Region

#Region "Invention UI Routines"

        Private Sub chkInventBPOs_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkInventBPOs.CheckedChanged
            cboOwner.Enabled = chkOwnedBPOs.Checked
            btnToggleInvention.Enabled = chkInventBPOs.Checked
            If chkOwnedBPOs.Checked = True Then
                Call DisplayOwnedBlueprints()
            Else
                Call DisplayAllBlueprints()
            End If
        End Sub

        Private Sub btnToggleInvention_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles btnToggleInvention.ValueChanged
            If chkOwnedBPOs.Checked = True Then
                Call DisplayOwnedBlueprints()
            Else
                Call DisplayAllBlueprints()
            End If
        End Sub

        Private Sub cboInventions_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboInventions.SelectedIndexChanged
            _inventionBpid = CInt(StaticData.TypeNames(cboInventions.SelectedItem.ToString))
            If _inventionStartUp = False Then
                Call CalculateInvention()
                ProductionChanged = True
            End If
        End Sub

        Private Sub cboDecryptor_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboDecryptor.SelectedIndexChanged

            If cboDecryptor.SelectedItem IsNot Nothing Then
                Dim didx As Integer = cboDecryptor.SelectedItem.ToString.IndexOf("(", StringComparison.Ordinal)
                If didx > 0 Then
                    Dim decryptorName As String = cboDecryptor.SelectedItem.ToString.Substring(0, didx - 1).Trim
                    If PlugInData.Decryptors.ContainsKey(decryptorName) Then
                        _inventionDecryptorName = decryptorName
                        _inventionDecryptorID = CInt(PlugInData.Decryptors(decryptorName).ID)
                    Else
                        _inventionDecryptorName = ""
                        _inventionDecryptorID = 0
                    End If
                Else
                    _inventionDecryptorName = ""
                    _inventionDecryptorID = 0
                End If
            End If
            PACDecryptor.TypeID = _inventionDecryptorID
            If _inventionStartUp = False Then
                Call CalculateInvention()
                ProductionChanged = True
            End If
        End Sub

        Private Sub cboMetaItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboMetaItem.SelectedIndexChanged
            If cboMetaItem.SelectedItem IsNot Nothing Then
                If cboMetaItem.SelectedItem.ToString <> "<None>" Then
                    _inventionMetaLevel = CInt(cboMetaItem.SelectedItem.ToString.Substring(0, 1))
                    _inventionMetaItemID = CInt(StaticData.TypeNames(cboMetaItem.SelectedItem.ToString.Remove(0, 3)))
                Else
                    _inventionMetaLevel = 0
                    _inventionMetaItemID = 0
                End If
            End If
            PACMetaItem.TypeID = _inventionMetaItemID
            If _inventionStartUp = False Then
                Call CalculateInvention()
                ProductionChanged = True
            End If
        End Sub

        Private Sub nudInventionBPCRuns_LockUpdateChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudInventionBPCRuns.LockUpdateChanged
            If _inventionStartUp = False Then
                Call CalculateInvention()
                ProductionChanged = True
            End If
        End Sub

        Private Sub nudInventionBPCRuns_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudInventionBPCRuns.ValueChanged
            If _inventionStartUp = False Then
                Call CalculateInvention()
                ProductionChanged = True
            End If
        End Sub

        Private Sub nudInventionBPCRuns_ButtonCustomClick(ByVal sender As Object, ByVal e As EventArgs) Handles nudInventionBPCRuns.ButtonCustomClick
            ' Set max runs
            nudInventionBPCRuns.Value = _currentInventionBP.MaxProductionLimit
        End Sub

        Private Sub nudInventionBPCRuns_ButtonCustom2Click(ByVal sender As Object, ByVal e As EventArgs) Handles nudInventionBPCRuns.ButtonCustom2Click
            ' Set single run
            nudInventionBPCRuns.Value = 1
        End Sub

        Private Sub lblFactoryCostsLbl_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles lblFactoryCostsLbl.LinkClicked
            Using newSettingsForm As New FrmPrismSettings
                newSettingsForm.Tag = "nodeCosts"
                newSettingsForm.ShowDialog()
                Call UpdateBlueprintInformation()
                Call CalculateInvention()
            End Using
        End Sub

        Private Sub lblInventionLabCostsLbl_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles lblInventionLabCostsLbl.LinkClicked
            Using newSettingsForm As New FrmPrismSettings
                newSettingsForm.Tag = "nodeCosts"
                newSettingsForm.ShowDialog()
                Call UpdateBlueprintInformation()
                Call CalculateInvention()
            End Using
        End Sub

        Private Sub lblInventionBPCCostLbl_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles lblInventionBPCCostLbl.LinkClicked
            Using newSettingsForm As New FrmPrismSettings
                newSettingsForm.Tag = "nodeCosts"
                newSettingsForm.ShowDialog()
                Call UpdateBlueprintInformation()
                Call CalculateInvention()
            End Using
        End Sub

        Private Sub lblInventionBPCCost_LinkClicked(ByVal sender As Object, ByVal e As LinkLabelLinkClickedEventArgs) Handles lblInventionBPCCost.LinkClicked
            Using priceForm As New FrmAddBPCPrice(_currentBP.Id)
                priceForm.ShowDialog()
                Call UpdateBlueprintInformation()
                Call CalculateInvention()
            End Using
        End Sub

        Private Sub nudInventionSkill1_ButtonCustomClick(ByVal sender As Object, ByVal e As EventArgs) Handles nudInventionSkill1.ButtonCustomClick
            If _bpPilot.PilotSkills.ContainsKey(lblInvSkill1.Tag.ToString) = True Then
                nudInventionSkill1.Value = _bpPilot.PilotSkills(lblInvSkill1.Tag.ToString).Level
            Else
                nudInventionSkill1.Value = 0
            End If
            _currentJob.InventionJob.EncryptionSkill = nudInventionSkill1.Value
            If _inventionStartUp = False Then
                ProductionChanged = True
            End If
        End Sub

        Private Sub nudInventionSkill1_LockUpdateChanged(sender As Object, e As EventArgs) Handles nudInventionSkill1.LockUpdateChanged
            _currentJob.InventionJob.OverrideEncSkill = nudInventionSkill1.LockUpdateChecked
            If _inventionStartUp = False Then
                ProductionChanged = True
            End If
        End Sub

        Private Sub nudInventionSkill1_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudInventionSkill1.ValueChanged
            _inventionSkill1 = nudInventionSkill1.Value
            If _inventionStartUp = False Then
                Call CalculateInvention()
                ProductionChanged = True
            End If
        End Sub

        Private Sub nudInventionSkill2_ButtonCustomClick(ByVal sender As Object, ByVal e As EventArgs) Handles nudInventionSkill2.ButtonCustomClick
            If _bpPilot.PilotSkills.ContainsKey(lblInvSkill2.Tag.ToString) = True Then
                nudInventionSkill2.Value = _bpPilot.PilotSkills(lblInvSkill2.Tag.ToString).Level
            Else
                nudInventionSkill2.Value = 0
            End If
            _currentJob.InventionJob.DatacoreSkill1 = nudInventionSkill2.Value
            If _inventionStartUp = False Then
                ProductionChanged = True
            End If
        End Sub

        Private Sub nudInventionSkill2_LockUpdateChanged(sender As Object, e As EventArgs) Handles nudInventionSkill2.LockUpdateChanged
            _currentJob.InventionJob.OverrideDcSkill1 = nudInventionSkill2.LockUpdateChecked
            If _inventionStartUp = False Then
                ProductionChanged = True
            End If
        End Sub

        Private Sub nudInventionSkill2_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudInventionSkill2.ValueChanged
            _inventionSkill2 = nudInventionSkill2.Value
            If _inventionStartUp = False Then
                Call CalculateInvention()
                ProductionChanged = True
            End If
        End Sub

        Private Sub nudInventionSkill3_ButtonCustomClick(ByVal sender As Object, ByVal e As EventArgs) Handles nudInventionSkill3.ButtonCustomClick
            If _bpPilot.PilotSkills.ContainsKey(lblInvSkill3.Tag.ToString) = True Then
                nudInventionSkill3.Value = _bpPilot.PilotSkills(lblInvSkill3.Tag.ToString).Level
            Else
                nudInventionSkill3.Value = 0
            End If
            _currentJob.InventionJob.DatacoreSkill2 = nudInventionSkill3.Value
            If _inventionStartUp = False Then
                ProductionChanged = True
            End If
        End Sub

        Private Sub nudInventionSkill3_LockUpdateChanged(sender As Object, e As EventArgs) Handles nudInventionSkill3.LockUpdateChanged
            _currentJob.InventionJob.OverrideDcSkill2 = nudInventionSkill3.LockUpdateChecked
            If _inventionStartUp = False Then
                ProductionChanged = True
            End If
        End Sub

        Private Sub nudInventionSkill3_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudInventionSkill3.ValueChanged
            If _inventionStartUp = False Then
                _inventionSkill3 = nudInventionSkill3.Value
                Call CalculateInvention()
                ProductionChanged = True
            End If
        End Sub

        Private Sub CalculateInvention()

            If _inventionBpid <> 0 Then

                ' Set the Invention Job Data
                Call SetInventionJobData()

                ' Calculate Invention Chance
                _inventionChance = _currentJob.InventionJob.CalculateInventionChance

                ' Calculate Cost
                Dim invCost As InventionCost = _currentJob.InventionJob.CalculateInventionCost

                lblInventionChance.Text = "Total Invention Chance: " & _inventionChance.ToString("N2") & "%"
                lblInventionBaseCost.Text = invCost.DatacoreCost.ToString("N2") & " Isk"
                lblInventionDecryptorCost.Text = invCost.DecryptorCost.ToString("N2") & " Isk"
                lblInventionMetaItemCost.Text = invCost.MetaItemCost.ToString("N2") & " Isk"
                lblInventionLabCosts.Text = invCost.LabCost.ToString("N2") & " Isk"
                lblInventionBPCCost.Text = invCost.BPCCost.ToString("N2") & " Isk"
                lblInventionCost.Text = invCost.TotalCost.ToString("N2") & " Isk"

                _inventionAttempts = Math.Max(Math.Round(100 / _inventionChance, 4, MidpointRounding.AwayFromZero), 1)
                _inventionSuccessCost = _inventionAttempts * invCost.TotalCost

                lblInventedBP.Text = "ME:" & _inventedBP.MELevel.ToString & "  PE:" & _inventedBP.PELevel.ToString & "  Runs: " & _inventedBP.Runs.ToString("N0")
                lblInventionTime.Text = SkillFunctions.TimeToString(_currentInventionBP.ResearchTechTime, False)
                lblAvgAttempts.Text = "Average Attempts Until Success: " & _inventionAttempts.ToString("N4")
                lblSuccessCost.Text = _inventionSuccessCost.ToString("N2") & " Isk"
                PACSalesPrice.TypeID = _inventedBP.ProductId

                ' Calculate and show Resources
                PPRInvention.ProductionJob = _currentJob.InventionJob.ProductionJob
                Call DisplayInventionProfitInfo()

            End If

        End Sub

        Private Sub SetInventionJobData()
            ' Set the relevant parts of the current job
            Dim currentInventionJob As BPCalc.InventionJob = _currentJob.InventionJob
            currentInventionJob.OverrideBpcRuns = nudInventionBPCRuns.LockUpdateChecked
            currentInventionJob.BpcRuns = nudInventionBPCRuns.Value
            If nudInventionBPCRuns.LockUpdateChecked = False Then
                ' Use current BP Runs, replacing max for unlimited
                If _currentBP.Runs = -1 Then
                    ' Use max runs
                    If currentInventionJob.InventedBpid <> 0 Then
                        currentInventionJob.BpcRuns = currentInventionJob.GetBaseBP.MaxProductionLimit
                    End If
                End If
            End If
            If PlugInData.Decryptors.ContainsKey(_inventionDecryptorName) Then
                currentInventionJob.DecryptorUsed = PlugInData.Decryptors(_inventionDecryptorName)
            Else
                currentInventionJob.DecryptorUsed = Nothing
            End If
            currentInventionJob.InventedBpid = _inventionBpid
            currentInventionJob.EncryptionSkill = _inventionSkill1
            currentInventionJob.DatacoreSkill1 = _inventionSkill2
            currentInventionJob.DatacoreSkill2 = _inventionSkill3
            currentInventionJob.MetaItemId = _inventionMetaItemID
            currentInventionJob.MetaItemLevel = _inventionMetaLevel
            currentInventionJob.OverrideBpcRuns = nudInventionBPCRuns.LockUpdateChecked
            currentInventionJob.BaseChance = _inventionBaseChance
            currentInventionJob.OverrideEncSkill = nudInventionSkill1.LockUpdateChecked
            currentInventionJob.OverrideDcSkill1 = nudInventionSkill2.LockUpdateChecked
            currentInventionJob.OverrideDcSkill2 = nudInventionSkill3.LockUpdateChecked
            currentInventionJob.EncryptionSkill = nudInventionSkill1.Value
            currentInventionJob.DatacoreSkill1 = nudInventionSkill2.Value
            currentInventionJob.DatacoreSkill2 = nudInventionSkill3.Value
            _inventedBP = _currentJob.InventionJob.CalculateInventedBPC
            If currentInventionJob.ProductionJob Is Nothing Then
                currentInventionJob.ProductionJob = _inventedBP.CreateProductionJob(_bpOwnerName, cboPilot.SelectedItem.ToString, cboProdEffSkill.SelectedIndex, cboIndustrySkill.SelectedIndex, CInt(cboIndustryImplant.SelectedItem.ToString.TrimEnd(CChar("%"))), "", "", 1, _productionArray, False)
            Else
                If _resetInventedBP = True Then
                    currentInventionJob.ProductionJob = _inventedBP.CreateProductionJob(_bpOwnerName, cboPilot.SelectedItem.ToString, cboProdEffSkill.SelectedIndex, cboIndustrySkill.SelectedIndex, CInt(cboIndustryImplant.SelectedItem.ToString.TrimEnd(CChar("%"))), "", "", 1, _productionArray, False)
                    _resetInventedBP = False
                Else
                    _inventedBP.UpdateProductionJob(currentInventionJob.ProductionJob)
                End If
            End If
        End Sub

        Private Sub DisplayInventionProfitInfo()
            ' Show Production Cost

            Dim batchQty As Integer = StaticData.Types(_inventedBP.ProductId).PortionSize
            Dim factoryCost As Double = Math.Round((PrismSettings.UserSettings.FactoryRunningCost / 3600 * _currentJob.InventionJob.ProductionJob.RunTime) + PrismSettings.UserSettings.FactoryInstallCost, 2, MidpointRounding.AwayFromZero)
            Dim avgCost As Double = (Math.Round(_inventionSuccessCost / _inventedBP.Runs, 2, MidpointRounding.AwayFromZero) + PPRInvention.ProductionJob.Cost + factoryCost) / batchQty
            Dim salesPrice As Double = DataFunctions.GetPrice(_inventedBP.ProductId)
            Dim unitProfit As Double = salesPrice - avgCost
            Dim totalProfit As Double = unitProfit * _inventedBP.Runs * batchQty

            lblBatchProductionCost.Text = PPRInvention.ProductionJob.Cost.ToString("N2") & " Isk"
            lblBatchTotalCost.Text = (avgCost * batchQty).ToString("N2") & " Isk"
            lblAvgInventionCost.Text = avgCost.ToString("N2") & " Isk"
            lblInventionSalesPrice.Text = salesPrice.ToString("N2") & " Isk"
            lblUnitInventionProfit.Text = unitProfit.ToString("N2") & " Isk"
            lblTotalInventionProfit.Text = totalProfit.ToString("N2") & " Isk"

            If unitProfit >= 0 Then
                lblUnitInventionProfitLbl.Text = "Profit per Unit:"
                lblUnitInventionProfit.ForeColor = Color.Green
                lblTotalInventionProfitLbl.Text = "Total Profit:"
                lblTotalInventionProfit.ForeColor = Color.Green
            Else
                lblUnitInventionProfitLbl.Text = "Loss per Unit:"
                lblUnitInventionProfit.ForeColor = Color.Red
                lblTotalInventionProfitLbl.Text = "Total Loss:"
                lblTotalInventionProfit.ForeColor = Color.Red
            End If

            Call DisplayProfitTable()

        End Sub

        Private Sub DisplayProfitTable()

            Dim decryptorID As Integer
            Dim decryptorMod As Double
            Dim cj As Job = _currentJob.Clone
            Dim pj As Job = _currentJob.InventionJob.ProductionJob.Clone

            adtInventionProfits.BeginUpdate()
            adtInventionProfits.Nodes.Clear()

            For Each decryptor As String In cboDecryptor.Items
                Dim didx As Integer = decryptor.ToString.IndexOf("(", StringComparison.Ordinal)
                If didx > 0 Then
                    Dim decryptorName As String = decryptor.ToString.Substring(0, didx - 1).Trim
                    If PlugInData.Decryptors.ContainsKey(decryptorName) Then
                        decryptorMod = PlugInData.Decryptors(decryptorName).ProbMod
                        decryptorID = CInt(PlugInData.Decryptors(decryptorName).ID)
                        cj.InventionJob.DecryptorUsed = PlugInData.Decryptors(decryptorName)
                    Else
                        decryptorMod = 1
                        decryptorID = 0
                        cj.InventionJob.DecryptorUsed = Nothing
                    End If
                Else
                    decryptorMod = 1
                    decryptorID = 0
                    cj.InventionJob.DecryptorUsed = Nothing
                End If

                Dim bpcRuns As Integer
                If nudInventionBPCRuns.LockUpdateChecked = False Then
                    ' Use current BP Runs, replacing max for unlimited
                    Select Case StaticData.Types(_currentBP.ProductId).Category
                        Case 6
                            bpcRuns = 1
                        Case Else
                            ' Use max runs
                            bpcRuns = _currentInventionBP.MaxProductionLimit
                    End Select
                Else
                    bpcRuns = nudInventionBPCRuns.Value
                End If

                Dim ic As Double = Invention.CalculateInventionChance(_inventionBaseChance, _inventionSkill1, _inventionSkill2, _inventionSkill3, _inventionMetaLevel, decryptorMod)

                Dim cost As Double = _currentInventionBP.CalculateInventionCost(_inventionMetaItemID, decryptorID, bpcRuns)
                Dim ibp As OwnedBlueprint = cj.InventionJob.CalculateInventedBPC
                Dim ia As Double = Math.Max(Math.Round(100 / ic, 4, MidpointRounding.AwayFromZero), 1)
                Dim isc As Double = ia * cost
                ibp.UpdateProductionJob(pj)
                Dim batchQty As Integer = StaticData.Types(_inventedBP.ProductId).PortionSize
                Dim factoryCost As Double = Math.Round((PrismSettings.UserSettings.FactoryRunningCost / 3600 * pj.RunTime) + PrismSettings.UserSettings.FactoryInstallCost, 2, MidpointRounding.AwayFromZero)
                Dim avgCost As Double = (Math.Round(isc / ibp.Runs, 2, MidpointRounding.AwayFromZero) + pj.Cost + factoryCost) / batchQty
                Dim salesPrice As Double = DataFunctions.GetPrice(ibp.ProductId)
                Dim unitProfit As Double = salesPrice - avgCost
                Dim totalProfit As Double = (unitProfit * ibp.Runs) * batchQty

                Dim newLine As New Node
                If decryptorID = 0 Then
                    newLine.Text = "None (" & ibp.Runs.ToString & " run" & IIf(ibp.Runs = 1, ")", "s)").ToString
                Else
                    newLine.Text = decryptorMod.ToString("N1") & "x (" & ibp.Runs.ToString & " run" & IIf(ibp.Runs = 1, ")", "s)").ToString
                End If
                newLine.Cells.Add(New Cell(unitProfit.ToString("N2") & "<br />" & totalProfit.ToString("N2")))
                adtInventionProfits.Nodes.Add(newLine)

            Next
            adtInventionProfits.EndUpdate()
        End Sub

        Private Sub chkInventionFlag_CheckedChanged(sender As Object, e As EventArgs) Handles chkInventionFlag.CheckedChanged
            _currentJob.HasInventionJob = chkInventionFlag.Checked
            If _inventionStartUp = False Then
                ProductionChanged = True
            End If
        End Sub

        Private Sub InventionResourcesChanged()
            If _startUp = False Then
                ' Set change flag
                ProductionChanged = True
                _currentJob.InventionJob.ProductionJob = PPRInvention.ProductionJob
                Call DisplayInventionProfitInfo()
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
End Namespace