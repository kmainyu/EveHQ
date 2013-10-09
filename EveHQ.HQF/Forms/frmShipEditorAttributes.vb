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
Imports System.Text
Imports System.Globalization
Imports DevComponents.AdvTree

Public Class frmShipEditorAttributes

    Dim NewShipID As Integer
    Dim LockedAttributes As Boolean = False
    Dim LockedBonuses As Boolean = False

    Dim cCustomShip As New CustomShip
    Dim FormIsInitialising As Boolean = False
    Dim OldShipName As String = ""

#Region "Public properties"

    Public Property CustomShip() As CustomShip
        Get
            Return cCustomShip
        End Get
        Set(ByVal value As CustomShip)
            cCustomShip = value
        End Set
    End Property

#End Region

#Region "Form Constructor & Routines"

    Public Sub New(ByVal StartingShip As CustomShip)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Set the form flag
        FormIsInitialising = True

        ' Initialise the data to be shown on the form
        Call Me.ObtainNewShipID()
        Call Me.LoadShips()
        Call Me.LoadShipClasses()

        If StartingShip IsNot Nothing Then
            cCustomShip = StartingShip
            OldShipName = cCustomShip.Name
            Call Me.UpdateUI()
        Else
            cCustomShip = New CustomShip
        End If

        ' Reset the flag
        FormIsInitialising = False

    End Sub

    Private Sub UpdateUI()
        ' Update the UI with the ship to edit

        ' Update Ship Hull
        cboShipHull.SelectedItem = cCustomShip.BaseShipName
        cboShipClass.Enabled = True

        ' Update ship class
        cboShipClass.SelectedItem = cCustomShip.ShipClass

        ' Update ship name and ID
        lblShipID.Text = "New Custom Ship ID: " & cCustomShip.ID
        txtShipName.Text = cCustomShip.Name

        ' Update the bonus list and description
        Call Me.UpdateBonusList()

        chkAutoBonusDescription.Checked = cCustomShip.AutoBonusDescription
        Call Me.UpdateDescription()

        ' Update the attributes
        apg1.SelectedObject = CustomShip.ShipData

    End Sub

#End Region

#Region "Form Loading Routines"

    Private Sub frmShipEditor_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
      
    End Sub

    Private Sub ObtainNewShipID()
        Dim shipID As Integer = 1000000
        Do
            shipID += 1
        Loop Until CustomHQFClasses.CustomShipIDs.ContainsKey(shipID) = False
        NewShipID = shipID
        lblShipID.Text = "New Custom Ship ID: " & NewShipID.ToString
    End Sub

    Private Sub LoadShips()
        cboShipHull.BeginUpdate()
        cboShipHull.Items.Clear()
        For Each shipName As String In ShipLists.shipList.Keys
            cboShipHull.Items.Add(shipName)
        Next
        cboShipHull.Sorted = True
        cboShipHull.EndUpdate()
    End Sub

    Private Sub LoadShipClasses()
        cboShipClass.BeginUpdate()
        cboShipClass.Items.Clear()
        ' Add the basic ship classes
        For Each ClassName As String In Market.ShipClasses.Values
            If cboShipClass.Items.Contains(ClassName) = False Then
                cboShipClass.Items.Add(ClassName)
            End If
        Next
        ' Add the custom ship classes
        For Each ClassName As String In CustomHQFClasses.CustomShipClasses.Keys
            cboShipClass.Items.Add(ClassName)
        Next
        cboShipClass.Sorted = True
        cboShipClass.EndUpdate()
    End Sub

#End Region

