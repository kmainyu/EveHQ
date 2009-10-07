Imports System.Windows.Forms

Public Class frmVoid

    Private Sub frmVoid_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Me.LoadWHData()
        Call Me.LoadWHSystemData()
    End Sub

    Private Sub LoadWHData()
        cboWHType.BeginUpdate()
        cboWHType.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboWHType.AutoCompleteSource = AutoCompleteSource.CustomSource
        cboWHType.Items.Clear()
        For Each WH As Void.WormHole In VoidData.Wormholes.Values
            cboWHType.Items.Add(WH.Name)
            cboWHType.AutoCompleteCustomSource.Add(WH.Name)
        Next
        cboWHType.EndUpdate()
    End Sub

    Private Sub LoadWHSystemData()
        ' Load up pilot information
        cboWHSystem.BeginUpdate()
        cboWHSystem.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboWHSystem.AutoCompleteSource = AutoCompleteSource.CustomSource
        cboWHSystem.Items.Clear()
        For Each WH As Void.WormholeSystem In VoidData.WormholeSystems.Values
            cboWHSystem.Items.Add(WH.Name)
            cboWHSystem.AutoCompleteCustomSource.Add(WH.Name)
        Next
        cboWHSystem.EndUpdate()
    End Sub

    Private Sub cboWHType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWHType.SelectedIndexChanged
        ' Update the WH Details
        If VoidData.Wormholes.ContainsKey(cboWHType.SelectedItem.ToString) = True Then
            Dim WH As Void.WormHole = VoidData.Wormholes(cboWHType.SelectedItem.ToString)
            If WH.Name <> "K162" Then
                lblTargetSystemClass.Text = WH.TargetClass
                Select Case CInt(WH.TargetClass)
                    Case 1 To 6
                        lblTargetSystemClass.Text &= " (Wormhole Class " & WH.TargetClass & ")"
                    Case 7
                        lblTargetSystemClass.Text &= " (High Security Space)"
                    Case 8
                        lblTargetSystemClass.Text &= " (Low Security Space)"
                    Case 9
                        lblTargetSystemClass.Text &= " (Null Security Space)"
                End Select
                lblMaxJumpableMass.Text = CLng(WH.MaxJumpableMass).ToString("N0") & " kg"
                lblMaxTotalMass.Text = CLng(WH.MaxMassCapacity).ToString("N0") & " kg"
                lblStabilityWindow.Text = (CDbl(WH.MaxStabilityWindow) / 60).ToString("N0") & " hours"
            Else
                lblTargetSystemClass.Text = "n/a (Return wormhole)"
                lblMaxJumpableMass.Text = "n/a"
                lblMaxTotalMass.Text = "n/a"
                lblStabilityWindow.Text = "n/a"
            End If
        End If
    End Sub

    Private Sub cboWHSystem_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboWHSystem.SelectedIndexChanged
        ' Update the WH System Details
        If VoidData.WormholeSystems.ContainsKey(cboWHSystem.SelectedItem.ToString) = True Then
            Dim WH As Void.WormholeSystem = VoidData.WormholeSystems(cboWHSystem.SelectedItem.ToString)
            lblAnomalyName.Text = WH.WEffect
            lblSystemClass.Text = WH.WClass
        End If
    End Sub
End Class