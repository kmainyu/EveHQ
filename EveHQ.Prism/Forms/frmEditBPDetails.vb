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

Public Class frmEditBPDetails

    Dim _assetIDs As New List(Of Integer)
    Dim _ownerName As String
    Dim _currentBP As New BlueprintAsset

    Public Property AssetIDs() As List(Of Integer)
        Get
            Return _assetIDs
        End Get
        Set(ByVal value As List(Of Integer))
            _assetIDs = value
            Call UpdateCurrentBP()
        End Set
    End Property

    Public Property OwnerName() As String
        Get
            Return _ownerName
        End Get
        Set(ByVal value As String)
            _ownerName = value
        End Set
    End Property

    Private Sub UpdateCurrentBP()

        ' Load the Enum data
        cboStatus.BeginUpdate()
        cboStatus.Items.Clear()
        For Each status As String In [Enum].GetNames(GetType(BPStatus))
            cboStatus.Items.Add(status)
        Next
        cboStatus.EndUpdate()

        ' Loads a single Blueprint
        Dim cAssetID As Integer = _assetIDs(0)

        ' Fetch the ownerBPs if it exists
        _currentBP = PlugInData.BlueprintAssets(_ownerName).Item(cAssetID)

        ' Get the image
        pbBP.ImageLocation = Core.ImageHandler.GetImageLocation(CInt(_currentBP.TypeID))

        If _assetIDs.Count = 1 Then

            ' Update the name and assetID details
            lblAssetID.Text = "AssetID: " & cAssetID
            lblBPName.Text = EveData.StaticData.Types(CInt(_currentBP.TypeID)).Name

            ' Update the current BP Info
            lblCurrentME.Text = _currentBP.MELevel.ToString
            lblCurrentPE.Text = _currentBP.PELevel.ToString
            lblCurrentRuns.Text = _currentBP.Runs.ToString
            lblCurrentStatus.Text = [Enum].GetName(GetType(BPStatus), _currentBP.Status)
            cboStatus.SelectedItem = lblCurrentStatus.Text
            nudMELevel.Value = _currentBP.MELevel
            nudPELevel.Value = _currentBP.PELevel
            nudRuns.Value = _currentBP.Runs

            ' Check for User Asset
            If _currentBP.BPType = BPType.User Then
                lblAssetID.Text &= " (Custom Blueprint)"
                nudRuns.Enabled = False
                cboStatus.Enabled = False
            Else
                nudRuns.Enabled = True
                cboStatus.Enabled = True
            End If
        Else

            ' Handles multiple Blueprints
            lblAssetID.Text = "Warning: This will set all selected Blueprints!"
            lblBPName.Text = "<Mulitple Blueprints>"
            lblCurrentME.Text = "<Multiple>"
            lblCurrentPE.Text = "<Multiple>"
            lblCurrentRuns.Text = "<Multiple>"
            lblCurrentStatus.Text = "<Multiple>"
            cboStatus.SelectedIndex = 0
            nudMELevel.Value = 0
            nudPELevel.Value = 0
            nudRuns.Value = -1
        End If

    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
        For Each cAssetID As Integer In _assetIDs
            _currentBP = PlugInData.BlueprintAssets(_ownerName).Item(cAssetID)
            _currentBP.MELevel = CInt(nudMELevel.Value)
            _currentBP.PELevel = CInt(nudPELevel.Value)
            ' Only update runs on non-user BPs
            If _currentBP.BPType <> BPType.User Then
                _currentBP.Runs = CInt(nudRuns.Value)
                _currentBP.Status = CInt([Enum].Parse(GetType(BPStatus), cboStatus.SelectedItem.ToString))
                If _currentBP.Runs = -1 Then
                    _currentBP.BPType = BPType.Original
                Else
                    _currentBP.BPType = BPType.Copy
                    If _currentBP.Runs = 0 Then
                        _currentBP.Status = BPStatus.Exhausted
                    End If
                End If
            End If
        Next
        Close()
    End Sub

    Private Sub cboStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboStatus.SelectedIndexChanged
        If cboStatus.SelectedIndex = BPStatus.Exhausted Then
            nudRuns.Value = 0
        Else
            If nudRuns.Value = 0 Then
                nudRuns.Value = 1
            End If
        End If
    End Sub

    Private Sub nudRuns_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles nudRuns.ValueChanged
        If nudRuns.Value = 0 Then
            cboStatus.SelectedIndex = BPStatus.Exhausted
        Else
            If cboStatus.SelectedIndex = BPStatus.Exhausted Then
                cboStatus.SelectedIndex = BPStatus.Present
            End If
        End If
    End Sub
End Class