#Region "Main Form UI Routines"
    Private Sub cboShipHull_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboShipHull.SelectedIndexChanged
        If FormIsInitialising = False Then
            ' This happens when a ship hull type is selected
            ' What we need to do is create a copy of the selected ship for use in the CustomShip class
            ' Then add the additional data
            ' We need to take into account also if the bonuses and attributes are "locked"

            ' Enable other UI elements as required
            cboShipClass.Enabled = True

            ' Get a clone of the selected ship
            Dim BaseShip As Ship = CType(ShipLists.shipList(cboShipHull.SelectedItem.ToString), Ship).Clone

            ' Store the base ship name
            CustomShip.BaseShipName = BaseShip.Name
            Me.UpdateImage()

            'Check if we have locked attributes
            'If so, we don't actually want to change anything other than the base ship hull

            If Me.LockedAttributes = False Then
                ' Set the ship data using the presented information
                CustomShip.ShipData = BaseShip
            End If

            ' Check if we have locked bonuses
            ' If so, we don't want to inherit the new ones
            If Me.LockedBonuses = False Then

                ' Fetch the bonuses for the new ship from the Engine.ShipBonusesMap
                Dim BaseShipBonuses As New List(Of ShipEffect)
                If Engine.ShipBonusesMap.ContainsKey(BaseShip.ID) Then
                    Dim OriginalBonuses As List(Of ShipEffect) = Engine.ShipBonusesMap(BaseShip.ID)
                    For Each SE As ShipEffect In OriginalBonuses
                        BaseShipBonuses.Add(SE.Clone)
                    Next
                    'BaseShipBonuses = Engine.ShipBonusesMap(BaseShip.ID)
                End If

                ' Update the bonuses to reflect the ID of the new custom ship
                For Each ShipBonus As ShipEffect In BaseShipBonuses
                    ' Modify the shipID of the effect
                    ShipBonus.ShipID = NewShipID
                    ' Check the affectedIDs for the ship type
                    For idx As Integer = 0 To ShipBonus.AffectedID.Count - 1
                        If ShipBonus.AffectedID(idx) = BaseShip.ID Then
                            ShipBonus.AffectedID(idx) = NewShipID
                        End If
                    Next
                Next
                ' Add the bonuses to the custom ship
                CustomShip.Bonuses = BaseShipBonuses
            End If

            ' Display the bonuses
            Call Me.UpdateBonusList()

            ' Process the Ship Description
            Call Me.UpdateDescription()

            ' Process the ship ID
            CustomShip.ShipData.ID = NewShipID
            CustomShip.ID = NewShipID

            ' Process the ship name
            CustomShip.ShipData.Name = txtShipName.Text.Trim
            CustomShip.Name = txtShipName.Text.Trim

            ' Set the property grid object for editing the attributes
            apg1.SelectedObject = CustomShip.ShipData
        End If
    End Sub

    Private Sub txtDescription_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtDescription.TextChanged
        Call Me.UpdateDescription()
    End Sub

    Private Sub chkAutoBonusDescription_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoBonusDescription.CheckedChanged
        If FormIsInitialising = False Then
            CustomShip.AutoBonusDescription = chkAutoBonusDescription.Checked
            Call Me.UpdateDescription()
        End If
    End Sub

    Private Sub txtShipName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtShipName.TextChanged
        Call Me.UpdateShipName()
        ' Enable the other sections if not blank
        If txtShipName.Text.Trim <> "" Then
            txtDescription.Enabled = True
            chkAutoBonusDescription.Enabled = True
            btnAddBonus.Enabled = True
            btnClearBonuses.Enabled = True
            btnDeleteBonus.Enabled = True
            btnEditBonus.Enabled = True
            btnLockAttributes.Enabled = True
            btnLockBonuses.Enabled = True
            tvwBonuses.Enabled = True
            apg1.Enabled = True
        Else
            txtDescription.Enabled = False
            chkAutoBonusDescription.Enabled = False
            btnAddBonus.Enabled = False
            btnClearBonuses.Enabled = False
            btnDeleteBonus.Enabled = False
            btnEditBonus.Enabled = False
            btnLockAttributes.Enabled = False
            btnLockBonuses.Enabled = False
            tvwBonuses.Enabled = False
            apg1.Enabled = False
        End If
    End Sub

    Private Sub cboShipClass_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboShipClass.SelectedIndexChanged
        ' Activate the disabled UI elements on first change
        If cboShipClass.SelectedItem IsNot Nothing Then
            CustomShip.ShipClass = cboShipClass.SelectedItem.ToString
            txtShipName.Enabled = True
        End If
    End Sub

    Private Sub btnLockAttributes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLockAttributes.Click
        Me.LockedAttributes = btnLockAttributes.Checked
    End Sub

    Private Sub btnLockBonuses_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLockBonuses.Click
        Me.LockedBonuses = btnLockBonuses.Checked
    End Sub

