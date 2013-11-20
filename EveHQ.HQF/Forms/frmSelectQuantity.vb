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