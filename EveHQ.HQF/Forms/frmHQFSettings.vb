' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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
Imports System.Xml
Imports System.IO
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Net
Imports System.Runtime.Serialization.Formatters.Binary

Public Class frmHQFSettings
    Dim forceUpdate As Boolean = False
    Dim redrawColumns As Boolean = True
    Dim startUp As Boolean = True

#Region "Form Opening & Closing"

    Private Sub frmSettings_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' Process the slot selection
        Settings.HQFSettings.UserSlotColumns.Clear()
        For Each slotItem As ListViewItem In lvwColumns.Items
            If slotItem.Checked = False Then
                Settings.HQFSettings.UserSlotColumns.Add(slotItem.Name & "0")
            Else
                Settings.HQFSettings.UserSlotColumns.Add(slotItem.Name & "1")
            End If
        Next
        ' Save the settings
        Call Settings.HQFSettings.SaveHQFSettings()
        If forceUpdate = True Then
            HQFEvents.StartUpdateFitting = True
        End If
    End Sub

    Private Sub frmSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        startUp = True
        Call Me.UpdateGeneralOptions()
        Call Me.UpdateSlotFormatOptions()
        Call Me.UpdateConstantsOptions()

        If Me.Tag IsNot Nothing Then
            If Me.Tag.ToString = "" Then
                Me.Tag = "tabGeneral"
            End If
        End If
        forceUpdate = False
        redrawColumns = True
        startUp = False
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub gbSlotFormat_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles gbSlotFormat.Paint
        redrawColumns = False
    End Sub

#End Region

#Region "General Options"
    Private Sub UpdateGeneralOptions()
        cboStartupPilot.Items.Clear()
        Dim myPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
        For Each myPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If myPilot.Active = True Then
                cboStartupPilot.Items.Add(myPilot.Name)
            End If
        Next
        If EveHQ.Core.HQ.EveHQSettings.Pilots.Contains(Settings.HQFSettings.DefaultPilot) = False Then
            If EveHQ.Core.HQ.EveHQSettings.Pilots.Count > 0 Then
                cboStartupPilot.SelectedIndex = 0
            End If
        Else
            cboStartupPilot.SelectedItem = Settings.HQFSettings.DefaultPilot
        End If
        chkRestoreLastSession.Checked = Settings.HQFSettings.RestoreLastSession
        chkAutoUpdateHQFSkills.Checked = Settings.HQFSettings.AutoUpdateHQFSkills
        chkShowPerformance.Checked = Settings.HQFSettings.ShowPerformanceData
    End Sub
    Private Sub cboStartupPilot_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboStartupPilot.SelectedIndexChanged
        Settings.HQFSettings.DefaultPilot = CStr(cboStartupPilot.SelectedItem)
    End Sub
    Private Sub chkRestoreLastSession_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRestoreLastSession.CheckedChanged
        Settings.HQFSettings.RestoreLastSession = chkRestoreLastSession.Checked
    End Sub
    Private Sub chkAutoUpdateHQFSkills_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoUpdateHQFSkills.CheckedChanged
        Settings.HQFSettings.AutoUpdateHQFSkills = chkAutoUpdateHQFSkills.Checked
    End Sub
    Private Sub chkShowPerformance_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowPerformance.CheckedChanged
        Settings.HQFSettings.ShowPerformanceData = chkShowPerformance.Checked
    End Sub
#End Region

