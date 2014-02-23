'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

Imports EveHQ.EveData
Imports EveHQ.Common.Extensions
Imports System.Windows.Forms

Public Class FrmModifyPrice

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

    Private Sub txtNewPrice_TextChanged(sender As Object, e As EventArgs) Handles txtNewPrice.TextChanged
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