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

Imports EveHQ.EveData
Imports EveHQ.Prism.BPCalc
Imports DevComponents.AdvTree
Imports System.Text
Imports System.Windows.Forms
Imports System.Xml
Imports DevComponents.DotNetBar
Imports System.Threading.Tasks

Public Class PrismResources

    Dim CurrentJob As Job
    Dim CurrentBP As OwnedBlueprint
    Dim CurrentBatch As Prism.BatchJob
    Dim OwnedResources As New SortedList(Of String, SortedList(Of String, Long)) ' ItemID, (Location, Long)
    Dim GroupResources As New SortedList(Of String, Long)
    Dim SwapResources As New SortedList(Of String, SwapResource)
    Dim ProductionList As New SortedList(Of String, ProductionItem)

    Public Property ProductionJob() As Job
        Get
            Return CurrentJob
        End Get
        Set(ByVal value As Job)
            CurrentJob = Nothing
            ' Reset the "build all resources" check box, since we are switching jobs
            ' Setting the current job to nothing should prevent any side effect processing of the checked/unchecked event.
            chkUseStandardCosting.Checked = False
            CurrentJob = value
            If CurrentJob IsNot Nothing Then
                If CurrentJob.CurrentBlueprint IsNot Nothing Then
                    CurrentJob.RecalculateResourceRequirements()
                    DisplayRequiredResources()
                    RaiseEvent ProductionResourcesChanged()
                End If
            Else
                DisplayRequiredResources()
                RaiseEvent ProductionResourcesChanged()
            End If

        End Set
    End Property

    Public Property InventionBP() As OwnedBlueprint
        Get
            Return CurrentBP
        End Get
        Set(ByVal value As OwnedBlueprint)
            If value IsNot Nothing Then
                CurrentBP = value
                DisplayInventionResources()
                tiInvention.Visible = True
            Else
                tiInvention.Visible = False
            End If
        End Set
    End Property

    Public Property BatchJob() As Prism.BatchJob
        Get
            Return CurrentBatch
        End Get
        Set(ByVal value As Prism.BatchJob)
            CurrentBatch = value
            Call Me.DisplayOwnedResources()
        End Set
    End Property

    Public ReadOnly Property BuildResources() As Boolean
        Get
            Return chkUseStandardCosting.Checked
        End Get
    End Property

    Public Event ProductionResourcesChanged()

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        cboAssetSelection.DropDownControl = New PrismSelectionControl(PrismSelectionType.AllOwners, True, cboAssetSelection)

    End Sub

    Private Sub PrismResources_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        CType(cboAssetSelection.DropDownControl, PrismSelectionControl).UpdateOwnerList()
    End Sub

#Region "Invention Routines"

    Public Sub DisplayInventionResources()
        adtInventionResources.BeginUpdate()
        adtInventionResources.Nodes.Clear()
        Dim priceData As Task(Of Dictionary(Of String, Double)) = Core.DataFunctions.GetMarketPrices(From r As BlueprintResource In CurrentBP.Resources(BlueprintActivity.Invention).Values Select CStr(r.TypeId))
        priceData.Wait()
        Dim prices As Dictionary(Of String, Double) = priceData.Result

        For Each resource As BlueprintResource In CurrentBP.Resources(BlueprintActivity.Invention).Values
            ' Add the resource to the list
            Dim newIr As New Node
            newIr.Text = StaticData.Types(resource.TypeId.ToString).Name
            newIr.Name = resource.TypeId.ToString
            newIr.Cells.Add(New Cell(resource.Quantity.ToString))
            Dim irPrice As Double = prices(resource.TypeId.ToString)
            newIr.Cells.
                Add(New Cell(irPrice.ToString))
            newIr.Cells.Add(New Cell((irPrice * resource.Quantity).ToString))
            adtInventionResources.Nodes.Add(newIr)
            For c As Integer = 1 To 3
                newIr.Cells(c).TextDisplayFormat = "N0"
            Next
        Next
        EveHQ.Core.AdvTreeSorter.Sort(adtInventionResources, 1, True, True)
        adtInventionResources.EndUpdate()
        ' Hide the invention tab if we don't have invention resources
        If adtInventionResources.Nodes.Count = 0 Then
            tiInvention.Visible = False
        Else
            tiInvention.Visible = True
        End If
    End Sub

    Private Sub adtInventionResources_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As Windows.Forms.MouseEventArgs) Handles adtInventionResources.ColumnHeaderMouseDown
        Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        Core.AdvTreeSorter.Sort(ch, True, False)
    End Sub

#End Region