#End Region

#Region "Item Update Routines"

    Private Sub UpdateShipName()
        Dim chkString As String = StrConv(txtShipName.Text.Trim, VbStrConv.ProperCase)
        If (chkString.Length > 2 And ShipLists.shipList.ContainsKey(chkString) = False And CustomHQFClasses.CustomShips.ContainsKey(chkString) = False) Or chkString = OldShipName Then
            picValidShipName.Image = My.Resources.tick_small
        Else
            picValidShipName.Image = My.Resources.cross_small
        End If
        CustomShip.Name = chkString
        CustomShip.ShipData.Name = chkString
        apg1.UpdatePropertyValue("Name")
    End Sub

    Private Sub UpdateDescription()
        ' Update the descriptions with the text
        CustomShip.Description = txtDescription.Text.Trim
        CustomShip.ShipData.Description = txtDescription.Text.Trim
        If chkAutoBonusDescription.Checked = True Then
            ' Convert the bonuses into a readable description
            If txtDescription.Text.Trim <> "" Then
                CustomShip.ShipData.Description &= ControlChars.CrLf & ControlChars.CrLf
            End If
            CustomShip.ShipData.Description &= EffectFunctions.ConvertShipBonusesToDescription(CustomShip.Bonuses)
        End If
        Dim tti As New DevComponents.DotNetBar.SuperTooltipInfo("", "Ship Description", CustomShip.ShipData.Description, My.Resources.imgInfo1, Nothing, DevComponents.DotNetBar.eTooltipColor.Apple)
        sttBonus.SetSuperTooltip(picDescription, tti)
        apg1.UpdatePropertyValue("Description")
    End Sub

    Private Sub UpdateImage()
        Dim img As Drawing.Image = Core.ImageHandler.GetImage(CInt(ShipLists.ShipListKeyName(CustomShip.BaseShipName)), 128)
        If img IsNot Nothing Then
            picShip.Image = img
        End If
    End Sub

    Private Sub UpdateBonusList()
        ' Display the bonuses
        tvwBonuses.BeginUpdate()
        tvwBonuses.Nodes.Clear()
        For Each bonus As ShipEffect In CustomShip.Bonuses
            Dim newNode As New Node
            newNode.Text = bonus.ShipID.ToString
            newNode.Cells.Add(New Cell(bonus.AffectingType.ToString))
            newNode.Cells.Add(New Cell(bonus.AffectingID.ToString))
            newNode.Cells.Add(New Cell(bonus.AffectedAtt.ToString))
            newNode.Cells.Add(New Cell(bonus.AffectedType.ToString))
            Dim IDs As New StringBuilder
            If bonus.AffectedID.Count > 0 Then
                For Each ID As String In bonus.AffectedID
                    IDs.Append(";" & ID)
                Next
                IDs.Remove(0, 1)
            End If
            newNode.Cells.Add(New Cell(IDs.ToString))
            newNode.Cells.Add(New Cell(bonus.Value.ToString("N4")))
            newNode.Cells.Add(New Cell(bonus.IsPerLevel.ToString))
            newNode.Cells.Add(New Cell(bonus.StackNerf.ToString))
            newNode.Cells.Add(New Cell(bonus.CalcType.ToString))
            newNode.Cells.Add(New Cell(bonus.Status.ToString))
            tvwBonuses.Nodes.Add(newNode)
            Dim tti As New DevComponents.DotNetBar.SuperTooltipInfo("", "Bonus Description", EffectFunctions.ConvertShipBonusesToDescription(bonus), My.Resources.imgInfo2, Nothing, DevComponents.DotNetBar.eTooltipColor.Apple)
            sttBonus.SetSuperTooltip(newNode, tti)
        Next
        tvwBonuses.EndUpdate()
    End Sub

#End Region

