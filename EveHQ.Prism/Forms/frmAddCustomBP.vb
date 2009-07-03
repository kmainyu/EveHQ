﻿Imports System.Windows.Forms

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
        For Each newBP As Blueprint In PlugInData.Blueprints.Values
            cboBPs.Items.Add(newBP.Name)
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
        Dim bpID As String = CStr(EveHQ.Core.HQ.itemList(cboBPs.SelectedItem.ToString.Trim))
        currentBP = New BlueprintAsset
        currentBP.TypeID = bpID
        currentBP.AssetID = bpID
        currentBP.MELevel = CInt(nudMELevel.Value)
        currentBP.PELevel = CInt(nudPELevel.Value)
        currentBP.Runs = -1
        currentBP.BPType = BPType.User
        currentBP.Status = BPStatus.Present
        ' First get the image
        pbBP.ImageLocation = EveHQ.Core.ImageHandler.GetImageLocation(bpID, EveHQ.Core.ImageHandler.ImageType.Blueprints)
    End Sub

    Private Sub nudMELevel_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudMELevel.ValueChanged
        currentBP.MELevel = CInt(nudMELevel.Value)
    End Sub

    Private Sub nudPELevel_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudPELevel.ValueChanged
        currentBP.PELevel = CInt(nudPELevel.Value)
    End Sub
End Class