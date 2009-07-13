Imports System.Windows.Forms

<Serializable()> Public Class Blueprint
    Public ID As Integer
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
                    Dim catID As String = CStr(CType(EveHQ.Core.HQ.itemData(newBP.ProductID.ToString), EveHQ.Core.EveItem).Category)
                    If PlugInData.CategoryNames.ContainsKey(CStr(EveHQ.Core.HQ.itemCats(catID))) = False Then
                        PlugInData.CategoryNames.Add(CStr(EveHQ.Core.HQ.itemCats(catID)), catID)
                    End If
                Next
                ' Ok so far so let's add the material requirements
                strSQL = "SELECT typeActivityMaterials.*, invTypes.typeName, invGroups.groupID, invGroups.categoryID FROM invGroups INNER JOIN (invTypes INNER JOIN typeActivityMaterials ON invTypes.typeID = typeActivityMaterials.requiredTypeID) ON invGroups.groupID = invTypes.groupID ORDER BY typeActivityMaterials.typeID;"
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
                                    If newBP.Resources.ContainsKey(newReq.TypeID.ToString & "_" & newReq.Activity.ToString) = False Then
                                        newBP.Resources.Add(newReq.TypeID.ToString & "_" & newReq.Activity.ToString, newReq)
                                    End If
                                Next
                            End If
                        Next
                        ' Return finished
                        Return True
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
End Class

<Serializable()> Public Class ProductionJob
    Public TypeID As Integer
    Public TypeName As String
    Public PerfectUnits As Double
    Public WasteUnits As Double
    Public Runs As Integer
    Public StartTime As Date
    Public RunTime As Long
    Public RequiredResources As New SortedList
End Class

<Serializable()> Public Class RequiredResource
    Public TypeID As Integer
    Public TypeName As String
    Public TypeGroup As Integer
    Public TypeCategory As Integer
    Public PerfectUnits As Double
    Public WasteUnits As Double
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

