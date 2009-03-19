Imports System.Windows.Forms

Public Class frmAddTransaction

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        ' Check if everything is filled out as it should be
        If cboType.SelectedItem Is Nothing Then
            MessageBox.Show("You must enter a valid Transaction Type to continue.", "Error Creating Transaction", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If cboType.SelectedIndex < 2 Then
            If IsNumeric(txtQuantity.Text) = False Then
                MessageBox.Show("You must enter a valid Transaction Quantity to continue.", "Error Creating Transaction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            If CDbl(txtQuantity.Text) <= 0 Then
                MessageBox.Show("You cannot enter a negative or nil Transaction Quantity.", "Error Creating Transaction", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
        If IsNumeric(txtUnitValue.Text) = False Then
            MessageBox.Show("You must enter a valid Transaction Unit Value to continue.", "Error Creating Transaction", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If CDbl(txtUnitValue.Text) < 0 Then
            MessageBox.Show("You cannot enter a negative Transaction Unit Value.", "Error Creating Transaction", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        ' Can we actually dispose of the quantity we are asking?
        Dim newInvestment As Investment = CType(Portfolio.Investments(CLng(txtInvestmentID.Text)), Investment)
        If cboType.SelectedItem.ToString = "Sale" Then
            If CDbl(txtQuantity.Text) > newInvestment.CurrentQuantity Then
                MessageBox.Show("You don't have the quantities to sell this amount. Please revise the transaction", "Error Creating Transaction", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
        End If
        ' Save the Investment
        Dim newTransaction As New InvestmentTransaction
        ' Check if we are adding or editing the investment
        If Portfolio.Transactions.ContainsKey(CLng(txtTransactionID.Text)) = False Then
            ' We are adding a transaction
            newTransaction.ID = CLng(txtTransactionID.Text)
            newTransaction.InvestmentID = CLng(txtInvestmentID.Text)
            newTransaction.TransDate = CDate(dtpDate.Value)
            newTransaction.Type = CInt(Portfolio.TransactionTypes(cboType.SelectedItem.ToString))
            newTransaction.Quantity = CDbl(txtQuantity.Text)
            newTransaction.UnitValue = CDbl(txtUnitValue.Text)
            ' Add this to the list of transactions
            Portfolio.Transactions.Add(newTransaction.ID, newTransaction)
            ' Add the transaction to the investment
            newInvestment.Transactions.Add(newTransaction.ID, newTransaction)
            ' Calculate the effect of the new transaction on the investment
            Select Case newTransaction.Type
                Case 0 ' Purchase
                    Dim totalCost As Double = newInvestment.CurrentQuantity * newInvestment.CurrentCost
                    totalCost += newTransaction.Quantity * newTransaction.UnitValue
                    newInvestment.CurrentQuantity += newTransaction.Quantity
                    newInvestment.CurrentCost = totalCost / newInvestment.CurrentQuantity
                Case 1 ' Sale
                    newInvestment.CurrentQuantity -= newTransaction.Quantity
                    Dim profits As Double = (newTransaction.UnitValue - newInvestment.CurrentCost) * newTransaction.Quantity
                    newInvestment.CurrentProfits += profits
                Case 2 ' Valuation
                    ' Disregard the quantity
                    newInvestment.CurrentValue = newTransaction.UnitValue
                    newInvestment.LastValuation = newTransaction.TransDate
                Case 3 ' Income
                    newInvestment.CurrentIncome += newTransaction.UnitValue
                    If newInvestment.Type = InvestmentType.Cash Then
                        newInvestment.TotalCostsForYield += newInvestment.CurrentCost
                    Else
                        newInvestment.TotalCostsForYield += (newInvestment.CurrentCost * newInvestment.CurrentQuantity)
                    End If
                Case 4 ' Cost
                    newInvestment.CurrentCosts += newTransaction.UnitValue
                Case 5 ' Income retained
                    newInvestment.CurrentIncome += newTransaction.UnitValue
                    If newInvestment.Type = InvestmentType.Cash Then
                        newInvestment.TotalCostsForYield += newInvestment.CurrentCost
                    Else
                        newInvestment.TotalCostsForYield += (newInvestment.CurrentCost * newInvestment.CurrentQuantity)
                    End If
                    newInvestment.CurrentCost += newTransaction.UnitValue
                Case 6 ' Costs retained
                    newInvestment.CurrentCosts += newTransaction.UnitValue
                    newInvestment.CurrentCost -= newTransaction.UnitValue
                Case 7 ' Transfer To Investment
                    newInvestment.CurrentCost += newTransaction.UnitValue
                Case 8 ' Transfer From Investment
                    newInvestment.CurrentCost -= newTransaction.UnitValue
            End Select
        Else
            ' We are editing a transaction - will need to recalculate!
            newTransaction = CType(Portfolio.Transactions(CLng(txtTransactionID.Text)), InvestmentTransaction)
            newTransaction.Type = cboType.SelectedIndex
            newTransaction.TransDate = CDate(dtpDate.Value)
            newTransaction.Quantity = CDbl(txtQuantity.Text)
            newTransaction.UnitValue = CDbl(txtUnitValue.Text)
            ' Replace the transaction under the investment
            newInvestment.Transactions.Item(newTransaction.ID) = newTransaction
        End If
        ' Close the form
        Me.Close()
    End Sub

    Private Sub cboType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboType.SelectedIndexChanged
        Dim newInvestment As Investment = CType(Portfolio.Investments(CLng(txtInvestmentID.Text)), Investment)
        Select Case newInvestment.Type
            Case 0 ' Cash
                lblQuantity.Visible = False
                txtQuantity.Visible = False
                txtQuantity.Text = "0"
                Select Case CInt(Portfolio.TransactionTypes(cboType.SelectedItem))
                    Case 0 ' Purchase
                        lblValue.Text = "Unit Value:"
                    Case 1 ' Sale
                        lblValue.Text = "Unit Value:"
                    Case 2 ' Valuation
                        lblValue.Text = "Unit Value:"
                        lblQuantity.Visible = False
                    Case 3, 5 ' Income
                        lblValue.Text = "Total Income:"
                    Case 4, 6 ' Cost
                        lblValue.Text = "Total Cost:"
                    Case 7, 8 ' Transfer Value
                        lblValue.Text = "Transfer Value:"
                End Select
            Case 1 ' Shares
                Select Case CInt(Portfolio.TransactionTypes(cboType.SelectedItem))
                    Case 0 ' Purchase
                        lblValue.Text = "Unit Value:"
                        lblQuantity.Visible = True
                        txtQuantity.Visible = True
                    Case 1 ' Sale
                        lblValue.Text = "Unit Value:"
                        lblQuantity.Visible = True
                        txtQuantity.Visible = True
                    Case 2 ' Valuation
                        lblValue.Text = "Unit Value:"
                        lblQuantity.Visible = False
                        txtQuantity.Visible = False
                        txtQuantity.Text = "0"
                    Case 3 ' Income
                        lblValue.Text = "Total Income:"
                        lblQuantity.Visible = False
                        txtQuantity.Visible = False
                        txtQuantity.Text = "0"
                    Case 4 ' Cost
                        lblValue.Text = "Total Cost:"
                        lblQuantity.Visible = False
                        txtQuantity.Visible = False
                        txtQuantity.Text = "0"
                End Select
        End Select
    End Sub
End Class