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
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization

<Serializable()> Public Class ProductionJob
    Public JobName As String
    Public CurrentBP As BlueprintSelection
    Public BPID As Integer
    Public TypeID As Integer
    Public TypeName As String
    Public PerfectUnits As Double
    Public WasteUnits As Double
    Public Runs As Integer
    Public Manufacturer As String
    Public BPOwner As String
    Public PESkill As Integer
    Public IndSkill As Integer
    Public ProdImplant As Integer
    Public OverridingME As String
    Public OverridingPE As String
    Public AssemblyArray As AssemblyArray
    Public StartTime As Date
    Public RunTime As Long
    Public Cost As Double
    Public RequiredResources As New SortedList(Of String, Object)
    Public HasInventionJob As Boolean
    Public InventionJob As New InventionJob
    Public SubJobMEs As New SortedList(Of String, Integer)


    Public Function Clone() As ProductionJob
        Dim CloneMemoryStream As New MemoryStream
        Dim objBinaryFormatter As New BinaryFormatter(Nothing, New StreamingContext(StreamingContextStates.Clone))
        objBinaryFormatter.Serialize(CloneMemoryStream, Me)
        CloneMemoryStream.Seek(0, SeekOrigin.Begin)
        Dim newJob As ProductionJob = CType(objBinaryFormatter.Deserialize(CloneMemoryStream), ProductionJob)
        CloneMemoryStream.Close()
        CloneMemoryStream.Dispose()
        Return newJob
    End Function

    Public Sub CalculateResourceRequirements(ByVal ComponentIteration As Boolean, BPOwner As String)

        Dim MatMod As Double = 1
        If Me.AssemblyArray IsNot Nothing Then
            MatMod = Me.AssemblyArray.MaterialMultiplier
        End If

        If Me.OverridingPE = "" Then
            Me.RunTime = CurrentBP.CalculateProductionTime(IndSkill, ProdImplant, AssemblyArray, Me.Runs)
        Else
            Me.RunTime = CurrentBP.CalculateProductionTime(CInt(Me.OverridingPE), IndSkill, ProdImplant, AssemblyArray, Me.Runs)
        End If

        Dim BPWF As Double = CurrentBP.WasteFactor
        If Me.OverridingME = "" Then
            BPWF = CurrentBP.CalculateWasteFactor(Me.PESkill)
        Else
            BPWF = CurrentBP.CalculateWasteFactor(CInt(Me.OverridingME), Me.PESkill)
        End If

        If Me.SubJobMEs Is Nothing Then
            Me.SubJobMEs = New SortedList(Of String, Integer)
        End If

        Dim ReqdResources As New SortedList(Of String, Object)
        For Each resource As BlueprintResource In CurrentBP.Resources.Values
            If resource.Activity = 1 And resource.Quantity >= 0 Then
                Dim subBP As BlueprintAsset = Nothing

                ' Calculate the current perfect and waste resources
                Dim waste As Integer = 0
                Dim perfectRaw As Integer = resource.Quantity

                ' Calculate Waste - Mark II!
                waste = CalculateWasteUnits(resource, BPWF, MatMod)

                ' Check if we have component iteration active
                If ComponentIteration = True Then
                    ' Check for a blueprint
                    If PlugInData.Products.ContainsKey(resource.TypeID.ToString) = True Then
                        Dim BPID As String = PlugInData.Products(resource.TypeID.ToString)
                        ' Check if we need to use owned BPs
                        If BPOwner <> "" Then
                            ' Use owned BPs
                            If PlugInData.BlueprintAssets.ContainsKey(Me.BPOwner) = True Then
                                For Each BPAsset As BlueprintAsset In PlugInData.BlueprintAssets(Me.BPOwner).Values
                                    If BPAsset.TypeID = BPID Then
                                        If subBP IsNot Nothing Then
                                            If BPAsset.MELevel >= subBP.MELevel Then
                                                subBP = BPAsset
                                            End If
                                        Else
                                            subBP = BPAsset
                                        End If
                                    End If
                                Next
                            End If
                        Else
                            ' Use any BPs
                            subBP = New BlueprintAsset
                            subBP.TypeID = BPID
                            subBP.MELevel = 0
                            subBP.PELevel = 0
                            subBP.Runs = -1
                        End If
                    End If
                End If

                ' See if we need to examine the BP
                If ComponentIteration = True And subBP IsNot Nothing Then
                    ' Convert the BP to a BlueprintSelection ready for processing
                    Dim subBPS As BlueprintSelection = BlueprintSelection.CopyFromBlueprint(PlugInData.Blueprints(subBP.TypeID))
                    subBPS.MELevel = subBP.MELevel
                    subBPS.PELevel = subBP.PELevel
                    subBPS.Runs = subBP.Runs
                    Dim subBPWF As Double = subBPS.CalculateWasteFactor(Me.PESkill)
                    ' Create a new production job
                    Dim subPJ As New ProductionJob
                    subPJ.CurrentBP = subBPS
                    subPJ.TypeID = resource.TypeID
                    subPJ.TypeName = resource.TypeName
                    subPJ.PerfectUnits = perfectRaw
                    subPJ.WasteUnits = waste
                    subPJ.Runs = (perfectRaw + waste) * Runs
                    subPJ.Manufacturer = Manufacturer
                    subPJ.BPOwner = Me.BPOwner
                    subPJ.PESkill = Me.PESkill
                    subPJ.IndSkill = Me.IndSkill
                    subPJ.ProdImplant = Me.ProdImplant
                    subPJ.AssemblyArray = Me.AssemblyArray
                    subPJ.StartTime = Now
                    ' Set SubJob ME
                    If Me.SubJobMEs.ContainsKey(resource.TypeID.ToString) = False Then
                        Me.SubJobMEs.Add(resource.TypeID.ToString, subBPS.MELevel)
                    Else
                        Me.SubJobMEs(resource.TypeID.ToString) = subBPS.MELevel
                    End If
                    ' Do the iteration on the component BP
                    subPJ.CalculateResourceRequirements(ComponentIteration, BPOwner)
                    ReqdResources.Add(resource.TypeID.ToString, subPJ)
                Else
                    Dim newResource As New RequiredResource
                    newResource.TypeID = resource.TypeID
                    newResource.TypeName = resource.TypeName
                    newResource.TypeGroup = resource.TypeGroup
                    newResource.TypeCategory = resource.TypeCategory
                    newResource.PerfectUnits = perfectRaw
                    newResource.WasteUnits = waste
                    newResource.BaseUnits = resource.BaseMaterial
                    ReqdResources.Add(resource.TypeID.ToString, newResource)
                End If
            End If
        Next
        Me.RequiredResources = ReqdResources
        Me.Cost = Me.CalculateCost()
    End Sub

    Private Function CalculateCost() As Double
        Dim cost As Double = 0
        For Each resource As Object In Me.RequiredResources.Values
            If TypeOf (resource) Is RequiredResource Then
                Dim rResource As RequiredResource = CType(resource, RequiredResource)
                If rResource.TypeCategory <> 16 Then
                    cost += (rResource.PerfectUnits + rResource.WasteUnits) * EveHQ.Core.DataFunctions.GetPrice(CStr(rResource.TypeID)) * Me.Runs
                End If
            Else
                Dim subJob As ProductionJob = CType(resource, ProductionJob)
                cost += subJob.CalculateCost
            End If
        Next
        Return cost
    End Function

    Public Sub ReplaceJobWithResource(ByVal TypeID As String)

        Dim key As String = TypeID & "_1"
        If CurrentBP.Resources.ContainsKey(key) = True Then

            Dim MatMod As Double = 1
            If Me.AssemblyArray IsNot Nothing Then
                MatMod = Me.AssemblyArray.MaterialMultiplier
            End If

            If Me.OverridingPE = "" Then
                Me.RunTime = CurrentBP.CalculateProductionTime(IndSkill, ProdImplant, AssemblyArray, Me.Runs)
            Else
                Me.RunTime = CurrentBP.CalculateProductionTime(CInt(Me.OverridingPE), IndSkill, ProdImplant, AssemblyArray, Me.Runs)
            End If

            Dim BPWF As Double = CurrentBP.WasteFactor
            If Me.OverridingME = "" Then
                BPWF = CurrentBP.CalculateWasteFactor(Me.PESkill)
            Else
                BPWF = CurrentBP.CalculateWasteFactor(CInt(Me.OverridingME), Me.PESkill)
            End If

            Dim resource As BlueprintResource = CurrentBP.Resources(key)
            ' Calculate the current perfect and waste resources
            Dim waste As Integer = 0
            Dim perfectRaw As Integer = resource.Quantity

            ' Calculate Waste - Mark II!
            waste = CalculateWasteUnits(resource, BPWF, MatMod)

            ' Remove the existing production job
            Me.RequiredResources.Remove(TypeID.ToString)

            ' Add the new resource
            Dim newResource As New RequiredResource
            newResource.TypeID = resource.TypeID
            newResource.TypeName = resource.TypeName
            newResource.TypeGroup = resource.TypeGroup
            newResource.TypeCategory = resource.TypeCategory
            newResource.PerfectUnits = perfectRaw
            newResource.WasteUnits = waste
            newResource.BaseUnits = resource.BaseMaterial
            Me.RequiredResources.Add(resource.TypeID.ToString, newResource)

            Me.Cost = Me.CalculateCost()

        End If

    End Sub

    Public Sub ReplaceResourceWithJob(ByVal TypeID As String)

        Dim key As String = TypeID & "_1"
        If CurrentBP.Resources.ContainsKey(key) = True Then

            Dim MatMod As Double = 1
            If Me.AssemblyArray IsNot Nothing Then
                MatMod = Me.AssemblyArray.MaterialMultiplier
            End If

            If Me.OverridingPE = "" Then
                Me.RunTime = CurrentBP.CalculateProductionTime(IndSkill, ProdImplant, AssemblyArray, Me.Runs)
            Else
                Me.RunTime = CurrentBP.CalculateProductionTime(CInt(Me.OverridingPE), IndSkill, ProdImplant, AssemblyArray, Me.Runs)
            End If

            Dim BPWF As Double = CurrentBP.WasteFactor
            If Me.OverridingME = "" Then
                BPWF = CurrentBP.CalculateWasteFactor(Me.PESkill)
            Else
                BPWF = CurrentBP.CalculateWasteFactor(CInt(Me.OverridingME), Me.PESkill)
            End If

            Dim resource As BlueprintResource = CurrentBP.Resources(key)
            ' Calculate the current perfect and waste resources
            Dim waste As Integer = 0
            Dim perfectRaw As Integer = resource.Quantity

            ' Calculate Waste - Mark II!
            waste = CalculateWasteUnits(resource, BPWF, MatMod)

            ' Remove the resource
            Me.RequiredResources.Remove(TypeID.ToString)

            Dim BPID As String = PlugInData.Products(TypeID)
            Dim subBPS As BlueprintSelection = BlueprintSelection.CopyFromBlueprint(PlugInData.Blueprints(BPID))

            If Me.SubJobMEs.ContainsKey(resource.TypeID.ToString) = True Then
                subBPS.MELevel = Me.SubJobMEs(resource.TypeID.ToString)
            End If

            subBPS.PELevel = 0
            subBPS.Runs = -1
            Dim subBPWF As Double = subBPS.CalculateWasteFactor(Me.PESkill)

            ' Create a new production job
            Dim subPJ As New ProductionJob
            subPJ.CurrentBP = subBPS
            subPJ.TypeID = resource.TypeID
            subPJ.TypeName = resource.TypeName
            subPJ.PerfectUnits = perfectRaw
            subPJ.WasteUnits = waste
            subPJ.Runs = (perfectRaw + waste) * Runs
            subPJ.Manufacturer = Manufacturer
            subPJ.BPOwner = Me.BPOwner
            subPJ.PESkill = Me.PESkill
            subPJ.IndSkill = Me.IndSkill
            subPJ.ProdImplant = Me.ProdImplant
            subPJ.AssemblyArray = Me.AssemblyArray
            subPJ.StartTime = Now

            ' Do the iteration on the component BP
            subPJ.CalculateResourceRequirements(False, Me.BPOwner)

            Me.RequiredResources.Add(resource.TypeID.ToString, subPJ)

            Me.Cost = Me.CalculateCost()

        End If
    End Sub

    Public Sub RecalculateResourceRequirements()

        If Me.SubJobMEs Is Nothing Then
            Me.SubJobMEs = New SortedList(Of String, Integer)
        End If

        Dim MatMod As Double = 1
        If Me.AssemblyArray IsNot Nothing Then
            MatMod = Me.AssemblyArray.MaterialMultiplier
        End If

        If Me.OverridingPE = "" Then
            Me.RunTime = CurrentBP.CalculateProductionTime(IndSkill, ProdImplant, AssemblyArray, Me.Runs)
        Else
            Me.RunTime = CurrentBP.CalculateProductionTime(CInt(Me.OverridingPE), IndSkill, ProdImplant, AssemblyArray, Me.Runs)
        End If

        Dim BPWF As Double = CurrentBP.WasteFactor
        If Me.OverridingME = "" Then
            BPWF = CurrentBP.CalculateWasteFactor(Me.PESkill)
        Else
            BPWF = CurrentBP.CalculateWasteFactor(CInt(Me.OverridingME), Me.PESkill)
        End If

        Dim waste As Integer = 0

        For Each resource As Object In Me.RequiredResources.Values
            If TypeOf (resource) Is RequiredResource Then
                ' Get the resource
                Dim newResource As RequiredResource = CType(resource, RequiredResource)
                Dim key As String = newResource.TypeID.ToString & "_1"
                If CurrentBP.Resources.ContainsKey(key) Then
                    Dim BPResource As BlueprintResource = CurrentBP.Resources(key)
                    ' Calculate Waste - Mark II!
                    waste = CalculateWasteUnits(BPResource, BPWF, MatMod)
                    newResource.WasteUnits = waste
                End If
            Else
                ' Get the production job
                Dim subPJ As ProductionJob = CType(resource, ProductionJob)
                Dim key As String = subPJ.TypeID.ToString & "_1"
                Dim BPResource As BlueprintResource = CurrentBP.Resources(key)
                ' Set SubJob ME
                If Me.SubJobMEs.ContainsKey(BPResource.TypeID.ToString) = False Then
                    Me.SubJobMEs.Add(BPResource.TypeID.ToString, subPJ.CurrentBP.MELevel)
                Else
                    Me.SubJobMEs(BPResource.TypeID.ToString) = subPJ.CurrentBP.MELevel
                End If
                ' Calculate Waste - Mark II!
                waste = CalculateWasteUnits(BPResource, BPWF, MatMod)
                subPJ.WasteUnits = waste
                subPJ.Runs = CInt((subPJ.PerfectUnits + waste) * Runs)
                subPJ.RecalculateResourceRequirements()
            End If
        Next

        Me.Cost = Me.CalculateCost()

    End Sub

    Public Function CalculateWasteUnits(resource As BlueprintResource, BPWF As Double, MatMod As Double) As Integer
        Dim waste As Integer = 0
        ' Calculate Waste - Mark II!
        waste = CInt(Math.Round((BPWF * resource.BaseMaterial) + (resource.BaseMaterial * (MatMod - 1)), 0, MidpointRounding.AwayFromZero))
        ' Provisional adjustment for "extra" mats
        Dim ExtraWaste As Integer = CInt(Math.Round(((resource.Quantity - resource.BaseMaterial) * (1.25 - (0.05 * Me.PESkill))) - (resource.Quantity - resource.BaseMaterial), 0, MidpointRounding.AwayFromZero))
        waste += ExtraWaste
        Return waste
    End Function

    Public Sub UpdateManufacturer(ByVal PilotName As String)
        Me.Manufacturer = PilotName
        For Each resource As Object In Me.RequiredResources.Values
            If TypeOf (resource) Is ProductionJob Then
                ' Get the production job
                Dim subPJ As ProductionJob = CType(resource, ProductionJob)
                subPJ.UpdateManufacturer(PilotName)
            End If
        Next
    End Sub

    Public Sub UpdateJobSkills(ByVal NewPESkill As Integer, ByVal NewIndSkill As Integer, ByVal NewProdImplant As Integer)
        Me.PESkill = NewPESkill
        Me.IndSkill = NewIndSkill
        Me.ProdImplant = NewProdImplant
        For Each resource As Object In Me.RequiredResources.Values
            If TypeOf (resource) Is ProductionJob Then
                ' Get the production job
                Dim subPJ As ProductionJob = CType(resource, ProductionJob)
                subPJ.UpdateJobSkills(NewPESkill, NewIndSkill, NewProdImplant)
            End If
        Next
    End Sub

    Public Sub UpdateJobPESkill(ByVal NewPESkill As Integer)
        Me.PESkill = NewPESkill
        For Each resource As Object In Me.RequiredResources.Values
            If TypeOf (resource) Is ProductionJob Then
                ' Get the production job
                Dim subPJ As ProductionJob = CType(resource, ProductionJob)
                subPJ.UpdateJobPESkill(NewPESkill)
            End If
        Next
    End Sub

    Public Sub UpdateJobIndSkill(ByVal NewIndSkill As Integer)
        Me.IndSkill = NewIndSkill
        For Each resource As Object In Me.RequiredResources.Values
            If TypeOf (resource) Is ProductionJob Then
                ' Get the production job
                Dim subPJ As ProductionJob = CType(resource, ProductionJob)
                subPJ.UpdateJobIndSkill(NewIndSkill)
            End If
        Next
    End Sub

    Public Sub UpdateJobProdImplant(ByVal NewProdImplant As Integer)
        Me.ProdImplant = NewProdImplant
        For Each resource As Object In Me.RequiredResources.Values
            If TypeOf (resource) Is ProductionJob Then
                ' Get the production job
                Dim subPJ As ProductionJob = CType(resource, ProductionJob)
                subPJ.UpdateJobProdImplant(NewProdImplant)
            End If
        Next
    End Sub

End Class

