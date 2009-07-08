Imports System.Windows.Forms
Imports System.Xml
Imports DotNetLib.Windows.Forms

Public Class frmBPCalculator

#Region "Class Variables"
    Dim BPPilot As EveHQ.Core.Pilot
    Dim UpdateBPInfo As Boolean = False
    Dim StartUp As Boolean = True
    Dim CurrentBP As New BlueprintSelection
    Dim OwnedBP As BPAssetComboboxItem
    Dim currentJob As New ProductionJob
    Dim ownedResources As New SortedList(Of String, Long)
    Dim groupResources As New SortedList(Of String, Long)
    Dim CurrentBPWF As Double
    Dim ProductionRuns As Integer = 1
    Dim ProductionArray As AssemblyArray
    Dim BatchSize As Integer
    Dim UnitBuildTime As Double
    Dim FactoryCosts As Double
    Dim UnitMaterial As Double
    Dim UnitWaste As Double
#End Region

#Region "Public Properties & Property Variables"
    Dim cBPName As String = ""
    Dim cBPOwnerName As String = ""
    Dim cUsingOwnedBPs As Boolean = False
    Dim cOwnedBPID As String = ""


    Public Property BPName() As String
        Get
            Return cBPName
        End Get
        Set(ByVal value As String)
            cBPName = value
        End Set
    End Property

    Public Property BPOwnerName() As String
        Get
            Return cBPOwnerName
        End Get
        Set(ByVal value As String)
            cBPOwnerName = value
        End Set
    End Property

    Public Property UsingOwnedBPs() As Boolean
        Get
            Return cUsingOwnedBPs
        End Get
        Set(ByVal value As Boolean)
            cUsingOwnedBPs = value
        End Set
    End Property

    Public Property OwnedBPID() As String
        Get
            Return cOwnedBPID
        End Get
        Set(ByVal value As String)
            cOwnedBPID = value
        End Set
    End Property

#End Region

#Region "Form Loading Routines"
    Private Sub frmBPCalculator_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Set the implants
        cboResearchImplant.SelectedIndex = 0
        cboMetallurgyImplant.SelectedIndex = 0
        cboScienceImplant.SelectedIndex = 0
        cboIndustyImplant.SelectedIndex = 0

        'Load the characters into the combobox
        cboPilot.BeginUpdate()
        cboPilot.Items.Clear()
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If cPilot.Active = True Then
                cboPilot.Items.Add(cPilot.Name)
            End If
        Next
        cboPilot.EndUpdate()

        ' Set the Prism pilot as selected owner
        If cboPilot.Items.Contains(BPOwnerName) Then
            cboPilot.SelectedItem = BPOwnerName
        Else
            cboPilot.SelectedIndex = 0
        End If

        ' Update the list of Blueprints
        If UsingOwnedBPs = True Then
            chkOwnedBPOs.Checked = True
        Else
            Call Me.DisplayAllBlueprints()
        End If

        ' Set the AssetSelection box
        cboAssetSelection.SelectedIndex = 0

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

        StartUp = False

    End Sub

    Private Sub DisplayAllBlueprints()
        ' Load the Blueprints into the combo box
        cboBPs.BeginUpdate()
        cboBPs.Items.Clear()
        cboBPs.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboBPs.AutoCompleteSource = AutoCompleteSource.ListItems
        For Each newBP As Blueprint In PlugInData.Blueprints.Values
            cboBPs.Items.Add(newBP.Name)
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
            Dim BPACBI As New BPAssetComboboxItem(PlugInData.Blueprints(BP.TypeID).Name, BP.AssetID, BP.MELevel, BP.PELevel, BP.Runs)
            cboBPs.Items.Add(BPACBI)
            ' Check if this matches the ownedBPID
            If BPACBI.AssetID = cOwnedBPID Then
                OwnedBP = BPACBI
            End If
        Next
        cboBPs.Sorted = True
        cboBPs.EndUpdate()
    End Sub

#End Region

#Region "Pilot Selection Routines"
    Private Sub cboPilot_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilot.SelectedIndexChanged
        ' Set the pilot
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(cboPilot.SelectedItem.ToString) Then
            BPPilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilot.SelectedItem.ToString), Core.Pilot)
            Call Me.UpdatePilotSkills()
        End If
        If StartUp = False And UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub

    Private Sub chkOverrideSkills_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverrideSkills.CheckedChanged

        ' Toggle the enabled status of the skill combo boxes
        cboResearchSkill.Enabled = chkOverrideSkills.Checked
        cboMetallurgySkill.Enabled = chkOverrideSkills.Checked
        cboIndustrySkill.Enabled = chkOverrideSkills.Checked
        cboProdEffSkill.Enabled = chkOverrideSkills.Checked
        cboScienceSkill.Enabled = chkOverrideSkills.Checked

        ' Determine whether to change the skills or leave the existing ones
        If chkOverrideSkills.Checked = False Then
            ' Use pilot skills
            Call Me.UpdatePilotSkills()
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
    End Sub