#Region "Production Routines"

    Private Sub chkShowSkills_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowSkills.CheckedChanged
        Call DisplayRequiredResources()
    End Sub

    Private Sub chkUseStandardCosting_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseStandardCosting.CheckedChanged
        If CurrentJob IsNot Nothing Then
            If (CurrentJob.CurrentBlueprint IsNot Nothing) Then
                CurrentJob.CalculateResourceRequirements(chkUseStandardCosting.Checked, CurrentJob.BlueprintOwner)
                Call Me.DisplayRequiredResources()
                RaiseEvent ProductionResourcesChanged()
            End If
        End If
    End Sub

    Private Sub DisplayRequiredResources()

        adtResources.BeginUpdate()
        adtResources.Nodes.Clear()

        Dim maxProducableUnits As Long = -1

        If CurrentJob IsNot Nothing Then
            Dim priceData As Task(Of Dictionary(Of String, Double)) = Core.DataFunctions.GetMarketPrices(From r In CurrentJob.Resources.Values Where TypeOf (r) Is JobResource Let res = r Select CStr(res.TypeID))
            Dim jobCostTask As Task(Of Dictionary(Of String, Double)) = Core.DataFunctions.GetMarketPrices(From r In CurrentJob.SubJobs.Values Where TypeOf (r) Is Job Let res = r Select CStr(res.TypeID))
            Task.WaitAll(priceData, jobCostTask)
            ' resource costs
            Dim resourceCosts As Dictionary(Of String, Double) = priceData.Result
            ' production job costs
            Dim jobCosts As Dictionary(Of String, Double) = jobCostTask.Result
            For Each resource As JobResource In CurrentJob.Resources.Values
                ' This is a resource so add it
                If resource.TypeCategory <> 16 Or (resource.TypeCategory = 16 And chkShowSkills.Checked = True) Then
                    Dim perfectRaw As Integer = CInt(resource.PerfectUnits)
                    Dim waste As Integer = CInt(resource.WasteUnits)
                    Dim total As Integer = perfectRaw + waste
                    Dim price As Double = resourceCosts(resource.TypeID.ToString)
                    Dim value As Double = total * price
                    ' Add a new list view item
                    If total > 0 Then
                        Dim newRes As New Node(resource.TypeName)
                        newRes.TextDisplayFormat = "N0"
                        ' Calculate costs
                        If resource.TypeCategory <> 16 Then
                            ' Not a skill
                        Else
                            ' A skill
                            newRes.Text &= " (Lvl " & Core.SkillFunctions.Roman(perfectRaw) & ")"
                            ' Check for skill of recycler
                            If Core.SkillFunctions.IsSkillTrained(Core.HQ.Settings.Pilots(CurrentJob.Manufacturer), resource.TypeName, perfectRaw) = True Then
                                ' TODO - add colour and alignment styles
                                'newRes.BackColor = Drawing.Color.LightGreen
                            Else
                                'newRes.BackColor = Drawing.Color.LightCoral
                            End If
                            perfectRaw = 0 : waste = 0 : total = 0 : value = 0
                        End If
                        If PlugInData.Products.ContainsKey(resource.TypeID.ToString) = True Then
                            newRes.Cells.Add(New Cell("0"))
                            Dim bpme As New BPMEControl
                            bpme.nudME.Value = 0

                            bpme.AssignedTypeID = resource.TypeID.ToString
                            bpme.ParentJob = CurrentJob
                            AddHandler bpme.ResourcesChanged, AddressOf UpdateResources
                            newRes.Cells(1).HostedControl = bpme
                        Else
                            newRes.Cells.Add(New Cell(""))
                        End If

                        Dim perfectTotal As Long = CLng(perfectRaw) * CLng(CurrentJob.Runs)
                        Dim wasteTotal As Long = CLng(waste) * CLng(CurrentJob.Runs)
                        Dim totalTotal As Long = CLng(total) * CLng(CurrentJob.Runs)

                        newRes.Cells.Add(New Cell(perfectTotal.ToString))
                        newRes.Cells.Add(New Cell(wasteTotal.ToString))
                        newRes.Cells.Add(New Cell(totalTotal.ToString))
                        newRes.Cells.Add(New Cell(price.ToString))
                        newRes.Cells.Add(New Cell((value * CurrentJob.Runs).ToString))
                        If CurrentJob.CurrentBlueprint.MaterialModifier = 0 Or CurrentJob.CurrentBlueprint.WasteFactor = 0 Then
                            newRes.Cells.Add(New Cell("0"))
                        Else
                            newRes.Cells.Add(New Cell((Int(resource.BaseUnits * CurrentJob.CurrentBlueprint.WasteFactor / 50)).ToString("N0")))
                        End If
                        For c As Integer = 1 To 7
                            Select Case c
                                Case 2, 3, 4, 7
                                    newRes.Cells(c).TextDisplayFormat = "N0"
                                Case Else
                                    newRes.Cells(c).TextDisplayFormat = "N2"
                            End Select
                        Next

                        adtResources.Nodes.Add(newRes)
                    End If
                End If
            Next
            For Each subJob As Job In CurrentJob.SubJobs.Values
                ' This is another production job
                Dim perfectRaw As Integer = CInt(subJob.PerfectUnits)
                Dim waste As Integer = CInt(subJob.WasteUnits)
                Dim total As Integer = perfectRaw + waste
                Dim price As Double = jobCosts(subJob.TypeID.ToString)
                Dim value As Double = total * price
                Dim newRes As New Node(subJob.TypeName)
                newRes.TextDisplayFormat = "N0"
                If PlugInData.Products.ContainsKey(subJob.TypeID.ToString) = True Then
                    newRes.Cells.Add(New Cell(subJob.CurrentBlueprint.MELevel.ToString))
                    Dim BPME As New BPMEControl
                    BPME.nudME.Value = subJob.CurrentBlueprint.MELevel
                    BPME.nudME.LockUpdateChecked = subJob.ProduceSubJob

                    BPME.nudME.ButtonCustom.Enabled = True
                    BPME.nudME.ButtonCustom.Enabled = False
                    BPME.AssignedTypeID = subJob.TypeID.ToString
                    BPME.ParentJob = CurrentJob
                    BPME.AssignedJob = subJob
                    AddHandler BPME.ResourcesChanged, AddressOf UpdateResources
                    newRes.Cells(1).HostedControl = BPME
                Else
                    newRes.Cells.Add(New Cell(""))
                End If
                newRes.Cells.Add(New Cell((subJob.PerfectUnits * CurrentJob.Runs).ToString))
                newRes.Cells.Add(New Cell((subJob.WasteUnits * CurrentJob.Runs).ToString))
                newRes.Cells.Add(New Cell((total * CurrentJob.Runs).ToString))
                newRes.Cells.Add(New Cell(price.ToString))
                newRes.Cells.Add(New Cell((value * CurrentJob.Runs).ToString))
                newRes.Cells.Add(New Cell((Int(subJob.PerfectUnits * CurrentJob.CurrentBlueprint.WasteFactor / 50)).ToString))
                Dim BPDetails As New StringBuilder
                BPDetails.AppendLine("The blueprint used for this job is as follows:")
                BPDetails.AppendLine("")
                BPDetails.AppendLine("ME Level: " & subJob.CurrentBlueprint.MELevel.ToString)
                BPDetails.AppendLine("PE Level: " & subJob.CurrentBlueprint.PELevel.ToString)
                BPDetails.AppendLine("Runs: " & subJob.CurrentBlueprint.Runs.ToString)
                Dim TTI As New SuperTooltipInfo("Blueprint Details", subJob.TypeName, BPDetails.ToString, Nothing, Nothing, eTooltipColor.Yellow)
                STT.SetSuperTooltip(newRes, TTI)
                Call DisplayJob(subJob, CurrentJob.Runs, newRes, maxProducableUnits)
                ' Recalculate sub prices
                Dim subprice As Double = 0
                For Each subRes As Node In newRes.Nodes
                    subprice += CDbl(subRes.Cells(6).Text)
                Next
                newRes.Cells(6).Text = subprice.ToString
                newRes.Cells(5).Text = (subprice / subJob.Runs).ToString
                For c As Integer = 1 To 7
                    Select Case c
                        Case 2, 3, 4, 7
                            newRes.Cells(c).TextDisplayFormat = "N0"
                        Case Else
                            newRes.Cells(c).TextDisplayFormat = "N2"
                    End Select
                Next
                adtResources.Nodes.Add(newRes)
            Next
            For c As Integer = 0 To 7
                adtResources.Columns(c).Image = Nothing
            Next
        End If
        Core.AdvTreeSorter.Sort(adtResources, 5, True, True)
        adtResources.EndUpdate()

        ' Display owned resources
        Call DisplayOwnedResources()

    End Sub

    Private Sub DisplayJob(ByVal parentJob As Job, ByVal BaseRuns As Integer, ByVal parentRes As Node, ByRef maxProducableUnits As Long)
        Dim resourceTask As Task(Of Dictionary(Of String, Double)) = Core.DataFunctions.GetMarketPrices(From r In parentJob.Resources.Values Where TypeOf (r) Is JobResource Let res = r Select CStr(res.TypeID))
        Dim jobTask As Task(Of Dictionary(Of String, Double)) = Core.DataFunctions.GetMarketPrices(From r In parentJob.SubJobs.Values Where TypeOf (r) Is Job Let res = r Select CStr(res.TypeID))
        Task.WaitAll(resourceTask, jobTask)
        ' resource costs
        Dim resourceCosts As Dictionary(Of String, Double) = resourceTask.Result
        ' production job costs
        Dim jobCosts As Dictionary(Of String, Double) = jobTask.Result
        For Each resource As JobResource In parentJob.Resources.Values
            ' This is a resource so add it
            If resource.TypeCategory <> 16 Or (resource.TypeCategory = 16 And chkShowSkills.Checked = True) Then
                Dim perfectRaw As Long = CLng(resource.PerfectUnits) * parentJob.Runs
                Dim waste As Long = CLng(resource.WasteUnits) * parentJob.Runs
                Dim total As Long = perfectRaw + waste
                Dim price As Double = resourceCosts(CStr(resource.TypeID))
                Dim value As Double = total * price
                ' Add a new list view item
                Dim newRes As New Node(resource.TypeName)
                newRes.TextDisplayFormat = "N0"
                ' Calculate costs
                If resource.TypeCategory <> 16 Then
                    ' Not a skill
                Else
                    ' A skill
                    newRes.Text &= " (Lvl " & EveHQ.Core.SkillFunctions.Roman(CInt(resource.PerfectUnits)) & ")"
                    ' Check for skill of recycler
                    If EveHQ.Core.SkillFunctions.IsSkillTrained(EveHQ.Core.HQ.Settings.Pilots(parentJob.Manufacturer), resource.TypeName, CInt(resource.PerfectUnits)) = True Then
                        ' TODO - add colour and alignment styles
                        'newRes.BackColor = Drawing.Color.LightGreen
                    Else
                        'newRes.BackColor = Drawing.Color.LightCoral
                    End If
                    perfectRaw = 0 : waste = 0 : total = 0 : value = 0
                End If
                If PlugInData.Products.ContainsKey(resource.TypeID.ToString) = True Then
                    newRes.Cells.Add(New Cell("0"))
                    Dim BPME As New BPMEControl
                    BPME.nudME.Value = 0
                    BPME.AssignedTypeID = resource.TypeID.ToString
                    BPME.ParentJob = parentJob
                    AddHandler BPME.ResourcesChanged, AddressOf Me.UpdateResources
                    newRes.Cells(1).HostedControl = BPME
                Else
                    newRes.Cells.Add(New Cell(""))
                End If
                newRes.Cells.Add(New Cell(perfectRaw.ToString))
                newRes.Cells.Add(New Cell(waste.ToString))
                newRes.Cells.Add(New Cell(total.ToString))
                newRes.Cells.Add(New Cell(price.ToString))
                newRes.Cells.Add(New Cell(value.ToString))
                newRes.Cells.Add(New Cell((Int(resource.BaseUnits / parentJob.CurrentBlueprint.MaterialModifier)).ToString))
                For c As Integer = 1 To 7
                    Select Case c
                        Case 2, 3, 4, 7
                            newRes.Cells(c).TextDisplayFormat = "N0"
                        Case Else
                            newRes.Cells(c).TextDisplayFormat = "N2"
                    End Select
                Next
                parentRes.Nodes.Add(newRes)
            End If
        Next
        For Each subJob As Job In parentJob.SubJobs.Values
            ' This is another production job
            Dim perfectRaw As Integer = CInt(subJob.PerfectUnits)
            Dim waste As Integer = CInt(subJob.WasteUnits)
            Dim runs As Integer = parentJob.Runs
            Dim total As Integer = perfectRaw + waste
            Dim price As Double = jobCosts(CStr(subJob.TypeID))
            Dim value As Double = total * price
            Dim newRes As New Node(subJob.TypeName)
            newRes.TextDisplayFormat = "N0"
            If PlugInData.Products.ContainsKey(subJob.TypeID.ToString) = True Then
                newRes.Cells.Add(New Cell(subJob.CurrentBlueprint.MELevel.ToString))
                Dim BPME As New BPMEControl
                BPME.nudME.Value = subJob.CurrentBlueprint.MELevel
                BPME.nudME.LockUpdateChecked = True
                BPME.nudME.ButtonCustom.Enabled = True
                BPME.nudME.ButtonCustom.Enabled = False
                BPME.AssignedTypeID = subJob.TypeID.ToString
                BPME.ParentJob = parentJob
                BPME.AssignedJob = subJob
                AddHandler BPME.ResourcesChanged, AddressOf Me.UpdateResources
                newRes.Cells(1).HostedControl = BPME
            Else
                newRes.Cells.Add(New Cell(""))
            End If
            newRes.Cells.Add(New Cell((perfectRaw * runs).ToString))
            newRes.Cells.Add(New Cell((waste * runs).ToString))
            newRes.Cells.Add(New Cell((total * runs).ToString))
            newRes.Cells.Add(New Cell(price.ToString))
            newRes.Cells.Add(New Cell((value * runs).ToString))
            newRes.Cells.Add(New Cell((Int(perfectRaw / parentJob.CurrentBlueprint.MaterialModifier)).ToString))
            Dim BPDetails As New StringBuilder
            BPDetails.AppendLine("The blueprint used for this job is as follows:")
            BPDetails.AppendLine("")
            BPDetails.AppendLine("ME Level: " & subJob.CurrentBlueprint.MELevel.ToString)
            BPDetails.AppendLine("PE Level: " & subJob.CurrentBlueprint.PELevel.ToString)
            BPDetails.AppendLine("Runs: " & subJob.CurrentBlueprint.Runs.ToString)
            Dim TTI As New SuperTooltipInfo("Blueprint Details", subJob.TypeName, BPDetails.ToString, Nothing, Nothing, eTooltipColor.Yellow)
            STT.SetSuperTooltip(newRes, TTI)
            Call DisplayJob(subJob, BaseRuns, newRes, maxProducableUnits)
            ' Recalculate sub prices
            Dim subprice As Double = 0
            For Each subRes As Node In newRes.Nodes
                subprice += CDbl(subRes.Cells(6).Text)
            Next
            newRes.Cells(6).Text = subprice.ToString
            newRes.Cells(5).Text = (subprice / subJob.Runs).ToString
            For c As Integer = 1 To 7
                Select Case c
                    Case 2, 3, 4, 7
                        newRes.Cells(c).TextDisplayFormat = "N0"
                    Case Else
                        newRes.Cells(c).TextDisplayFormat = "N2"
                End Select
            Next
            parentRes.Nodes.Add(newRes)
        Next
    End Sub

    Private Sub adtResources_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles adtResources.ColumnHeaderMouseDown
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
    End Sub

