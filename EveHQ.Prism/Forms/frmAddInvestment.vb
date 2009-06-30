Imports System.Windows.Forms

Public Class frmAddInvestment

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        ' Check if everything is filled out as it should be
        If txtInvestmentName.Text = "" Then
            MessageBox.Show("You must enter a valid Investment Name to continue", "Error Creating Investment", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If cboOwner.SelectedItem Is Nothing Then
            MessageBox.Show("You must enter a valid Investment Owner to continue", "Error Creating Investment", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If cboType.SelectedItem Is Nothing Then
            MessageBox.Show("You must enter a valid Investment Type to continue", "Error Creating Investment", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        ' Are we adding or are we editing?
        If Portfolio.Investments.ContainsKey(CLng(txtInvestmentID.Text)) = True Then
            ' We are editing
            Dim newInvestment As Investment = CType(Portfolio.Investments(CLng(txtInvestmentID.Text)), Investment)
            newInvestment.ID = CLng(txtInvestmentID.Text)
            newInvestment.Name = txtInvestmentName.Text
            newInvestment.Description = txtDescription.Text
            newInvestment.DateCreated = CDate(txtDateCreated.Text)
            newInvestment.Owner = cboOwner.SelectedItem.ToString
            newInvestment.Type = cboType.SelectedIndex
            newInvestment.ValueIsCost = chkValueIsCost.Checked
            Me.Close()
        Else
            ' We are adding
            Dim newInvestment As New Investment
            newInvestment.ID = CLng(txtInvestmentID.Text)
            newInvestment.Name = txtInvestmentName.Text
            newInvestment.Description = txtDescription.Text
            newInvestment.DateCreated = CDate(txtDateCreated.Text)
            newInvestment.Owner = cboOwner.SelectedItem.ToString
            newInvestment.Type = cboType.SelectedIndex
            newInvestment.ValueIsCost = chkValueIsCost.Checked
            Portfolio.Investments.Add(newInvestment.ID, newInvestment)
            Me.Close()
        End If
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ' Load in the owners into the combobox
        cboOwner.BeginUpdate()
        cboOwner.Items.Clear()
        For Each selPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            cboOwner.Items.Add(selPilot.Name)
            If cboOwner.Items.Contains(selPilot.Corp) = False Then
                cboOwner.Items.Add(selPilot.Corp)
            End If
        Next
        cboOwner.EndUpdate()

    End Sub
End Class