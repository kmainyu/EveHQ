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
Imports System.ComponentModel
Imports System.Windows.Forms
Imports DevComponents.AdvTree
Imports EveHQ.Core
Imports EveHQ.EveData

Namespace Forms

    Public Class FrmMetaVariations

        Dim _baseModule As New ShipModule
        ReadOnly _activeFitting As Fitting
        ReadOnly _compItems As New SortedList(Of Integer, String)
        ReadOnly _columnIndexes As New SortedList(Of String, Integer)
        Dim _currentColumnIndex As Integer = -1
        ReadOnly _startup As Boolean = True

#Region "Constructor"

        Public Sub New(ByVal activeFitting As Fitting, ByVal baseModule As ShipModule)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            _activeFitting = activeFitting
            btnAddToFitting.Enabled = False
            btnReplaceModules.Enabled = False
            _baseModule = BaseModule
            Text = "HQF Meta Variations - " & baseModule.Name
            btnReplaceModules.Text = "Replace " & BaseModule.Name
            Call GetVariations(_baseModule)
            _startup = False
        End Sub

#End Region

        Private Sub GetVariations(ByVal startModule As ShipModule)
            Dim metaTypeID As Integer = startModule.ID
            Dim metaItems As List(Of Integer) = StaticData.GetVariationsForItem(CInt(metaTypeID))

            ' Generate Comparisons
            _compItems.Clear()
            For Each metaItem As Integer In metaItems
                _compItems.Add(metaItem, StaticData.Types(metaItem).Name)
            Next
            ' Get all the comparatives
            Call GetComparatives()
        End Sub

        Private Sub GetComparatives()
            Dim moduleList As New List(Of ShipModule)

            For Each modID As Integer In _compItems.Keys
                If ModuleLists.ModuleList.ContainsKey(modID) = True Then
                    Dim sModule As ShipModule = ModuleLists.ModuleList.Item(modID).Clone
                    If chkApplySkills.Checked = True Then
                        If _activeFitting IsNot Nothing Then
                            _activeFitting.ApplySkillEffectsToModule(sModule, True)
                        End If
                    End If
                    moduleList.Add(sModule)
                End If
            Next

            adtComparisons.BeginUpdate()
            adtComparisons.Nodes.Clear()

            ' Add columns
            adtComparisons.Columns.Clear()
            Dim itemColumn As New DevComponents.AdvTree.ColumnHeader("Item")
            itemColumn.SortingEnabled = False
            itemColumn.Width.Absolute = 275
            itemColumn.DisplayIndex = 1
            Dim metaColumn As New DevComponents.AdvTree.ColumnHeader("Meta")
            metaColumn.SortingEnabled = False
            metaColumn.Width.Absolute = 50
            metaColumn.DisplayIndex = 2
            adtComparisons.Columns.Add(itemColumn)
            adtComparisons.Columns.Add(metaColumn)

            Dim baseModule As ShipModule = moduleList.Item(0)

            ' Check which columns are required
            Dim sortColumn As Integer = 2 ' Defaults to the meta level
            _columnIndexes.Clear()
            Dim columnIdx As Integer = 2
            For Each att As Integer In baseModule.Attributes.Keys
                Dim colRequired As Boolean = False
                If PluginSettings.HQFSettings.IgnoredAttributeColumns.Contains(CStr(att)) = False And att <> 633 Then
                    If chkShowAllColumns.Checked = False Then
                        For Each sMod As ShipModule In moduleList
                            If sMod.Attributes.ContainsKey(att) = True Then
                                If CDbl(sMod.Attributes(att)) <> CDbl(baseModule.Attributes(att)) Then
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
                    newCol.Text = Attributes.AttributeList(att).DisplayName
                    newCol.SortingEnabled = False
                    newCol.Name = CStr(att)
                    newCol.Tag = Attributes.AttributeList(att).UnitName
                    newCol.Width.AutoSize = True
                    newCol.Width.AutoSizeMinHeader = True
                    newCol.EditorType = eCellEditorType.Custom
                    If newCol.Text <> "" Then
                        columnIdx += 1
                        newCol.DisplayIndex = columnIdx
                        adtComparisons.Columns.Add(newCol)
                        _columnIndexes.Add(CStr(att), columnIdx - 1)
                        ' Check if this is our sorted column
                        If PluginSettings.HQFSettings.SortedAttributeColumn <> "" Then
                            If CInt(PluginSettings.HQFSettings.SortedAttributeColumn) = att Then
                                sortColumn = columnIdx
                            End If
                        End If
                    End If
                End If
            Next

            ' Add the modules
            For Each sMod As ShipModule In moduleList
                Dim newMod As New Node
                newMod.Text = sMod.Name
                newMod.Name = sMod.Name
                Dim mlItem As New Cell(sMod.MetaLevel.ToString, adtComparisons.Styles("RightAlign"))
                mlItem.Tag = sMod.MetaLevel.ToString
                newMod.Cells.Add(mlItem)
                ' Add column placeholders
                ' ReSharper disable once RedundantAssignment - Incorrect warning by R#
                For c As Integer = 2 To adtComparisons.Columns.Count
                    newMod.Cells.Add(New Cell("", adtComparisons.Styles("RightAlign")))
                Next
                ' Now populate the list
                Dim i As Integer
                For Each att As Integer In sMod.Attributes.Keys
                    If _columnIndexes.ContainsKey(CStr(att)) Then
                        i = _columnIndexes(CStr(att))
                        ' Adjust for TypeIDs
                        Select Case adtComparisons.Columns(CStr(att)).Tag.ToString
                            Case "typeID"
                                newMod.Cells(i).Text = StaticData.Types(CInt(sMod.Attributes(att))).Name
                                newMod.Cells(i).Tag = newMod.Cells(i).Text
                                newMod.Cells(i).Name = i.ToString
                            Case "Level"
                                newMod.Cells(i).Text = Format(sMod.Attributes(att), "#,###,##0.########")
                                newMod.Cells(i).Tag = CStr(sMod.Attributes(att))
                                newMod.Cells(i).Name = i.ToString
                            Case Else
                                newMod.Cells(i).Text = Format(sMod.Attributes(att), "#,###,##0.########") & " " & adtComparisons.Columns(CStr(att)).Tag.ToString
                                newMod.Cells(i).Tag = CStr(sMod.Attributes(att))
                                newMod.Cells(i).Name = i.ToString
                        End Select
                    End If
                Next
                adtComparisons.Nodes.Add(newMod)
            Next
            AdvTreeSorter.Sort(adtComparisons, sortColumn, False, True)
            adtComparisons.EndUpdate()

        End Sub

        Private Sub chkShowAllColumns_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkShowAllColumns.CheckedChanged
            Call GetVariations(_baseModule)
        End Sub

        Private Sub adtComparisons_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles adtComparisons.ColumnHeaderMouseDown
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, True, False)
            ' Set the last forced sort
            PluginSettings.HQFSettings.SortedAttributeColumn = ch.Name
        End Sub

        Private Sub chkApplySkills_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkApplySkills.CheckedChanged
            Call GetVariations(_baseModule)
        End Sub

        Private Sub ctxItems_Opening(ByVal sender As Object, ByVal e As CancelEventArgs) Handles ctxItems.Opening
            If adtComparisons.SelectedNodes.Count = 0 Then
                e.Cancel = True
            ElseIf adtComparisons.SelectedNodes.Count = 1 Then
                Dim moduleName As String = adtComparisons.SelectedNodes(0).Name
                mnuModuleName.Text = moduleName
                mnuReplaceModule.Text = "Replace " & _baseModule.Name
                If _activeFitting IsNot Nothing Then
                    mnuAddToExistingFitting.Enabled = True
                    mnuReplaceModule.Enabled = True
                Else
                    mnuAddToExistingFitting.Enabled = False
                    mnuReplaceModule.Enabled = False
                End If
            End If
            If _currentColumnIndex > -1 Then
                mnuRemoveColumn.Text = "Remove '" & adtComparisons.Columns(_currentColumnIndex).Text & "' Column"
                mnuRemoveColumn.Enabled = True
            Else
                mnuRemoveColumn.Text = "Remove Column"
                mnuRemoveColumn.Enabled = False
            End If
        End Sub

        Private Sub mnuAddToExistingFitting_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuAddToExistingFitting.Click
            Call AddModuleToFitting()
        End Sub

        Private Sub AddModuleToFitting()
            If _activeFitting IsNot Nothing Then
                If _activeFitting.ShipSlotCtrl IsNot Nothing Then
                    If adtComparisons.SelectedNodes.Count = 1 Then
                        Dim moduleName As String = adtComparisons.SelectedNodes(0).Name
                        Dim moduleID As Integer = ModuleLists.ModuleListName(moduleName)
                        Dim shipMod As ShipModule = ModuleLists.ModuleList(moduleID).Clone
                        If shipMod.IsDrone = True Then
                            Call _activeFitting.AddDrone(shipMod, 1, False, False)
                        Else
                            ' Check if module is a charge
                            If shipMod.IsCharge = True Or shipMod.IsContainer Then
                                _activeFitting.AddItem(shipMod, 1, False)
                            Else
                                ' Must be a proper module then!
                                _activeFitting.AddModule(shipMod, 0, True, False, Nothing, False, False)
                                ' Add it to the MRU
                                HQFEvents.StartUpdateMRUModuleList = shipMod.Name
                            End If
                        End If
                    End If
                End If
            End If
        End Sub

        Private Sub mnuReplaceModule_Click(ByVal sender As Object, ByVal e As EventArgs) Handles mnuReplaceModule.Click
            Call ReplaceModules()
        End Sub

        Private Sub ReplaceModules()
            If _activeFitting IsNot Nothing Then
                If _activeFitting.ShipSlotCtrl IsNot Nothing Then
                    Dim moduleName As String = adtComparisons.SelectedNodes(0).Name
                    Dim moduleID As Integer = ModuleLists.ModuleListName(moduleName)
                    Dim newModule As ShipModule = ModuleLists.ModuleList(moduleID).Clone
                    newModule.ModuleState = _baseModule.ModuleState
                    If _baseModule.LoadedCharge IsNot Nothing Then
                        Dim currentChargeGroup As Integer = _baseModule.LoadedCharge.DatabaseGroup
                        If newModule.Charges.Contains(currentChargeGroup) = False Then
                            Dim reply As DialogResult = MessageBox.Show(_baseModule.LoadedCharge.Name & " cannot be loaded into the " & newModule.Name & ". Would you like to remove the charges and continue?", "Charge Incompatability", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            If reply = DialogResult.No Then
                                Exit Sub
                            End If
                        Else
                            newModule.LoadedCharge = _baseModule.LoadedCharge.Clone
                        End If
                    End If

                    Select Case _baseModule.SlotType
                        Case SlotTypes.Rig
                            For slot As Integer = 1 To _activeFitting.BaseShip.RigSlots
                                If _activeFitting.BaseShip.RigSlot(slot) IsNot Nothing Then
                                    If _activeFitting.BaseShip.RigSlot(slot).Name = _baseModule.Name Then
                                        _activeFitting.AddModule(newModule, slot, False, False, _baseModule, False, False)
                                    End If
                                End If
                            Next
                        Case SlotTypes.Low
                            For slot As Integer = 1 To _activeFitting.BaseShip.LowSlots
                                If _activeFitting.BaseShip.LowSlot(slot) IsNot Nothing Then
                                    If _activeFitting.BaseShip.LowSlot(slot).Name = _baseModule.Name Then
                                        _activeFitting.AddModule(newModule, slot, False, False, _baseModule, False, False)
                                    End If
                                End If
                            Next
                        Case SlotTypes.Mid
                            For slot As Integer = 1 To _activeFitting.BaseShip.MidSlots
                                If _activeFitting.BaseShip.MidSlot(slot) IsNot Nothing Then
                                    If _activeFitting.BaseShip.MidSlot(slot).Name = _baseModule.Name Then
                                        _activeFitting.AddModule(newModule, slot, False, False, _baseModule, False, False)
                                    End If
                                End If
                            Next
                        Case SlotTypes.High
                            For slot As Integer = 1 To _activeFitting.BaseShip.HiSlots
                                If _activeFitting.BaseShip.HiSlot(slot) IsNot Nothing Then
                                    If _activeFitting.BaseShip.HiSlot(slot).Name = _baseModule.Name Then
                                        _activeFitting.AddModule(newModule, slot, False, False, _baseModule, False, False)
                                    End If
                                End If
                            Next
                        Case SlotTypes.Subsystem
                            For slot As Integer = 1 To _activeFitting.BaseShip.SubSlots
                                If _activeFitting.BaseShip.SubSlot(slot) IsNot Nothing Then
                                    If _activeFitting.BaseShip.SubSlot(slot).Name = _baseModule.Name Then
                                        _activeFitting.AddModule(newModule, slot, False, False, _baseModule, False, False)
                                    End If
                                End If
                            Next
                    End Select

                    If _baseModule.DatabaseCategory = ModuleEnum.CategorySubsystems Then
                        _activeFitting.BaseShip = _activeFitting.BuildSubSystemEffects(_activeFitting.BaseShip)
                        If _activeFitting.ShipSlotCtrl IsNot Nothing Then
                            Call _activeFitting.ShipSlotCtrl.UpdateShipSlotLayout()
                        End If
                        _activeFitting.ApplyFitting(BuildType.BuildEverything)
                    Else
                        _activeFitting.ApplyFitting(BuildType.BuildFromEffectsMaps)
                    End If

                    ' Update the base module
                    _baseModule = newModule
                    Text = "HQF Meta Variations - " & newModule.Name
                    btnReplaceModules.Text = "Replace " & newModule.Name
                    ' Add it to the MRU
                    HQFEvents.StartUpdateMRUModuleList = moduleName
                End If
            End If
        End Sub

        Private Sub btnAddToFitting_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddToFitting.Click
            Call AddModuleToFitting()
        End Sub

        Private Sub btnReplaceModules_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReplaceModules.Click
            Call ReplaceModules()
        End Sub

        Private Sub adtComparisons_MouseDown(sender As Object, e As MouseEventArgs) Handles adtComparisons.MouseDown
            Dim testCell As Cell = adtComparisons.GetCellAt(e.Location)
            If testCell IsNot Nothing Then
                If IsNumeric(testCell.Name) Then
                    _currentColumnIndex = CInt(testCell.Name)
                Else
                    _currentColumnIndex = -1
                End If
            Else
                _currentColumnIndex = -1
            End If
        End Sub

        Private Sub adtComparisons_NodeDoubleClick(ByVal sender As Object, ByVal e As TreeNodeMouseEventArgs) Handles adtComparisons.NodeDoubleClick
            Call AddModuleToFitting()
        End Sub

        Private Sub adtComparisons_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles adtComparisons.SelectionChanged
            If adtComparisons.SelectedNodes.Count = 1 Then
                If _activeFitting IsNot Nothing Then
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

        Private Sub mnuRemoveColumn_Click(sender As Object, e As EventArgs) Handles mnuRemoveColumn.Click
            If _currentColumnIndex > -1 Then
                ' Get the attribute we want to ignore
                Dim ignoredAtt As String = ""
                For Each att As String In _columnIndexes.Keys
                    If _columnIndexes(att) = _currentColumnIndex Then
                        ignoredAtt = att
                    End If
                Next
                If PluginSettings.HQFSettings.IgnoredAttributeColumns.Contains(ignoredAtt) = False Then
                    PluginSettings.HQFSettings.IgnoredAttributeColumns.Add(ignoredAtt)
                    ' Rebuild the column list
                    Call GetComparatives()
                End If
            End If
        End Sub

        Private Sub frmMetaVariations_Resize(sender As Object, e As EventArgs) Handles Me.Resize
            If _startup = False Then
                PluginSettings.HQFSettings.MetaVariationsFormSize = Size
            End If
        End Sub

    End Class
End NameSpace