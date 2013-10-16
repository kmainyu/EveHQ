Imports System.Collections.ObjectModel
Imports EveHQ.Core
Imports EveHQ.EveData
Imports System.Threading.Tasks

Namespace BPCalc

    <Serializable()> Public Class OwnedBlueprint
        Inherits EveData.Blueprint
        Public MELevel As Integer
        Public PELevel As Integer
        Public Runs As Integer

        Public Shared Sub CheckForInventionItems(ByVal originalBlueprint As OwnedBlueprint)
            ' Check if the Inventions are null
            If OriginalBlueprint.Inventions Is Nothing Then
                OriginalBlueprint.Inventions = New Collection(Of Integer)
                Dim baseBP As EveData.Blueprint = StaticData.Blueprints(OriginalBlueprint.Id)
                For Each invention As Integer In baseBP.Inventions
                    OriginalBlueprint.Inventions.Add(invention)
                Next
            End If
            ' Check if the Inventeds are null
            If OriginalBlueprint.InventFrom Is Nothing Then
                OriginalBlueprint.InventFrom = New Collection(Of Integer)
                Dim baseBP As EveData.Blueprint = StaticData.Blueprints(OriginalBlueprint.Id)
                For Each invention As Integer In baseBP.InventFrom
                    OriginalBlueprint.InventFrom.Add(invention)
                Next
            End If
        End Sub

        Public Shared Function CopyFromBlueprint(ByVal originalBlueprint As EveData.Blueprint) As OwnedBlueprint
            Dim newBP As New OwnedBlueprint
            ' Copy BP Data
            newBP.Id = originalBlueprint.Id
            newBP.ProductId = originalBlueprint.ProductId
            newBP.ProductionTime = originalBlueprint.ProductionTime
            newBP.TechLevel = originalBlueprint.TechLevel
            newBP.ResearchCopyTime = originalBlueprint.ResearchCopyTime
            newBP.ResearchMaterialLevelTime = originalBlueprint.ResearchMaterialLevelTime
            newBP.ResearchProductionLevelTime = originalBlueprint.ResearchProductionLevelTime
            newBP.ResearchTechTime = originalBlueprint.ResearchTechTime
            newBP.ProductivityModifier = originalBlueprint.ProductivityModifier
            newBP.MaterialModifier = originalBlueprint.MaterialModifier
            newBP.WasteFactor = originalBlueprint.WasteFactor
            newBP.MaxProductionLimit = originalBlueprint.MaxProductionLimit
            ' Copy resources
            For Each activity As Integer In originalBlueprint.Resources.Keys
                newBP.Resources.Add(activity, New Dictionary(Of Integer, EveData.BlueprintResource))
                For Each resource As Integer In originalBlueprint.Resources(activity).Keys
                    newBP.Resources(activity).Add(resource, originalBlueprint.Resources(activity).Item(resource))
                Next
            Next
            ' Check if the Inventions are null
            If OriginalBlueprint.Inventions Is Nothing Then
                originalBlueprint.Inventions = New Collection(Of Integer)
                Dim baseBP As EveData.Blueprint = StaticData.Blueprints(originalBlueprint.Id)
                For Each invention As Integer In baseBP.Inventions
                    originalBlueprint.Inventions.Add(invention)
                Next
            End If
            ' Copy Inventions
            For Each invention As Integer In originalBlueprint.Inventions
                newBP.Inventions.Add(invention)
            Next
            ' Check if the Inventeds are null
            If OriginalBlueprint.InventFrom Is Nothing Then
                originalBlueprint.InventFrom = New Collection(Of Integer)
                Dim baseBP As EveData.Blueprint = StaticData.Blueprints(originalBlueprint.Id)
                For Each invention As Integer In baseBP.InventFrom
                    originalBlueprint.InventFrom.Add(invention)
                Next
            End If
            ' Copy Inventeds
            For Each invention As Integer In originalBlueprint.InventFrom
                newBP.InventFrom.Add(invention)
            Next
            ' Copy meta items
            For Each metaitem As Integer In originalBlueprint.InventionMetaItems
                newBP.InventionMetaItems.Add(metaitem)
            Next
            Return newBP
        End Function

        Public Function CreateProductionJob(ByVal bpOwner As String, ByVal manufacturer As String, ByVal prodEffSkill As Integer, ByVal indSkill As Integer, ByVal prodImplantBonus As Integer, ByVal overridingMELevel As String, ByVal overridingPELevel As String, ByVal bpRuns As Integer, ByVal slotArray As EveData.AssemblyArray, ByVal componentIteration As Boolean) As Job
            ' Set up a new production job
            Dim newPj As New Job
            newPj.CurrentBlueprint = Me
            newPj.TypeID = Id
            newPj.TypeName = StaticData.Types(Id).Name
            newPj.Runs = bpRuns
            newPj.StartTime = Now
            newPj.Manufacturer = manufacturer
            newPj.BlueprintOwner = bpOwner
            newPj.PESkill = prodEffSkill
            newPj.IndSkill = indSkill
            newPj.ProdImplant = prodImplantBonus
            newPj.AssemblyArray = slotArray
            If IsNumeric(overridingMELevel) = False Then
                newPj.OverridingME = ""
            Else
                newPj.OverridingME = overridingMELevel
            End If
            If IsNumeric(overridingPELevel) = False Then
                newPj.OverridingPE = ""
            Else
                newPj.OverridingPE = overridingPELevel
            End If
            ' Get the required resources
            newPj.CalculateResourceRequirements(componentIteration, bpOwner)
            Return newPj
        End Function

        Public Sub UpdateProductionJob(ByRef job As Job)
            job.CurrentBlueprint = Me
            job.TypeID = Id
            job.TypeName = StaticData.Types(Id).Name
            job.RecalculateResourceRequirements()
        End Sub

        Public Function CalculateWasteFactor(ByVal prodEffSkill As Integer) As Double
            Return CalculateBPWasteFactor(MELevel, prodEffSkill)
        End Function

        Public Function CalculateWasteFactor(ByVal bpmeLevel As Integer, ByVal prodEffSkill As Integer) As Double
            Return CalculateBPWasteFactor(BPMELevel, prodEffSkill)
        End Function

        Private Function CalculateBPWasteFactor(ByVal bpmeLevel As Integer, ByVal prodEffSkill As Integer) As Double
            If WasteFactor <> 0 Then
                If bpmeLevel < 0 Then
                    ' This is for negative ME
                    Return ((WasteFactor / 100) * (1 - bpmeLevel)) + (0.25 - (0.05 * prodEffSkill))
                Else
                    ' This is for zero and positive ME
                    Return ((WasteFactor / 100) / (1 + bpmeLevel)) + (0.25 - (0.05 * prodEffSkill))
                End If
            End If
        End Function

        Public Function CalculateProductionTime(ByVal indSkill As Integer, ByVal prodImplantBonus As Double, ByVal productionArray As EveData.AssemblyArray, ByVal bpRuns As Integer) As Long
            Return CalculateBPProductionTime(PELevel, indSkill, prodImplantBonus, productionArray, bpRuns)
        End Function

        Public Function CalculateProductionTime(ByVal bppeLevel As Integer, ByVal indSkill As Integer, ByVal prodImplantBonus As Double, ByVal productionArray As EveData.AssemblyArray, ByVal bpRuns As Integer) As Long
            Return CalculateBPProductionTime(bppeLevel, indSkill, prodImplantBonus, productionArray, bpRuns)
        End Function

        Private Function CalculateBPProductionTime(ByVal bppeLevel As Integer, ByVal indSkill As Integer, ByVal prodImplantBonus As Double, ByVal productionArray As EveData.AssemblyArray, ByVal bpRuns As Integer) As Long
            prodImplantBonus = 1 - (prodImplantBonus / 100)
            Dim time As Double = ProductionTime * (1 - (0.04 * indSkill)) * prodImplantBonus
            ' Calculate the production time
            If bppeLevel >= 0 Then
                time *= (1 - (ProductivityModifier / ProductionTime) * bppeLevel / (1 + bppeLevel))
            Else
                time *= (1 - (ProductivityModifier / ProductionTime) * (bppeLevel - 1))
            End If
            If productionArray IsNot Nothing Then
                time *= productionArray.TimeMultiplier
            End If
            Return CLng(time * bpRuns)
        End Function

        Public Function CalculateInventionCost(ByVal metaItemId As Integer, ByVal decryptorId As Integer, ByVal bpcRuns As Integer) As Double

            Dim quantityTable As New Dictionary(Of Integer, Integer)

            ' Gather a list of resources and quantities
            For Each resource As EveData.BlueprintResource In Resources(BlueprintActivity.Invention).Values
                ' Only include datacores
                If resource.TypeGroup = 333 Then
                    Dim idKey As Integer = resource.TypeId
                    If quantityTable.ContainsKey(idKey) = False Then
                        quantityTable.Add(idKey, resource.Quantity)
                    Else
                        quantityTable(idKey) = quantityTable(idKey) + resource.Quantity
                    End If
                End If
            Next

            ' Add in the meta item id
            If metaItemId <> 0 Then
                If quantityTable.ContainsKey(metaItemId) = False Then
                    quantityTable.Add(metaItemId, 1)
                Else
                    quantityTable(metaItemId) = quantityTable(metaItemId) + 1
                End If
            End If

            ' add the decryptor to the list of items to get prices for
            If decryptorId <> 0 Then
                If quantityTable.ContainsKey(decryptorId) = False Then
                    quantityTable.Add(decryptorId, 1)
                Else
                    quantityTable(decryptorId) = quantityTable(decryptorId) + 1
                End If
            End If

            ' Total the item costs
            Dim prices As Task(Of Dictionary(Of Integer, Double)) = DataFunctions.GetMarketPrices(quantityTable.Keys)
            prices.Wait()
            Dim itemCost As Dictionary(Of Integer, Double) = prices.Result
            Dim invCost As Double = itemCost.Keys.Sum(Function(key) itemCost(key) * quantityTable(key))


            ' Calculate lab cost
            invCost += PrismSettings.UserSettings.LabInstallCost
            invCost += Math.Round(PrismSettings.UserSettings.LabRunningCost * (ResearchTechTime / 3600), 2, MidpointRounding.AwayFromZero)

            ' Calculate BPC cost
            If PrismSettings.UserSettings.BPCCosts.ContainsKey(Id) Then
                Dim pricerange As Double = PrismSettings.UserSettings.BPCCosts(Id).MaxRunCost - PrismSettings.UserSettings.BPCCosts(Id).MinRunCost
                Dim runrange As Integer = MaxProductionLimit - 1
                If runrange = 0 Then
                    invCost += PrismSettings.UserSettings.BPCCosts(Id).MinRunCost
                Else
                    invCost += PrismSettings.UserSettings.BPCCosts(Id).MinRunCost + Math.Round((pricerange / runrange) * (bpcRuns - 1), 2, MidpointRounding.AwayFromZero)
                End If
            End If

            Return invCost
        End Function

        Public Function CalculateInventedBpc(ByVal inventedBpid As Integer, ByVal decryptorID As Integer, ByVal bpcRuns As Integer) As OwnedBlueprint
            Dim ibp As OwnedBlueprint = CopyFromBlueprint(StaticData.Blueprints(inventedBpid))

            Dim ime As Integer = -4
            Dim ipe As Integer = -4
            Dim irc As Integer

            Dim decryptorName As String = StaticData.Types(decryptorID).Name

            If decryptorName <> "" Then
                If PlugInData.Decryptors.ContainsKey(decryptorName) Then
                    Dim useDecryptor As Decryptor = PlugInData.Decryptors(decryptorName)
                    ime += useDecryptor.MEMod
                    ipe += useDecryptor.PEMod
                    irc = CInt(Math.Min(Math.Max(Math.Truncate((bpcRuns / ibp.MaxProductionLimit) * (ibp.MaxProductionLimit / 10)), 1) + useDecryptor.RunMod, ibp.MaxProductionLimit))
                Else
                    irc = CInt(Math.Min(Math.Max(Math.Truncate((bpcRuns / ibp.MaxProductionLimit) * (ibp.MaxProductionLimit / 10)), 1), ibp.MaxProductionLimit))
                End If
            Else
                irc = CInt(Math.Min(Math.Max(Math.Truncate((bpcRuns / ibp.MaxProductionLimit) * (ibp.MaxProductionLimit / 10)), 1), ibp.MaxProductionLimit))
            End If

            ibp.MELevel = ime
            ibp.PELevel = ipe
            ibp.Runs = irc

            Return ibp

        End Function
    End Class
End Namespace