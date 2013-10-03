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
Imports EveHQ.EveData

Public Class frmAddCustomBP

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
        ' Load the Blueprints into the combo box
        cboBPs.BeginUpdate()
        cboBPs.Items.Clear()
        cboBPs.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboBPs.AutoCompleteSource = AutoCompleteSource.ListItems
        For Each newBP As EveData.Blueprint In StaticData.Blueprints.Values
            cboBPs.Items.Add(StaticData.Types(newBP.Id))
        Next
        cboBPs.Sorted = True
        cboBPs.EndUpdate()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
        ' Fetch the ownerBPs if it exists
        Dim ownerBPs As New SortedList(Of Integer, BlueprintAsset)
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

        DialogResult = Windows.Forms.DialogResult.OK
        Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
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
            pbBP.ImageLocation = Core.ImageHandler.GetImageLocation(bpID)
        End If
    End Sub

    Private Sub nudMELevel_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudMELevel.ValueChanged
        _currentBP.MELevel = CInt(nudMELevel.Value)
    End Sub

    Private Sub nudPELevel_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudPELevel.ValueChanged
        _currentBP.PELevel = CInt(nudPELevel.Value)
    End Sub
End Class