Public Class BlueprintSelection
    Inherits Blueprint
    Public MELevel As Integer
    Public PELevel As Integer
    Public Runs As Integer

    Public Shared Function CopyFromBlueprint(ByVal OriginalBlueprint As Blueprint) As BlueprintSelection
        Dim newBP As New BlueprintSelection
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
        For Each resource As String In OriginalBlueprint.Resources.Keys
            newBP.Resources.Add(resource, OriginalBlueprint.Resources(resource))
        Next
        Return newBP
    End Function

    Public Function CalculateProductionJob(ByVal BPOwner As String, ByVal ProdEffSkill As Integer, ByVal IndSkill As Integer, ByVal ProdImplantBonus As Integer, ByVal BPWF As Double, ByVal Runs As Integer, ByVal SlotArray As AssemblyArray, ByVal ComponentIteration As Boolean) As ProductionJob
        ' Set up a new production job
        Dim newPJ As New ProductionJob
        newPJ.TypeID = Me.ID
        newPJ.TypeName = Me.Name
        newPJ.Runs = Runs
        newPJ.StartTime = Now
        newPJ.RunTime = CalculateProductionTime(IndSkill, ProdImplantBonus, SlotArray)
        ' Get the required resources
        newPJ.RequiredResources = CalculateResourceRequirements(BPOwner, ProdEffSkill, IndSkill, ProdImplantBonus, BPWF, Runs, SlotArray, ComponentIteration)
        Return newPJ
    End Function

    Public Function CalculateResourceRequirements(ByVal BPOwner As String, ByVal ProdEffSkill As Integer, ByVal IndSkill As Integer, ByVal ProdImplantBonus As Integer, ByVal BPWF As Double, ByVal Runs As Integer, ByVal SlotArray As AssemblyArray, ByVal ComponentIteration As Boolean) As SortedList

        Dim MatMod As Double = 1
        If SlotArray IsNot Nothing Then
            MatMod = SlotArray.MaterialMultiplier
        End If

        Dim ReqdResources As New SortedList
        For Each resource As BlueprintResource In Me.Resources.Values
            If resource.Activity = 1 And resource.Quantity >= 0 Then
                Dim subBP As BlueprintAsset = Nothing

                ' Calculate the current perfect and waste resources
                Dim perfectExtra As Integer = 0
                Dim perfectRaw As Integer = resource.Quantity
                Dim waste As Integer = 0
                ' Check for data in the activityID=6
                If Me.Resources.ContainsKey(resource.TypeID.ToString & "_6") Then
                    perfectExtra = Me.Resources(resource.TypeID.ToString & "_6").Quantity
                    If perfectExtra >= perfectRaw Then
                        ' Material is raw
                        waste = CInt(Math.Round(BPWF * perfectRaw * MatMod, 0))
                    Else
                        ' Material is extra
                        waste = CInt(Math.Round(BPWF * perfectExtra * MatMod, 0))
                    End If
                Else
                    ' Material is extra and therfore not subject to waste?
                    Select Case Me.GroupID
                        Case 371, 447, 914, 915
                            Select Case resource.TypeGroup
                                Case 18, 429
                                    ' Material is raw
                                    waste = CInt(Math.Round(BPWF * perfectRaw * MatMod, 0))
                                Case Else
                                    waste = 0
                            End Select
                        Case 535
                            Select Case resource.TypeGroup
                                Case 536
                                    ' Material is raw
                                    waste = CInt(Math.Round(BPWF * perfectRaw * MatMod, 0))
                                Case Else
                                    waste = 0
                            End Select
                        Case Else
                            waste = 0
                    End Select
                End If

                ' Check if we have component iteration active
                If ComponentIteration = True Then
                    ' Check for a blueprint
                    Dim BPName As String = (StrConv(resource.TypeName, VbStrConv.ProperCase) & " Blueprint")
                    If EveHQ.Core.HQ.itemList.ContainsKey(BPName) = True Then
                        Dim BPID As String = CStr(EveHQ.Core.HQ.itemList(BPName))
                        ' Search BPs
                        If PlugInData.BlueprintAssets.ContainsKey(BPOwner) = True Then
                            For Each BPAsset As BlueprintAsset In PlugInData.BlueprintAssets(BPOwner).Values
                                If BPAsset.TypeID = BPID Then
                                    subBP = BPAsset
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                End If

                ' See if we need to examine the BP
                If ComponentIteration = True And subBP IsNot Nothing Then
                    ' Convert the BP to a BlueprintSelection ready for processing
                    Dim subBPS As BlueprintSelection = CopyFromBlueprint(PlugInData.Blueprints(subBP.TypeID))
                    subBPS.MELevel = subBP.MELevel
                    subBPS.PELevel = subBP.PELevel
                    subBPS.Runs = subBP.Runs
                    Dim subBPWF As Double = subBPS.CalculateWasteFactor(ProdEffSkill)
                    ' Create a new production job
                    Dim subPJ As New ProductionJob
                    subPJ.TypeID = resource.TypeID
                    subPJ.TypeName = resource.TypeName
                    subPJ.PerfectUnits = perfectRaw
                    subPJ.WasteUnits = waste
                    subPJ.Runs = perfectRaw + waste
                    subPJ.StartTime = Now
                    subPJ.RunTime = subBPS.CalculateProductionTime(IndSkill, ProdImplantBonus, SlotArray)
                    ' Do the iteration on the component BP
                    subPJ.RequiredResources = subBPS.CalculateResourceRequirements(BPOwner, ProdEffSkill, IndSkill, ProdImplantBonus, subBPWF, subPJ.Runs, SlotArray, ComponentIteration)
                    ReqdResources.Add(resource.TypeID, subPJ)
                Else
                    Dim newResource As New RequiredResource
                    newResource.TypeID = resource.TypeID
                    newResource.TypeName = resource.TypeName
                    newResource.TypeGroup = resource.TypeGroup
                    newResource.TypeCategory = resource.TypeCategory
                    newResource.PerfectUnits = perfectRaw
                    newResource.WasteUnits = waste
                    ReqdResources.Add(resource.TypeID, newResource)
                End If
            End If
        Next
        Return ReqdResources
    End Function

    Private Function CalculateWasteFactor(ByVal ProdEffSkill As Integer) As Double
        If Me.MELevel < 0 Then
            ' This is for negative ME
            Return ((1 / Me.WasteFactor) * (1 - Me.MELevel)) + (0.25 - (0.05 * ProdEffSkill))
        Else
            ' This is for zero and positive ME
            Return ((1 / Me.WasteFactor) / (1 + Me.MELevel)) + (0.25 - (0.05 * ProdEffSkill))
        End If
    End Function

    Private Function CalculateProductionTime(ByVal IndSkill As Integer, ByVal ProdImplantBonus As Double, ByVal ProductionArray As AssemblyArray) As Long
        ProdImplantBonus = 1 - (ProdImplantBonus / 100)
        Dim productionTime As Double = Me.ProdTime * (1 - (0.04 * IndSkill)) * ProdImplantBonus
        ' Calculate the production time
        If Me.PELevel >= 0 Then
            productionTime *= (1 - (Me.ProdMod / Me.ProdTime) * Me.PELevel / (1 + Me.PELevel))
        Else
            productionTime *= (1 - (Me.ProdMod / Me.ProdTime) * (Me.PELevel - 1))
        End If
        If ProductionArray IsNot Nothing Then
            productionTime *= ProductionArray.TimeMultiplier
        End If
        Return CLng(productionTime)
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
