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

Imports System.Threading.Tasks
Imports System.Windows.Forms
Imports DevComponents.AdvTree
Imports EveHQ.Core
Imports EveHQ.EveData
Imports EveHQ.Prism.BPCalc

Namespace Forms

    Public Class FrmQuickProduction

#Region "Constructors"

        Public Sub New()

            ' This call is required by the designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Call DisplayAllBlueprints()

        End Sub

        Public Sub New(blueprintName As String)

            ' This call is required by the designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Call DisplayAllBlueprints()
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
            For Each newBP As EveData.Blueprint In StaticData.Blueprints.Values
                If StaticData.Types.ContainsKey(newBP.Id) Then
                    cboBPs.Items.Add(StaticData.Types(newBP.Id).Name)
                End If
            Next
            cboBPs.Sorted = True
            cboBPs.EndUpdate()
        End Sub

        Private Sub CalculateMaterials()
            Dim bpID As Integer = StaticData.TypeNames(cboBPs.SelectedItem.ToString.Trim)
            Dim currentBP As OwnedBlueprint = OwnedBlueprint.CopyFromBlueprint(StaticData.Blueprints(bpID))
            currentBP.MELevel = nudMELevel.Value
            currentBP.PELevel = nudPELevel.Value
            currentBP.Runs = -1
            currentBP.AssetID = CLng(bpID)
            Dim currentJob As Job = currentBP.CreateProductionJob("", "", 5, 5, 0, CStr(currentBP.MELevel), CStr(currentBP.PELevel), nudCopyRuns.Value, Nothing, False)
            Call DisplayMaterials(currentJob)
        End Sub

        Private Sub DisplayMaterials(currentJob As Job)
            adtResources.BeginUpdate()
            adtResources.Nodes.Clear()
            If CurrentJob IsNot Nothing Then
                Dim priceTask As Task(Of Dictionary(Of Integer, Double)) = DataFunctions.GetMarketPrices(From r In currentJob.Resources.Values Where TypeOf (r) Is JobResource Select r.TypeID)
                priceTask.Wait()
                For Each resource As JobResource In CurrentJob.Resources.Values
                    ' This is a resource so add it
                    If resource.TypeCategory <> 16 Then
                        Dim perfectRaw As Integer = CInt(resource.PerfectUnits)
                        Dim waste As Integer = CInt(resource.WasteUnits)
                        Dim total As Integer = perfectRaw + waste
                        ' Add a new list view item
                        If total > 0 Then
                            Dim newRes As New Node(resource.TypeName)
                            newRes.TextDisplayFormat = "N0"
                            ' Calculate costs
                            Dim totalTotal As Long = CLng(total) * CLng(CurrentJob.Runs)
                            newRes.Cells.Add(New Cell(totalTotal.ToString))
                            newRes.Cells(1).TextDisplayFormat = "N0"
                            adtResources.Nodes.Add(newRes)
                        End If
                    End If
                Next
            End If
            AdvTreeSorter.Sort(adtResources, 2, False, True)
            adtResources.EndUpdate()
        End Sub

        Private Sub cboBPs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboBPs.SelectedIndexChanged
            Call CalculateMaterials()
        End Sub

        Private Sub nudMELevel_ValueChanged(sender As Object, e As EventArgs) Handles nudMELevel.ValueChanged
            Call CalculateMaterials()
        End Sub

        Private Sub nudPELevel_ValueChanged(sender As Object, e As EventArgs) Handles nudPELevel.ValueChanged
            Call CalculateMaterials()
        End Sub

        Private Sub nudCopyRuns_ValueChanged(sender As Object, e As EventArgs) Handles nudCopyRuns.ValueChanged
            Call CalculateMaterials()
        End Sub
        
    End Class
End NameSpace