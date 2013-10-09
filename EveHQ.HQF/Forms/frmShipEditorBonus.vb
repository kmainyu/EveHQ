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
Imports DevComponents.AdvTree
Imports EveHQ.EveData

Public Class frmShipEditorBonus

    Dim cNewShipEffect As New ShipEffect
    Dim cShipID As Integer
    Dim FormIsInitialising As Boolean = False
    Dim FlashCount As Integer = 0

#Region "Public Properties"
    ''' <summary>
    ''' Gets or sets the ShipEffect being edited on the form
    ''' </summary>
    ''' <value></value>
    ''' <returns>The ShipEffect being added/edited on the form</returns>
    ''' <remarks></remarks>
    Public Property NewShipEffect() As ShipEffect
        Get
            Return cNewShipEffect
        End Get
        Set(ByVal value As ShipEffect)
            cNewShipEffect = value
        End Set
    End Property

#End Region

#Region "Constructor and Initial Routines"

    ''' <summary>
    ''' Creates a new instance of the frmShipEditorBonus
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New(ByVal shipID As Integer, ByVal oldShipEffect As ShipEffect)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Set the initilising flag
        FormIsInitialising = True

        ' Setup the shipID
        cShipID = shipID

        ' Set the ship effect
        If OldShipEffect IsNot Nothing Then
            cNewShipEffect = OldShipEffect
        Else
            cNewShipEffect = New ShipEffect
        End If

        ' Setup the UI from various data
        Call SetupUI()

        ' Reset the flag
        FormIsInitialising = False

    End Sub

    ''' <summary>
    ''' Sets up the UI on creation of the form
    ''' </summary>
    ''' <remarks>Called from New()</remarks>
    Private Sub SetupUI()

        ' Set up the skills combo box
        cboSkill.BeginUpdate()
        cboSkill.Items.Clear()
        For Each NewSkill As Core.EveSkill In Core.HQ.SkillListID.Values
            If NewSkill.Published = True Then
                cboSkill.Items.Add(NewSkill.Name)
            End If
        Next
        cboSkill.Sorted = True
        cboSkill.EndUpdate()

        ' Set up the Stack Type combo box
        cboStackEffect.BeginUpdate()
        cboStackEffect.Items.Clear()
        Dim StackItems() As String = [Enum].GetNames(GetType(EffectStackType))
        For Each StackItem As String In StackItems
            cboStackEffect.Items.Add(StackItem)
        Next
        cboStackEffect.EndUpdate()
        cboStackEffect.SelectedIndex = 0

        ' Set up the calculation type
        cboCalcType.BeginUpdate()
        cboCalcType.Items.Clear()
        Dim CalcItems() As String = [Enum].GetNames(GetType(EffectCalcType))
        For Each CalcItem As String In CalcItems
            cboCalcType.Items.Add(CalcItem)
        Next
        cboCalcType.EndUpdate()
        cboCalcType.SelectedIndex = 0

        ' Set up the effect type
        cboEffectType.BeginUpdate()
        cboEffectType.Items.Clear()
        Dim EffectItems() As String = [Enum].GetNames(GetType(HQFEffectType))
        For Each EffectItem As String In EffectItems
            cboEffectType.Items.Add(EffectItem)
        Next
        cboEffectType.EndUpdate()

        ' Set up attributes
        cboAttribute.BeginUpdate()
        cboAttribute.Items.Clear()
        For Each Att As Integer In Attributes.AttributeQuickList.Keys
            cboAttribute.Items.Add(Attributes.AttributeQuickList.Item(Att).ToString & " (" & Att & ")")
        Next
        cboAttribute.Sorted = True
        cboAttribute.EndUpdate()

        ' Set the shipID
        lblShipID.Text = "ShipID: " & cShipID

        ' Check if the form has been passed a ShipEffect to edit
        If cNewShipEffect.Status = 0 Then
            ' Set the effect type
            cboEffectType.SelectedIndex = 0
            ' Setup Initial ShipEffect class values
            cNewShipEffect = New ShipEffect
            cNewShipEffect.ShipID = CInt(cShipID)
            cNewShipEffect.Value = 0
            cNewShipEffect.AffectingType = HQFEffectType.All
            cNewShipEffect.AffectingID = 0
            cNewShipEffect.Status = 15

        Else
            ' Editing, so update the UI with the values
            Call Me.UpdateUI()
        End If

    End Sub

    Private Sub UpdateUI()
        ' Updates the UI with the values to edit

        ' Update the role
        If cNewShipEffect.IsPerLevel = True Then
            radSkillBonus.Checked = True
            cboSkill.SelectedItem = StaticData.Types(cNewShipEffect.AffectingID).Name
            cboSkill.Enabled = True
        Else
            radRole.Checked = True
            cboSkill.Enabled = False
        End If

        ' Update the attribute
        cboAttribute.SelectedItem = Attributes.AttributeQuickList.Item(cNewShipEffect.AffectedAtt).ToString & " (" & cNewShipEffect.AffectedAtt.ToString & ")"

        ' Update the stacking type
        cboStackEffect.SelectedIndex = cNewShipEffect.StackNerf

        ' Update the calculation type
        cboCalcType.SelectedIndex = cNewShipEffect.CalcType

        ' Update the status
        If (cNewShipEffect.Status And 1) = 1 Then
            chkOffline.Checked = True
        Else
            chkOffline.Checked = False
        End If
        If (cNewShipEffect.Status And 2) = 2 Then
            chkInactive.Checked = True
        Else
            chkInactive.Checked = False
        End If
        If (cNewShipEffect.Status And 4) = 4 Then
            chkActive.Checked = True
        Else
            chkActive.Checked = False
        End If
        If (cNewShipEffect.Status And 8) = 8 Then
            chkOverloaded.Checked = True
        Else
            chkOverloaded.Checked = False
        End If

        ' Update the value
        diValue.Value = cNewShipEffect.Value

        ' Update the Use Active Ship checkbox
        If cNewShipEffect.AffectedID.Count = 1 And cNewShipEffect.AffectedID(0) = cShipID Then
            chkUseActiveShip.Checked = True
        Else
            ' Update the EffectType
            cboEffectType.SelectedIndex = cNewShipEffect.AffectedType
            ' Update the item lists
            lvwItems.BeginUpdate()
            lvwItems.Items.Clear()
            For Each id As Integer In cNewShipEffect.AffectedID
                Dim newItem As New ListViewItem
                newItem.Name = CStr(id)
                Select Case cNewShipEffect.AffectedType
                    Case HQFEffectType.All
                        ' Nothing here?
                    Case HQFEffectType.Item
                        newItem.Text = StaticData.Types(CInt(id)).Name
                    Case HQFEffectType.Group
                        newItem.Text = StaticData.TypeGroups(CInt(id))
                    Case HQFEffectType.Category
                        newItem.Text = StaticData.TypeCats(CInt(id))
                    Case HQFEffectType.MarketGroup
                        newItem.Text = Market.MarketGroupPath(id).ToString.Replace("&", "&&")
                    Case HQFEffectType.Skill
                        newItem.Text = StaticData.Types(CInt(id)).Name
                    Case HQFEffectType.Slot
                        ' Not supported!!
                    Case HQFEffectType.Attribute
                        newItem.Text = Attributes.AttributeQuickList(id).ToString
                End Select
                lvwItems.Items.Add(newItem)
            Next
            lvwItems.EndUpdate()
        End If

    End Sub

