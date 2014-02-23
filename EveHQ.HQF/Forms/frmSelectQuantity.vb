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

Imports System.Windows.Forms

Namespace Forms

    Public Class FrmSelectQuantity

        Public FittedShip As Ship
        Public CurrentShip As Ship
        Public NewQuantity As Integer
        Public BayType As Integer
        Public IsSplit As Boolean
        Public Dbi As DroneBayItem
        Public Cbi As CargoBayItem
        Public Sbi As ShipBayItem

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            Close()
        End Sub

        Private Sub frmSelectQuantity_Shown(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Shown
            lblDetails.Text = "Please enter a new quantity" & ControlChars.CrLf & "(between " & nudQuantity.Minimum.ToString & " and " & nudQuantity.Maximum.ToString & ")"
            nudQuantity.Select(0, Len(nudQuantity.Value.ToString))
        End Sub

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            newQuantity = CInt(nudQuantity.Value)
            If IsSplit = False Then
                Call UpdateQuantity()
            Else
                Call SplitQuantity()
            End If
            Close()
        End Sub

        Private Sub UpdateQuantity()
            Select Case BayType
                Case BayTypes.DroneBay
                    ' For drone bay
                    Dim reqQ As Integer = NewQuantity - Dbi.Quantity
                    ' Double-check we can get them in the drone bay
                    If fittedShip.DroneBayUsed + (reqQ * DBI.DroneType.Volume) > fittedShip.DroneBay Then
                        ' Cannot do this because our drone bay space is insufficient
                        MessageBox.Show("You do not have the space in the Drone Bay to store that many drones. Please try again.", "Drone Bay Volume Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If
                    ' Check we aren't going to cause any issues with drone control or bandwidth
                    If DBI.IsActive = True Then
                        If fittedShip.UsedDrones + reqQ > fittedShip.MaxDrones Then
                            ' Cannot do this because our drone control skill in insufficient
                            MessageBox.Show("You do not have the ability to control this many drones. This drone group will be made inactive.", "Drone Control Limit Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            DBI.IsActive = False
                        End If
                    End If
                    If DBI.IsActive = True Then
                        If fittedShip.DroneBandwidthUsed + (reqQ * CDbl(DBI.DroneType.Attributes(1272))) > fittedShip.DroneBandwidth Then
                            ' Cannot do this because we don't have enough bandwidth
                            MessageBox.Show("You do not have the spare bandwidth to control this many drones. This drone group will be made inactive.", "Drone Bandwidth Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                            DBI.IsActive = False
                            Exit Sub
                        End If
                    End If
                    If newQuantity <> DBI.Quantity Then
                        DBI.Quantity = newQuantity
                    End If
                Case BayTypes.CargoBay
                    ' For cargo bay
                    Dim reqQ As Integer = NewQuantity - Cbi.Quantity
                    ' Double-check we can get them in the cargo bay
                    If fittedShip.CargoBayUsed + (reqQ * CBI.ItemType.Volume) > fittedShip.CargoBay Then
                        ' Cannot do this because our cargo bay space is insufficient
                        MessageBox.Show("You do not have the space in the Cargo Bay to store that many units. Please try again.", "Cargo Bay Volume Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If
                    If newQuantity <> CBI.Quantity Then
                        CBI.Quantity = newQuantity
                    End If
                Case BayTypes.ShipBay
                    ' For ship bay
                    Dim reqQ As Integer = NewQuantity - Sbi.Quantity
                    ' Double-check we can get them in the ship bay
                    If fittedShip.ShipBayUsed + (reqQ * SBI.ShipType.Volume) > fittedShip.ShipBay Then
                        ' Cannot do this because our cargo bay space is insufficient
                        MessageBox.Show("You do not have the space in the Ship Maintenance Bay to store that many units. Please try again.", "Ship Maintenance Bay Volume Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If
                    If newQuantity <> SBI.Quantity Then
                        SBI.Quantity = newQuantity
                    End If
            End Select
        End Sub
        Private Sub SplitQuantity()
            Select Case BayType
                Case BayTypes.DroneBay
                    ' For drone bay
                    If newQuantity <> DBI.Quantity Then
                        ' Add a new drone bay item
                        Dim newDbi As New DroneBayItem
                        newDbi.DroneType = Dbi.DroneType.Clone
                        newDbi.Quantity = Dbi.Quantity - NewQuantity
                        newDbi.IsActive = Dbi.IsActive
                        CurrentShip.DroneBayItems.Add(CurrentShip.DroneBayItems.Count, newDbi)
                        ' Modify the existing quantity
                        DBI.Quantity = newQuantity
                    End If
                Case BayTypes.CargoBay
                    ' For cargo bay
                    If newQuantity <> CBI.Quantity Then
                        ' Add a new cargo bay item
                        Dim newCbi As New CargoBayItem
                        newCbi.ItemType = Cbi.ItemType.Clone
                        newCbi.Quantity = Cbi.Quantity - NewQuantity
                        CurrentShip.CargoBayItems.Add(CurrentShip.CargoBayItems.Count, newCbi)
                        ' Modify the existing quantity
                        CBI.Quantity = newQuantity
                    End If
                Case BayTypes.ShipBay
                    ' For ship bay
                    If newQuantity <> SBI.Quantity Then
                        ' Add a new cargo bay item
                        Dim newSbi As New ShipBayItem
                        newSbi.ShipType = Sbi.ShipType.Clone
                        newSbi.Quantity = Sbi.Quantity - NewQuantity
                        CurrentShip.ShipBayItems.Add(CurrentShip.ShipBayItems.Count, newSbi)
                        ' Modify the existing quantity
                        SBI.Quantity = newQuantity
                    End If
            End Select
        End Sub

        Public Enum BayTypes As Integer
            DroneBay = 1
            CargoBay = 2
            ShipBay = 3
            FuelBay = 4
        End Enum

    End Class
End NameSpace