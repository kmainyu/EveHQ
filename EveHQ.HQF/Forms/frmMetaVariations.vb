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

Public Class frmMetaVariations

    Dim cBaseModule As New ShipModule
    Dim cActiveFitting As Fitting
    Dim itemVariations(,) As String
    Dim compItems As New SortedList
    Dim ColumnIndexes As New SortedList(Of String, Integer)
    Dim CurrentColumnIndex As Integer = -1
    Dim Startup As Boolean = True

#Region "Constructor"

    Public Sub New(ByVal ActiveFitting As Fitting, ByVal BaseModule As ShipModule)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        cActiveFitting = ActiveFitting
        btnAddToFitting.Enabled = False
        btnReplaceModules.Enabled = False
        cBaseModule = BaseModule
        Me.Text = "HQF Meta Variations - " & BaseModule.Name
        btnReplaceModules.Text = "Replace " & BaseModule.Name
        Call Me.GetVariations(cBaseModule)
        Startup = False
    End Sub

#End Region

    Private Sub GetVariations(ByVal startModule As ShipModule)
        Dim metaTypeID As String = startModule.ID

        Dim strSQL As String = ""
        strSQL &= "SELECT invMetaTypes.typeID, invMetaTypes.parentTypeID"
        strSQL &= " FROM invMetaTypes"
        strSQL &= " WHERE (((invMetaTypes.typeID)=" & metaTypeID & ") OR ((invMetaTypes.parentTypeID)=" & metaTypeID & "));"
        Dim eveData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
        If eveData.Tables(0).Rows.Count > 0 Then
            Dim metaParentID As String = eveData.Tables(0).Rows(0).Item("parentTypeID").ToString
            strSQL = ""
            strSQL &= "SELECT invTypes.typeID AS invTypes_typeID, invTypes.typeName, invMetaTypes.typeID AS invMetaTypes_typeID, invMetaTypes.parentTypeID, invMetaTypes.metaGroupID AS invMetaTypes_metaGroupID, invMetaGroups.metaGroupID AS invMetaGroups_metaGroupID, invMetaGroups.metaGroupName"
            strSQL &= " FROM invMetaGroups INNER JOIN (invTypes INNER JOIN invMetaTypes ON invTypes.typeID = invMetaTypes.typeID) ON invMetaGroups.metaGroupID = invMetaTypes.metaGroupID"
            strSQL &= " WHERE (((invMetaTypes.parentTypeID)=" & metaParentID & "));"
            eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
            Dim metaItemCount As Integer = eveData.Tables(0).Rows.Count
            ReDim itemVariations(2, metaItemCount)
            For item As Integer = 0 To metaItemCount - 1
                itemVariations(0, item + 1) = eveData.Tables(0).Rows(item).Item("invTypes_typeID").ToString
                itemVariations(1, item + 1) = eveData.Tables(0).Rows(item).Item("typeName").ToString.Trim
                itemVariations(2, item + 1) = eveData.Tables(0).Rows(item).Item("metaGroupName").ToString.Trim
            Next
            strSQL = "SELECT invTypes.typeID, invTypes.typeName FROM invTypes WHERE invTypes.typeID=" & metaParentID & ";"
            eveData = EveHQ.Core.DataFunctions.GetData(strSQL)
            itemVariations(0, 0) = eveData.Tables(0).Rows(0).Item("typeID").ToString.Trim
            itemVariations(1, 0) = eveData.Tables(0).Rows(0).Item("typeName").ToString.Trim
            itemVariations(2, 0) = "Tech I"

            ' Generate Comparisons
            compItems.Clear()
            For item As Integer = 0 To metaItemCount
                compItems.Add(itemVariations(0, item), itemVariations(1, item))
            Next
            ' Get all the comparatives
            Call Me.GetComparatives()

        End If
    End Sub
    Private Sub GetComparatives()
        Dim ModuleList As New ArrayList

        For Each modID As String In compItems.Keys
            If ModuleLists.moduleList.ContainsKey(modID) = True Then
                Dim sModule As ShipModule = CType(ModuleLists.moduleList.Item(modID), ShipModule).Clone
                If chkApplySkills.Checked = True Then
                    If cActiveFitting IsNot Nothing Then
                        cActiveFitting.ApplySkillEffectsToModule(sModule, True)
                    End If
                End If
                ModuleList.Add(sModule)
            End If
        Next

        adtComparisons.BeginUpdate()
        adtComparisons.Nodes.Clear()

        ' Add columns
        adtComparisons.Columns.Clear()
        Dim ItemColumn As New DevComponents.AdvTree.ColumnHeader("Item")
        ItemColumn.Width.Absolute = 275
        ItemColumn.DisplayIndex = 1
        Dim MetaColumn As New DevComponents.AdvTree.ColumnHeader("Meta")
        MetaColumn.Width.Absolute = 50
        MetaColumn.DisplayIndex = 2
        adtComparisons.Columns.Add(ItemColumn)
        adtComparisons.Columns.Add(MetaColumn)

        Dim noColumn As New ArrayList
        Dim BaseModule As ShipModule = CType(ModuleList.Item(0), ShipModule)

        ' Check which columns are required
        Dim SortColumn As Integer = 2 ' Defaults to the meta level
        ColumnIndexes.Clear()
        Dim ColumnIdx As Integer = 2
        For Each att As String In BaseModule.Attributes.Keys
            Dim colRequired As Boolean = False
            If HQF.Settings.HQFSettings.IgnoredAttributeColumns.Contains(att) = False And att <> "633" Then
                If Me.chkShowAllColumns.Checked = False Then
                    For Each sMod As ShipModule In ModuleList
                        If sMod.Attributes.ContainsKey(att) = True Then
                            If CDbl(sMod.Attributes(att)) <> CDbl(BaseModule.Attributes(att)) Then
                                colRequired = True
                                Exit For
                            End If
                        Else
                            colRequired = True
                            Exit For
                        End If
                    Next
                Else
                    colRequired = True
                End If
            End If
            If colRequired = True Then
                Dim newCol As New DevComponents.AdvTree.ColumnHeader
                newCol.Text = CType(Attributes.AttributeList(att), Attribute).DisplayName
                newCol.Name = att
                newCol.Tag = CType(Attributes.AttributeList(att), Attribute).UnitName
                newCol.Width.AutoSize = True
                newCol.Width.AutoSizeMinHeader = True
                newCol.EditorType = eCellEditorType.Custom
                newCol.StyleMouseOver = "Yellow"
                If newCol.Text <> "" Then
                    ColumnIdx += 1
                    newCol.DisplayIndex = ColumnIdx
                    adtComparisons.Columns.Add(newCol)
                    ColumnIndexes.Add(att, ColumnIdx - 1)
                    ' Check if this is our sorted column
                    If HQF.Settings.HQFSettings.SortedAttributeColumn = att Then
                        SortColumn = ColumnIdx
                    End If
                End If
            End If
        Next

        ' Add the modules
        For Each sMod As ShipModule In ModuleList
            Dim newMod As New Node
            newMod.Text = sMod.Name
            newMod.Name = sMod.Name
            Dim mlItem As New Cell(sMod.MetaLevel.ToString, adtComparisons.Styles("RightAlign"))
            mlItem.Tag = sMod.MetaLevel.ToString
            newMod.Cells.Add(mlItem)
            ' Add column placeholders
            For c As Integer = 2 To adtComparisons.Columns.Count
                newMod.Cells.Add(New Cell("", adtComparisons.Styles("RightAlign")))
            Next
            ' Now populate the list
            Dim i As Integer = 0
            For Each att As String In sMod.Attributes.Keys
                If ColumnIndexes.ContainsKey(att) Then
                    i = ColumnIndexes(att)
                    ' Adjust for TypeIDs
                    Select Case adtComparisons.Columns(att).Tag.ToString
                        Case "typeID"
                            newMod.Cells(i).Text = EveHQ.Core.HQ.itemData(CStr(sMod.Attributes(att))).Name
                            newMod.Cells(i).Tag = newMod.Cells(i).Text
                            newMod.Cells(i).Name = i.ToString
                        Case "Level"
                            newMod.Cells(i).Text = Format(sMod.Attributes(att), "#,###,##0.########")
                            newMod.Cells(i).Tag = CStr(sMod.Attributes(att))
                            newMod.Cells(i).Name = i.ToString
                        Case Else
                            newMod.Cells(i).Text = Format(sMod.Attributes(att), "#,###,##0.########") & " " & adtComparisons.Columns(att).Tag.ToString
                            newMod.Cells(i).Tag = CStr(sMod.Attributes(att))
                            newMod.Cells(i).Name = i.ToString
                    End Select
                End If
            Next
            adtComparisons.Nodes.Add(newMod)
        Next
        EveHQ.Core.AdvTreeSorter.Sort(adtComparisons, SortColumn, False, True)
        adtComparisons.EndUpdate()

    End Sub

    Private Sub chkShowAllColumns_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowAllColumns.CheckedChanged
        Call Me.GetVariations(cBaseModule)
    End Sub

    Private Sub adtComparisons_ColumnHeaderMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles adtComparisons.ColumnHeaderMouseUp
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
        ' Set the last forced sort
        HQF.Settings.HQFSettings.SortedAttributeColumn = CH.Name
    End Sub

    Private Sub chkApplySkills_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkApplySkills.CheckedChanged
        Call Me.GetVariations(cBaseModule)
    End Sub

    Private Sub ctxItems_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ctxItems.Opening
        If adtComparisons.SelectedNodes.Count = 0 Then
            e.Cancel = True
        ElseIf adtComparisons.SelectedNodes.Count = 1 Then
            Dim moduleName As String = adtComparisons.SelectedNodes(0).Name
            mnuModuleName.Text = moduleName
            mnuReplaceModule.Text = "Replace " & cBaseModule.Name
            If cActiveFitting IsNot Nothing Then
                mnuAddToExistingFitting.Enabled = True
                mnuReplaceModule.Enabled = True
            Else
                mnuAddToExistingFitting.Enabled = False
                mnuReplaceModule.Enabled = False
            End If
        End If
        If CurrentColumnIndex > -1 Then
            mnuRemoveColumn.Text = "Remove '" & adtComparisons.Columns(CurrentColumnIndex).Text & "' Column"
            mnuRemoveColumn.Enabled = True
        Else
            mnuRemoveColumn.Text = "Remove Column"
            mnuRemoveColumn.Enabled = False
        End If
    End Sub

    Private Sub mnuAddToExistingFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuAddToExistingFitting.Click
        Call Me.AddModuleToFitting()
    End Sub

    Private Sub AddModuleToFitting()
        If cActiveFitting IsNot Nothing Then
            If cActiveFitting.ShipSlotCtrl IsNot Nothing Then
                If adtComparisons.SelectedNodes.Count = 1 Then
                    Dim moduleName As String = adtComparisons.SelectedNodes(0).Name
                    Dim moduleID As String = CStr(ModuleLists.moduleListName(moduleName))
                    Dim shipMod As ShipModule = CType(ModuleLists.moduleList(moduleID), ShipModule).Clone
                    If shipMod.IsDrone = True Then
                        Dim active As Boolean = False
                        Call cActiveFitting.AddDrone(shipMod, 1, False, False)
                    Else
                        ' Check if module is a charge
                        If shipMod.IsCharge = True Or shipMod.IsContainer Then
                            cActiveFitting.AddItem(shipMod, 1, False)
                        Else
                            ' Must be a proper module then!
                            cActiveFitting.AddModule(shipMod, 0, True, False, Nothing, False, False)
                            ' Add it to the MRU
                            HQFEvents.StartUpdateMRUModuleList = shipMod.Name
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub mnuReplaceModule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuReplaceModule.Click
        Call Me.ReplaceModules()
    End Sub

    Private Sub ReplaceModules()
        If cActiveFitting IsNot Nothing Then
            If cActiveFitting.ShipSlotCtrl IsNot Nothing Then
                Dim moduleName As String = adtComparisons.SelectedNodes(0).Name
                Dim moduleID As String = CStr(ModuleLists.moduleListName(moduleName))
                Dim NewModule As ShipModule = CType(ModuleLists.moduleList(moduleID), ShipModule).Clone
                NewModule.ModuleState = cBaseModule.ModuleState
                Dim OldChargeName As String = ""
                Dim NewChargeName As String = ""
                If cBaseModule.LoadedCharge IsNot Nothing Then
                    OldChargeName = cBaseModule.LoadedCharge.Name
                    Dim CurrentChargeGroup As String = cBaseModule.LoadedCharge.DatabaseGroup
                    If NewModule.Charges.Contains(CurrentChargeGroup) = False Then
                        Dim reply As DialogResult = MessageBox.Show(cBaseModule.LoadedCharge.Name & " cannot be loaded into the " & NewModule.Name & ". Would you like to remove the charges and continue?", "Charge Incompatability", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        If reply = Windows.Forms.DialogResult.No Then
                            Exit Sub
                        End If
                    Else
                        NewModule.LoadedCharge = cBaseModule.LoadedCharge.Clone
                        NewChargeName = NewModule.LoadedCharge.Name
                    End If
                End If

                Select Case cBaseModule.SlotType
                    Case SlotTypes.Rig  ' Rig
                        For slot As Integer = 1 To cActiveFitting.BaseShip.RigSlots
                            If cActiveFitting.BaseShip.RigSlot(slot) IsNot Nothing Then
                                If cActiveFitting.BaseShip.RigSlot(slot).Name = cBaseModule.Name Then
                                    cActiveFitting.ShipSlotCtrl.UndoStack.Push(New UndoInfo(UndoInfo.TransType.ReplacedModule, SlotTypes.Rig, slot, NewModule.Name, NewChargeName, slot, cBaseModule.Name, OldChargeName))
                                    cActiveFitting.BaseShip.RigSlot(slot) = NewModule.Clone
                                End If
                            End If
                        Next
                    Case SlotTypes.Low  ' Low
                        For slot As Integer = 1 To cActiveFitting.BaseShip.LowSlots
                            If cActiveFitting.BaseShip.LowSlot(slot) IsNot Nothing Then
                                If cActiveFitting.BaseShip.LowSlot(slot).Name = cBaseModule.Name Then
                                    cActiveFitting.ShipSlotCtrl.UndoStack.Push(New UndoInfo(UndoInfo.TransType.ReplacedModule, SlotTypes.Low, slot, NewModule.Name, NewChargeName, slot, cBaseModule.Name, OldChargeName))
                                    cActiveFitting.BaseShip.LowSlot(slot) = NewModule.Clone
                                End If
                            End If
                        Next
                    Case SlotTypes.Mid  ' Mid
                        For slot As Integer = 1 To cActiveFitting.BaseShip.MidSlots
                            If cActiveFitting.BaseShip.MidSlot(slot) IsNot Nothing Then
                                If cActiveFitting.BaseShip.MidSlot(slot).Name = cBaseModule.Name Then
                                    cActiveFitting.ShipSlotCtrl.UndoStack.Push(New UndoInfo(UndoInfo.TransType.ReplacedModule, SlotTypes.Mid, slot, NewModule.Name, NewChargeName, slot, cBaseModule.Name, OldChargeName))
                                    cActiveFitting.BaseShip.MidSlot(slot) = NewModule.Clone
                                End If
                            End If
                        Next
                    Case SlotTypes.High  ' High
                        For slot As Integer = 1 To cActiveFitting.BaseShip.HiSlots
                            If cActiveFitting.BaseShip.HiSlot(slot) IsNot Nothing Then
                                If cActiveFitting.BaseShip.HiSlot(slot).Name = cBaseModule.Name Then
                                    cActiveFitting.ShipSlotCtrl.UndoStack.Push(New UndoInfo(UndoInfo.TransType.ReplacedModule, SlotTypes.High, slot, NewModule.Name, NewChargeName, slot, cBaseModule.Name, OldChargeName))
                                    cActiveFitting.BaseShip.HiSlot(slot) = NewModule.Clone
                                End If
                            End If
                        Next
                    Case SlotTypes.Subsystem    ' Subsystem
                        For slot As Integer = 1 To cActiveFitting.BaseShip.SubSlots
                            If cActiveFitting.BaseShip.SubSlot(slot) IsNot Nothing Then
                                If cActiveFitting.BaseShip.SubSlot(slot).Name = cBaseModule.Name Then
                                    cActiveFitting.ShipSlotCtrl.UndoStack.Push(New UndoInfo(UndoInfo.TransType.ReplacedModule, SlotTypes.Subsystem, slot, NewModule.Name, NewChargeName, slot, cBaseModule.Name, OldChargeName))
                                    cActiveFitting.BaseShip.SubSlot(slot) = NewModule.Clone
                                End If
                            End If
                        Next
                End Select
                cActiveFitting.ShipSlotCtrl.UpdateHistory()

                If CDbl(cBaseModule.DatabaseCategory) = 32 Then
                    cActiveFitting.BaseShip = cActiveFitting.BuildSubSystemEffects(cActiveFitting.BaseShip)
                    If cActiveFitting.ShipSlotCtrl IsNot Nothing Then
                        Call cActiveFitting.ShipSlotCtrl.UpdateShipSlotLayout()
                    End If
                    cActiveFitting.ApplyFitting(BuildType.BuildEverything)
                Else
                    cActiveFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
                End If

                ' Update the base module
                cBaseModule = NewModule
                Me.Text = "HQF Meta Variations - " & NewModule.Name
                btnReplaceModules.Text = "Replace " & NewModule.Name
                ' Add it to the MRU
                HQFEvents.StartUpdateMRUModuleList = moduleName
            End If
        End If
    End Sub

    Private Sub btnAddToFitting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddToFitting.Click
        Call Me.AddModuleToFitting()
    End Sub

    Private Sub btnReplaceModules_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReplaceModules.Click
        Call Me.ReplaceModules()
    End Sub

    Private Sub adtComparisons_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles adtComparisons.MouseDown
        Dim TestCell As Cell = adtComparisons.GetCellAt(e.Location)
        If TestCell IsNot Nothing Then
            If IsNumeric(TestCell.Name) Then
                CurrentColumnIndex = CInt(TestCell.Name)
            Else
                CurrentColumnIndex = -1
            End If
        Else
            CurrentColumnIndex = -1
        End If
    End Sub

    Private Sub adtComparisons_NodeDoubleClick(ByVal sender As Object, ByVal e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtComparisons.NodeDoubleClick
        Call Me.AddModuleToFitting()
    End Sub

    Private Sub adtComparisons_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles adtComparisons.SelectionChanged
        If adtComparisons.SelectedNodes.Count = 1 Then
            If cActiveFitting IsNot Nothing Then
                btnReplaceModules.Enabled = True
                btnAddToFitting.Enabled = True
            Else
                btnReplaceModules.Enabled = False
                btnAddToFitting.Enabled = False
            End If
        Else
            btnReplaceModules.Enabled = False
            btnAddToFitting.Enabled = False
        End If
    End Sub

    Private Sub mnuRemoveColumn_Click(sender As System.Object, e As System.EventArgs) Handles mnuRemoveColumn.Click
        If CurrentColumnIndex > -1 Then
            ' Get the attribute we want to ignore
            Dim IgnoredAtt As String = ""
            For Each att As String In ColumnIndexes.Keys
                If ColumnIndexes(att) = CurrentColumnIndex Then
                    IgnoredAtt = att
                End If
            Next
            If HQF.Settings.HQFSettings.IgnoredAttributeColumns.Contains(IgnoredAtt) = False Then
                HQF.Settings.HQFSettings.IgnoredAttributeColumns.Add(IgnoredAtt)
                ' Rebuild the column list
                Call Me.GetComparatives()
            End If
        End If
    End Sub

    Private Sub frmMetaVariations_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        If Startup = False Then
            HQF.Settings.HQFSettings.MetaVariationsFormSize = Me.Size
        End If
    End Sub

End Class