Public Class frmEditBPDetails

    Dim cAssetIDs As New ArrayList
    Dim cOwnerName As String
    Dim CurrentBP As New BlueprintAsset

    Public Property AssetIDs() As ArrayList
        Get
            Return cAssetIDs
        End Get
        Set(ByVal value As ArrayList)
            cAssetIDs = value
            Call Me.UpdateCurrentBP()
        End Set
    End Property

    Public Property OwnerName() As String
        Get
            Return cOwnerName
        End Get
        Set(ByVal value As String)
            cOwnerName = value
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
        Dim cAssetID As String = cAssetIDs(0).ToString

        ' Fetch the ownerBPs if it exists
        CurrentBP = PlugInData.BlueprintAssets(cOwnerName).Item(cAssetID)

        ' Get the image
        pbBP.ImageLocation = EveHQ.Core.ImageHandler.GetImageLocation(CurrentBP.TypeID, EveHQ.Core.ImageHandler.ImageType.Blueprints)


        If cAssetIDs.Count = 1 Then

            ' Update the name and assetID details
            lblAssetID.Text = "AssetID: " & cAssetID
            lblBPName.Text = PlugInData.Blueprints(CurrentBP.TypeID).Name

            ' Update the current BP Info
            lblCurrentME.Text = CurrentBP.MELevel.ToString
            lblCurrentPE.Text = CurrentBP.PELevel.ToString
            lblCurrentRuns.Text = CurrentBP.Runs.ToString
            lblCurrentStatus.Text = [Enum].GetName(GetType(BPStatus), CurrentBP.Status)
            cboStatus.SelectedItem = lblCurrentStatus.Text
            nudMELevel.Value = CurrentBP.MELevel
            nudPELevel.Value = CurrentBP.PELevel
            nudRuns.Value = CurrentBP.Runs

            ' Check for User Asset
            If CurrentBP.BPType = BPType.User Then
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

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        For Each cAssetID As String In cAssetIDs
            CurrentBP = PlugInData.BlueprintAssets(cOwnerName).Item(cAssetID)
            CurrentBP.MELevel = CInt(nudMELevel.Value)
            CurrentBP.PELevel = CInt(nudPELevel.Value)
            ' Only update runs on non-user BPs
            If CurrentBP.BPType <> BPType.User Then
                CurrentBP.Runs = CInt(nudRuns.Value)
                CurrentBP.Status = CInt([Enum].Parse(GetType(BPStatus), cboStatus.SelectedItem.ToString))
                If CurrentBP.Runs = -1 Then
                    CurrentBP.BPType = BPType.BPO
                Else
                    CurrentBP.BPType = BPType.BPC
                    If CurrentBP.Runs = 0 Then
                        CurrentBP.Status = BPStatus.Exhausted
                    End If
                End If
            End If
        Next
        Me.Close()
    End Sub

    Private Sub cboStatus_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboStatus.SelectedIndexChanged
        If cboStatus.SelectedIndex = BPStatus.Exhausted Then
            nudRuns.Value = 0
        Else
            If nudRuns.Value = 0 Then
                nudRuns.Value = 1
            End If
        End If
    End Sub

    Private Sub nudRuns_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudRuns.ValueChanged
        If nudRuns.Value = 0 Then
            cboStatus.SelectedIndex = BPStatus.Exhausted
        Else
            If cboStatus.SelectedIndex = BPStatus.Exhausted Then
                cboStatus.SelectedIndex = BPStatus.Present
            End If
        End If
    End Sub
End Class