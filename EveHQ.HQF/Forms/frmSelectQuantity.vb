Imports System.Windows.Forms

Public Class frmSelectQuantity

    Public fittedShip As Ship
    Public currentShip As Ship
    Public newQuantity As Integer
    Public IsDroneBay As Boolean
    Public IsSplit As Boolean
    Public DBI As DroneBayItem
    Public CBI As CargoBayItem

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub frmSelectQuantity_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
       
    End Sub

    Private Sub frmSelectQuantity_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        lblDetails.Text = "Please enter a new quantity" & ControlChars.CrLf & "(between " & nudQuantity.Minimum.ToString & " and " & nudQuantity.Maximum.ToString & ")"
        nudQuantity.Select(0, Len(nudQuantity.Value.ToString))
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        newQuantity = CInt(nudQuantity.Value)
        If IsSplit = False Then
            Call Me.UpdateQuantity()
        Else
            Call Me.SplitQuantity()
        End If
        Me.Close()
    End Sub

    Private Sub UpdateQuantity()
        If IsDroneBay = True Then
            ' For drone bay
            Dim reqQ As Integer = newQuantity - DBI.Quantity
            Dim reqVolume As Double = reqQ * DBI.DroneType.Volume
            ' Double-check we can get them in the drone bay
            If fittedShip.DroneBay_Used + (reqQ * DBI.DroneType.Volume) > fittedShip.DroneBay Then
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
                If fittedShip.DroneBandwidth_Used + (reqQ * CDbl(DBI.DroneType.Attributes("1272"))) > fittedShip.DroneBandwidth Then
                    ' Cannot do this because we don't have enough bandwidth
                    MessageBox.Show("You do not have the spare bandwidth to control this many drones. This drone group will be made inactive.", "Drone Bandwidth Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    DBI.IsActive = False
                    Exit Sub
                End If
            End If
            If newQuantity <> DBI.Quantity Then
                DBI.Quantity = newQuantity
            End If
        Else
            ' For cargo bay
            Dim reqQ As Integer = newQuantity - CBI.Quantity
            Dim reqVolume As Double = reqQ * CBI.ItemType.Volume
            ' Double-check we can get them in the cargo bay
            If fittedShip.CargoBay_Used + (reqQ * CBI.ItemType.Volume) > fittedShip.CargoBay Then
                ' Cannot do this because our cargo bay space is insufficient
                MessageBox.Show("You do not have the space in the Cargo Bay to store that many units. Please try again.", "Cargo Bay Volume Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If
            If newQuantity <> CBI.Quantity Then
                CBI.Quantity = newQuantity
            End If
        End If
    End Sub
    Private Sub SplitQuantity()
        If IsDroneBay = True Then
            ' For drone bay
            If newQuantity <> DBI.Quantity Then
                ' Add a new drone bay item
                Dim newDBI As New DroneBayItem
                newDBI.DroneType = DBI.DroneType.Clone
                newDBI.Quantity = DBI.Quantity - newQuantity
                newDBI.IsActive = DBI.IsActive
                currentShip.DroneBayItems.Add(currentShip.DroneBayItems.Count, newDBI)
                ' Modify the existing quantity
                DBI.Quantity = newQuantity
            End If
        Else
            ' For cargo bay
            If newQuantity <> CBI.Quantity Then
                ' Add a new cargo bay item
                Dim newCBI As New CargoBayItem
                newCBI.ItemType = CBI.ItemType.Clone
                newCBI.Quantity = CBI.Quantity - newQuantity
                currentShip.CargoBayItems.Add(currentShip.CargoBayItems.Count, newCBI)
                ' Modify the existing quantity
                CBI.Quantity = newQuantity
            End If
        End If
    End Sub

    
End Class