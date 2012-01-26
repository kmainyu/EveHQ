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
Imports DevComponents.AdvTree

Public Class frmQuickProduction

    Dim FormStartup As Boolean = True

#Region "Constructors"

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Call Me.DisplayAllBlueprints()

    End Sub

    Public Sub New(BlueprintName As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Call Me.DisplayAllBlueprints()
        If cboBPs.Items.Contains(BlueprintName) Then
            cboBPs.SelectedItem = BlueprintName
        End If

    End Sub

#End Region

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

    Private Sub CalculateMaterials()
        Dim bpID As String = EveHQ.Core.HQ.itemList(cboBPs.SelectedItem.ToString.Trim)
        Dim CurrentBP As BlueprintSelection = BlueprintSelection.CopyFromBlueprint(PlugInData.Blueprints(bpID))
        CurrentBP.MELevel = nudMELevel.Value
        CurrentBP.PELevel = nudPELevel.Value
        CurrentBP.Runs = -1
        CurrentBP.AssetID = CLng(bpID)
        Dim CurrentJob As ProductionJob = CurrentBP.CreateProductionJob("", "", 5, 5, 0, CStr(CurrentBP.MELevel), CStr(CurrentBP.PELevel), nudCopyRuns.Value, Nothing, False)
        Call Me.DisplayMaterials(CurrentJob)
    End Sub

    Private Sub DisplayMaterials(CurrentJob As ProductionJob)
        adtResources.BeginUpdate()
        adtResources.Nodes.Clear()
        Dim maxProducableUnits As Long = -1
        Dim UnitMaterial As Double = 0
        Dim UnitWaste As Double = 0
        If CurrentJob IsNot Nothing Then
            For Each resource As Object In CurrentJob.RequiredResources.Values
                If TypeOf (resource) Is RequiredResource Then
                    ' This is a resource so add it
                    Dim rResource As RequiredResource = CType(resource, RequiredResource)
                    If rResource.TypeCategory <> 16 Then
                        Dim perfectRaw As Integer = CInt(rResource.PerfectUnits)
                        Dim waste As Integer = CInt(rResource.WasteUnits)
                        Dim total As Integer = perfectRaw + waste
                        Dim price As Double = EveHQ.Core.DataFunctions.GetPrice(CStr(rResource.TypeID))
                        Dim value As Double = total * price
                        ' Add a new list view item
                        If total > 0 Then
                            Dim newRes As New Node(rResource.TypeName)
                            newRes.TextDisplayFormat = "N0"
                            ' Calculate costs
                            UnitMaterial += value
                            UnitWaste += waste * price
                            Dim PerfectTotal As Long = CLng(perfectRaw) * CLng(CurrentJob.Runs)
                            Dim WasteTotal As Long = CLng(waste) * CLng(CurrentJob.Runs)
                            Dim TotalTotal As Long = CLng(total) * CLng(CurrentJob.Runs)
                            newRes.Cells.Add(New Cell(TotalTotal.ToString))
                            newRes.Cells(1).TextDisplayFormat = "N0"
                            adtResources.Nodes.Add(newRes)
                        End If
                    End If
                End If
            Next
        End If
        EveHQ.Core.AdvTreeSorter.Sort(adtResources, 2, False, True)
        adtResources.EndUpdate()
    End Sub

    Private Sub cboBPs_SelectedIndexChanged(sender As System.Object, e As System.EventArgs) Handles cboBPs.SelectedIndexChanged
        Call Me.CalculateMaterials()
    End Sub

    Private Sub nudMELevel_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nudMELevel.ValueChanged
        Call Me.CalculateMaterials()
    End Sub

    Private Sub nudPELevel_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nudPELevel.ValueChanged
        Call Me.CalculateMaterials()
    End Sub

    Private Sub nudCopyRuns_ValueChanged(sender As System.Object, e As System.EventArgs) Handles nudCopyRuns.ValueChanged
        Call Me.CalculateMaterials()
    End Sub

   
End Class