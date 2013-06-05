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

Imports System.Windows.Forms
Imports EveHQ.Market
Imports System.Threading.Tasks

<Serializable()> Public Class Blueprint
    Public ID As Integer
    Public AssetID As Long
    Public Name As String = ""
    Public GroupID As Integer
    Public ProductID As Integer
    Public ProdTime As Double
    Public TechLevel As Integer
    Public ResearchProdTime As Double
    Public ResearchMatTime As Double
    Public ResearchCopyTime As Double
    Public ResearchTechTime As Double
    Public ProdMod As Integer
    Public MatMod As Integer
    Public WasteFactor As Double
    Public MaxProdLimit As Integer
    Public Resources As New SortedList(Of String, BlueprintResource)
    Public Inventions As New List(Of String)
    Public InventionMetaItems As New SortedList(Of String, String)
    Public InventFrom As New List(Of String)

    Public Shared Function LoadBluePrintData() As Boolean
        ' Get the Blueprint data from the DB
        'Dim strSQL As String = "SELECT invBlueprintTypes.*, invTypes.typeName, invTypes.groupID FROM invTypes INNER JOIN invBlueprintTypes ON invTypes.typeID = invBlueprintTypes.blueprintTypeID WHERE invTypes.published=1;"
        Dim strSQL As String = "SELECT invBlueprintTypes.*, invTypes.typeName, invTypes.groupID FROM invTypes INNER JOIN invBlueprintTypes ON invTypes.typeID = invBlueprintTypes.blueprintTypeID;"
        Dim BPDataSet As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        If BPDataSet IsNot Nothing Then
            If BPDataSet.Tables(0).Rows.Count > 0 Then
                ' Reset the list
                PlugInData.Blueprints.Clear()
                ' Populate the main data
                For Each BP As DataRow In BPDataSet.Tables(0).Rows
                    Dim newBP As New Blueprint
                    newBP.ID = CInt(BP.Item("blueprintTypeID"))
                    newBP.Name = CStr(BP.Item("typeName"))
                    newBP.GroupID = CInt(BP.Item("groupID"))
                    newBP.ProductID = CInt(BP.Item("productTypeID"))
                    newBP.ProdTime = CInt(BP.Item("productionTime"))
                    newBP.TechLevel = CInt(BP.Item("techlevel"))
                    newBP.ResearchProdTime = CInt(BP.Item("researchProductivityTime"))
                    newBP.ResearchMatTime = CInt(BP.Item("researchMaterialTime"))
                    newBP.ResearchCopyTime = CInt(BP.Item("researchCopyTime"))
                    newBP.ResearchTechTime = CInt(BP.Item("researchTechTime"))
                    newBP.ProdMod = CInt(BP.Item("productivityModifier"))
                    newBP.MatMod = CInt(BP.Item("materialModifier"))
                    newBP.WasteFactor = CInt(BP.Item("wasteFactor"))
                    newBP.MaxProdLimit = CInt(BP.Item("maxProductionLimit"))
                    newBP.Resources = New SortedList(Of String, BlueprintResource)
                    PlugInData.Blueprints.Add(newBP.ID.ToString, newBP)
                    If EveHQ.Core.HQ.itemData.ContainsKey(newBP.ProductID.ToString) Then
                        Dim catID As String = CStr(EveHQ.Core.HQ.itemData(newBP.ProductID.ToString).Category)
                        If PlugInData.CategoryNames.ContainsKey(EveHQ.Core.HQ.itemCats(catID)) = False Then
                            PlugInData.CategoryNames.Add(EveHQ.Core.HQ.itemCats(catID), catID)
                        End If
                    End If
                    If PlugInData.Products.ContainsKey(newBP.ProductID.ToString) = False Then
                        PlugInData.Products.Add(newBP.ProductID.ToString, newBP.ID.ToString)
                    End If
                Next
                ' Ok so far so let's add the material requirements
                strSQL = "SELECT invBuildMaterials.*, invTypes.typeName, invGroups.groupID, invGroups.categoryID FROM invGroups INNER JOIN (invTypes INNER JOIN invBuildMaterials ON invTypes.typeID = invBuildMaterials.requiredTypeID) ON invGroups.groupID = invTypes.groupID ORDER BY invBuildMaterials.typeID;"
                BPDataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
                If BPDataSet IsNot Nothing Then
                    If BPDataSet.Tables(0).Rows.Count > 0 Then
                        ' Go through each BP and refine the Dataset
                        For Each newBP As Blueprint In PlugInData.Blueprints.Values
                            ' Select resource data for the blueprint
                            Dim BPRows() As DataRow = BPDataSet.Tables(0).Select("typeID=" & newBP.ID.ToString)
                            For Each req As DataRow In BPRows
                                Dim newReq As New BlueprintResource
                                newReq.Activity = CInt(req.Item("activityID"))
                                newReq.DamagePerJob = CDbl(req.Item("damagePerJob"))
                                newReq.TypeID = CInt(req.Item("requiredTypeID"))
                                newReq.TypeGroup = CInt(req.Item("groupID"))
                                newReq.TypeCategory = CInt(req.Item("categoryID"))
                                newReq.Quantity = CInt(req.Item("quantity"))
                                newReq.TypeName = CStr(req.Item("typeName"))
                                If IsDBNull(req.Item("baseMaterial")) = False Then
                                    newReq.BaseMaterial = CInt(req.Item("baseMaterial"))
                                Else
                                    newReq.BaseMaterial = 0
                                End If
                                If newBP.Resources.ContainsKey(newReq.TypeID.ToString & "_" & newReq.Activity.ToString) = False Then
                                    newBP.Resources.Add(newReq.TypeID.ToString & "_" & newReq.Activity.ToString, newReq)
                                End If
                            Next
                            ' Select resource data for the product
                            If newBP.ProductID <> newBP.ID Then
                                BPRows = BPDataSet.Tables(0).Select("typeID=" & newBP.ProductID.ToString)
                                For Each req As DataRow In BPRows
                                    Dim newReq As New BlueprintResource
                                    newReq.TypeID = CInt(req.Item("requiredTypeID"))
                                    newReq.Activity = CInt(req.Item("activityID"))
                                    newReq.DamagePerJob = CDbl(req.Item("damagePerJob"))
                                    newReq.TypeGroup = CInt(req.Item("groupID"))
                                    newReq.TypeCategory = CInt(req.Item("categoryID"))
                                    newReq.Quantity = CInt(req.Item("quantity"))
                                    newReq.TypeName = CStr(req.Item("typeName"))
                                    If IsDBNull(req.Item("baseMaterial")) = False Then
                                        newReq.BaseMaterial = CInt(req.Item("baseMaterial"))
                                    Else
                                        newReq.BaseMaterial = 0
                                    End If
                                    If newBP.Resources.ContainsKey(newReq.TypeID.ToString & "_" & newReq.Activity.ToString) = False Then
                                        newBP.Resources.Add(newReq.TypeID.ToString & "_" & newReq.Activity.ToString, newReq)
                                    End If
                                Next
                            End If
                        Next
                        ' Fetch the relevant Invention Data
                        strSQL = "SELECT SourceBP.blueprintTypeID AS SBP, InventedBP.blueprintTypeID AS IBP"
                        strSQL &= " FROM invBlueprintTypes AS SourceBP INNER JOIN"
                        strSQL &= " invMetaTypes ON SourceBP.productTypeID = invMetaTypes.parentTypeID INNER JOIN"
                        strSQL &= " invBlueprintTypes AS InventedBP ON invMetaTypes.typeID = InventedBP.productTypeID"
                        strSQL &= " WHERE (invMetaTypes.metaGroupID = 2);"
                        BPDataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
                        If BPDataSet IsNot Nothing Then
                            If BPDataSet.Tables(0).Rows.Count > 0 Then
                                For Each InvRow As DataRow In BPDataSet.Tables(0).Rows
                                    ' Add the "Inventable" item
                                    If PlugInData.Blueprints.ContainsKey(InvRow.Item("SBP").ToString) Then
                                        Dim CurrentBP As Blueprint = PlugInData.Blueprints(InvRow.Item("SBP").ToString)
                                        CurrentBP.Inventions.Add(InvRow.Item("IBP").ToString)
                                    End If
                                    ' Add the "Invented From" item
                                    If PlugInData.Blueprints.ContainsKey(InvRow.Item("IBP").ToString) Then
                                        Dim CurrentBP As Blueprint = PlugInData.Blueprints(InvRow.Item("IBP").ToString)
                                        CurrentBP.InventFrom.Add(InvRow.Item("SBP").ToString)
                                    End If
                                Next
                                ' Return finished
                                Return True
                            Else
                                MessageBox.Show("Blueprint Invention Data returned no valid rows.", "Prism Blueprint Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                Return False
                            End If
                        Else
                            MessageBox.Show("Blueprint Invention Data returned a null dataset.", "Prism Blueprint Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return False
                        End If
                    Else
                        MessageBox.Show("Blueprint Materials Data returned a null dataset.", "Prism Blueprint Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return False
                    End If
                Else
                    MessageBox.Show("Blueprint Materials Data returned no valid rows.", "Prism Blueprint Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
            Else
                MessageBox.Show("Blueprint Data returned a null dataset.", "Prism Blueprint Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If
        Else
            MessageBox.Show("Blueprint Data returned no valid rows.", "Prism Blueprint Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If

    End Function
End Class

<Serializable()> Public Class BlueprintResource
    Public TypeID As Integer
    Public TypeName As String
    Public TypeGroup As Integer
    Public TypeCategory As Integer
    Public Activity As Integer
    Public Quantity As Integer
    Public DamagePerJob As Double
    Public BaseMaterial As Integer
End Class

<Serializable()> Public Class RequiredResource
    Public TypeID As Integer
    Public TypeName As String
    Public TypeGroup As Integer
    Public TypeCategory As Integer
    Public PerfectUnits As Double
    Public BaseUnits As Double
    Public WasteUnits As Double
End Class

<Serializable()> Public Class SwapResource
    Public ID As Integer
    Public Quantity As Long
    Public Resources As New SortedList(Of String, Long)
End Class

<Serializable()> Public Class BlueprintAsset
    Public AssetID As String
    Public TypeID As String
    Public LocationID As String
    Public LocationDetails As String
    Public MELevel As Integer
    Public PELevel As Integer
    Public Runs As Integer
    Public Status As Integer = 0
    Public BPType As Integer = 0
    Public Notes As String
End Class

<Serializable()> Public Class BlueprintSelection
    Inherits Blueprint
    Public MELevel As Integer
    Public PELevel As Integer
    Public Runs As Integer

    Public Shared Sub CheckForInventionItems(ByVal OriginalBlueprint As BlueprintSelection)
        ' Check if the Inventions are null
        If OriginalBlueprint.Inventions Is Nothing Then
            OriginalBlueprint.Inventions = New List(Of String)
            Dim BaseBP As Blueprint = PlugInData.Blueprints(OriginalBlueprint.ID.ToString)
            For Each Invention As String In BaseBP.Inventions
                OriginalBlueprint.Inventions.Add(Invention)
            Next
        End If
        ' Check if the Inventeds are null
        If OriginalBlueprint.InventFrom Is Nothing Then
            OriginalBlueprint.InventFrom = New List(Of String)
            Dim BaseBP As Blueprint = PlugInData.Blueprints(OriginalBlueprint.ID.ToString)
            For Each Invention As String In BaseBP.InventFrom
                OriginalBlueprint.InventFrom.Add(Invention)
            Next
        End If
    End Sub

    Public Shared Function CopyFromBlueprint(ByVal OriginalBlueprint As Blueprint) As BlueprintSelection
        Dim newBP As New BlueprintSelection
        ' Copy BP Data
        newBP.ID = OriginalBlueprint.ID
        newBP.Name = OriginalBlueprint.Name
        newBP.GroupID = OriginalBlueprint.GroupID
        newBP.ProductID = OriginalBlueprint.ProductID
        newBP.ProdTime = OriginalBlueprint.ProdTime
        newBP.TechLevel = OriginalBlueprint.TechLevel
        newBP.ResearchCopyTime = OriginalBlueprint.ResearchCopyTime
        newBP.ResearchMatTime = OriginalBlueprint.ResearchMatTime
        newBP.ResearchProdTime = OriginalBlueprint.ResearchProdTime
        newBP.ResearchTechTime = OriginalBlueprint.ResearchTechTime
        newBP.ProdMod = OriginalBlueprint.ProdMod
        newBP.MatMod = OriginalBlueprint.MatMod
        newBP.WasteFactor = OriginalBlueprint.WasteFactor
        newBP.MaxProdLimit = OriginalBlueprint.MaxProdLimit
        ' Copy resources
        For Each resource As String In OriginalBlueprint.Resources.Keys
            newBP.Resources.Add(resource, OriginalBlueprint.Resources(resource))
        Next
        ' Check if the Inventions are null
        If OriginalBlueprint.Inventions Is Nothing Then
            OriginalBlueprint.Inventions = New List(Of String)
            Dim BaseBP As Blueprint = PlugInData.Blueprints(OriginalBlueprint.ID.ToString)
            For Each Invention As String In BaseBP.Inventions
                OriginalBlueprint.Inventions.Add(Invention)
            Next
        End If
        ' Copy Inventions
        For Each invention As String In OriginalBlueprint.Inventions
            newBP.Inventions.Add(invention)
        Next
        ' Check if the Inventeds are null
        If OriginalBlueprint.InventFrom Is Nothing Then
            OriginalBlueprint.InventFrom = New List(Of String)
            Dim BaseBP As Blueprint = PlugInData.Blueprints(OriginalBlueprint.ID.ToString)
            For Each Invention As String In BaseBP.InventFrom
                OriginalBlueprint.InventFrom.Add(Invention)
            Next
        End If
        ' Copy Inventeds
        For Each Invention As String In OriginalBlueprint.InventFrom
            newBP.InventFrom.Add(Invention)
        Next
        ' Copy meta items
        For Each metaitem As String In OriginalBlueprint.InventionMetaItems.Keys
            newBP.InventionMetaItems.Add(metaitem, metaitem)
        Next
        Return newBP
    End Function

    Public Function CreateProductionJob(ByVal BPOwner As String, ByVal Manufacturer As String, ByVal ProdEffSkill As Integer, ByVal IndSkill As Integer, ByVal ProdImplantBonus As Integer, ByVal OverridingMELevel As String, ByVal OverridingPELevel As String, ByVal Runs As Integer, ByVal SlotArray As AssemblyArray, ByVal ComponentIteration As Boolean) As ProductionJob
        ' Set up a new production job
        Dim newPJ As New ProductionJob
        newPJ.CurrentBP = Me
        newPJ.TypeID = Me.ID
        newPJ.TypeName = Me.Name
        newPJ.Runs = Runs
        newPJ.StartTime = Now
        newPJ.Manufacturer = Manufacturer
        newPJ.BPOwner = BPOwner
        newPJ.PESkill = ProdEffSkill
        newPJ.IndSkill = IndSkill
        newPJ.ProdImplant = ProdImplantBonus
        newPJ.AssemblyArray = SlotArray
        If IsNumeric(OverridingMELevel) = False Then
            newPJ.OverridingME = ""
        Else
            newPJ.OverridingME = OverridingMELevel
        End If
        If IsNumeric(OverridingPELevel) = False Then
            newPJ.OverridingPE = ""
        Else
            newPJ.OverridingPE = OverridingPELevel
        End If
        ' Get the required resources
        newPJ.CalculateResourceRequirements(ComponentIteration, BPOwner)
        Return newPJ
    End Function

    Public Sub UpdateProductionJob(ByRef Job As ProductionJob)
        Job.CurrentBP = Me
        Job.TypeID = Me.ID
        Job.TypeName = Me.Name
        Job.RecalculateResourceRequirements()
    End Sub

    Public Function CalculateWasteFactor(ByVal ProdEffSkill As Integer) As Double
        Return CalculateBPWasteFactor(Me.MELevel, ProdEffSkill)
    End Function

    Public Function CalculateWasteFactor(ByVal BPMELevel As Integer, ByVal ProdEffSkill As Integer) As Double
        Return CalculateBPWasteFactor(BPMELevel, ProdEffSkill)
    End Function

    Private Function CalculateBPWasteFactor(ByVal BPMELevel As Integer, ByVal ProdEffSkill As Integer) As Double
        If Me.WasteFactor <> 0 Then
            If BPMELevel < 0 Then
                ' This is for negative ME
                Return ((Me.WasteFactor / 100) * (1 - BPMELevel)) + (0.25 - (0.05 * ProdEffSkill))
            Else
                ' This is for zero and positive ME
                Return ((Me.WasteFactor / 100) / (1 + BPMELevel)) + (0.25 - (0.05 * ProdEffSkill))
            End If
        End If
    End Function

    Public Function CalculateProductionTime(ByVal IndSkill As Integer, ByVal ProdImplantBonus As Double, ByVal ProductionArray As AssemblyArray, ByVal Runs As Integer) As Long
        Return CalculateBPProductionTime(Me.PELevel, IndSkill, ProdImplantBonus, ProductionArray, Runs)
    End Function

    Public Function CalculateProductionTime(ByVal BPPELevel As Integer, ByVal IndSkill As Integer, ByVal ProdImplantBonus As Double, ByVal ProductionArray As AssemblyArray, ByVal Runs As Integer) As Long
        Return CalculateBPProductionTime(BPPELevel, IndSkill, ProdImplantBonus, ProductionArray, Runs)
    End Function

    Private Function CalculateBPProductionTime(ByVal BPPELevel As Integer, ByVal IndSkill As Integer, ByVal ProdImplantBonus As Double, ByVal ProductionArray As AssemblyArray, ByVal Runs As Integer) As Long
        ProdImplantBonus = 1 - (ProdImplantBonus / 100)
        Dim productionTime As Double = Me.ProdTime * (1 - (0.04 * IndSkill)) * ProdImplantBonus
        ' Calculate the production time
        If BPPELevel >= 0 Then
            productionTime *= (1 - (Me.ProdMod / Me.ProdTime) * BPPELevel / (1 + BPPELevel))
        Else
            productionTime *= (1 - (Me.ProdMod / Me.ProdTime) * (BPPELevel - 1))
        End If
        If ProductionArray IsNot Nothing Then
            productionTime *= ProductionArray.TimeMultiplier
        End If
        Return CLng(productionTime * Runs)
    End Function

    Public Function CalculateInventionCost(ByVal metaItemId As String, ByVal decryptorId As String, ByVal bpcRuns As Integer) As Double
        Dim quantityTable As New Dictionary(Of String, Integer)
        ' Gather a list of resources and quantities
        For Each resource As BlueprintResource In Me.Resources.Values
            If resource.Activity = 8 Then
                ' Only include datacores
                If resource.TypeGroup = 333 Then
                    Dim idKey As String = resource.TypeID.ToString
                    If quantityTable.ContainsKey(idKey) = False Then
                        quantityTable.Add(idKey, resource.Quantity)
                    Else
                        quantityTable(idKey) = quantityTable(idKey) + resource.Quantity
                    End If
                End If
            End If
        Next

        ' Add in the meta item id
        If metaItemId.IsNullOrWhiteSpace() = False And metaItemId <> "0" Then
            If quantityTable.ContainsKey(metaItemId) = False Then
                quantityTable.Add(metaItemId, 1)
            Else
                quantityTable(metaItemId) = quantityTable(metaItemId) + 1
            End If
        End If

        ' add the decryptor to the list of items to get prices for
        If decryptorId.IsNullOrWhiteSpace() = False And decryptorId <> "0" Then
            If quantityTable.ContainsKey(decryptorId) = False Then
                quantityTable.Add(decryptorId, 1)
            Else
                quantityTable(decryptorId) = quantityTable(decryptorId) + 1
            End If
        End If

        ' Total the item costs
        Dim prices As Task(Of Dictionary(Of String, Double)) = Core.DataFunctions.GetMarketPrices(quantityTable.Keys)
        prices.Wait()
        Dim itemCost As Dictionary(Of String, Double) = prices.Result
        Dim invCost As Double = itemCost.Keys.Sum(Function(key) itemCost(key) * quantityTable(key))


        ' Calculate lab cost
        invCost += Settings.PrismSettings.LabInstallCost
        invCost += Math.Round(Settings.PrismSettings.LabRunningCost * (Me.ResearchTechTime / 3600), 2, MidpointRounding.AwayFromZero)

        ' Calculate BPC cost
        If Settings.PrismSettings.BPCCosts.ContainsKey(Me.ID.ToString) Then
            Dim pricerange As Double = Settings.PrismSettings.BPCCosts(Me.ID.ToString).MaxRunCost - Settings.PrismSettings.BPCCosts(Me.ID.ToString).MinRunCost
            Dim runrange As Integer = Me.MaxProdLimit - 1
            If runrange = 0 Then
                invCost += Settings.PrismSettings.BPCCosts(Me.ID.ToString).MinRunCost
            Else
                invCost += Settings.PrismSettings.BPCCosts(Me.ID.ToString).MinRunCost + Math.Round((pricerange / runrange) * (bpcRuns - 1), 2, MidpointRounding.AwayFromZero)
            End If
        End If

        Return invCost
    End Function

    Public Function CalculateInventedBPC(ByVal InventedBPID As Integer, ByVal DecryptorID As Integer, ByVal BPCRuns As Integer) As BlueprintSelection
        Dim IBP As BlueprintSelection = BlueprintSelection.CopyFromBlueprint(PlugInData.Blueprints(InventedBPID.ToString))

        Dim IME As Integer = -4
        Dim IPE As Integer = -4
        Dim IRC As Integer = 10

        Dim DecryptorName As String = EveHQ.Core.HQ.itemData(DecryptorID.ToString).Name

        If DecryptorName <> "" Then
            If PlugInData.Decryptors.ContainsKey(DecryptorName) Then
                Dim UseDecryptor As Decryptor = PlugInData.Decryptors(DecryptorName)
                IME += UseDecryptor.MEMod
                IPE += UseDecryptor.PEMod
                'IRC = Math.Min(Math.Max(CInt((BPCRuns / BaseBP.MaxProdLimit) * (IBP.MaxProdLimit / 10)), 1) + UseDecryptor.RunMod, IBP.MaxProdLimit)
                IRC = CInt(Math.Min(Math.Max(Math.Truncate((BPCRuns / IBP.MaxProdLimit) * (IBP.MaxProdLimit / 10) + UseDecryptor.RunMod), 1), IBP.MaxProdLimit))
            Else
                IRC = CInt(Math.Min(Math.Max(Math.Truncate((BPCRuns / IBP.MaxProdLimit) * (IBP.MaxProdLimit / 10)), 1), IBP.MaxProdLimit))
            End If
        Else
            IRC = CInt(Math.Min(Math.Max(Math.Truncate((BPCRuns / IBP.MaxProdLimit) * (IBP.MaxProdLimit / 10)), 1), IBP.MaxProdLimit))
        End If


        IBP.MELevel = IME
        IBP.PELevel = IPE
        IBP.Runs = IRC

        Return IBP

    End Function
End Class

Public Class BPAssetComboboxItem
    Public Name As String = ""
    Public AssetID As String = ""
    Public MELevel As Integer
    Public PELevel As Integer
    Public Runs As Integer
    Public Sub New(ByVal cName As String, ByVal cAssetID As String, ByVal cME As Integer, ByVal cPE As Integer, ByVal cRuns As Integer)
        Me.Name = cName
        Me.AssetID = cAssetID
        Me.MELevel = cME
        Me.PELevel = cPE
        Me.Runs = cRuns
    End Sub
    Public Overrides Function ToString() As String
        Return Me.Name & " (ME:" & Me.MELevel.ToString & ", PE:" & Me.PELevel.ToString & ", Runs:" & Me.Runs.ToString & ")"
    End Function
End Class

<Serializable()> Public Class BPCCostInfo
    Public ID As String
    Public MinRunCost As Double
    Public MaxRunCost As Double

    Public Sub New(ByVal BPCID As String, ByVal BPCMinRunCost As Double, ByVal BPCMaxRunCost As Double)
        Me.ID = BPCID
        Me.MinRunCost = BPCMinRunCost
        Me.MaxRunCost = BPCMaxRunCost
    End Sub
End Class

Public Enum BPStatus As Integer
    Present = 0
    Missing = 1
    Exhausted = 2
End Enum

Public Enum BPType As Integer
    Unknown = 0
    BPO = 1
    BPC = 2
    User = 3
End Enum

Public Enum BPActivity As Integer
    Manufacturing = 1
    TimeResearch = 3
    MaterialResearch = 4
    Copying = 5
    ReverseEngineering = 7
    Invention = 8
End Enum
