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

Imports System.Windows.Forms
Imports EveHQ.Core
Imports EveHQ.EveData
Imports EveHQ.Prism.Classes

Namespace Forms

    Public Class FrmAddCustomBP

        Dim _currentBP As New BlueprintAsset
        Dim _bpOwner As String = ""
        Public Property BPOwner() As String
            Get
                Return _bpOwner
            End Get
            Set(ByVal value As String)
                _bpOwner = value
            End Set
        End Property

        Private Sub frmAddCustomBP_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            Call DisplayAllBlueprints()
        End Sub

        Private Sub DisplayAllBlueprints()
            ' Load the published Blueprints into the combo box
            cboBPs.BeginUpdate()
            cboBPs.Items.Clear()
            cboBPs.AutoCompleteSource = AutoCompleteSource.ListItems
            cboBPs.AutoCompleteMode = AutoCompleteMode.SuggestAppend
            For Each newBP As EveData.Blueprint In StaticData.Blueprints.Values
                If StaticData.Types(newBP.Id).Published = True Then
                    cboBPs.Items.Add(StaticData.Types(newBP.Id).Name)
                End If
            Next
            cboBPs.Sorted = True
            cboBPs.EndUpdate()
        End Sub

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            ' Fetch the ownerBPs if it exists
            Dim ownerBPs As New SortedList(Of Long, BlueprintAsset)
            If PlugInData.BlueprintAssets.ContainsKey(_bpOwner) = True Then
                ownerBPs = PlugInData.BlueprintAssets(_bpOwner)
            Else
                PlugInData.BlueprintAssets.Add(_bpOwner, ownerBPs)
            End If

            ' Check for the assetID in the owner's assets
            If ownerBPs.ContainsKey(CInt(_currentBP.AssetID)) = True Then
                MessageBox.Show(_bpOwner & " already has this custom blueprint in their collection and cannot be added again.", "Duplicate Custom Blueprint", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' Add the custom BPO into the owner's assets
            ownerBPs.Add(CInt(_currentBP.AssetID), _currentBP)

            DialogResult = DialogResult.OK
            Close()
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            DialogResult = DialogResult.Cancel
            Close()
        End Sub

        Private Sub cboBPs_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboBPs.SelectedIndexChanged
            ' This is a standard blueprint
            If StaticData.TypeNames.ContainsKey(cboBPs.SelectedItem.ToString.Trim) = True Then
                Dim bpID As Integer = CInt(StaticData.TypeNames(cboBPs.SelectedItem.ToString.Trim))
                _currentBP = New BlueprintAsset
                _currentBP.TypeID = CStr(bpID)
                _currentBP.AssetID = CStr(bpID)
                _currentBP.MELevel = CInt(nudMELevel.Value)
                _currentBP.PELevel = CInt(nudPELevel.Value)
                _currentBP.Runs = -1
                _currentBP.BPType = BPType.User
                _currentBP.Status = BPStatus.Present
                ' First get the image
                pbBP.ImageLocation = ImageHandler.GetImageLocation(bpID)
            End If
        End Sub

        Private Sub nudMELevel_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudMELevel.ValueChanged
            _currentBP.MELevel = CInt(nudMELevel.Value)
        End Sub

        Private Sub nudPELevel_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudPELevel.ValueChanged
            _currentBP.PELevel = CInt(nudPELevel.Value)
        End Sub
    End Class
End NameSpace