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
Imports EveHQ.Common.Extensions

Public Class frmModifyPrice

    ReadOnly _itemID As Integer
    ReadOnly _previousPrice As Double = 0
    ReadOnly _editingPrice As Boolean = False

#Region "Form Constructors"

    Public Sub New(ByVal defaultItemID As Integer, ByVal defaultValue As Double)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Setup the defaults
        _itemID = defaultItemID
       
        If HQ.CustomPriceList.ContainsKey(_itemID) Then
            _editingPrice = True
            _previousPrice = HQ.CustomPriceList(_itemID)
        Else
            _editingPrice = False
            _previousPrice = 0
        End If

        ' Set the prices
        lblBasePrice.Text = StaticData.Types(_itemID).BasePrice.ToInvariantString("N2")

        lblMarketPrice.Text = DataFunctions.GetPrice(_itemID).ToInvariantString("N2")

        If HQ.CustomPriceList.ContainsKey(_itemID) = True Then
            lblCustomPrice.Text = HQ.CustomPriceList(_itemID).ToInvariantString("N2")
        Else
            lblCustomPrice.Text = CInt("0").ToInvariantString("N2")
        End If

        If _editingPrice = False Then
            Text = "Add Price - " & StaticData.Types(_itemID).Name
        Else
            Text = "Modify Price - " & StaticData.Types(_itemID).Name
        End If

        ' Set the default price if not zero
        If Math.Abs(defaultValue - 0) < 0.000001 Then
            txtNewPrice.Text = DataFunctions.GetPrice(_itemID).ToString()
        Else
            txtNewPrice.Text = defaultValue.ToString
        End If

    End Sub

#End Region

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
        ' Check if everything is filled out as it should be
        If IsNumeric(txtNewPrice.Text) = False Then
            MessageBox.Show("You must enter a valid price for this item.", "Error In Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If CDbl(txtNewPrice.Text) < 0 Then
            MessageBox.Show("You cannot enter a negative price for this item.", "Error In Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If CDbl(txtNewPrice.Text) = 0 Then
            Call CustomDataFunctions.DeleteCustomPrice(_itemID)
        Else
            Call CustomDataFunctions.SetCustomPrice(_itemID, CDbl(txtNewPrice.Text))
        End If

        ' Close the form
        Close()
    End Sub

    Private Sub txtNewPrice_TextChanged(sender As System.Object, e As EventArgs) Handles txtNewPrice.TextChanged
        ' Check if the original value is non-zero and therefore if the price should be removed
        If IsNumeric(txtNewPrice.Text) = True Then
            If _previousPrice <> 0 And CDbl(txtNewPrice.Text) = 0 Then
                lblClearingCustomPrice.Visible = True
            Else
                lblClearingCustomPrice.Visible = False
            End If
        Else
            lblClearingCustomPrice.Visible = False
        End If
    End Sub

End Class