#End Region

#Region "Blueprint Selection & Calculation Routines"
    Private Sub cboBPs_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboBPs.SelectedIndexChanged
        ' Enable the various parts
        gbSkills.Enabled = True
        gbAddResearch.Enabled = True
        gbProduction.Enabled = True
        tabBPResources.Enabled = True
        UpdateBPInfo = False
        If TypeOf (cboBPs.SelectedItem) Is BPAssetComboboxItem Then
            ' This is an owner bluepint!
            Dim selBP As BPAssetComboboxItem = CType(cboBPs.SelectedItem, BPAssetComboboxItem)
            Dim bpID As String = CStr(EveHQ.Core.HQ.itemList(selBP.Name))
            CurrentBP = BlueprintSelection.CopyFromBlueprint(PlugInData.Blueprints(bpID))
            CurrentBP.MELevel = selBP.MELevel
            CurrentBP.PELevel = selBP.PELevel
            CurrentBP.Runs = selBP.Runs
            ' Update the research boxes
            nudMELevel.Minimum = CurrentBP.MELevel : nudMELevel.Value = CurrentBP.MELevel
            nudPELevel.Minimum = CurrentBP.PELevel : nudPELevel.Value = CurrentBP.PELevel
        Else
            ' This is a standard blueprint
            Dim bpID As String = CStr(EveHQ.Core.HQ.itemList(cboBPs.SelectedItem.ToString.Trim))
            CurrentBP = BlueprintSelection.CopyFromBlueprint(PlugInData.Blueprints(bpID))
            CurrentBP.MELevel = 0
            CurrentBP.PELevel = 0
            CurrentBP.Runs = -1
            ' Update the research boxes
            nudMELevel.Minimum = -10 : nudMELevel.Value = CurrentBP.MELevel
            nudPELevel.Minimum = -10 : nudPELevel.Value = CurrentBP.PELevel
        End If
        ' Update the form title
        Me.Text = "BPCalc - " & cboBPs.SelectedItem.ToString
        ' First get the image
        pbBP.ImageLocation = EveHQ.Core.ImageHandler.GetImageLocation(CStr(CurrentBP.ID), EveHQ.Core.ImageHandler.ImageType.Blueprints)
        ' Update the standard BP Info
        lblBPME.Text = CurrentBP.MELevel.ToString
        lblBPPE.Text = CurrentBP.PELevel.ToString
        lblBPRuns.Text = CurrentBP.Runs.ToString
        ' Update the prices
        lblBPOMarketValue.Text = FormatNumber(CDbl(EveHQ.Core.HQ.BasePriceList(CurrentBP.ID.ToString)) * 0.9, 2) & " Isk"
        ' Update the limits on the Runs
        nudCopyRuns.Maximum = CurrentBP.MaxProdLimit
        ToolTip1.SetToolTip(nudCopyRuns, "Limited to " & CurrentBP.MaxProdLimit.ToString & " runs by the Blueprint data")
        ToolTip1.SetToolTip(lblRunsPerCopy, "Limited to " & CurrentBP.MaxProdLimit.ToString & " runs by the Blueprint data")
        UpdateBPInfo = True
        ' Calculate what arrays we can use to manufacture this
        Call Me.CalculateAssemblyLocations()
        ' Calculate the remaining blueprint information
        Call Me.SetBlueprintInformation()
        ' Update Waste Factor
        lblBPWF.Text = FormatNumber(CurrentBPWF * 100, 6) & "%"
    End Sub

    Private Sub SetBlueprintInformation()
        ' Calculate and display the waste factor
        Call Me.CalculateWasteFactor()
        ' Display the research times
        Call Me.CalculateBlueprintTimes()
        ' Display production times
        Call Me.CalculateProductionDetails()
        ' Get the required resources
        Call Me.GetRequiredResources()
        ' Get the owned Resources
        Call Me.GetOwnedResources()
        ' Display the Resources
        Call Me.DisplayRequiredResources()
        ' Display costs information
        Call Me.DisplayCostInformation()
    End Sub

    Private Sub UpdateBlueprintInformation()
        ' Calculate and display the waste factor
        Call Me.CalculateWasteFactor()
        ' Display the research times
        Call Me.CalculateBlueprintTimes()
        ' Display production times
        Call Me.CalculateProductionDetails()
        ' Get the required resources
        Call Me.GetRequiredResources()
        ' Display the Resources
        Call Me.DisplayRequiredResources()
        ' Display costs information
        Call Me.DisplayCostInformation()
    End Sub

   
    Private Sub CalculateAssemblyLocations()
        Dim productID As String = CurrentBP.ProductID.ToString
        Dim product As EveHQ.Core.EveItem = CType(EveHQ.Core.HQ.itemData(productID), Core.EveItem)
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
        If nudMELevel.Value < 0 Then
            ' This is for negative ME
            CurrentBPWF = ((1 / CurrentBP.WasteFactor) * (1 - nudMELevel.Value)) + (0.25 - (0.05 * cboProdEffSkill.SelectedIndex))
        Else
            ' This is for zero and positive ME
            CurrentBPWF = ((1 / CurrentBP.WasteFactor) / (1 + nudMELevel.Value)) + (0.25 - (0.05 * cboProdEffSkill.SelectedIndex))
        End If
        txtNewWasteFactor.Text = FormatNumber(CurrentBPWF * 100, 6) & "%"
    End Sub

    Private Sub CalculateBlueprintTimes()
        Dim MEImplant As Double = 1 - (CDbl(cboMetallurgyImplant.SelectedItem.ToString.TrimEnd(CChar("%"))) / 100)
        Dim PEImplant As Double = 1 - (CDbl(cboResearchImplant.SelectedItem.ToString.TrimEnd(CChar("%"))) / 100)
        Dim CopyImplant As Double = 1 - (CDbl(cboScienceImplant.SelectedItem.ToString.TrimEnd(CChar("%"))) / 100)
        Dim ProdImplant As Double = 1 - (CDbl(cboIndustyImplant.SelectedItem.ToString.TrimEnd(CChar("%"))) / 100)
        Dim METime As Double = CurrentBP.ResearchMatTime * (1 - (0.05 * cboMetallurgySkill.SelectedIndex)) * MEImplant
        Dim PETime As Double = CurrentBP.ResearchProdTime * (1 - (0.05 * cboResearchSkill.SelectedIndex)) * PEImplant
        Dim CopyTime As Double = CurrentBP.ResearchCopyTime * 0.2 * (1 - (0.05 * cboScienceSkill.SelectedIndex)) * CopyImplant
        Dim prodTime As Double = CurrentBP.ProdTime * (1 - (0.04 * cboIndustrySkill.SelectedIndex)) * ProdImplant
        ' Calculate the production time
        If nudPELevel.Value >= 0 Then
            prodTime *= (1 - (CurrentBP.ProdMod / CurrentBP.ProdTime) * nudPELevel.Value / (1 + nudPELevel.Value))
        Else
            prodTime *= (1 - (CurrentBP.ProdMod / CurrentBP.ProdTime) * (nudPELevel.Value - 1))
        End If
        If chkPOSProduction.Checked = True Then
            If ProductionArray IsNot Nothing Then
                prodTime *= ProductionArray.TimeMultiplier
            End If
        End If
        UnitBuildTime = prodTime
        If chkResearchAtPOS.Checked = True Then
            METime *= 0.75
            PETime *= 0.75
            CopyTime *= 0.75
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

    Private Sub CalculateProductionDetails()
        ' Calculate the batch size
        Dim productID As String = CurrentBP.ProductID.ToString
        Dim product As EveHQ.Core.EveItem = CType(EveHQ.Core.HQ.itemData(productID), Core.EveItem)
        BatchSize = product.PortionSize
        txtBatchSize.Text = FormatNumber(product.PortionSize, 0)
        txtProdQuantity.Text = FormatNumber(product.PortionSize * ProductionRuns, 0)
        ' Calculate the factory costs
        FactoryCosts = Math.Round((nudRunningCost.Value / 3600 * UnitBuildTime * ProductionRuns) + nudInstallCost.Value, 2)
    End Sub

    Private Sub GetRequiredResources()

        ' Check for production array
        Dim arrayMod As Double = 1
        If chkPOSProduction.Checked = True Then
            If ProductionArray IsNot Nothing Then
                arrayMod = ProductionArray.MaterialMultiplier
            End If
        End If

        ' Set blueprint runs
        Dim runs As Integer = CInt(nudRuns.Value)

        ' Get resources
        Dim ProdImplant As Integer = CInt(cboIndustyImplant.SelectedItem.ToString.TrimEnd(CChar("%")))
        currentJob = CurrentBP.CalculateProductionJob(cBPOwnerName, cboProdEffSkill.SelectedIndex, cboIndustrySkill.SelectedIndex, ProdImplant, CurrentBPWF, runs, ProductionArray, Not (chkUseStandardBPCosting.Checked))

        ' Add the resources required from the production job
        groupResources = New SortedList(Of String, Long)
        Call Me.GetResourcesFromJob(currentJob)

    End Sub

    Private Sub GetOwnedResources()

        ' Establish a list of owners whose assets we are going to query
        Dim ownerList As New ArrayList
        Select Case cboAssetSelection.SelectedIndex
            Case 0 ' Owner Only
                ownerList.Add(cBPOwnerName)
            Case 1 ' Owner + Corp
                ownerList.Add(cBPOwnerName)
                ' See if this is a pilot
                If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(cBPOwnerName) = True Then
                    ' Insert pilot corp
                    ownerList.Add(CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cBPOwnerName), EveHQ.Core.Pilot).Corp)
                End If
            Case 2 ' Corp + Members
                Dim Corp As String = ""
                ' See if this is a pilot
                If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(cBPOwnerName) = True Then
                    ' Insert pilot corp
                    Corp = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cBPOwnerName), EveHQ.Core.Pilot).Corp
                Else
                    ' Assume a corp
                    Corp = BPOwnerName
                End If
                ownerList.Add(Corp)
                ' Go through each pilot and match the corp
                For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                    If cPilot.Corp = Corp Then
                        ownerList.Add(cPilot.Name)
                    End If
                Next
        End Select

        ' Clear the current owned resources list
        ownedResources.Clear()

        ' Iterate through our list of owners
        For Each Owner As String In ownerList

            ' Fetch the resources owned
            Dim IsCorp As Boolean = False
            ' See if this owner is a corp
            If PlugInData.CorpList.ContainsKey(Owner) = True Then
                IsCorp = True
                ' See if we have a representative
                Dim CorpRep As SortedList = CType(PlugInData.CorpReps(0), Collections.SortedList)
                If CorpRep IsNot Nothing Then
                    If CorpRep.ContainsKey(CStr(PlugInData.CorpList(Owner))) = True Then
                        Owner = CStr(CorpRep(CStr(PlugInData.CorpList(Owner))))
                    Else
                        Owner = ""
                    End If
                Else
                    Owner = ""
                End If
            End If

            If Owner <> "" Then
                ' Parse the Assets XML
                Dim assetXML As New XmlDocument
                Dim selPilot As EveHQ.Core.Pilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(Owner), Core.Pilot)
                Dim accountName As String = selPilot.Account
                Dim pilotAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts.Item(accountName), Core.EveAccount)
                If IsCorp = True Then
                    assetXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsCorp, pilotAccount, selPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
                Else
                    assetXML = EveHQ.Core.EveAPI.GetAPIXML(EveHQ.Core.EveAPI.APIRequest.AssetsChar, pilotAccount, selPilot.ID, EveHQ.Core.EveAPI.APIReturnMethod.ReturnCacheOnly)
                End If
                If assetXML IsNot Nothing Then

                    Dim locList As XmlNodeList = assetXML.SelectNodes("/eveapi/result/rowset/row")
                    If locList.Count > 0 Then
                        ' Define what we want to obtain
                        Dim categories, groups As New ArrayList
                        For Each loc As XmlNode In locList
                            Call GetAssetQuantitesFromNode(loc, categories, groups, ownedResources)
                        Next
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub GetResourcesFromJob(ByVal pJob As ProductionJob)
        For Each resource As Object In pJob.RequiredResources.Values
            If TypeOf (resource) Is RequiredResource Then
                Dim rResource As RequiredResource = CType(resource, RequiredResource)
                ' This is a resource so add it
                If rResource.TypeCategory <> 16 Then
                    If groupResources.ContainsKey(CStr(rResource.TypeID)) = False Then
                        groupResources.Add(CStr(rResource.TypeID), CLng((rResource.PerfectUnits + rResource.WasteUnits) * pJob.Runs))
                    Else
                        groupResources(CStr(rResource.TypeID)) += CLng((rResource.PerfectUnits + rResource.WasteUnits) * pJob.Runs)
                    End If
                End If
            Else
                ' This is another production job
                Dim subJob As ProductionJob = CType(resource, ProductionJob)
                Call Me.GetResourcesFromJob(subJob)
            End If
        Next
    End Sub

    Private Sub DisplayRequiredResources()

        clvResources.BeginUpdate() : clvOwnedResources.BeginUpdate()
        clvResources.Items.Clear() : clvOwnedResources.Items.Clear()

        Dim maxProducableUnits As Long = -1
        UnitMaterial = 0 : UnitWaste = 0

        For Each resource As Object In currentJob.RequiredResources.Values
            If TypeOf (resource) Is RequiredResource Then
                ' This is a resource so add it
                Dim rResource As RequiredResource = CType(resource, RequiredResource)
                If rResource.TypeCategory <> 16 Or (rResource.TypeCategory = 16 And chkShowSkills.Checked = True) Then
                    Dim perfectRaw As Integer = CInt(rResource.PerfectUnits)
                    Dim waste As Integer = CInt(rResource.WasteUnits)
                    Dim total As Integer = perfectRaw + waste
                    Dim price As Double = EveHQ.Core.DataFunctions.GetPrice(CStr(rResource.TypeID))
                    Dim value As Double = total * price
                    ' Add a new list view item
                    If total > 0 Then
                        Dim newRes As New ContainerListViewItem(rResource.TypeName)
                        clvResources.Items.Add(newRes)
                        ' Calculate costs
                        If rResource.TypeCategory <> 16 Then
                            ' Not a skill
                            UnitMaterial += value
                            UnitWaste += waste * price
                        Else
                            ' A skill
                            newRes.Text &= " (Lvl " & EveHQ.Core.SkillFunctions.Roman(perfectRaw) & ")"
                            ' Check for skill of recycler
                            If EveHQ.Core.SkillFunctions.IsSkillTrained(CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilot.SelectedItem), Core.Pilot), rResource.TypeName, perfectRaw) = True Then
                                newRes.BackColor = Drawing.Color.LightGreen
                            Else
                                newRes.BackColor = Drawing.Color.LightCoral
                            End If
                            perfectRaw = 0 : waste = 0 : total = 0 : value = 0
                        End If

                        newRes.SubItems(1).Text = FormatNumber(perfectRaw * ProductionRuns, 0)
                        newRes.SubItems(2).Text = FormatNumber(waste * ProductionRuns, 0)
                        newRes.SubItems(3).Text = FormatNumber(total * ProductionRuns, 0)
                        newRes.SubItems(4).Text = FormatNumber(price, 2)
                        newRes.SubItems(5).Text = FormatNumber(value * ProductionRuns, 2)
                        newRes.SubItems(6).Text = FormatNumber(Int(rResource.PerfectUnits / CurrentBP.MatMod), 0)

                        ' Add this into the required resources if necessary i.e. is not a skill
                        'If rResource.TypeCategory <> 16 Then
                        '    Dim reqd, owned, surplus As Long
                        '    reqd = total * ProductionRuns
                        '    If ownedResources.ContainsKey(rResource.TypeName) = True Then
                        '        owned = ownedResources(rResource.TypeName)
                        '        If maxProducableUnits = -1 Then
                        '            maxProducableUnits = CLng(Int(owned / total))
                        '        Else
                        '            maxProducableUnits = Math.Min(maxProducableUnits, CLng(Int(owned / total)))
                        '        End If
                        '    Else
                        '        owned = 0
                        '        maxProducableUnits = 0
                        '    End If
                        '    surplus = owned - reqd
                        '    Dim newORes As New ContainerListViewItem(rResource.TypeName)
                        '    clvOwnedResources.Items.Add(newORes)
                        '    newORes.SubItems(1).Text = FormatNumber(reqd, 0)
                        '    newORes.SubItems(2).Text = FormatNumber(owned, 0)
                        '    newORes.SubItems(3).Text = FormatNumber(surplus, 0)
                        '    newORes.SubItems(3).Tag = surplus
                        '    If surplus < 0 Then
                        '        newORes.SubItems(3).ForeColor = Drawing.Color.Red
                        '    Else
                        '        newORes.SubItems(3).ForeColor = Drawing.Color.Green
                        '    End If
                        'End If
                    End If
                End If
            Else
                ' This is another production job
                Dim subJob As ProductionJob = CType(resource, ProductionJob)
                Dim newRes As New ContainerListViewItem(subJob.TypeName)
                Dim perfectRaw As Integer = CInt(subJob.PerfectUnits)
                Dim waste As Integer = CInt(subJob.WasteUnits)
                Dim total As Integer = perfectRaw + waste
                Dim price As Double = EveHQ.Core.DataFunctions.GetPrice(CStr(subJob.TypeID))
                Dim value As Double = total * price
                clvResources.Items.Add(newRes)
                newRes.SubItems(1).Text = FormatNumber(subJob.PerfectUnits * ProductionRuns, 0)
                newRes.SubItems(2).Text = FormatNumber(subJob.WasteUnits * ProductionRuns, 0)
                newRes.SubItems(3).Text = FormatNumber(total * ProductionRuns, 0)
                newRes.SubItems(4).Text = FormatNumber(price, 2)
                newRes.SubItems(5).Text = FormatNumber(value * ProductionRuns, 2)
                newRes.SubItems(6).Text = FormatNumber(Int(subJob.PerfectUnits / CurrentBP.MatMod), 0)
                Call DisplayJob(subJob, newRes, maxProducableUnits)
                ' Recalculate sub prices
                Dim subprice As Double = 0
                For Each subRes As ContainerListViewItem In newRes.Items
                    subprice += CDbl(subRes.SubItems(5).Text)
                Next
                newRes.SubItems(5).Text = FormatNumber(subprice, 2)
                newRes.SubItems(4).Text = FormatNumber(subprice / subJob.Runs, 2)
            End If
        Next

        ' Display the resources owned
        Dim ItemData As EveHQ.Core.EveItem
        Dim reqd, owned, surplus As Long
        For Each itemID As String In groupResources.Keys
            reqd = groupResources(itemID)
            If reqd > 0 Then
                ItemData = CType(EveHQ.Core.HQ.itemData(itemID), Core.EveItem)
                Dim newORes As New ContainerListViewItem(ItemData.Name)
                If ownedResources.ContainsKey(itemID) = True Then
                    owned = ownedResources(itemID)
                    If maxProducableUnits = -1 Then
                        maxProducableUnits = CLng(Int(owned / reqd))
                    Else
                        maxProducableUnits = Math.Min(maxProducableUnits, CLng(Int(owned / reqd)))
                    End If
                Else
                    owned = 0
                    maxProducableUnits = 0
                End If
                surplus = owned - reqd
                clvOwnedResources.Items.Add(newORes)
                newORes.SubItems(1).Text = FormatNumber(reqd, 0)
                newORes.SubItems(2).Text = FormatNumber(owned, 0)
                newORes.SubItems(3).Text = FormatNumber(surplus, 0)
                newORes.SubItems(3).Tag = surplus
                If surplus < 0 Then
                    newORes.SubItems(3).ForeColor = Drawing.Color.Red
                Else
                    newORes.SubItems(3).ForeColor = Drawing.Color.Green
                End If
            End If
        Next

        clvResources.Sort(3, SortOrder.Descending, False) : clvOwnedResources.Sort(1, SortOrder.Descending, False)
        clvResources.EndUpdate() : clvOwnedResources.EndUpdate()
        lblMaxUnits.Text = "Maximum Producable Units: " & FormatNumber(maxProducableUnits, 0)

    End Sub

    Private Sub DisplayJob(ByVal parentJob As ProductionJob, ByVal parentRes As ContainerListViewItem, ByRef maxProducableUnits As Long)
        For Each resource As Object In parentJob.RequiredResources.Values
            If TypeOf (resource) Is RequiredResource Then
                ' This is a resource so add it
                Dim rResource As RequiredResource = CType(resource, RequiredResource)
                If rResource.TypeCategory <> 16 Or (rResource.TypeCategory = 16 And chkShowSkills.Checked = True) Then
                    Dim perfectRaw As Integer = CInt(rResource.PerfectUnits) * parentJob.Runs
                    Dim waste As Integer = CInt(rResource.WasteUnits) * parentJob.Runs
                    Dim total As Integer = perfectRaw + waste
                    Dim price As Double = EveHQ.Core.DataFunctions.GetPrice(CStr(rResource.TypeID))
                    Dim value As Double = total * price
                    ' Add a new list view item
                    Dim newRes As New ContainerListViewItem(rResource.TypeName)
                    parentRes.Items.Add(newRes)
                    ' Calculate costs
                    If rResource.TypeCategory <> 16 Then
                        ' Not a skill
                        UnitMaterial += value
                        UnitWaste += waste * price
                    Else
                        ' A skill
                        newRes.Text &= " (Lvl " & EveHQ.Core.SkillFunctions.Roman(CInt(rResource.PerfectUnits)) & ")"
                        ' Check for skill of recycler
                        If EveHQ.Core.SkillFunctions.IsSkillTrained(CType(EveHQ.Core.HQ.EveHQSettings.Pilots(cboPilot.SelectedItem), Core.Pilot), rResource.TypeName, CInt(rResource.PerfectUnits)) = True Then
                            newRes.BackColor = Drawing.Color.LightGreen
                        Else
                            newRes.BackColor = Drawing.Color.LightCoral
                        End If
                        perfectRaw = 0 : waste = 0 : total = 0 : value = 0
                    End If

                    newRes.SubItems(1).Text = FormatNumber(perfectRaw, 0)
                    newRes.SubItems(2).Text = FormatNumber(waste, 0)
                    newRes.SubItems(3).Text = FormatNumber(total, 0)
                    newRes.SubItems(4).Text = FormatNumber(price, 2)
                    newRes.SubItems(5).Text = FormatNumber(value, 2)
                    newRes.SubItems(6).Text = FormatNumber(Int(rResource.PerfectUnits / CurrentBP.MatMod), 0)

                    ' Add this into the required resources if necessary i.e. is not a skill
                    'If rResource.TypeCategory <> 16 Then
                    '    Dim reqd, owned, surplus As Long
                    '    reqd = total * ProductionRuns
                    '    If ownedResources.ContainsKey(rResource.TypeName) = True Then
                    '        owned = ownedResources(rResource.TypeName)
                    '        If maxProducableUnits = -1 Then
                    '            maxProducableUnits = CLng(Int(owned / total))
                    '        Else
                    '            maxProducableUnits = Math.Min(maxProducableUnits, CLng(Int(owned / total)))
                    '        End If
                    '    Else
                    '        owned = 0
                    '        maxProducableUnits = 0
                    '    End If
                    '    surplus = owned - reqd
                    '    Dim newORes As New ContainerListViewItem(rResource.TypeName)
                    '    clvOwnedResources.Items.Add(newORes)
                    '    newORes.SubItems(1).Text = FormatNumber(reqd, 0)
                    '    newORes.SubItems(2).Text = FormatNumber(owned, 0)
                    '    newORes.SubItems(3).Text = FormatNumber(surplus, 0)
                    '    newORes.SubItems(3).Tag = surplus
                    '    If surplus < 0 Then
                    '        newORes.SubItems(3).ForeColor = Drawing.Color.Red
                    '    Else
                    '        newORes.SubItems(3).ForeColor = Drawing.Color.Green
                    '    End If
                    'End If

                End If
            Else
                ' This is another production job
                Dim subJob As ProductionJob = CType(resource, ProductionJob)
                Dim newRes As New ContainerListViewItem(subJob.TypeName)
                Dim perfectRaw As Integer = CInt(subJob.PerfectUnits)
                Dim waste As Integer = CInt(subJob.WasteUnits)
                Dim runs As Integer = subJob.Runs
                Dim total As Integer = perfectRaw + waste
                Dim price As Double = EveHQ.Core.DataFunctions.GetPrice(CStr(subJob.TypeID))
                Dim value As Double = total * price
                parentRes.Items.Add(newRes)
                newRes.SubItems(1).Text = FormatNumber(perfectRaw * runs, 0)
                newRes.SubItems(2).Text = FormatNumber(waste * runs, 0)
                newRes.SubItems(3).Text = FormatNumber(total * runs, 0)
                newRes.SubItems(4).Text = FormatNumber(price, 2)
                newRes.SubItems(5).Text = FormatNumber(value * runs, 2)
                newRes.SubItems(6).Text = FormatNumber(Int(perfectRaw / CurrentBP.MatMod), 0)
                Call DisplayJob(subJob, newRes, maxProducableUnits)
                ' Recalculate sub prices
                Dim subprice As Double = 0
                For Each subRes As ContainerListViewItem In newRes.Items
                    subprice += CDbl(subRes.SubItems(5).Text)
                Next
                newRes.SubItems(5).Text = FormatNumber(subprice, 2)
                newRes.SubItems(4).Text = FormatNumber(subprice / subJob.Runs, 2)
            End If
        Next
    End Sub

    Private Sub GetAssetQuantitesFromNode(ByVal item As XmlNode, ByVal categories As ArrayList, ByVal groups As ArrayList, ByRef Assets As SortedList(Of String, Long))
        Dim ItemData As New EveHQ.Core.EveItem
        Dim AssetID As String = ""
        Dim itemID As String = ""
        AssetID = item.Attributes.GetNamedItem("itemID").Value
        itemID = item.Attributes.GetNamedItem("typeID").Value
        If EveHQ.Core.HQ.itemData.ContainsKey(itemID) Then
            ItemData = CType(EveHQ.Core.HQ.itemData(itemID), Core.EveItem)
            If categories.Contains(ItemData.Category) Or groups.Contains(ItemData.Group) Or groupResources.ContainsKey(CStr(ItemData.ID)) Then
                ' Check if the item is in the list
                If Assets.ContainsKey(CStr(ItemData.ID)) = False Then
                    Assets.Add(CStr(ItemData.ID), CLng(item.Attributes.GetNamedItem("quantity").Value))
                Else
                    Assets(CStr(ItemData.ID)) = Assets(CStr(ItemData.ID)) + CLng(item.Attributes.GetNamedItem("quantity").Value)
                End If
            End If
        End If
        ' Check child items if they exist
        If item.ChildNodes.Count > 0 Then
            For Each subitem As XmlNode In item.ChildNodes(0).ChildNodes
                Call GetAssetQuantitesFromNode(subitem, categories, groups, Assets)
            Next
        End If
    End Sub

    Private Sub DisplayCostInformation()
        ' Display Build Time Information
        lblUnitBuildTime.Text = EveHQ.Core.SkillFunctions.TimeToString(UnitBuildTime, False)
        lblTotalBuildTime.Text = EveHQ.Core.SkillFunctions.TimeToString(UnitBuildTime * ProductionRuns, False)
        ' Display Waste Cost Info
        lblUnitWasteCost.Text = FormatNumber(UnitWaste, 2) & " isk"
        lblTotalWasteCost.Text = FormatNumber(UnitWaste * ProductionRuns, 2) & " isk"
        lblBPEfficiency.Text = FormatNumber((UnitMaterial - UnitWaste) / UnitMaterial * 100, 4) & "%"
        ' Display Materials costs
        lblUnitBuildCost.Text = FormatNumber(UnitMaterial, 2) & " isk"
        lblTotalBuildCost.Text = FormatNumber(UnitMaterial * ProductionRuns, 2) & " isk"
        ' Display Factory costs
        lblFactoryCosts.Text = FormatNumber(FactoryCosts, 2) & " isk"
        Dim totalCosts As Double = (UnitMaterial * ProductionRuns) + FactoryCosts
        Dim unitcosts As Double = Math.Round(totalCosts / (ProductionRuns * BatchSize), 2)
        Dim productID As String = CurrentBP.ProductID.ToString
        Dim value As Double = EveHQ.Core.DataFunctions.GetPrice(productID)
        Dim profit As Double = value - unitcosts
        lblTotalCosts.Text = FormatNumber(totalCosts, 2) & " isk"
        lblUnitCost.Text = FormatNumber(unitcosts, 2) & " isk"
        lblUnitValue.Text = FormatNumber(value, 2) & " isk"
        If profit > 0 Then
            lblUnitProfit.ForeColor = Drawing.Color.Green
            lblUnitProfit.Text = FormatNumber(profit, 2) & " isk"
        Else
            If profit < 0 Then
                lblUnitProfit.ForeColor = Drawing.Color.Red
                lblUnitProfit.Text = FormatNumber(profit, 2) & " isk"
            Else
                lblUnitProfit.ForeColor = Drawing.Color.Black
                lblUnitProfit.Text = FormatNumber(profit, 2) & " isk"
            End If
        End If
    End Sub

