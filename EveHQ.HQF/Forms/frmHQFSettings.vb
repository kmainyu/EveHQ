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
        Call Me.UpdateGeneralOptions()
        Call Me.UpdateSlotFormatOptions()
        Call Me.UpdateRechargeRateOptions()

        If Me.Tag IsNot Nothing Then
            If Me.Tag.ToString = "" Then
                Me.Tag = "tabGeneral"
            End If
        End If
        forceUpdate = False
        redrawColumns = True
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
        For Each myPilot In EveHQ.Core.HQ.Pilots
            If myPilot.Active = True Then
                cboStartupPilot.Items.Add(myPilot.Name)
            End If
        Next
        If EveHQ.Core.HQ.Pilots.Contains(Settings.HQFSettings.DefaultPilot) = False Then
            If EveHQ.Core.HQ.Pilots.Count > 0 Then
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
    Private Sub chkCloseInfoPanel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCloseInfoPanel.CheckedChanged
        Settings.HQFSettings.CloseInfoPanel = chkCloseInfoPanel.Checked
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
                    If My.Computer.FileSystem.FileExists(HQF.Settings.HQFFolder & "\HQFFittings.bin") = True Then
                        My.Computer.FileSystem.DeleteFile(HQF.Settings.HQFFolder & "\HQFFittings.bin", FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
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

#Region "Recharge Rate Options"
    Private Sub UpdateRechargeRateOptions()
        nudCapRecharge.Value = CDec(HQF.Settings.HQFSettings.CapRechargeConstant)
        nudShieldRecharge.Value = CDec(HQF.Settings.HQFSettings.ShieldRechargeConstant)
    End Sub
    Private Sub nudCapRecharge_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudCapRecharge.Click
        HQF.Settings.HQFSettings.CapRechargeConstant = nudCapRecharge.Value
        forceUpdate = True
    End Sub
    Private Sub nudCapRecharge_HandleDestroyed(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudCapRecharge.HandleDestroyed
        HQF.Settings.HQFSettings.CapRechargeConstant = nudCapRecharge.Value
    End Sub
    Private Sub nudShieldRecharge_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudShieldRecharge.Click
        HQF.Settings.HQFSettings.ShieldRechargeConstant = nudShieldRecharge.Value
        forceUpdate = True
    End Sub
    Private Sub nudShieldRecharge_HandleDestroyed(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudShieldRecharge.HandleDestroyed
        HQF.Settings.HQFSettings.ShieldRechargeConstant = nudShieldRecharge.Value
    End Sub
#End Region

    Private Sub btnCheckData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckData.Click
        Dim dataCheckList As New SortedList
        Dim itemcount As Integer = 0
        ' Count number of items
        Dim items As Integer = ModuleLists.moduleList.Count
        ' Check MarketGroups
        Dim marketError As Integer = 0
        For Each item As ShipModule In ModuleLists.moduleList.Values
            If Market.MarketGroupList.ContainsKey(item.MarketGroup) = False Then
                marketError += 1
                'MessageBox.Show(item.Name)
            End If
        Next
        ' Check MarketGroups
        Dim metaError As Integer = 0
        For Each item As ShipModule In ModuleLists.moduleList.Values
            If ModuleLists.moduleMetaGroups.ContainsKey(item.ID) = False Then
                metaError += 1
                'MessageBox.Show(item.Name)
            End If
        Next

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
        Dim sw As New IO.StreamWriter(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\missingItems.csv")
        For Each shipMod As ShipModule In ModuleLists.moduleList.Values
            If dataCheckList.Contains(shipMod.ID) = False Then
                sw.WriteLine(shipMod.ID & "," & shipMod.Name)
                dataCheckList.Add(shipMod.ID, shipMod.Name)
            End If
        Next
        sw.Flush()
        sw.Close()

        MessageBox.Show("Total traversed items: " & itemCount, "Tree Walk Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

   
End Class