#Region "Slot Format Options"

    Private Sub UpdateSlotFormatOptions()
        Dim HColor As Color = Color.FromArgb(CInt(Settings.HQFSettings.HiSlotColour))
        Me.pbHiSlotColour.BackColor = HColor
        Dim MColor As Color = Color.FromArgb(CInt(Settings.HQFSettings.MidSlotColour))
        Me.pbMidSlotColour.BackColor = MColor
        Dim LColor As Color = Color.FromArgb(CInt(Settings.HQFSettings.LowSlotColour))
        Me.pbLowSlotColour.BackColor = LColor
        Dim RColor As Color = Color.FromArgb(CInt(Settings.HQFSettings.RigSlotColour))
        Me.pbRigSlotColour.BackColor = RColor
        Dim SColor As Color = Color.FromArgb(CInt(Settings.HQFSettings.SubSlotColour))
        Me.pbSubSlotColour.BackColor = SColor
        redrawColumns = True
        Call Me.RedrawSlotColumnList()
        redrawColumns = False
    End Sub
    Private Sub RedrawSlotColumnList()
        ' Setup the listview
        Dim newCol As New ListViewItem
        lvwColumns.BeginUpdate()
        lvwColumns.Items.Clear()
        For Each slot As String In Settings.HQFSettings.UserSlotColumns
            For Each stdSlot As ListViewItem In Settings.HQFSettings.StandardSlotColumns
                If slot.Substring(0, Len(slot) - 1) = stdSlot.Name Then
                    newCol = CType(stdSlot.Clone, ListViewItem)
                    newCol.Name = stdSlot.Name
                    If slot.EndsWith("0") = True Then
                        newCol.Checked = False
                    Else
                        newCol.Checked = True
                    End If
                    lvwColumns.Items.Add(newCol)
                End If
            Next
        Next
        lvwColumns.EndUpdate()
    End Sub
    Private Sub btnMoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveUp.Click
        ' Check we have something selected
        If lvwColumns.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select an item before trying it move it!", "Item Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        ' Save the selected item
        ' Get the slot name of the item selected
        Dim slotName As String = lvwColumns.SelectedItems(0).Name
        Dim selName As String = slotName
        ' Find the index in the user column list
        Dim idx As Integer = Settings.HQFSettings.UserSlotColumns.IndexOf(slotName & "0")
        If idx = -1 Then
            idx = Settings.HQFSettings.UserSlotColumns.IndexOf(slotName & "1")
        End If
        ' Switch with the one above if the index is not zero
        If idx <> 0 Then
            slotName = CStr(Settings.HQFSettings.UserSlotColumns(idx - 1))
            Settings.HQFSettings.UserSlotColumns(idx - 1) = Settings.HQFSettings.UserSlotColumns(idx)
            Settings.HQFSettings.UserSlotColumns(idx) = slotName
            ' Redraw the list
            redrawColumns = True
            Call Me.RedrawSlotColumnList()
            redrawColumns = False
            lvwColumns.Items(selName).Selected = True
            lvwColumns.Select()
            forceUpdate = True
        End If
    End Sub
    Private Sub btnMoveDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveDown.Click
        ' Check we have something selected
        If lvwColumns.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select an item before trying it move it!", "Item Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        ' Get the slot name of the item selected
        Dim slotName As String = lvwColumns.SelectedItems(0).Name
        Dim selName As String = slotName
        ' Find the index in the user column list
        Dim idx As Integer = Settings.HQFSettings.UserSlotColumns.IndexOf(slotName & "0")
        If idx = -1 Then
            idx = Settings.HQFSettings.UserSlotColumns.IndexOf(slotName & "1")
        End If
        ' Switch with the one above if the index is not the last
        If idx <> Settings.HQFSettings.UserSlotColumns.Count - 1 Then
            slotName = CStr(Settings.HQFSettings.UserSlotColumns(idx + 1))
            Settings.HQFSettings.UserSlotColumns(idx + 1) = Settings.HQFSettings.UserSlotColumns(idx)
            Settings.HQFSettings.UserSlotColumns(idx) = slotName
            ' Redraw the list
            redrawColumns = True
            Call Me.RedrawSlotColumnList()
            redrawColumns = False
            lvwColumns.Items(selName).Selected = True
            lvwColumns.Select()
            forceUpdate = True
        End If
    End Sub
    Private Sub lvwColumns_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwColumns.ItemChecked
        If redrawColumns = False Then
            ' Get the slot name of the ticked item
            Dim slotName As String = e.Item.Name
            ' Find the index in the user column list
            Dim idx As Integer = Settings.HQFSettings.UserSlotColumns.IndexOf(slotName & "0")
            If idx = -1 Then
                idx = Settings.HQFSettings.UserSlotColumns.IndexOf(slotName & "1")
            End If
            If e.Item.Checked = False Then
                Settings.HQFSettings.UserSlotColumns(idx) = slotName & "0"
            Else
                Settings.HQFSettings.UserSlotColumns(idx) = slotName & "1"
            End If
            forceUpdate = True
        End If
    End Sub

    Private Sub pbHiSlotColour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbHiSlotColour.Click
        Dim dlgResult As Integer = 0
        With cd1
            .AllowFullOpen = True
            .AnyColor = True
            .FullOpen = True
            dlgResult = .ShowDialog()
        End With
        If dlgResult = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        Else
            Me.pbHiSlotColour.BackColor = cd1.Color
            Settings.HQFSettings.HiSlotColour = cd1.Color.ToArgb
        End If
    End Sub

    Private Sub pbMidSlotColour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbMidSlotColour.Click
        Dim dlgResult As Integer = 0
        With cd1
            .AllowFullOpen = True
            .AnyColor = True
            .FullOpen = True
            dlgResult = .ShowDialog()
        End With
        If dlgResult = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        Else
            Me.pbMidSlotColour.BackColor = cd1.Color
            Settings.HQFSettings.MidSlotColour = cd1.Color.ToArgb
        End If
    End Sub

    Private Sub pbLowSlotColour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbLowSlotColour.Click
        Dim dlgResult As Integer = 0
        With cd1
            .AllowFullOpen = True
            .AnyColor = True
            .FullOpen = True
            dlgResult = .ShowDialog()
        End With
        If dlgResult = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        Else
            Me.pbLowSlotColour.BackColor = cd1.Color
            Settings.HQFSettings.LowSlotColour = cd1.Color.ToArgb
        End If
    End Sub

    Private Sub pbRigSlotColour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbRigSlotColour.Click
        Dim dlgResult As Integer = 0
        With cd1
            .AllowFullOpen = True
            .AnyColor = True
            .FullOpen = True
            dlgResult = .ShowDialog()
        End With
        If dlgResult = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        Else
            Me.pbRigSlotColour.BackColor = cd1.Color
            Settings.HQFSettings.RigSlotColour = cd1.Color.ToArgb
        End If
    End Sub

    Private Sub pbSubSlotColour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbSubSlotColour.Click
        Dim dlgResult As Integer = 0
        With cd1
            .AllowFullOpen = True
            .AnyColor = True
            .FullOpen = True
            dlgResult = .ShowDialog()
        End With
        If dlgResult = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        Else
            Me.pbSubSlotColour.BackColor = cd1.Color
            Settings.HQFSettings.SubSlotColour = cd1.Color.ToArgb
        End If
    End Sub

