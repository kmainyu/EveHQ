Imports System.Windows.Forms

Public Class frmModifyPrice

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        ' Check if everything is filled out as it should be
        If IsNumeric(txtNewPrice.Text) = False Then
            MessageBox.Show("You must enter a valid price for this item.", "Error In Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If CDbl(txtNewPrice.Text) < 0 Then
            MessageBox.Show("You cannot enter a negative price for this item.", "Error In Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Retrieve the txt tag which = itemID
        Dim itemID As String = txtNewPrice.Tag.ToString

        Select Case Me.Tag.ToString
            Case "Add"
                ' Add the custom price
                Call EveHQ.Core.DataFunctions.AddCustomPrice(itemID, CDbl(txtNewPrice.Text), False)
            Case "Edit"
                ' Edit the custom price
                Call EveHQ.Core.DataFunctions.EditCustomPrice(itemID, CDbl(txtNewPrice.Text), False)
        End Select
        
        ' Close the form
        Me.Close()
    End Sub
End Class