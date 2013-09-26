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

    Dim currentBP As New BlueprintAsset
    Dim cBPOwner As String = ""
    Public Property BPOwner() As String
        Get
            Return cBPOwner
        End Get
        Set(ByVal value As String)
            cBPOwner = value
        End Set
    End Property

    Private Sub frmAddCustomBP_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Me.DisplayAllBlueprints()
    End Sub

    Private Sub DisplayAllBlueprints()
        ' Load the Blueprints into the combo box
        cboBPs.BeginUpdate()
        cboBPs.Items.Clear()
        cboBPs.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboBPs.AutoCompleteSource = AutoCompleteSource.ListItems
        For Each newBP As Blueprint In StaticData.Blueprints.Values
            cboBPs.Items.Add(StaticData.Types(newBP.ID.ToString))
        Next
        cboBPs.Sorted = True
        cboBPs.EndUpdate()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        ' Fetch the ownerBPs if it exists
        Dim ownerBPs As New SortedList(Of String, BlueprintAsset)
        If PlugInData.BlueprintAssets.ContainsKey(cBPOwner) = True Then
            ownerBPs = PlugInData.BlueprintAssets(cBPOwner)
        Else
            PlugInData.BlueprintAssets.Add(cBPOwner, ownerBPs)
        End If

        ' Check for the assetID in the owner's assets
        If ownerBPs.ContainsKey(currentBP.AssetID) = True Then
            MessageBox.Show(cBPOwner & " already has this custom blueprint in their collection and cannot be added again.", "Duplicate Custom Blueprint", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Add the custom BPO into the owner's assets
        ownerBPs.Add(currentBP.AssetID, currentBP)

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub cboBPs_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboBPs.SelectedIndexChanged
        ' This is a standard blueprint
        If EveHQ.Core.HQ.itemList.ContainsKey(cboBPs.SelectedItem.ToString.Trim) = True Then
            Dim bpID As String = EveHQ.Core.HQ.itemList(cboBPs.SelectedItem.ToString.Trim)
            currentBP = New BlueprintAsset
            currentBP.TypeID = bpID
            currentBP.AssetID = bpID
            currentBP.MELevel = CInt(nudMELevel.Value)
            currentBP.PELevel = CInt(nudPELevel.Value)
            currentBP.Runs = -1
            currentBP.BPType = BPType.User
            currentBP.Status = BPStatus.Present
            ' First get the image
            pbBP.ImageLocation = EveHQ.Core.ImageHandler.GetImageLocation(bpID)
        End If
    End Sub

    Private Sub nudMELevel_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudMELevel.ValueChanged
        currentBP.MELevel = CInt(nudMELevel.Value)
    End Sub

    Private Sub nudPELevel_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudPELevel.ValueChanged
        currentBP.PELevel = CInt(nudPELevel.Value)
    End Sub
End Class