#End Region

#Region "Treeview Routines"
    Private Sub tvwSettings_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwSettings.NodeMouseClick
        Dim nodeName As String = e.Node.Name
        Dim gbName As String = nodeName.TrimStart("node".ToCharArray)
        gbName = "gb" & gbName
        For Each setControl As Control In Me.Controls
            If setControl.Name = "tvwSettings" Or setControl.Name = "btnClose" Or setControl.Name = gbName Then
                Me.Controls(gbName).Top = 12
                Me.Controls(gbName).Left = 195
                Me.Controls(gbName).Width = 500
                Me.Controls(gbName).Height = 500
                Me.Controls(gbName).Visible = True
            Else
                setControl.Visible = False
            End If
        Next
    End Sub

#End Region

#Region "Data Cache Options"
    Private Sub btnDeleteCache_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteCache.Click
        If My.Computer.FileSystem.DirectoryExists(Settings.HQFFolder) = True Then
            Try
                My.Computer.FileSystem.DeleteDirectory(Settings.HQFCacheFolder, FileIO.DeleteDirectoryOption.DeleteAllContents)
                MessageBox.Show("HQF Cache Directory successfully deleted. Please restart EveHQ to reload the latest data.", "Cache Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Unable to delete Cache Directory: " & ex.Message, "Unable to Delete Cache", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub
    Private Sub btnDeleteAllFittings_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteAllFittings.Click
        Dim response As Integer = MessageBox.Show("This will delete all your existing fittings. Are you sure you wish to proceed?", "Confirm Delete ALL Fittings", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If response = Windows.Forms.DialogResult.Yes Then
            Dim cResponse As Integer = MessageBox.Show("Are you really sure you wish to proceed?", "Confirm Delete ALL Profiles", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If cResponse = Windows.Forms.DialogResult.Yes Then
                Try
                    If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFFolder, "HQFFittings.bin")) = True Then
                        My.Computer.FileSystem.DeleteFile(Path.Combine(HQF.Settings.HQFFolder, "HQFFittings.bin"), FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                    End If
                    Fittings.FittingList.Clear()
                    Fittings.FittingTabList.Clear()
                    MessageBox.Show("All fittings successfully deleted", "All Fittings Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("Unable to delete the fittings file", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Else
                Exit Sub
            End If
        Else
            Exit Sub
        End If
    End Sub

#End Region

#Region "Constants Options"
    Private Sub UpdateConstantsOptions()
        If HQF.Settings.HQFSettings.CapRechargeConstant > nudCapRecharge.Maximum Then
            HQF.Settings.HQFSettings.CapRechargeConstant = nudCapRecharge.Maximum
        ElseIf HQF.Settings.HQFSettings.CapRechargeConstant < nudCapRecharge.Minimum Then
            HQF.Settings.HQFSettings.CapRechargeConstant = nudCapRecharge.Minimum
        End If
        If HQF.Settings.HQFSettings.ShieldRechargeConstant > nudShieldRecharge.Maximum Then
            HQF.Settings.HQFSettings.ShieldRechargeConstant = nudShieldRecharge.Maximum
        ElseIf HQF.Settings.HQFSettings.ShieldRechargeConstant < nudShieldRecharge.Minimum Then
            HQF.Settings.HQFSettings.ShieldRechargeConstant = nudShieldRecharge.Minimum
        End If
        If HQF.Settings.HQFSettings.MissileRangeConstant > nudMissileRange.Maximum Then
            HQF.Settings.HQFSettings.MissileRangeConstant = nudMissileRange.Maximum
        ElseIf HQF.Settings.HQFSettings.MissileRangeConstant < nudMissileRange.Minimum Then
            HQF.Settings.HQFSettings.MissileRangeConstant = nudMissileRange.Minimum
        End If
        nudCapRecharge.Value = CDec(HQF.Settings.HQFSettings.CapRechargeConstant)
        nudShieldRecharge.Value = CDec(HQF.Settings.HQFSettings.ShieldRechargeConstant)
        nudMissileRange.Value = CDec(HQF.Settings.HQFSettings.MissileRangeConstant)
    End Sub

    Private Sub nudCapRecharge_HandleDestroyed(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudCapRecharge.HandleDestroyed
        HQF.Settings.HQFSettings.CapRechargeConstant = nudCapRecharge.Value
    End Sub
    Private Sub nudShieldRecharge_HandleDestroyed(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudShieldRecharge.HandleDestroyed
        HQF.Settings.HQFSettings.ShieldRechargeConstant = nudShieldRecharge.Value
    End Sub
    Private Sub nudMissileRange_HandleDestroyed(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudMissileRange.HandleDestroyed
        HQF.Settings.HQFSettings.MissileRangeConstant = nudMissileRange.Value
    End Sub
    Private Sub nudCapRecharge_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudCapRecharge.ValueChanged
        If startUp = False Then
            HQF.Settings.HQFSettings.CapRechargeConstant = CDbl(nudCapRecharge.Value)
            forceUpdate = True
        End If
    End Sub
    Private Sub nudShieldRecharge_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudShieldRecharge.ValueChanged
        If startUp = False Then
            HQF.Settings.HQFSettings.ShieldRechargeConstant = CDbl(nudShieldRecharge.Value)
            forceUpdate = True
        End If
    End Sub
    Private Sub nudMissileRange_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudMissileRange.ValueChanged
        If startUp = False Then
            HQF.Settings.HQFSettings.MissileRangeConstant = CDbl(nudMissileRange.Value)
            forceUpdate = True
        End If
    End Sub

#End Region

#Region "Data Checking Routines"
    Private Sub btnCheckData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckData.Click
        Dim dataCheckList As New SortedList
        Dim itemcount As Integer = 0
        ' Count number of items
        Dim items As Integer = ModuleLists.moduleList.Count
        ' Check MarketGroups
        Dim marketError As Integer = 0
        Dim sw As New StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "HQFErrors.txt"))
        For Each item As ShipModule In ModuleLists.moduleList.Values
            If Market.MarketGroupList.ContainsKey(item.MarketGroup) = False Then
                marketError += 1
                sw.WriteLine("Market Error: " & item.Name)
                'MessageBox.Show(item.Name)
            End If
        Next
        ' Check MarketGroups
        Dim metaError As Integer = 0
        For Each item As ShipModule In ModuleLists.moduleList.Values
            If ModuleLists.moduleMetaGroups.ContainsKey(item.ID) = False Then
                metaError += 1
                sw.WriteLine("Meta Type Error: " & item.Name)
                'MessageBox.Show(item.Name)
            End If
        Next

        sw.Flush()
        sw.Close()
        Dim msg As String = ""
        msg &= "Total items: " & items & ControlChars.CrLf
        msg &= "Orphaned market items: " & marketError & ControlChars.CrLf
        msg &= "Orphaned meta items: " & metaError & ControlChars.CrLf
        MessageBox.Show(msg)

        ' Traverse the tree, looking for goodies!

        'itemCount = 0
        'For Each rootNode As TreeNode In tvwItems.Nodes
        '    SearchChildNodes(rootNode)
        'Next

        ' Write missing items to a file
        Dim sw2 As New IO.StreamWriter(Path.Combine(EveHQ.Core.HQ.reportFolder, "HQFmissingItems.csv"))
        For Each shipMod As ShipModule In ModuleLists.moduleList.Values
            If dataCheckList.Contains(shipMod.ID) = False Then
                sw2.WriteLine(shipMod.ID & "," & shipMod.Name)
                dataCheckList.Add(shipMod.ID, shipMod.Name)
            End If
        Next
        sw2.Flush()
        sw2.Close()

        MessageBox.Show("Total traversed items: " & itemCount, "Tree Walk Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub btnCheckModuleMetaData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckModuleMetaData.Click
        For Each chkModule As ShipModule In ModuleLists.moduleList.Values
            Select Case chkModule.MetaType
                Case 1, 2, 4, 8, 16, 32
                Case Else
                    MessageBox.Show(chkModule.Name & " has an invalid meta type: " & chkModule.MetaType, "Error Found", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Select
        Next
        MessageBox.Show("Module Meta Check Completed.", "Module Check Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnCheckAttributeIntFloat_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckAttributeIntFloat.Click
        Try
            Dim strSQL As String = ""
            strSQL &= "SELECT invCategories.categoryID, invGroups.groupID, invTypes.typeID, invTypes.description, invTypes.typeName, invTypes.radius, invTypes.mass, invTypes.volume, invTypes.capacity, invTypes.basePrice, invTypes.published, invTypes.marketGroupID, dgmTypeAttributes.attributeID, dgmTypeAttributes.valueInt, dgmTypeAttributes.valueFloat, dgmAttributeTypes.attributeName, dgmAttributeTypes.displayName, dgmAttributeTypes.unitID, eveUnits.unitName, eveUnits.displayName"
            strSQL &= " FROM invCategories INNER JOIN ((invGroups INNER JOIN invTypes ON invGroups.groupID = invTypes.groupID) INNER JOIN (eveUnits INNER JOIN (dgmAttributeTypes INNER JOIN dgmTypeAttributes ON dgmAttributeTypes.attributeID = dgmTypeAttributes.attributeID) ON eveUnits.unitID = dgmAttributeTypes.unitID) ON invTypes.typeID = dgmTypeAttributes.typeID) ON invCategories.categoryID = invGroups.categoryID"
            strSQL &= " WHERE (((invCategories.categoryID) In (7,8,18,20)) AND ((invTypes.published)=1))"
            strSQL &= " ORDER BY invTypes.typeName, dgmTypeAttributes.attributeID;"

            Dim moduleAttributeData As DataSet = EveHQ.Core.DataFunctions.GetData(strSQL)
            If moduleAttributeData IsNot Nothing Then
                If moduleAttributeData.Tables(0).Rows.Count <> 0 Then
                    ' Find module information
                    For Each attRow As DataRow In moduleAttributeData.Tables(0).Rows
                        If IsDBNull(attRow.Item("valueFloat")) = False And IsDBNull(attRow.Item("valueInt")) = False Then
                            MessageBox.Show(CStr(attRow.Item("typeID")) & ": " & CStr(attRow.Item("typeName")))
                        End If
                    Next
                Else
                    MessageBox.Show("Module Attribute Data returned no rows", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            Else
                MessageBox.Show("Module Attribute Data returned a null dataset", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
        Catch ex As Exception
            MessageBox.Show("Error loading Module Attribute Data", "HQF Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub
#End Region

    Private Sub btnExportEffects_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportEffects.Click
        Try
            Dim sw As New StreamWriter(Settings.HQFFolder & "/HQFEffects.csv")
            sw.Write(My.Resources.Effects.ToString)
            sw.Flush()
            sw.Close()
            MessageBox.Show("HQF Effects file successfully written to disk", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Error writing the HQF Effects file to disk: " & ex.Message.ToString, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnExportImplantEffects_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportImplantEffects.Click
        Try
            Dim sw As New StreamWriter(Settings.HQFFolder & "/HQFImplantEffects.csv")
            sw.Write(My.Resources.ImplantEffects.ToString)
            sw.Flush()
            sw.Close()
            MessageBox.Show("HQF Implant Effects file successfully written to disk", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Error writing the HQF Implant Effects file to disk: " & ex.Message.ToString, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnExportShipBonuses_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExportShipBonuses.Click
        Try
            Dim sw As New StreamWriter(Settings.HQFFolder & "/HQFShipEffects.csv")
            sw.Write(My.Resources.ShipEffects.ToString)
            sw.Flush()
            sw.Close()
            MessageBox.Show("HQF Ship Effects file successfully written to disk", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Error writing the HQF Ship Effects file to disk: " & ex.Message.ToString, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCheckMarket_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckMarket.Click
        Dim chargeGroups As New ArrayList
        Dim chargeGroupData() As String
        Dim chargeItems As New SortedList
        Dim groupName As String = ""
        For Each chargeGroup As String In Charges.ChargeGroups
            chargeGroupData = chargeGroup.Split("_".ToCharArray)
            If chargeGroupData(0) = "0" Then
                MessageBox.Show(chargeGroupData(2))
            End If
        Next
    End Sub
End Class
