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
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
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
        Public Property RequiredResources As New SortedList(Of String, Object)
        Public Property HasInventionJob As Boolean
        Public Property InventionJob As New InventionJob
        Public Property SubJobMEs As New SortedList(Of String, Integer)
        Public Property ProduceSubJob As Boolean = False

        Public Function Clone() As Job
            Dim cloneMemoryStream As New MemoryStream
            Dim objBinaryFormatter As New BinaryFormatter(Nothing, New StreamingContext(StreamingContextStates.Clone))
            objBinaryFormatter.Serialize(cloneMemoryStream, Me)
            cloneMemoryStream.Seek(0, SeekOrigin.Begin)
            Dim newJob As Job = CType(objBinaryFormatter.Deserialize(cloneMemoryStream), Job)
            cloneMemoryStream.Close()
            cloneMemoryStream.Dispose()
            Return newJob
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
                SubJobMEs = New SortedList(Of String, Integer)
            End If

            Dim reqResources As New SortedList(Of String, Object)
            For Each resource As EveData.BlueprintResource In CurrentBlueprint.Resources(EveData.BlueprintActivity.Manufacturing).Values
                If resource.Activity = 1 And resource.Quantity >= 0 Then
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
                                    For Each bpAsset As BlueprintAsset In PlugInData.BlueprintAssets(BlueprintOwner).Values
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
                        Dim subBps As OwnedBlueprint = OwnedBlueprint.CopyFromBlueprint(EveData.StaticData.Blueprints(CInt(subBP.TypeID)))
                        subBps.MELevel = subBP.MELevel
                        subBps.PELevel = subBP.PELevel
                        subBps.Runs = subBP.Runs
                        ' Create a new production job
                        Dim subJob As New Job
                        subJob.CurrentBlueprint = subBps
                        subJob.TypeID = resource.TypeId
                        subJob.TypeName = EveData.StaticData.Types(resource.TypeId.ToString).Name
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
                        If SubJobMEs.ContainsKey(resource.TypeId.ToString) = False Then
                            SubJobMEs.Add(resource.TypeId.ToString, subBps.MELevel)
                        Else
                            SubJobMEs(resource.TypeId.ToString) = subBps.MELevel
                        End If
                        ' Do the iteration on the component BP
                        subJob.CalculateResourceRequirements(componentIteration, owner)
                        reqResources.Add(resource.TypeId.ToString, subJob)
                    Else
                        Dim newResource As New JobResource
                        newResource.TypeID = resource.TypeId
                        newResource.TypeName = EveData.StaticData.Types(resource.TypeId.ToString).Name
                        newResource.TypeGroup = resource.TypeGroup
                        newResource.TypeCategory = resource.TypeCategory
                        newResource.PerfectUnits = perfectRaw
                        newResource.WasteUnits = waste
                        newResource.BaseUnits = resource.BaseMaterial
                        reqResources.Add(resource.TypeId.ToString, newResource)
                    End If
                End If
            Next
            RequiredResources = reqResources
            Cost = CalculateCost()
        End Sub

        Private Function CalculateCost() As Double
            Dim costs As Double = 0

            'Get the ID's of the required resources
            Dim resources As IEnumerable(Of JobResource) = RequiredResources.Values.Where(Function(value) value.GetType.Name = GetType(JobResource).Name).Select(Function(value) CType(value, JobResource)).Where(Function(r) r.TypeCategory <> 16)
            Dim subJobs As IEnumerable(Of Job) = RequiredResources.Values.Where(Function(value) value.GetType.Name = GetType(Job).Name).Select(Function(v) CType(v, Job))

            'Get the prices for the resource
            Dim enumerable As IEnumerable(Of JobResource) = If(TryCast(resources, JobResource()), resources.ToArray())
            Dim prices As Task(Of Dictionary(Of String, Double)) = Core.DataFunctions.GetMarketPrices(enumerable.Select(Function(r) r.TypeID.ToString))
            prices.Wait()
            Dim resourceCost As Dictionary(Of String, Double) = prices.Result
            costs += enumerable.Select(Function(r) ((r.PerfectUnits + r.WasteUnits) * resourceCost(r.TypeID.ToString())) * Runs).Sum()

            ' Add in the costs for the sub jobs
            costs += subJobs.Sum(Function(j) j.CalculateCost)

            Return costs
        End Function

       Public Sub ReplaceJobWithResource(ByVal itemID As Integer)

            If CurrentBlueprint.Resources(EveData.BlueprintActivity.Manufacturing).ContainsKey(itemID) = True Then

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

                Dim resource As EveData.BlueprintResource = CurrentBlueprint.Resources(EveData.BlueprintActivity.Manufacturing).Item(itemID)
                ' Calculate the current perfect and waste resources
                Dim waste As Integer
                Dim perfectRaw As Integer = resource.Quantity

                ' Calculate Waste - Mark II!
                waste = CalculateWasteUnits(resource, bpwf, matMod)

                ' Remove the existing production job
                RequiredResources.Remove(itemID.ToString)

                ' Add the new resource
                Dim newResource As New JobResource
                newResource.TypeID = resource.TypeId
                newResource.TypeName = EveData.StaticData.Types(resource.TypeId.ToString).Name
                newResource.TypeGroup = resource.TypeGroup
                newResource.TypeCategory = resource.TypeCategory
                newResource.PerfectUnits = perfectRaw
                newResource.WasteUnits = waste
                newResource.BaseUnits = resource.BaseMaterial
                RequiredResources.Add(resource.TypeId.ToString, newResource)

                Cost = CalculateCost()

            End If

        End Sub

        Public Sub ReplaceResourceWithJob(ByVal itemID As Integer)

            If CurrentBlueprint.Resources(EveData.BlueprintActivity.Manufacturing).ContainsKey(itemID) = True Then

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

                Dim resource As EveData.BlueprintResource = CurrentBlueprint.Resources(EveData.BlueprintActivity.Manufacturing).Item(itemID)
                ' Calculate the current perfect and waste resources
                Dim waste As Integer
                Dim perfectRaw As Integer = resource.Quantity

                ' Calculate Waste - Mark II!
                waste = CalculateWasteUnits(resource, bpwf, matMod)

                ' Remove the resource
                RequiredResources.Remove(itemID.ToString)

                Dim bpsID As String = PlugInData.Products(itemID.ToString)
                Dim subBps As OwnedBlueprint = OwnedBlueprint.CopyFromBlueprint(EveData.StaticData.Blueprints(CInt(bpsID)))

                If SubJobMEs.ContainsKey(resource.TypeID.ToString) = True Then
                    subBps.MELevel = SubJobMEs(resource.TypeID.ToString)
                End If

                subBps.PELevel = 0
                subBps.Runs = -1
                subBps.CalculateWasteFactor(PESkill)

                ' Create a new production job
                Dim subJob As New Job
                subJob.CurrentBlueprint = subBps
                subJob.TypeID = resource.TypeID
                subJob.TypeName = EveData.StaticData.Types(resource.TypeId.ToString).Name
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

                RequiredResources.Add(resource.TypeID.ToString, subJob)

                Cost = CalculateCost()

            End If
        End Sub

        Public Sub RecalculateResourceRequirements()

            If SubJobMEs Is Nothing Then
                SubJobMEs = New SortedList(Of String, Integer)
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

            For Each resource As Object In RequiredResources.Values
                If TypeOf (resource) Is JobResource Then
                    ' Get the resource
                    Dim newResource As JobResource = CType(resource, JobResource)
                    Dim bpResource As EveData.BlueprintResource = CurrentBlueprint.Resources(EveData.BlueprintActivity.Manufacturing).Item(newResource.TypeID)
                    ' Calculate Waste - Mark II!
                    waste = CalculateWasteUnits(bpResource, wasteFactor, matMod)
                    newResource.WasteUnits = waste
                Else
                    ' Get the production job
                    Dim subJob As Job = CType(resource, Job)
                    Dim bpResource As EveData.BlueprintResource = CurrentBlueprint.Resources(EveData.BlueprintActivity.Manufacturing).Item(subJob.TypeID)
                    ' Set SubJob ME
                    If SubJobMEs.ContainsKey(bpResource.TypeId.ToString) = False Then
                        SubJobMEs.Add(bpResource.TypeId.ToString, subJob.CurrentBlueprint.MELevel)
                    Else
                        SubJobMEs(bpResource.TypeId.ToString) = subJob.CurrentBlueprint.MELevel
                    End If
                    ' Calculate Waste - Mark II!
                    waste = CalculateWasteUnits(bpResource, wasteFactor, matMod)
                    subJob.WasteUnits = waste
                    subJob.Runs = CInt((subJob.PerfectUnits + waste) * Runs)
                    subJob.RecalculateResourceRequirements()
                End If
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
            For Each resource As Object In RequiredResources.Values
                If TypeOf (resource) Is Job Then
                    ' Get the production job
                    Dim subJob As Job = CType(resource, Job)
                    subJob.UpdateManufacturer(PilotName)
                End If
            Next
        End Sub

        Public Sub UpdateJobSkills(ByVal newPESkill As Integer, ByVal newIndSkill As Integer, ByVal newProdImplant As Integer)
            PESkill = newPESkill
            IndSkill = newIndSkill
            ProdImplant = newProdImplant
            For Each resource As Object In RequiredResources.Values
                If TypeOf (resource) Is Job Then
                    ' Get the production job
                    Dim subJob As Job = CType(resource, Job)
                    subJob.UpdateJobSkills(newPESkill, newIndSkill, newProdImplant)
                End If
            Next
        End Sub

        Public Sub UpdateJobPESkill(ByVal newPESkill As Integer)
            PESkill = NewPESkill
            For Each resource As Object In RequiredResources.Values
                If TypeOf (resource) Is Job Then
                    ' Get the production job
                    Dim subJob As Job = CType(resource, Job)
                    subJob.UpdateJobPESkill(NewPESkill)
                End If
            Next
        End Sub

        Public Sub UpdateJobIndSkill(ByVal newIndSkill As Integer)
            IndSkill = NewIndSkill
            For Each resource As Object In RequiredResources.Values
                If TypeOf (resource) Is Job Then
                    ' Get the production job
                    Dim subJob As Job = CType(resource, Job)
                    subJob.UpdateJobIndSkill(NewIndSkill)
                End If
            Next
        End Sub

        Public Sub UpdateJobProdImplant(ByVal newProdImplant As Integer)
            ProdImplant = NewProdImplant
            For Each resource As Object In RequiredResources.Values
                If TypeOf (resource) Is Job Then
                    ' Get the production job
                    Dim subJob As Job = CType(resource, Job)
                    subJob.UpdateJobProdImplant(NewProdImplant)
                End If
            Next
        End Sub
    End Class
End Namespace