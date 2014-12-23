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


Imports System.Collections.ObjectModel
Imports EveHQ.Prism.Classes
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
            newBP.InventionProbability = originalBlueprint.InventionProbability
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

        Public Function CalculateWasteFactor() As Double
            Return CalculateBPWasteFactor(MELevel)
        End Function

        Public Function CalculateWasteFactor(ByVal bpmeLevel As Integer) As Double
            Return CalculateBPWasteFactor(bpmeLevel)
        End Function

        Private Function CalculateBPWasteFactor(ByVal bpmeLevel As Integer) As Double
            If WasteFactor <> 0 Then
                Return bpmeLevel / 100
            End If
        End Function

        Public Function CalculateProductionTime(ByVal indSkill As Integer, advIndSkill As Integer, ByVal prodImplantBonus As Double, ByVal productionArray As EveData.AssemblyArray, ByVal bpRuns As Integer) As Long
            Return CalculateBPProductionTime(PELevel, indSkill, advIndSkill, prodImplantBonus, productionArray, bpRuns)
        End Function

        Public Function CalculateProductionTime(ByVal bppeLevel As Integer, ByVal indSkill As Integer, advIndSkill As Integer, ByVal prodImplantBonus As Double, ByVal productionArray As EveData.AssemblyArray, ByVal bpRuns As Integer) As Long
            Return CalculateBPProductionTime(bppeLevel, indSkill, advIndSkill, prodImplantBonus, productionArray, bpRuns)
        End Function

        Private Function CalculateBPProductionTime(ByVal bppeLevel As Integer, ByVal indSkill As Integer, advIndSkill As Integer, ByVal prodImplantBonus As Double, ByVal productionArray As EveData.AssemblyArray, ByVal bpRuns As Integer) As Long
            prodImplantBonus = 1 - (prodImplantBonus / 100)
            Dim time As Double = ProductionTime * (1 - (0.04 * indSkill)) * (1 - (0.03 * advIndSkill)) * prodImplantBonus
            time = time * (1 - (bppeLevel / 100))
            If productionArray IsNot Nothing Then
                time *= productionArray.TimeMultiplier
            End If
            Return CLng(time * bpRuns)
        End Function

        Public Function CalculateInventionCost(ByVal decryptorId As Integer, ByVal bpcRuns As Integer) As Double

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