#Region "Bonus Modification Routines"

    Private Sub btnAddBonus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddBonus.Click
        Dim newBonus As New frmShipEditorBonus(CInt(cCustomShip.ID), Nothing)
        newBonus.ShowDialog()
        ' Check the result from the bonus form
        If newBonus.DialogResult = Windows.Forms.DialogResult.OK Then
            ' Add the new bonus
            cCustomShip.Bonuses.Add(newBonus.NewShipEffect)
            ' Update the list of bonuses
            Call Me.UpdateBonusList()
            ' Update the description in case we have auto-bonus description enabled
            Call Me.UpdateDescription()
        End If
    End Sub

    Private Sub btnClearBonuses_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearBonuses.Click
        Dim reply As DialogResult = MessageBox.Show("Are you sure you wish to clear all the current ship bonuses?", "Confirm Delete Bonuses?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.Yes Then
            cCustomShip.Bonuses.Clear()
            ' Update the list of bonuses
            Call Me.UpdateBonusList()
            ' Update the description in case we have auto-bonus description enabled
            Call Me.UpdateDescription()
        End If
    End Sub

    Private Sub btnDeleteBonus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteBonus.Click
        Call Me.DeleteBonus()
    End Sub

    Private Sub DeleteBonus()
        ' Create a list of items to delete
        Dim NodesToDelete As New List(Of Integer)
        For Each delNode As Node In tvwBonuses.SelectedNodes
            NodesToDelete.Add(delNode.Index)
        Next
        ' Sort the list
        NodesToDelete.Sort()
        ' Reverse the list
        NodesToDelete.Reverse()
        ' Now delete the bonuses
        For Each idx As Integer In NodesToDelete
            cCustomShip.Bonuses.RemoveAt(idx)
        Next
        ' Display the bonuses
        Call Me.UpdateBonusList()
        ' Process the Ship Description
        Call Me.UpdateDescription()
    End Sub

    Private Sub btnEditBonus_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditBonus.Click
        If tvwBonuses.SelectedNodes.Count > 0 Then
            ' Only edit the first selected bonus
            Dim idx As Integer = tvwBonuses.SelectedNodes(0).Index
            Call Me.EditBonus(idx)
        End If
    End Sub

    Private Sub EditBonus(ByVal idx As Integer)
        ' Get the original effect
        Dim selEffect As ShipEffect = cCustomShip.Bonuses(idx)
        ' Store the old effect in case we need to revert
        Dim oldEffect As ShipEffect = selEffect.Clone
        ' Create a new form instance
        Dim newBonus As New frmShipEditorBonus(CInt(cCustomShip.ID), selEffect)
        newBonus.ShowDialog()
        ' Check the result from the bonus form
        If newBonus.DialogResult = Windows.Forms.DialogResult.OK Then
            ' Update the list of bonuses
            Call Me.UpdateBonusList()
            ' Update the description in case we have auto-bonus description enabled
            Call Me.UpdateDescription()
        Else
            ' If cancelled, restore the old effect (as the form will change something)
            cCustomShip.Bonuses(idx) = oldEffect
            ' Update the list of bonuses
            Call Me.UpdateBonusList()
            ' Update the description in case we have auto-bonus description enabled
            Call Me.UpdateDescription()
        End If
    End Sub

    Private Sub tvwBonuses_NodeDoubleClick(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles tvwBonuses.NodeDoubleClick
        ' Get the index of the node to pass to the proper method
        Dim idx As Integer = e.Node.Index
        Call Me.EditBonus(idx)
    End Sub

#End Region

#Region "Form Accept and Cancel Routines"

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        ' Check our data is ok

        ' Check ship name
        Dim chkString As String = StrConv(txtShipName.Text.Trim, VbStrConv.ProperCase)
        If Not ((chkString.Length > 2 And ShipLists.shipList.ContainsKey(chkString) = False And CustomHQFClasses.CustomShips.ContainsKey(chkString) = False) Or chkString = OldShipName) Then
            MessageBox.Show("Ship name is not valid. Please choose a name longer than 2 characters that is not already an existing (standard or custom) ship name.", "Ship Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Check bonuses
        If cCustomShip.Bonuses.Count = 0 Then
            Dim reply As DialogResult = MessageBox.Show("You have not configured any bonuses, would you like to add some to the ship before exiting?", "Add Bonuses?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.Yes Then
                Exit Sub
            End If
        End If

        ' Map the properties to attributes
        cCustomShip.ShipData.MapShipProperties()

        ' We should be ok from here, so set the form result and close the form
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

#End Region
   
   
    
End Class