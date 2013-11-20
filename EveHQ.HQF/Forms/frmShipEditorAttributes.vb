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
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar

Namespace Forms

    Public Class FrmShipEditorAttributes

        Dim _newShipID As Integer
        Dim _lockedAttributes As Boolean = False
        Dim _lockedBonuses As Boolean = False

        Dim _customShip As New CustomShip
        ReadOnly _formIsInitialising As Boolean = False
        ReadOnly _oldShipName As String = ""

#Region "Public properties"

        Public Property CustomShip() As CustomShip
            Get
                Return _customShip
            End Get
            Set(ByVal value As CustomShip)
                _customShip = value
            End Set
        End Property

#End Region

#Region "Form Constructor & Routines"

        Public Sub New(ByVal startingShip As CustomShip)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Set the form flag
            _formIsInitialising = True

            ' Initialise the data to be shown on the form
            Call ObtainNewShipID()
            Call LoadShips()
            Call LoadShipClasses()

            If StartingShip IsNot Nothing Then
                _customShip = StartingShip
                _oldShipName = _customShip.Name
                Call UpdateUI()
            Else
                _customShip = New CustomShip
            End If

            ' Reset the flag
            _formIsInitialising = False

        End Sub

        Private Sub UpdateUi()
            ' Update the UI with the ship to edit

            ' Update Ship Hull
            cboShipHull.SelectedItem = _customShip.BaseShipName
            cboShipClass.Enabled = True

            ' Update ship class
            cboShipClass.SelectedItem = _customShip.ShipClass

            ' Update ship name and ID
            lblShipID.Text = "New Custom Ship ID: " & _customShip.ID
            txtShipName.Text = _customShip.Name

            ' Update the bonus list and description
            Call UpdateBonusList()

            chkAutoBonusDescription.Checked = _customShip.AutoBonusDescription
            Call UpdateDescription()

            ' Update the attributes
            apg1.SelectedObject = CustomShip.ShipData

        End Sub

#End Region

#Region "Form Loading Routines"

        Private Sub frmShipEditor_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

        End Sub

        Private Sub ObtainNewShipID()
            Dim shipID As Integer = 1000000
            Do
                shipID += 1
            Loop Until CustomHQFClasses.CustomShipIDs.ContainsKey(shipID) = False
            _newShipID = shipID
            lblShipID.Text = "New Custom Ship ID: " & _newShipID.ToString
        End Sub

        Private Sub LoadShips()
            cboShipHull.BeginUpdate()
            cboShipHull.Items.Clear()
            For Each shipName As String In ShipLists.ShipList.Keys
                cboShipHull.Items.Add(shipName)
            Next
            cboShipHull.Sorted = True
            cboShipHull.EndUpdate()
        End Sub

        Private Sub LoadShipClasses()
            cboShipClass.BeginUpdate()
            cboShipClass.Items.Clear()
            ' Add the basic ship classes
            For Each className As String In Market.ShipClasses.Values
                If cboShipClass.Items.Contains(className) = False Then
                    cboShipClass.Items.Add(className)
                End If
            Next
            ' Add the custom ship classes
            For Each className As String In CustomHQFClasses.CustomShipClasses.Keys
                cboShipClass.Items.Add(className)
            Next
            cboShipClass.Sorted = True
            cboShipClass.EndUpdate()
        End Sub

#End Region

