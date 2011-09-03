' ========================================================================
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
Imports DevComponents.AdvTree

Public Class frmModifyPrices

#Region "Class Variables"

    Dim ItemIDList As New List(Of String)
    Dim PricesChanged As Boolean = False

#End Region

#Region "Form Constructors and related routines"

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(ItemIDs As List(Of String))

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.ItemIDList = ItemIDs
        Call Me.UpdatePriceMatrix()

    End Sub

    Private Sub UpdatePriceMatrix()

        ' Set style for the price list
        Dim NumberStyle As New DevComponents.DotNetBar.ElementStyle
        NumberStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Far

        adtPrices.BeginUpdate()
        adtPrices.Nodes.Clear()

        For Each itemID As String In Me.ItemIDList

            Dim item As EveHQ.Core.EveItem = EveHQ.Core.HQ.itemData(itemID)
            Dim itemNode As New DevComponents.AdvTree.Node
            itemNode.Text = item.Name
            itemNode.Name = item.ID.ToString
            itemNode.Image = EveHQ.Core.ImageHandler.GetImage(item.ID.ToString, 24)
            ' Add Market Price cell
            Dim MarketPrice As Double = 0
            If EveHQ.Core.HQ.MarketPriceList.ContainsKey(itemID) Then
                MarketPrice = EveHQ.Core.HQ.MarketPriceList(itemID)
            End If
            Dim qCell As New DevComponents.AdvTree.Cell(MarketPrice.ToString("N2"))
            qCell.StyleNormal = NumberStyle
            itemNode.Cells.Add(qCell)
            ' Add Custom Price cell
            Dim CustomPrice As Double = 0
            If EveHQ.Core.HQ.CustomPriceList.ContainsKey(itemID) Then
                CustomPrice = EveHQ.Core.HQ.CustomPriceList(itemID)
            End If
            Dim mlCell As New DevComponents.AdvTree.Cell(CustomPrice.ToString("N2"))
            mlCell.StyleNormal = NumberStyle
            itemNode.Cells.Add(mlCell)

            ' Add Node to the list
            adtPrices.Nodes.Add(itemNode)

        Next
        adtPrices.EndUpdate()

    End Sub

#End Region

#Region "Cell Editing Routines"

    Private Sub adtPrices_AfterCellEdit(sender As Object, e As DevComponents.AdvTree.CellEditEventArgs) Handles adtPrices.AfterCellEdit
        ' Check the new text is valid before comparison
        If e.NewText = "" Or IsNumeric(e.NewText) = False Then
            e.NewText = "0"
        End If
        If CDbl(e.Cell.Text) <> CDbl(e.NewText) Then
            PricesChanged = True
        End If
    End Sub

#End Region

    Private Sub btnCancel_Click(sender As System.Object, e As System.EventArgs) Handles btnCancel.Click
        ' Check if the prices have changed before closing
        If PricesChanged = True Then
            Dim msg As String = "At least one price has changed. Are you sure you wish to cancel the changes?"
            Dim reply As DialogResult = MessageBox.Show(msg, "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.Yes Then
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()
            Else
                Exit Sub
            End If
        Else
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End If
    End Sub

    Private Sub btnAccept_Click(sender As System.Object, e As System.EventArgs) Handles btnAccept.Click
        ' Check if anything has changed first
        If PricesChanged = False Then
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        Else
            ' Check we really want to save the prices
            Dim msg As String = "Are you sure you wish to save the changes to the price databases?"
            Dim reply As DialogResult = MessageBox.Show(msg, "Confirm Price Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.Yes Then
                ' Update the prices
                Call Me.UpdatePrices()
                ' Close the form
                Me.DialogResult = Windows.Forms.DialogResult.OK
                Me.Close()
            Else
                Exit Sub
            End If
        End If
    End Sub

    Private Sub UpdatePrices()

        ' Check if everything is filled out as it should be
        For Each PriceNode As Node In adtPrices.Nodes
            If IsNumeric(PriceNode.Cells(1).Text) = False Then
                MessageBox.Show("At least one item contains an invalid market price - you must enter a valid price for this item.", "Error In Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                If CDbl(PriceNode.Cells(1).Text) < 0 Then
                    MessageBox.Show("At least one item contains a negative market price - you cannot enter a negative price for this item.", "Error In Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
            End If
            If IsNumeric(PriceNode.Cells(2).Text) = False Then
                MessageBox.Show("At least one item contains an invalid custom price - you must enter a valid price for this item.", "Error In Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                If CDbl(PriceNode.Cells(2).Text) < 0 Then
                    MessageBox.Show("At least one item contains a negative custom price - you cannot enter a negative price for this item.", "Error In Price", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
            End If
        Next

        ' Add the prices
        For Each PriceNode As Node In adtPrices.Nodes
            Dim itemID As Long = CLng(PriceNode.Name)
            Dim MarketPrice As Double = CDbl(PriceNode.Cells(1).Text)
            Dim CustomPrice As Double = CDbl(PriceNode.Cells(2).Text)

            ' Set the market price
            If MarketPrice > 0 Then
                Call EveHQ.Core.DataFunctions.SetMarketPrice(itemID, MarketPrice, False)
            ElseIf MarketPrice = 0 Then
                Call EveHQ.Core.DataFunctions.DeleteMarketPrice(itemID.ToString)
            End If

            ' Set the custom price
            If CustomPrice > 0 Then
                Call EveHQ.Core.DataFunctions.SetCustomPrice(itemID, CustomPrice, False)
            ElseIf CustomPrice = 0 Then
                Call EveHQ.Core.DataFunctions.DeleteCustomPrice(itemID.ToString)
            End If

        Next

    End Sub

   

End Class