#End Region

#Region "Owned Resources Routines"

    Private Sub DisplayOwnedResources()
        OwnedResources.Clear()
        GroupResources.Clear()
        SwapResources.Clear()

        ' Get the relevant details
        Call Me.GetBatchResources()

        Call Me.GetOwnedResources()

        If chkAdvancedResourceAllocation.Checked = True Then
            Call Me.CheckSwapResources()
        End If

        ' Display the resources owned
        Dim ItemData As EveHQ.Core.EveItem
        Dim reqd, owned, surplus As Long
        Dim MaxProducableUnits As Long = -1
        adtOwnedResources.BeginUpdate()
        adtOwnedResources.Nodes.Clear()
        For Each itemID As String In GroupResources.Keys
            reqd = GroupResources(itemID)
            If reqd > 0 Then
                ItemData = EveHQ.Core.HQ.itemData(itemID)
                If OwnedResources.ContainsKey(itemID) = True Then
                    owned = OwnedResources(itemID).Item("TotalOwned")
                    If MaxProducableUnits = -1 Then
                        MaxProducableUnits = CLng(Int(owned / reqd))
                    Else
                        MaxProducableUnits = Math.Min(MaxProducableUnits, CLng(Int(owned / reqd)))
                    End If
                Else
                    owned = 0
                    MaxProducableUnits = 0
                End If
                surplus = owned - reqd
                Dim newORes As New Node(ItemData.Name)

                newORes.Cells.Add(New Cell(reqd.ToString("N0")))
                newORes.Cells.Add(New Cell(owned.ToString("N0")))
                newORes.Cells.Add(New Cell(surplus.ToString("N0")))
                newORes.Cells(3).Tag = surplus
                ' Add locations
                If OwnedResources.ContainsKey(itemID) = True Then
                    If OwnedResources(itemID).Count > 1 Then
                        For Each Location As String In OwnedResources(itemID).Keys
                            If Location <> "TotalOwned" Then
                                Dim newLoc As New Node(Locations.GetLocationNameFromID(Location))
                                newLoc.Cells.Add(New Cell(""))
                                newLoc.Cells.Add(New Cell(OwnedResources(itemID).Item(Location).ToString("N0")))
                                newLoc.Cells.Add(New Cell(""))
                                newORes.Nodes.Add(newLoc)
                            End If
                        Next
                    End If
                End If
                adtOwnedResources.Nodes.Add(newORes)
                ' TODO: Fix styles
                'If surplus < 0 Then
                '    newORes.SubItems(3).ForeColor = Drawing.Color.Red
                'Else
                '    newORes.SubItems(3).ForeColor = Drawing.Color.Green
                'End If
                For c As Integer = 1 To 3
                    newORes.Cells(c).TextDisplayFormat = "N0"
                Next
            End If
        Next
        EveHQ.Core.AdvTreeSorter.Sort(adtOwnedResources, 1, True, True)
        adtOwnedResources.EndUpdate()

        If MaxProducableUnits = -1 Then
            lblMaxUnits.Text = "Maximum Producable Units: n/a"
        Else
            lblMaxUnits.Text = "Maximum Producable Units: " & MaxProducableUnits.ToString("N0")
        End If

    End Sub

    Private Sub GetResourcesFromJob(ByVal pJob As Job)

        ' Set Production List
        Dim PI As New ProductionItem(pJob.TypeID.ToString, True, pJob.Runs)
        If ProductionList.ContainsKey(PI.Key) = False Then
            ProductionList.Add(PI.Key, PI)
        Else
            ProductionList(PI.Key).Quantity += PI.Quantity
        End If

        Dim SR As New SwapResource
        If pJob.TypeID <> pJob.CurrentBlueprint.ID Then
            If SwapResources.ContainsKey(pJob.TypeID.ToString) = True Then
                SR = SwapResources(pJob.TypeID.ToString)
                SR.Quantity += pJob.Runs
            Else
                SR.Quantity = pJob.Runs
                SwapResources.Add(pJob.TypeID.ToString, SR)
            End If
        End If

        For Each resource As JobResource In pJob.Resources.Values
            If resource.TypeCategory <> 16 Then
                ' Add as a "swap" resource - something we can later substitute for lower resources if we have them
                ' Check this is not a blueprint swap!
                If pJob.TypeID <> pJob.CurrentBlueprint.Id Then
                    If SR.Resources.ContainsKey(resource.TypeID.ToString) = False Then
                        SR.Resources.Add(resource.TypeID.ToString, CLng(resource.PerfectUnits + resource.WasteUnits))
                    Else
                        'SR.Resources(rResource.TypeID.ToString) += CLng(rResource.PerfectUnits + rResource.WasteUnits)
                    End If
                End If
                ' This is a resource so add it
                If GroupResources.ContainsKey(CStr(resource.TypeID)) = False Then
                    GroupResources.Add(CStr(resource.TypeID), CLng((resource.PerfectUnits + resource.WasteUnits) * pJob.Runs))
                Else
                    GroupResources(CStr(resource.TypeID)) += CLng((resource.PerfectUnits + resource.WasteUnits) * pJob.Runs)
                End If
                ' Set Production List
                Dim PI2 As New ProductionItem(resource.TypeID.ToString, False, CLng((resource.PerfectUnits + resource.WasteUnits) * pJob.Runs))
                If ProductionList.ContainsKey(PI2.Key) = False Then
                    ProductionList.Add(PI2.Key, PI2)
                Else
                    ProductionList(PI2.Key).Quantity += PI2.Quantity
                End If
            End If
        Next
        For Each subJob As Job In pJob.SubJobs.Values
            ' This is another production job
            ' Add as a "swap" resource - something we can later substitute for lower resources if we have them
            If pJob.TypeID <> pJob.CurrentBlueprint.Id Then
                If SR.Resources.ContainsKey(subJob.TypeID.ToString) = False Then
                    SR.Resources.Add(subJob.TypeID.ToString, subJob.Runs)
                Else
                    SR.Resources(subJob.TypeID.ToString) += subJob.Runs
                End If
            End If
            Call Me.GetResourcesFromJob(subJob)
        Next
       
    End Sub

    Private Sub CheckSwapResources()
        ' Loop through swap resources to see what we could potentially save (ignore the main job though!)
        For Each SwapID As String In SwapResources.Keys
            If SwapID <> CurrentJob.TypeID.ToString Then
                Dim SR As SwapResource = SwapResources(SwapID)
                ' Check if we have a partial or full match
                If OwnedResources.ContainsKey(SwapID) Then
                    ' We own something for the swap, let's see how much
                    Dim owned As Long = OwnedResources(SwapID).Item("TotalOwned")
                    ' How many do we need?
                    Dim reqs As Long = SR.Quantity
                    ' Can we make at least some saving of production?
                    Dim saving As Long = Math.Min(reqs, owned)
                    ' Substitute some of the resources
                    If saving > 0 Then
                        For Each SavedResource As String In SR.Resources.Keys
                            If GroupResources.ContainsKey(SavedResource) = True Then
                                GroupResources(SavedResource) -= SR.Resources(SavedResource) * saving
                            Else
                                Dim msg As String = EveHQ.Core.HQ.itemData(SavedResource).Name & " (ID:" & SavedResource & ") is not present in the group resources for " & CurrentJob.JobName
                                msg &= ControlChars.CrLf & ControlChars.CrLf
                                MessageBox.Show(msg, "Resource Allocation Error", MessageBoxButtons.OK)
                            End If
                        Next
                    End If
                    ' Add the current saving to the group resources
                    If GroupResources.ContainsKey(SwapID) = True Then
                        GroupResources(SwapID) += saving
                    Else
                        GroupResources.Add(SwapID, saving)
                    End If
                End If
            End If
        Next
    End Sub

    Private Sub GetOwnedResources()

        Dim Owner As New PrismOwner

        For Each cOwner As ListViewItem In CType(cboAssetSelection.DropDownControl, PrismSelectionControl).lvwItems.CheckedItems

            If PlugInData.PrismOwners.ContainsKey(cOwner.Text) = True Then
                Owner = PlugInData.PrismOwners(cOwner.Text)
                Dim OwnerAccount As EveHQ.Core.EveHQAccount = PlugInData.GetAccountForCorpOwner(Owner, CorpRepType.Assets)
                Dim OwnerID As String = PlugInData.GetAccountOwnerIDForCorpOwner(Owner, CorpRepType.Assets)

                If OwnerAccount IsNot Nothing Then

                    Dim AssetXML As New XmlDocument
                    Dim APIReq As New EveAPI.EveAPIRequest(EveHQ.Core.HQ.EveHQAPIServerInfo, EveHQ.Core.HQ.RemoteProxy, EveHQ.Core.HQ.Settings.APIFileExtension, EveHQ.Core.HQ.cacheFolder)
                    If Owner.IsCorp = True Then
                        AssetXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsCorp, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                    Else
                        AssetXML = APIReq.GetAPIXML(EveAPI.APITypes.AssetsChar, OwnerAccount.ToAPIAccount, OwnerID, EveAPI.APIReturnMethods.ReturnCacheOnly)
                    End If

                    If AssetXML IsNot Nothing Then
                        Dim locList As XmlNodeList = AssetXML.SelectNodes("/eveapi/result/rowset/row")
                        If locList.Count > 0 Then
                            ' Define what we want to obtain
                            Dim categories, groups As New ArrayList
                            For Each loc As XmlNode In locList
                                Call GetAssetQuantitesFromNode(loc, loc, categories, groups, OwnedResources)
                            Next
                        End If
                    End If
                End If
            End If
        Next

    End Sub

    Private Sub GetAssetQuantitesFromNode(ByVal Root As XmlNode, ByVal item As XmlNode, ByVal categories As ArrayList, ByVal groups As ArrayList, ByRef Assets As SortedList(Of String, SortedList(Of String, Long)))
        Dim ItemData As New EveHQ.Core.EveItem
        Dim AssetID As String = ""
        Dim itemID As String = ""
        AssetID = item.Attributes.GetNamedItem("itemID").Value
        itemID = item.Attributes.GetNamedItem("typeID").Value
        If EveHQ.Core.HQ.itemData.ContainsKey(itemID) Then
            ItemData = EveHQ.Core.HQ.itemData(itemID)
            If categories.Contains(ItemData.Category) Or groups.Contains(ItemData.Group) Or GroupResources.ContainsKey(CStr(ItemData.ID)) Or SwapResources.ContainsKey(CStr(ItemData.ID)) Then
                ' Check if the item is in the list
                If Assets.ContainsKey(CStr(ItemData.ID)) = False Then
                    Dim Locations As New SortedList(Of String, Long)
                    Locations.Add(Root.Attributes.GetNamedItem("locationID").Value, CLng(item.Attributes.GetNamedItem("quantity").Value))
                    Locations.Add("TotalOwned", CLng(item.Attributes.GetNamedItem("quantity").Value))
                    Assets.Add(CStr(ItemData.ID), Locations)
                Else
                    Dim Locations As SortedList(Of String, Long) = Assets(CStr(ItemData.ID))
                    If Locations.ContainsKey(Root.Attributes.GetNamedItem("locationID").Value) = False Then
                        Locations.Add(Root.Attributes.GetNamedItem("locationID").Value, CLng(item.Attributes.GetNamedItem("quantity").Value))
                        Locations("TotalOwned") += CLng(item.Attributes.GetNamedItem("quantity").Value)
                    Else
                        Locations(Root.Attributes.GetNamedItem("locationID").Value) += CLng(item.Attributes.GetNamedItem("quantity").Value)
                        Locations("TotalOwned") += CLng(item.Attributes.GetNamedItem("quantity").Value)
                    End If
                End If
            End If
        End If
        ' Check child items if they exist
        If item.ChildNodes.Count > 0 Then
            For Each subitem As XmlNode In item.ChildNodes(0).ChildNodes
                Call GetAssetQuantitesFromNode(Root, subitem, categories, groups, Assets)
            Next
        End If
    End Sub

    Private Sub adtOwnedResources_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles adtOwnedResources.ColumnHeaderMouseDown
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
    End Sub

    Private Sub cboAssetSelection_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAssetSelection.TextChanged
        Call Me.DisplayOwnedResources()
    End Sub

    Private Sub btnExportToCSV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportToCSV.Click
        Call Me.ExportToClipboard("Resource Availability for " & CurrentJob.TypeName & " (" & CurrentJob.Runs & " runs)", adtOwnedResources, EveHQ.Core.HQ.Settings.CSVSeparatorChar)
    End Sub

    Private Sub btnExportToTSV_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportToTSV.Click
        Call Me.ExportToClipboard("Resource Availability for " & CurrentJob.TypeName & " (" & CurrentJob.Runs & " runs)", adtOwnedResources, ControlChars.Tab)
    End Sub

    Private Sub ExportToClipboard(ByVal title As String, ByVal sourceList As AdvTree, ByVal sepChar As String)
        Dim str As New StringBuilder
        ' Add a line for the current build job
        str.AppendLine(title)
        str.AppendLine("")
        ' Add some headings
        For c As Integer = 0 To sourceList.Columns.Count - 2
            str.Append(sourceList.Columns(c).Text & sepChar)
        Next
        str.AppendLine(sourceList.Columns(sourceList.Columns.Count - 1).Text)
        ' Add the details
        For Each req As Node In sourceList.Nodes
            For c As Integer = 0 To sourceList.Columns.Count - 2
                str.Append(req.Cells(c).Text & sepChar)
            Next
            str.AppendLine(req.Cells(sourceList.Columns.Count - 1).Text)
        Next
        ' Copy to the clipboard
        Try
            Clipboard.SetText(str.ToString)
        Catch ex As Exception
            MessageBox.Show("Unable to copy Resource Data to the clipboard.", "Clipboard Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub btnAddShortfallToReq_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddShortfallToReq.Click
        ' Set up a new Sortedlist to store the required items
        Dim Orders As SortedList(Of String, Integer) = GetAmountsForRequisition(True)
        ' Setup the Requisition form for HQF and open it
        Dim newReq As New EveHQ.Core.frmAddRequisition("Prism", Orders)
        newReq.ShowDialog()
    End Sub

    Private Sub btnAddAllToReq_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAllToReq.Click
        ' Set up a new Sortedlist to store the required items
        Dim Orders As SortedList(Of String, Integer) = GetAmountsForRequisition(False)
        ' Setup the Requisition form for HQF and open it
        Dim newReq As New EveHQ.Core.frmAddRequisition("Prism", Orders)
        newReq.ShowDialog()
    End Sub

    Private Function GetAmountsForRequisition(ByVal SurplusOnly As Boolean) As SortedList(Of String, Integer)
        Dim ReqOrders As New SortedList(Of String, Integer)
        ' Display the resources owned
        Dim ItemData As EveHQ.Core.EveItem
        Dim reqd, owned, surplus As Long
        For Each itemID As String In GroupResources.Keys
            reqd = GroupResources(itemID)
            If reqd > 0 Then
                If OwnedResources.ContainsKey(itemID) = True Then
                    owned = OwnedResources(itemID).Item("TotalOwned")
                Else
                    owned = 0
                End If
                surplus = reqd - owned
                ItemData = EveHQ.Core.HQ.itemData(itemID)
                If SurplusOnly = False Then
                    ReqOrders.Add(ItemData.Name, CInt(reqd))
                Else
                    If surplus > 0 Then
                        ReqOrders.Add(ItemData.Name, CInt(surplus))
                    End If
                End If
            End If
        Next
        Return ReqOrders
    End Function

    Private Sub chkAdvancedResourceAllocation_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAdvancedResourceAllocation.CheckedChanged
        Call DisplayOwnedResources()
    End Sub

#End Region

#Region "Batch Routines"

    Private Sub GetBatchResources()
        ProductionList.Clear()
        If BatchJob IsNot Nothing Then
            GroupResources.Clear()
            For Each JobName As String In Me.BatchJob.ProductionJobs
                ' Bug 69 : if there is a mismatch between these two collections then an error will occur
                If (Jobs.JobList.ContainsKey((JobName))) Then
                    Call Me.GetResourcesFromJob(Jobs.JobList(JobName))
                End If
            Next
            lblBatchName.Text = "Batch: " & BatchJob.BatchName
        Else
            If CurrentJob IsNot Nothing Then
                If CurrentJob.TypeID <> 0 Then
                    Call Me.GetResourcesFromJob(CurrentJob)
                    lblBatchName.Text = "Batch: From Production Job - " & CurrentJob.JobName
                Else
                    lblBatchName.Text = "Batch: <None>"
                End If
            Else
                lblBatchName.Text = "Batch: <None>"
            End If
        End If
        ' Display the batch resources
        Call Me.DisplayBatchResources()
    End Sub

    Private Sub DisplayBatchResources()
        Dim ItemData As EveHQ.Core.EveItem
        Dim reqd As Long
        Dim BatchValue As Double = 0
        Dim BatchVolume As Double = 0
        adtBatchResources.BeginUpdate()
        adtBatchResources.Nodes.Clear()
        ' batch price request ... yes it goes through the collection more than once, but 1 possible web request is better than dozens.
        Dim costTask As Task(Of Dictionary(Of String, Double)) = Core.DataFunctions.GetMarketPrices(From items In GroupResources.Keys)
        costTask.Wait()
        Dim costs As Dictionary(Of String, Double) = costTask.Result
        For Each itemID As String In GroupResources.Keys
            reqd = GroupResources(itemID)
            If reqd > 0 Then
                ItemData = EveHQ.Core.HQ.itemData(itemID)
                Dim newORes As New Node(ItemData.Name)
                Dim Price As Double = costs(itemID)
                newORes.Cells.Add(New Cell(reqd.ToString("N0")))
                newORes.Cells.Add(New Cell(Price.ToString("N2")))
                newORes.Cells.Add(New Cell((Price * reqd).ToString("N2")))
                newORes.Cells.Add(New Cell((ItemData.Volume * reqd).ToString("N2")))
                BatchValue += (Price * reqd)
                BatchVolume += (ItemData.Volume * reqd)
                adtBatchResources.Nodes.Add(newORes)
                ' TODO: Fix styles
                'If surplus < 0 Then
                '    newORes.SubItems(3).ForeColor = Drawing.Color.Red
                'Else
                '    newORes.SubItems(3).ForeColor = Drawing.Color.Green
                'End If
            End If
        Next
        EveHQ.Core.AdvTreeSorter.Sort(adtBatchResources, 1, True, True)
        adtBatchResources.EndUpdate()
        lblBatchTotals.Text = "Batch Value: " & BatchValue.ToString("N2") & " isk  ,  Batch Volume: " & BatchVolume.ToString("N2") & " m³"
        Call Me.DisplayProductionList()
    End Sub

    Private Sub DisplayProductionList()
        Dim ItemData As EveHQ.Core.EveItem
        adtProductionList.BeginUpdate()
        adtProductionList.Nodes.Clear()
        Dim priceTask As Task(Of Dictionary(Of String, Double)) = Core.DataFunctions.GetMarketPrices(From pi In ProductionList.Values Select pi.ItemID)
        priceTask.Wait()
        Dim prices As Dictionary(Of String, Double) = priceTask.Result
        For Each PI As ProductionItem In ProductionList.Values
            If PI.IsBuild = True Then
                ItemData = EveHQ.Core.HQ.itemData(PI.ItemID)
                If ItemData.Category <> 9 Then
                    Dim NewPI As New Node(ItemData.Name)
                    Dim Price As Double = prices(PI.ItemID)
                    NewPI.Cells.Add(New Cell(PI.Quantity.ToString("N0")))
                    NewPI.Cells.Add(New Cell(Price.ToString("N2")))
                    NewPI.Cells.Add(New Cell((Price * PI.Quantity).ToString("N2")))
                    adtProductionList.Nodes.Add(NewPI)
                End If
            End If
        Next
        EveHQ.Core.AdvTreeSorter.Sort(adtProductionList, 1, True, True)
        adtProductionList.EndUpdate()
    End Sub

    Private Sub adtProductionList_ColumnHeaderMouseUp(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles adtProductionList.ColumnHeaderMouseUp
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
    End Sub

#End Region

#Region "BPMEControl Functions"

    Private Sub UpdateResources()
        Call Me.DisplayRequiredResources()
        RaiseEvent ProductionResourcesChanged()
    End Sub

#End Region

#Region "Pricing Update Routines"

    Private Sub btnAlterResourcePrices_Click(sender As System.Object, e As System.EventArgs) Handles btnAlterResourcePrices.Click

        Call Me.ModifyPrices()

    End Sub

    Private Sub btnUpdateBatchPrices_Click(sender As System.Object, e As System.EventArgs) Handles btnUpdateBatchPrices.Click

        Call Me.ModifyPrices()

    End Sub

    Private Sub ModifyPrices()

        ' Collect a list of resources from the Production Panel
        Dim itemIDs As New List(Of String)

        Call AddPricingResources(CurrentJob, itemIDs)

        ' Create a new price form instance
        Dim modifyPrices As New Core.frmModifyPrices(itemIDs)

        ' Show the form
        modifyPrices.ShowDialog()

        ' Check if we need to update prices
        If modifyPrices.DialogResult = DialogResult.OK Then
            CurrentJob.RecalculateResourceRequirements()
            Call Me.DisplayRequiredResources()
            RaiseEvent ProductionResourcesChanged()
        End If

        ' Dispose of the form
        modifyPrices.Dispose()

    End Sub

    Private Sub AddPricingResources(ByVal parentJob As Job, ByRef itemIDs As List(Of String))

        For Each resource As JobResource In parentJob.Resources.Values
            If resource.TypeCategory <> 16 Then
                If ItemIDs.Contains(resource.TypeID.ToString) = False Then
                    ItemIDs.Add(resource.TypeID.ToString)
                End If
            End If
        Next
        For Each subJob As Job In parentJob.SubJobs.Values
            ' This is another production job
            Call Me.AddPricingResources(subJob, ItemIDs)
        Next

    End Sub

#End Region


End Class

Public Class ProductionItem

    Dim cKey As String
    Public ReadOnly Property Key As String
        Get
            Return cKey
        End Get
    End Property

    Public Property ItemID As String

    Public Property IsBuild As Boolean

    Public Property Quantity As Long

    Public Sub New(ID As String, Build As Boolean, Qty As Long)
        cKey = ID & "_" & Build.ToString
        ItemID = ID
        IsBuild = Build
        Quantity = Qty
    End Sub

End Class