#End Region

#Region "UI Routines"

    Private Sub chkOwnedBPOs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOwnedBPOs.CheckedChanged
        If chkOwnedBPOs.Checked = True Then
            Call Me.DisplayOwnedBlueprints()
        Else
            Call Me.DisplayAllBlueprints()
        End If
    End Sub

    Private Sub chkPOSProduction_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPOSProduction.CheckedChanged
        cboPOSArrays.Enabled = chkPOSProduction.Checked
        If UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub
    Private Sub cboPOSArrays_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPOSArrays.SelectedIndexChanged
        If UpdateBPInfo = True Then
            ProductionArray = PlugInData.AssemblyArrays(cboPOSArrays.SelectedItem.ToString)
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub

    Private Sub nudMELevel_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudMELevel.ValueChanged
        If UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub
    Private Sub nudPELevel_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudPELevel.ValueChanged
        If UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub
    Private Sub nudCopyRuns_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudCopyRuns.ValueChanged
        If UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub
    Private Sub chkResearchAtPOS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkResearchAtPOS.CheckedChanged
        If UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub
    Private Sub nudRuns_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudRuns.ValueChanged
        If UpdateBPInfo = True Then
            ProductionRuns = CInt(nudRuns.Value)
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub

    Private Sub nudInstallCost_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudInstallCost.ValueChanged
        If UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub
    Private Sub nudRunningCost_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudRunningCost.ValueChanged
        If UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub

    Private Sub cboResearchSkill_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboResearchSkill.SelectedIndexChanged
        If UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub
    Private Sub cboMetallurgySkill_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboMetallurgySkill.SelectedIndexChanged
        If UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub
    Private Sub cboScienceSkill_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboScienceSkill.SelectedIndexChanged
        If UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub
    Private Sub cboIndustrySkill_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboIndustrySkill.SelectedIndexChanged
        If UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub
    Private Sub cboProdEffSkill_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboProdEffSkill.SelectedIndexChanged
        If UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub
    Private Sub cboResearchImplant_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboResearchImplant.SelectedIndexChanged
        If UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub
    Private Sub cboMetallurgyImplant_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboMetallurgyImplant.SelectedIndexChanged
        If UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub
    Private Sub cboScienceImplant_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboScienceImplant.SelectedIndexChanged
        If UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub
    Private Sub cboIndustyImplant_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboIndustyImplant.SelectedIndexChanged
        If UpdateBPInfo = True Then
            ' Update the Blueprint information
            Call Me.UpdateBlueprintInformation()
        End If
    End Sub
    Private Sub chkShowSkills_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowSkills.CheckedChanged
        Call Me.DisplayRequiredResources()
    End Sub
    Private Sub cboAssetSelection_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAssetSelection.SelectedIndexChanged
        If StartUp = False Then
            Call Me.GetRequiredResources()
            Call Me.GetOwnedResources()
            Call Me.DisplayRequiredResources()
            Call Me.DisplayCostInformation()
        End If
    End Sub
    Private Sub chkUseStandardBPCosting_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseStandardBPCosting.CheckedChanged
        If StartUp = False Then
            Call Me.GetRequiredResources()
            Call Me.GetOwnedResources()
            Call Me.DisplayRequiredResources()
            Call Me.DisplayCostInformation()
        End If
    End Sub

#End Region
   
   
    
End Class