#Region "Main Form UI Routines"
        Private Sub cboShipHull_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboShipHull.SelectedIndexChanged
            If _formIsInitialising = False Then
                ' This happens when a ship hull type is selected
                ' What we need to do is create a copy of the selected ship for use in the CustomShip class
                ' Then add the additional data
                ' We need to take into account also if the bonuses and attributes are "locked"

                ' Enable other UI elements as required
                cboShipClass.Enabled = True

                ' Get a clone of the selected ship
                Dim baseShip As Ship = ShipLists.ShipList(cboShipHull.SelectedItem.ToString).Clone

                ' Store the base ship name
                CustomShip.BaseShipName = baseShip.Name
                UpdateImage()

                'Check if we have locked attributes
                'If so, we don't actually want to change anything other than the base ship hull

                If _lockedAttributes = False Then
                    ' Set the ship data using the presented information
                    CustomShip.ShipData = baseShip
                End If

                ' Check if we have locked bonuses
                ' If so, we don't want to inherit the new ones
                If _lockedBonuses = False Then

                    ' Fetch the bonuses for the new ship from the Engine.ShipBonusesMap
                    Dim baseShipBonuses As New List(Of ShipEffect)
                    If Engine.ShipBonusesMap.ContainsKey(baseShip.ID) Then
                        Dim originalBonuses As List(Of ShipEffect) = Engine.ShipBonusesMap(baseShip.ID)
                        For Each se As ShipEffect In originalBonuses
                            baseShipBonuses.Add(se.Clone)
                        Next
                        'BaseShipBonuses = Engine.ShipBonusesMap(BaseShip.ID)
                    End If

                    ' Update the bonuses to reflect the ID of the new custom ship
                    For Each shipBonus As ShipEffect In baseShipBonuses
                        ' Modify the shipID of the effect
                        shipBonus.ShipID = _newShipID
                        ' Check the affectedIDs for the ship type
                        For idx As Integer = 0 To shipBonus.AffectedID.Count - 1
                            If shipBonus.AffectedID(idx) = baseShip.ID Then
                                shipBonus.AffectedID(idx) = _newShipID
                            End If
                        Next
                    Next
                    ' Add the bonuses to the custom ship
                    CustomShip.Bonuses = baseShipBonuses
                End If

                ' Display the bonuses
                Call UpdateBonusList()

                ' Process the Ship Description
                Call UpdateDescription()

                ' Process the ship ID
                CustomShip.ShipData.ID = _newShipID
                CustomShip.ID = _newShipID

                ' Process the ship name
                CustomShip.ShipData.Name = txtShipName.Text.Trim
                CustomShip.Name = txtShipName.Text.Trim

                ' Set the property grid object for editing the attributes
                apg1.SelectedObject = CustomShip.ShipData
            End If
        End Sub

        Private Sub txtDescription_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtDescription.TextChanged
            Call UpdateDescription()
        End Sub

        Private Sub chkAutoBonusDescription_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkAutoBonusDescription.CheckedChanged
            If _formIsInitialising = False Then
                CustomShip.AutoBonusDescription = chkAutoBonusDescription.Checked
                Call UpdateDescription()
            End If
        End Sub

        Private Sub txtShipName_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtShipName.TextChanged
            Call UpdateShipName()
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

        Private Sub cboShipClass_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboShipClass.SelectedIndexChanged
            ' Activate the disabled UI elements on first change
            If cboShipClass.SelectedItem IsNot Nothing Then
                CustomShip.ShipClass = cboShipClass.SelectedItem.ToString
                txtShipName.Enabled = True
            End If
        End Sub

        Private Sub btnLockAttributes_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLockAttributes.Click
            _lockedAttributes = btnLockAttributes.Checked
        End Sub

        Private Sub btnLockBonuses_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLockBonuses.Click
            _lockedBonuses = btnLockBonuses.Checked
        End Sub

#End Region

#Region "Item Update Routines"

        Private Sub UpdateShipName()
            Dim chkString As String = StrConv(txtShipName.Text.Trim, VbStrConv.ProperCase)
            If (chkString.Length > 2 And ShipLists.ShipList.ContainsKey(chkString) = False And CustomHQFClasses.CustomShips.ContainsKey(chkString) = False) Or chkString = _oldShipName Then
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
            Dim tti As New SuperTooltipInfo("", "Ship Description", CustomShip.ShipData.Description, My.Resources.imgInfo1, Nothing, eTooltipColor.Apple)
            sttBonus.SetSuperTooltip(picDescription, tti)
            apg1.UpdatePropertyValue("Description")
        End Sub

        Private Sub UpdateImage()
            Dim img As Image = Core.ImageHandler.GetImage(CInt(ShipLists.ShipListKeyName(CustomShip.BaseShipName)), 128)
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
                Dim ids As New StringBuilder
                If bonus.AffectedID.Count > 0 Then
                    For Each id As Integer In bonus.AffectedID
                        ids.Append(";" & id)
                    Next
                    ids.Remove(0, 1)
                End If
                newNode.Cells.Add(New Cell(ids.ToString))
                newNode.Cells.Add(New Cell(bonus.Value.ToString("N4")))
                newNode.Cells.Add(New Cell(bonus.IsPerLevel.ToString))
                newNode.Cells.Add(New Cell(bonus.StackNerf.ToString))
                newNode.Cells.Add(New Cell(bonus.CalcType.ToString))
                newNode.Cells.Add(New Cell(bonus.Status.ToString))
                tvwBonuses.Nodes.Add(newNode)
                Dim tti As New SuperTooltipInfo("", "Bonus Description", EffectFunctions.ConvertShipBonusesToDescription(bonus), My.Resources.imgInfo2, Nothing, eTooltipColor.Apple)
                sttBonus.SetSuperTooltip(newNode, tti)
            Next
            tvwBonuses.EndUpdate()
        End Sub

#End Region

