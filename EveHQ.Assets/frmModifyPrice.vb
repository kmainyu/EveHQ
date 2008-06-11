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
        EveHQ.Core.HQ.CustomPriceList(itemID) = txtNewPrice.Text

        ' Save the prices
        Dim culture As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB")
        Dim sw As New IO.StreamWriter(EveHQ.Core.HQ.cacheFolder & "\CustomPrices.txt")
        Dim price As Double = 0
        For Each marketPrice As String In EveHQ.Core.HQ.CustomPriceList.Keys
            price = Double.Parse(EveHQ.Core.HQ.CustomPriceList(marketPrice).ToString, Globalization.NumberStyles.Number)
            sw.WriteLine(marketPrice & "," & price.ToString(culture))
        Next
        sw.Flush()
        sw.Close()

        ' Close the form
        Me.Close()
    End Sub
End Class