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


Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization
Imports EveHQ.Core
Imports EveHQ.EveData
Imports System.Threading.Tasks

Namespace BPCalc

    <Serializable()>
    Public Class Job
        Public Property JobName As String
        Public Property CurrentBlueprint As OwnedBlueprint
        Public Property BlueprintId As Integer
        Public Property TypeID As Integer
        Public Property TypeName As String
        Public Property PerfectUnits As Double
        Public Property WasteUnits As Double
        Public Property Runs As Integer
        Public Property Manufacturer As String
        Public Property BlueprintOwner As String
        Public Property PESkill As Integer
        Public Property IndSkill As Integer
        Public Property ProdImplant As Integer
        Public Property OverridingME As String
        Public Property OverridingPE As String
        Public Property AssemblyArray As EveData.AssemblyArray
        Public Property StartTime As Date
        Public Property RunTime As Long
        Public Property Cost As Double
        Public Property Resources As New SortedList(Of Integer, JobResource)
        Public Property SubJobs As New SortedList(Of Integer, Job)
        Public Property HasInventionJob As Boolean
        Public Property InventionJob As New InventionJob
        Public Property SubJobMEs As New SortedList(Of Integer, Integer)
        Public Property ProduceSubJob As Boolean = False

        Public Function Clone() As Job
            Using cloneMemoryStream As New MemoryStream
                Dim objBinaryFormatter As New BinaryFormatter(Nothing, New StreamingContext(StreamingContextStates.Clone))
                objBinaryFormatter.Serialize(cloneMemoryStream, Me)
                cloneMemoryStream.Seek(0, SeekOrigin.Begin)
                Return CType(objBinaryFormatter.Deserialize(cloneMemoryStream), Job)
            End Using
        End Function

        Public Sub CalculateResourceRequirements(ByVal componentIteration As Boolean, owner As String)

            Dim matMod As Double = 1
            If AssemblyArray IsNot Nothing Then
                matMod = AssemblyArray.MaterialMultiplier
            End If

            If OverridingPE = "" Then
                RunTime = CurrentBlueprint.CalculateProductionTime(IndSkill, ProdImplant, AssemblyArray, Runs)
            Else
                RunTime = CurrentBlueprint.CalculateProductionTime(CInt(OverridingPE), IndSkill, ProdImplant, AssemblyArray, Runs)
            End If

            Dim wasteFactor As Double
            If OverridingME = "" Then
                wasteFactor = CurrentBlueprint.CalculateWasteFactor(PESkill)
            Else
                wasteFactor = CurrentBlueprint.CalculateWasteFactor(CInt(OverridingME), PESkill)
            End If

            If SubJobMEs Is Nothing Then
                SubJobMEs = New SortedList(Of Integer, Integer)
            End If

            Resources.Clear()
            SubJobs.Clear()
            If CurrentBlueprint.Resources.ContainsKey(BlueprintActivity.Manufacturing) Then
                For Each resource As EveData.BlueprintResource In _
                    CurrentBlueprint.Resources(BlueprintActivity.Manufacturing).Values
                    If resource.Quantity >= 0 Then
                        Dim subBP As BlueprintAsset = Nothing

                        ' Calculate the current perfect and waste resources
                        Dim waste As Integer
                        Dim perfectRaw As Integer = resource.Quantity

                        ' Calculate Waste - Mark II!
                        waste = CalculateWasteUnits(resource, wasteFactor, matMod)

                        ' Check if we have component iteration active
                        If componentIteration = True Then
                            ' Check for a blueprint
                            If PlugInData.Products.ContainsKey(resource.TypeId.ToString) = True Then
                                Dim bpid As String = PlugInData.Products(resource.TypeId.ToString)
                                ' Check if we need to use owned BPs
                                If owner <> "" Then
                                    ' Use owned BPs
                                    If PlugInData.BlueprintAssets.ContainsKey(BlueprintOwner) = True Then
                                        For Each bpAsset As BlueprintAsset In _
                                            PlugInData.BlueprintAssets(BlueprintOwner).Values
                                            If bpAsset.TypeID = bpid Then
                                                If subBP IsNot Nothing Then
                                                    If bpAsset.MELevel >= subBP.MELevel Then
                                                        subBP = bpAsset
                                                    End If
                                                Else
                                                    subBP = bpAsset
                                                End If
                                            End If
                                        Next
                                    End If
                                Else
                                    ' Use any BPs
                                    subBP = New BlueprintAsset
                                    subBP.TypeID = bpid
                                    subBP.MELevel = 0
                                    subBP.PELevel = 0
                                    subBP.Runs = -1
                                End If
                            End If
                        End If

                        ' See if we need to examine the BP
                        If componentIteration = True And subBP IsNot Nothing Then
                            ' Convert the BP to a BlueprintSelection ready for processing
                            Dim subBps As OwnedBlueprint =
                                    OwnedBlueprint.CopyFromBlueprint(StaticData.Blueprints(CInt(subBP.TypeID)))
                            subBps.MELevel = subBP.MELevel
                            subBps.PELevel = subBP.PELevel
                            subBps.Runs = subBP.Runs
                            ' Create a new production job
                            Dim subJob As New Job
                            subJob.CurrentBlueprint = subBps
                            subJob.TypeID = resource.TypeId
                            subJob.TypeName = StaticData.Types(resource.TypeId).Name
                            subJob.PerfectUnits = perfectRaw
                            subJob.WasteUnits = waste
                            subJob.Runs = (perfectRaw + waste) * Runs
                            subJob.Manufacturer = Manufacturer
                            subJob.BlueprintOwner = BlueprintOwner
                            subJob.PESkill = PESkill
                            subJob.IndSkill = IndSkill
                            subJob.ProdImplant = ProdImplant
                            subJob.AssemblyArray = AssemblyArray
                            subJob.StartTime = Now
                            subJob.ProduceSubJob = True
                            ' Set SubJob ME
                            If SubJobMEs.ContainsKey(resource.TypeId) = False Then
                                SubJobMEs.Add(resource.TypeId, subBps.MELevel)
                            Else
                                SubJobMEs(resource.TypeId) = subBps.MELevel
                            End If
                            ' Do the iteration on the component BP
                            subJob.CalculateResourceRequirements(componentIteration, owner)
                            SubJobs.Add(resource.TypeId, subJob)
                        Else
                            Dim newResource As New JobResource
                            newResource.TypeID = resource.TypeId
                            newResource.TypeName = StaticData.Types(resource.TypeId).Name
                            newResource.TypeGroup = resource.TypeGroup
                            newResource.TypeCategory = resource.TypeCategory
                            newResource.PerfectUnits = perfectRaw
                            newResource.WasteUnits = waste
                            newResource.BaseUnits = resource.BaseMaterial
                            Resources.Add(resource.TypeId, newResource)
                        End If
                    End If
                Next
            End If
            Cost = CalculateCost()
        End Sub

        Public Function CalculateCost() As Double
            Dim costs As Double = 0

            'Get the ID's of the required resources
            Dim resourceList As IEnumerable(Of JobResource) = Resources.Values.Where(Function(value) value.GetType.Name = GetType(JobResource).Name).Select(Function(value) value).Where(Function(r) r.TypeCategory <> 16)
            Dim subJobList As IEnumerable(Of Job) = SubJobs.Values.Where(Function(value) value.GetType.Name = GetType(Job).Name).Select(Function(v) v)

            'Get the prices for the resource
            Dim enumerable As IEnumerable(Of JobResource) = If(TryCast(resourceList, JobResource()), resourceList.ToArray())
            Dim prices As Task(Of Dictionary(Of Integer, Double)) = DataFunctions.GetMarketPrices(enumerable.Select(Function(r) r.TypeID))
            prices.Wait()
            Dim resourceCost As Dictionary(Of Integer, Double) = prices.Result
            costs += enumerable.Select(Function(r) ((r.PerfectUnits + r.WasteUnits) * resourceCost(r.TypeID)) * Runs).Sum()

            ' Add in the costs for the sub jobs
            costs += subJobList.Sum(Function(j) j.CalculateCost)

            Return costs
        End Function

        Public Sub ReplaceJobWithResource(ByVal itemID As Integer)

            If SubJobs.ContainsKey(itemID) Then

                If CurrentBlueprint.Resources(BlueprintActivity.Manufacturing).ContainsKey(itemID) = True Then

                    Dim matMod As Double = 1
                    If AssemblyArray IsNot Nothing Then
                        matMod = AssemblyArray.MaterialMultiplier
                    End If

                    If OverridingPE = "" Then
                        RunTime = CurrentBlueprint.CalculateProductionTime(IndSkill, ProdImplant, AssemblyArray, Runs)
                    Else
                        RunTime = CurrentBlueprint.CalculateProductionTime(CInt(OverridingPE), IndSkill, ProdImplant, AssemblyArray, Runs)
                    End If

                    Dim bpwf As Double
                    If OverridingME = "" Then
                        bpwf = Math.Round(CurrentBlueprint.CalculateWasteFactor(PESkill), 6, MidpointRounding.AwayFromZero)
                    Else
                        bpwf = Math.Round(CurrentBlueprint.CalculateWasteFactor(CInt(OverridingME), PESkill), 6)
                    End If

                    Dim resource As EveData.BlueprintResource = CurrentBlueprint.Resources(BlueprintActivity.Manufacturing).Item(itemID)
                    ' Calculate the current perfect and waste resources
                    Dim waste As Integer
                    Dim perfectRaw As Integer = resource.Quantity

                    ' Calculate Waste - Mark II!
                    waste = CalculateWasteUnits(resource, bpwf, matMod)

                    ' Remove the existing production job
                    SubJobs.Remove(itemID)

                    ' Add the new resource
                    Dim newResource As New JobResource
                    newResource.TypeID = resource.TypeId
                    newResource.TypeName = StaticData.Types(resource.TypeId).Name
                    newResource.TypeGroup = resource.TypeGroup
                    newResource.TypeCategory = resource.TypeCategory
                    newResource.PerfectUnits = perfectRaw
                    newResource.WasteUnits = waste
                    newResource.BaseUnits = resource.BaseMaterial
                    Resources.Add(resource.TypeId, newResource)

                    Cost = CalculateCost()

                End If

            End If

        End Sub

        Public Sub ReplaceResourceWithJob(ByVal itemID As Integer)

            If Resources.ContainsKey(itemID) Then

                If CurrentBlueprint.Resources(BlueprintActivity.Manufacturing).ContainsKey(itemID) = True Then

                    Dim matMod As Double = 1
                    If AssemblyArray IsNot Nothing Then
                        matMod = AssemblyArray.MaterialMultiplier
                    End If

                    If OverridingPE = "" Then
                        RunTime = CurrentBlueprint.CalculateProductionTime(IndSkill, ProdImplant, AssemblyArray, Runs)
                    Else
                        RunTime = CurrentBlueprint.CalculateProductionTime(CInt(OverridingPE), IndSkill, ProdImplant, AssemblyArray, Runs)
                    End If

                    Dim bpwf As Double
                    If OverridingME = "" Then
                        bpwf = Math.Round(CurrentBlueprint.CalculateWasteFactor(PESkill), 6, MidpointRounding.AwayFromZero)
                    Else
                        bpwf = Math.Round(CurrentBlueprint.CalculateWasteFactor(CInt(OverridingME), PESkill), 6, MidpointRounding.AwayFromZero)
                    End If

                    Dim resource As EveData.BlueprintResource = CurrentBlueprint.Resources(BlueprintActivity.Manufacturing).Item(itemID)
                    ' Calculate the current perfect and waste resources
                    Dim waste As Integer
                    Dim perfectRaw As Integer = resource.Quantity

                    ' Calculate Waste - Mark II!
                    waste = CalculateWasteUnits(resource, bpwf, matMod)

                    ' Remove the resource
                    Resources.Remove(itemID)

                    Dim bpsID As String = PlugInData.Products(itemID.ToString)
                    Dim subBps As OwnedBlueprint = OwnedBlueprint.CopyFromBlueprint(StaticData.Blueprints(CInt(bpsID)))

                    If SubJobMEs.ContainsKey(resource.TypeId) = True Then
                        subBps.MELevel = SubJobMEs(resource.TypeId)
                    End If

                    subBps.PELevel = 0
                    subBps.Runs = -1
                    subBps.CalculateWasteFactor(PESkill)

                    ' Create a new production job
                    Dim subJob As New Job
                    subJob.CurrentBlueprint = subBps
                    subJob.TypeID = resource.TypeId
                    subJob.TypeName = StaticData.Types(resource.TypeId).Name
                    subJob.PerfectUnits = perfectRaw
                    subJob.WasteUnits = waste
                    subJob.Runs = (perfectRaw + waste) * Runs
                    subJob.Manufacturer = Manufacturer
                    subJob.BlueprintOwner = BlueprintOwner
                    subJob.PESkill = PESkill
                    subJob.IndSkill = IndSkill
                    subJob.ProdImplant = ProdImplant
                    subJob.AssemblyArray = AssemblyArray
                    subJob.StartTime = Now

                    ' Do the iteration on the component BP
                    subJob.CalculateResourceRequirements(False, BlueprintOwner)

                    SubJobs.Add(resource.TypeId, subJob)

                    Cost = CalculateCost()

                End If

            End If

        End Sub

        Public Sub RecalculateResourceRequirements()

            If SubJobMEs Is Nothing Then
                SubJobMEs = New SortedList(Of Integer, Integer)
            End If

            Dim matMod As Double = 1
            If AssemblyArray IsNot Nothing Then
                matMod = AssemblyArray.MaterialMultiplier
            End If

            If OverridingPE = "" Then
                RunTime = CurrentBlueprint.CalculateProductionTime(IndSkill, ProdImplant, AssemblyArray, Runs)
            Else
                RunTime = CurrentBlueprint.CalculateProductionTime(CInt(OverridingPE), IndSkill, ProdImplant, AssemblyArray, Runs)
            End If

            Dim wasteFactor As Double
            If OverridingME = "" Then
                wasteFactor = CurrentBlueprint.CalculateWasteFactor(PESkill)
            Else
                wasteFactor = CurrentBlueprint.CalculateWasteFactor(CInt(OverridingME), PESkill)
            End If

            Dim waste As Integer

            For Each resource As JobResource In Resources.Values
                ' Get the resource
                Dim newResource As JobResource = resource
                Dim bpResource As EveData.BlueprintResource = CurrentBlueprint.Resources(BlueprintActivity.Manufacturing).Item(newResource.TypeID)
                ' Calculate Waste - Mark II!
                waste = CalculateWasteUnits(bpResource, wasteFactor, matMod)
                newResource.WasteUnits = waste
            Next
            For Each subJob As Job In SubJobs.Values
                ' Get the production job
                Dim bpResource As EveData.BlueprintResource = CurrentBlueprint.Resources(BlueprintActivity.Manufacturing).Item(subJob.TypeID)
                ' Set SubJob ME
                If SubJobMEs.ContainsKey(bpResource.TypeId) = False Then
                    SubJobMEs.Add(bpResource.TypeId, subJob.CurrentBlueprint.MELevel)
                Else
                    SubJobMEs(bpResource.TypeId) = subJob.CurrentBlueprint.MELevel
                End If
                ' Calculate Waste - Mark II!
                waste = CalculateWasteUnits(bpResource, wasteFactor, matMod)
                subJob.WasteUnits = waste
                subJob.Runs = CInt((subJob.PerfectUnits + waste) * Runs)
                subJob.RecalculateResourceRequirements()
            Next

            Cost = CalculateCost()

        End Sub

        Public Function CalculateWasteUnits(resource As EveData.BlueprintResource, wasteFactor As Double, matMod As Double) As Integer
             Dim waste As Integer
            ' Calculate Waste - Mark II!
            waste = CInt(Math.Round((wasteFactor * resource.BaseMaterial) + (resource.BaseMaterial * (matMod - 1)), 0, MidpointRounding.AwayFromZero))
            ' Provisional adjustment for "extra" mats
            Dim extraWaste As Integer = CInt(Math.Round(((resource.Quantity - resource.BaseMaterial) * (1.25 - (0.05 * PESkill))) - (resource.Quantity - resource.BaseMaterial), 0, MidpointRounding.AwayFromZero))
            waste += extraWaste
            Return waste
        End Function

        Public Sub UpdateManufacturer(ByVal pilotName As String)
            Manufacturer = PilotName
            For Each subjob As Job In SubJobs.Values
                ' Update the manufacturer for sub-jobs
                subjob.UpdateManufacturer(pilotName)
            Next
        End Sub

        Public Sub UpdateJobSkills(ByVal newPESkill As Integer, ByVal newIndSkill As Integer, ByVal newProdImplant As Integer)
            PESkill = newPESkill
            IndSkill = newIndSkill
            ProdImplant = newProdImplant
            For Each subjob As Job In SubJobs.Values
                ' Update the skill for sub-jobs
                subJob.UpdateJobSkills(newPESkill, newIndSkill, newProdImplant)
            Next
        End Sub

        Public Sub UpdateJobPESkill(ByVal newPESkill As Integer)
            PESkill = NewPESkill
            For Each subjob As Job In SubJobs.Values
                ' Update the skill for sub-jobs
                subJob.UpdateJobPESkill(newPESkill)
            Next
        End Sub

        Public Sub UpdateJobIndSkill(ByVal newIndSkill As Integer)
            IndSkill = NewIndSkill
            For Each subjob As Job In SubJobs.Values
                ' Update the skill for sub-jobs
                subJob.UpdateJobIndSkill(newIndSkill)
            Next
        End Sub

        Public Sub UpdateJobProdImplant(ByVal newProdImplant As Integer)
            ProdImplant = NewProdImplant
            For Each subjob As Job In SubJobs.Values
                ' Update the implant for sub-jobs
                subJob.UpdateJobProdImplant(newProdImplant)
            Next
        End Sub
    End Class
End Namespace