#End Region

#Region "UI Update Routines"
    Private Sub radRole_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radRole.CheckedChanged
        ' Do nothing as the toggle is handled by the radSkillBonus.CheckedChanged method
    End Sub

    Private Sub radSkillBonus_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radSkillBonus.CheckedChanged
        If FormIsInitialising = False Then
            ' Toggle skill combobox
            cboSkill.Enabled = radSkillBonus.Checked
            ' Alter the ship effect
            If radSkillBonus.Checked = True Then
                ' This is skill bonus
                cNewShipEffect.AffectingType = HQFEffectType.Item
                If cboSkill.SelectedItem Is Nothing Then
                    cboSkill.SelectedIndex = 0
                End If
                Dim SkillName As String = cboSkill.SelectedItem.ToString
                cNewShipEffect.AffectingID = CInt(Core.HQ.SkillListName(SkillName).ID)
                cNewShipEffect.IsPerLevel = True
            Else
                ' This is a role bonus
                cNewShipEffect.AffectingType = HQFEffectType.All
                cNewShipEffect.AffectingID = 0
                cNewShipEffect.IsPerLevel = False
            End If
            ' Update the description
            txtDescription.Text = EffectFunctions.ConvertShipBonusesToDescription(cNewShipEffect)
        End If
    End Sub

    Private Sub cboSkill_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSkill.SelectedIndexChanged
        If FormIsInitialising = False Then
            If cboSkill.SelectedItem IsNot Nothing Then
                Dim SkillName As String = cboSkill.SelectedItem.ToString
                cNewShipEffect.AffectingID = CInt(Core.HQ.SkillListName(SkillName).ID)
            Else
                cNewShipEffect.AffectingID = 0
            End If
            ' Update the description
            txtDescription.Text = EffectFunctions.ConvertShipBonusesToDescription(cNewShipEffect)
        End If
    End Sub

    Private Sub cboStackEffect_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboStackEffect.SelectedIndexChanged
        If FormIsInitialising = False Then
            cNewShipEffect.StackNerf = CType(cboStackEffect.SelectedIndex, EffectStackType)
            ' Update the description
            txtDescription.Text = EffectFunctions.ConvertShipBonusesToDescription(cNewShipEffect)
        End If
    End Sub

    Private Sub cboCalcType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboCalcType.SelectedIndexChanged
        If FormIsInitialising = False Then
            cNewShipEffect.CalcType = CType(cboCalcType.SelectedIndex, EffectCalcType)
            ' Update the description
            txtDescription.Text = EffectFunctions.ConvertShipBonusesToDescription(cNewShipEffect)
        End If
    End Sub

    Private Sub diValue_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles diValue.ValueChanged
        If FormIsInitialising = False Then
            cNewShipEffect.Value = diValue.Value
            ' Update the description
            txtDescription.Text = EffectFunctions.ConvertShipBonusesToDescription(cNewShipEffect)
        End If
    End Sub

    Private Sub cboAttribute_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboAttribute.SelectedIndexChanged
        If FormIsInitialising = False Then
            ' Extract the name and ID from the selected attribute with a Regex
            Dim attID As Integer = 0
            If cboAttribute.SelectedItem IsNot Nothing Then
                attID = EffectFunctions.ExtractIDFromAttributeDetails(cboAttribute.SelectedItem.ToString)
            Else
                attID = 0
            End If
            cNewShipEffect.AffectedAtt = attID
            ' Update the description
            txtDescription.Text = EffectFunctions.ConvertShipBonusesToDescription(cNewShipEffect)
        End If
    End Sub

    Private Sub cboEffectType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEffectType.SelectedIndexChanged
        Dim oldType As HQFEffectType = cNewShipEffect.AffectedType
        ' Check if we want to change this if there are items
        If lvwItems.Items.Count > 0 And CType(cboEffectType.SelectedIndex, HQFEffectType) <> oldType Then
            Dim reply As DialogResult = MessageBox.Show("This will clear any existing items. Are you sure you want to change the type?", "Confirm Type Change?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.Yes Then
                ' We want to change the type, so reset the list
                lvwItems.Items.Clear()
                ' Recalculate all IDs
                Call RecalculateAffectedIDs()
            Else
                ' We don't want to change type, so change the type back and exit
                cboEffectType.SelectedIndex = oldType
                Exit Sub
            End If
        End If
        cNewShipEffect.AffectedType = CType(cboEffectType.SelectedIndex, HQFEffectType)
        ' Update the contents of the affectedID box depending on our result
        Select Case cNewShipEffect.AffectedType
            Case HQFEffectType.All
                cboItems.Nodes.Clear()
                ' Disable the boxes
                cboItems.Enabled = False
                btnAddItem.Enabled = False
            Case HQFEffectType.Item
                ' Show items
                cboItems.SuspendLayout()
                cboItems.Nodes.Clear()
                For Each NewMod As String In ModuleLists.moduleListName.Keys
                    Dim NewNode As New DevComponents.AdvTree.Node
                    NewNode.NodesIndent = 0
                    NewNode.Text = NewMod
                    NewNode.Name = ModuleLists.moduleListName(NewMod).ToString
                    cboItems.Nodes.Add(NewNode)
                Next
                cboItems.ResumeLayout()
                ' Enable the box
                cboItems.Enabled = True
                btnAddItem.Enabled = True
                ' Add in the current ship ID if we have the Use Active Ship item checked
                If chkUseActiveShip.Checked = True Then
                    Dim newItem As New ListViewItem
                    newItem.Text = CStr(cShipID)
                    newItem.Name = CStr(cShipID)
                    lvwItems.Items.Add(newItem)
                    ' Recalculate all IDs
                    Call RecalculateAffectedIDs()
                    ' Update the description
                    txtDescription.Text = EffectFunctions.ConvertShipBonusesToDescription(cNewShipEffect)
                End If
            Case HQFEffectType.Group
                ' Show items
                cboItems.SuspendLayout()
                cboItems.Nodes.Clear()
                For Each newGroup As Integer In StaticData.TypeGroups.Keys
                    Dim newNode As New DevComponents.AdvTree.Node
                    newNode.NodesIndent = 0
                    newNode.Text = StaticData.TypeGroups(newGroup)
                    newNode.Name = newGroup.ToString
                    cboItems.Nodes.Add(newNode)
                Next
                cboItems.Nodes.Sort()
                cboItems.ResumeLayout()
                ' Enable the box
                cboItems.Enabled = True
                btnAddItem.Enabled = True
            Case HQFEffectType.Category
                ' Show items
                cboItems.SuspendLayout()
                cboItems.Nodes.Clear()
                For Each newCat As Integer In StaticData.TypeCats.Keys
                    Dim newNode As New Node
                    newNode.NodesIndent = 0
                    newNode.Text = StaticData.TypeCats(newCat)
                    newNode.Name = newCat.ToString
                    cboItems.Nodes.Add(newNode)
                Next
                cboItems.Nodes.Sort()
                cboItems.ResumeLayout()
                ' Enable the box
                cboItems.Enabled = True
                btnAddItem.Enabled = True
            Case HQFEffectType.MarketGroup
                ' Show items
                cboItems.SuspendLayout()
                cboItems.Nodes.Clear()
                For Each oldNode As Node In Market.MarketNodeList
                    If oldNode.Nodes.Count > 0 Then
                        cboItems.Nodes.Add(oldNode)
                    End If
                Next
                cboItems.ResumeLayout()
                ' Enable the box
                cboItems.Enabled = True
                btnAddItem.Enabled = True
            Case HQFEffectType.Skill
                ' Show items
                cboItems.SuspendLayout()
                cboItems.Nodes.Clear()
                For Each newSkill As Core.EveSkill In Core.HQ.SkillListName.Values
                    Dim newNode As New DevComponents.AdvTree.Node
                    newNode.NodesIndent = 0
                    newNode.Text = newSkill.Name
                    newNode.Name = CStr(newSkill.ID)
                    cboItems.Nodes.Add(newNode)
                Next
                cboItems.ResumeLayout()
                ' Enable the box
                cboItems.Enabled = True
                btnAddItem.Enabled = True
            Case HQFEffectType.Slot
                MessageBox.Show("Slot Effect Type is not implemented for Ship Bonuses.", "Effect Type Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' We don't want to change type, so change the type back and exit
                cboEffectType.SelectedIndex = oldType
                Exit Sub
            Case HQFEffectType.Attribute
                ' Show items
                cboItems.SuspendLayout()
                cboItems.Nodes.Clear()
                For Each att As Integer In Attributes.AttributeQuickList.Keys
                    Dim newNode As New DevComponents.AdvTree.Node
                    newNode.NodesIndent = 0
                    newNode.Text = Attributes.AttributeQuickList.Item(att).ToString & " (" & att & ")"
                    newNode.Name = CStr(att)
                    cboItems.Nodes.Add(newNode)
                Next
                cboItems.Nodes.Sort()
                cboItems.ResumeLayout()
                ' Enable the box
                cboItems.Enabled = True
                btnAddItem.Enabled = True
        End Select
        ' Update the description
        txtDescription.Text = EffectFunctions.ConvertShipBonusesToDescription(cNewShipEffect)
    End Sub

    Private Sub btnAddItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddItem.Click
        If cboItems.SelectedNode IsNot Nothing Then
            Dim ItemNode As DevComponents.AdvTree.Node = cboItems.SelectedNode
            Select Case cNewShipEffect.AffectedType
                Case HQFEffectType.MarketGroup
                    If ItemNode.Tag IsNot Nothing Then
                        If ItemNode.Tag.ToString <> "0" Then
                            If lvwItems.Items.ContainsKey(ItemNode.Tag.ToString) = False Then
                                Dim newItem As New ListViewItem
                                newItem.Text = HQF.Market.MarketGroupPath(ItemNode.Tag.ToString).ToString.Replace("&", "&&")
                                newItem.Name = ItemNode.Tag.ToString
                                lvwItems.Items.Add(newItem)
                                ' Recalculate all IDs
                                Call RecalculateAffectedIDs()
                                ' Update the description
                                txtDescription.Text = EffectFunctions.ConvertShipBonusesToDescription(cNewShipEffect)
                            End If
                        Else
                            MessageBox.Show("You cannot add this market group as it is a placeholder for sub-groups.", "Select Specific Group", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    Else
                        MessageBox.Show("You cannot add this market group as it is a placeholder for sub-groups.", "Select Specific Group", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                Case Else
                    If lvwItems.Items.ContainsKey(ItemNode.Name) = False Then
                        Dim newItem As New ListViewItem
                        newItem.Text = ItemNode.Text
                        newItem.Name = ItemNode.Name
                        lvwItems.Items.Add(newItem)
                        ' Recalculate all IDs
                        Call RecalculateAffectedIDs()
                        ' Update the description
                        txtDescription.Text = EffectFunctions.ConvertShipBonusesToDescription(cNewShipEffect)
                    End If
            End Select
        End If
    End Sub

    Private Sub lvwItems_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwItems.SelectedIndexChanged
        If lvwItems.SelectedItems.Count > 0 Then
            btnDelete.Enabled = True
        Else
            btnDelete.Enabled = False
        End If
    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        If lvwItems.Items.Count > 0 Then
            Dim reply As DialogResult = MessageBox.Show("Are you sure you want to clear all items?", "Confirm Clearing", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.Yes Then
                lvwItems.Items.Clear()
                ' Recalculate all IDs
                Call RecalculateAffectedIDs()
                ' Update the description
                txtDescription.Text = EffectFunctions.ConvertShipBonusesToDescription(cNewShipEffect)
            End If
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If lvwItems.SelectedItems.Count > 0 Then
            Dim reply As DialogResult = MessageBox.Show("Are you sure you want to delete the selected items?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = Windows.Forms.DialogResult.Yes Then
                ' Form a list of items to delete
                Dim DeleteList As New List(Of String)
                For Each item As ListViewItem In lvwItems.SelectedItems
                    DeleteList.Add(item.Name)
                Next
                ' Delete the items
                lvwItems.BeginUpdate()
                For Each item As String In DeleteList
                    lvwItems.Items.RemoveByKey(item)
                Next
                lvwItems.EndUpdate()
                ' Recalculate all IDs
                Call RecalculateAffectedIDs()
                ' Update the description
                txtDescription.Text = EffectFunctions.ConvertShipBonusesToDescription(cNewShipEffect)
            End If
        End If
    End Sub

    Private Sub RecalculateAffectedIDs()
        cNewShipEffect.AffectedID.Clear()
        For Each item As ListViewItem In lvwItems.Items
            cNewShipEffect.AffectedID.Add(CInt(item.Name))
        Next
    End Sub

#End Region

#Region "Form Accept and Cancel Routines"
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        ' Check we have everything we need for a bonus before closing

        ' Check value is not nothing
        If diValue.Value = 0 Then
            MessageBox.Show("Bonus value cannot be zero. Please select another value.", "Bonus Value Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Check attribute is not nothing
        If cboAttribute.SelectedItem Is Nothing Then
            MessageBox.Show("Target Attribute must be stated. Please select an option.", "Attribute Value Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Check status if not blank
        If cNewShipEffect.Status = 0 Then
            MessageBox.Show("At lease one status must be selected for the ship bonus to apply (recommended is all four).", "Bonus Status Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Check if not global, we have at least one affected item
        Select Case cboEffectType.SelectedIndex
            Case HQFEffectType.All
                ' Allow exit regardless of contents of items
            Case Else
                ' Check we have at least one item
                If cNewShipEffect.AffectedID.Count < 1 Then
                    MessageBox.Show("At least one item must be entered for the selected Type.", "Affected Item Error", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If
        End Select

        ' Set the result
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()

    End Sub


#End Region


    Private Sub chkOffline_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOffline.CheckedChanged
        If FormIsInitialising = False Then
            If chkOffline.Checked = True Then
                cNewShipEffect.Status += ModuleStates.Offline
            Else
                cNewShipEffect.Status -= ModuleStates.Offline
            End If
        End If
    End Sub

    Private Sub chkInactive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkInactive.CheckedChanged
        If FormIsInitialising = False Then
            If chkInactive.Checked = True Then
                cNewShipEffect.Status += ModuleStates.Inactive
            Else
                cNewShipEffect.Status -= ModuleStates.Inactive
            End If
        End If
    End Sub

    Private Sub chkActive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkActive.CheckedChanged
        If FormIsInitialising = False Then
            If chkActive.Checked = True Then
                cNewShipEffect.Status += ModuleStates.Active
            Else
                cNewShipEffect.Status -= ModuleStates.Active
            End If
        End If
    End Sub

    Private Sub chkOverloaded_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOverloaded.CheckedChanged
        If FormIsInitialising = False Then
            If chkOverloaded.Checked = True Then
                cNewShipEffect.Status += ModuleStates.Overloaded
            Else
                cNewShipEffect.Status -= ModuleStates.Overloaded
            End If
        End If
    End Sub

    Private Sub chkUseActiveShip_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseActiveShip.CheckedChanged
        ' If active, set the Type and items accordingly (and disable)
        If chkUseActiveShip.Checked = True Then
            cboEffectType.SelectedIndex = HQFEffectType.Item
            cboEffectType.Enabled = False
            cboItems.Enabled = False
            btnAddItem.Enabled = False
        Else
            cboEffectType.Enabled = True
            cboItems.Enabled = True
            btnAddItem.Enabled = True
        End If
    End Sub

End Class