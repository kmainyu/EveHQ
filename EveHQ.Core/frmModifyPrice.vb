﻿' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2011  EveHQ Development Team
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

Public Class frmModifyPrice

    Dim ItemID As String = ""
    Dim Value As Double = 0
    Dim EditingPrice As Boolean = False

#Region "Form Constructors"

    Public Sub New(ByVal DefaultItemID As String, ByVal DefaultValue As Double)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Setup the defaults
        Me.ItemID = DefaultItemID
        Me.Value = DefaultValue

        If EveHQ.Core.HQ.CustomPriceList.ContainsKey(Me.ItemID) Then
            EditingPrice = True
        Else
            EditingPrice = False
        End If

        ' Set the prices
        Me.lblBasePrice.Text = EveHQ.Core.HQ.BasePriceList(Me.ItemID).ToString("N2")
		If EveHQ.Core.HQ.MarketPriceList.ContainsKey(Me.ItemID) = True Then
			Me.lblMarketPrice.Text = EveHQ.Core.HQ.MarketPriceList(Me.ItemID).ToString("N2")
		Else
			Me.lblMarketPrice.Text = CInt("0").ToString("N2")
		End If
		If EveHQ.Core.HQ.CustomPriceList.ContainsKey(Me.ItemID) = True Then
			Me.lblCustomPrice.Text = EveHQ.Core.HQ.CustomPriceList(Me.ItemID).ToString("N2")
		Else
			Me.lblCustomPrice.Text = CInt("0").ToString("N2")
		End If

		If Me.EditingPrice = False Then
			Me.Text = "Add Price - " & EveHQ.Core.HQ.itemData(ItemID).Name
		Else
			Me.Text = "Modify Price - " & EveHQ.Core.HQ.itemData(ItemID).Name
		End If

		' Set the default price if not zero
		If DefaultValue = 0 Then
			txtNewPrice.Text = EveHQ.Core.DataFunctions.GetPrice(ItemID).ToString
		Else
			txtNewPrice.Text = DefaultValue.ToString
		End If

	End Sub

#End Region

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

        If Me.EditingPrice = False Then
            ' Add the custom price
            Call EveHQ.Core.DataFunctions.AddCustomPrice(ItemID, CDbl(txtNewPrice.Text), False)
        Else
            ' Edit the custom price
            Call EveHQ.Core.DataFunctions.EditCustomPrice(ItemID, CDbl(txtNewPrice.Text), False)
        End If

        ' Close the form
        Me.Close()
    End Sub


End Class