#Region "Bonus Modification Routines"

        Private Sub btnAddBonus_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddBonus.Click
            Using newBonus As New FrmShipEditorBonus(CInt(_customShip.ID), Nothing)
                newBonus.ShowDialog()
                ' Check the result from the bonus form
                If newBonus.DialogResult = DialogResult.OK Then
                    ' Add the new bonus
                    _customShip.Bonuses.Add(newBonus.NewShipEffect)
                    ' Update the list of bonuses
                    Call UpdateBonusList()
                    ' Update the description in case we have auto-bonus description enabled
                    Call UpdateDescription()
                End If
            End Using
        End Sub

        Private Sub btnClearBonuses_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClearBonuses.Click
            Dim reply As DialogResult = MessageBox.Show("Are you sure you wish to clear all the current ship bonuses?", "Confirm Delete Bonuses?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.Yes Then
                _customShip.Bonuses.Clear()
                ' Update the list of bonuses
                Call UpdateBonusList()
                ' Update the description in case we have auto-bonus description enabled
                Call UpdateDescription()
            End If
        End Sub

        Private Sub btnDeleteBonus_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeleteBonus.Click
            Call DeleteBonus()
        End Sub

        Private Sub DeleteBonus()
            ' Create a list of items to delete
            Dim nodesToDelete As New List(Of Integer)
            For Each delNode As Node In tvwBonuses.SelectedNodes
                nodesToDelete.Add(delNode.Index)
            Next
            ' Sort the list
            nodesToDelete.Sort()
            ' Reverse the list
            nodesToDelete.Reverse()
            ' Now delete the bonuses
            For Each idx As Integer In nodesToDelete
                _customShip.Bonuses.RemoveAt(idx)
            Next
            ' Display the bonuses
            Call UpdateBonusList()
            ' Process the Ship Description
            Call UpdateDescription()
        End Sub

        Private Sub btnEditBonus_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditBonus.Click
            If tvwBonuses.SelectedNodes.Count > 0 Then
                ' Only edit the first selected bonus
                Dim idx As Integer = tvwBonuses.SelectedNodes(0).Index
                Call EditBonus(idx)
            End If
        End Sub

        Private Sub EditBonus(ByVal idx As Integer)
            ' Get the original effect
            Dim selEffect As ShipEffect = _customShip.Bonuses(idx)
            ' Store the old effect in case we need to revert
            Dim oldEffect As ShipEffect = selEffect.Clone
            ' Create a new form instance
            Using newBonus As New FrmShipEditorBonus(CInt(_customShip.ID), selEffect)
                newBonus.ShowDialog()
                ' Check the result from the bonus form
                If newBonus.DialogResult = DialogResult.OK Then
                    ' Update the list of bonuses
                    Call UpdateBonusList()
                    ' Update the description in case we have auto-bonus description enabled
                    Call UpdateDescription()
                Else
                    ' If cancelled, restore the old effect (as the form will change something)
                    _customShip.Bonuses(idx) = oldEffect
                    ' Update the list of bonuses
                    Call UpdateBonusList()
                    ' Update the description in case we have auto-bonus description enabled
                    Call UpdateDescription()
                End If
            End Using
        End Sub

        Private Sub tvwBonuses_NodeDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles tvwBonuses.NodeDoubleClick
            ' Get the index of the node to pass to the proper method
            Dim idx As Integer = e.Node.Index
            Call EditBonus(idx)
        End Sub

#End Region

#Region "Form Accept and Cancel Routines"

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            DialogResult = DialogResult.Cancel
            Close()
        End Sub

        Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
            ' Check our data is ok

            ' Check ship name
            Dim chkString As String = StrConv(txtShipName.Text.Trim, VbStrConv.ProperCase)
            If Not ((chkString.Length > 2 And ShipLists.ShipList.ContainsKey(chkString) = False And CustomHQFClasses.CustomShips.ContainsKey(chkString) = False) Or chkString = _oldShipName) Then
                MessageBox.Show("Ship name is not valid. Please choose a name longer than 2 characters that is not already an existing (standard or custom) ship name.", "Ship Data Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            ' Check bonuses
            If _customShip.Bonuses.Count = 0 Then
                Dim reply As DialogResult = MessageBox.Show("You have not configured any bonuses, would you like to add some to the ship before exiting?", "Add Bonuses?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If reply = DialogResult.Yes Then
                    Exit Sub
                End If
            End If

            ' Map the properties to attributes
            _customShip.ShipData.MapShipProperties()

            ' We should be ok from here, so set the form result and close the form
            DialogResult = DialogResult.OK
            Close()
        End Sub

#End Region

    End Class
End NameSpace