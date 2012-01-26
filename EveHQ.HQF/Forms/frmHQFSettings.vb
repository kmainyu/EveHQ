' ========================================================================
' EveHQ - An Eve-Online� character assistance application
' Copyright � 2005-2012  EveHQ Development Team
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
Imports DevComponents.AdvTree

Public Class frmHQFSettings
    Dim forceUpdate As Boolean = False
    Dim redrawColumns As Boolean = True
    Dim startUp As Boolean = True

#Region "Form Opening & Closing"

    Private Sub frmSettings_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        If e.CloseReason = CloseReason.None Then
            e.Cancel = True
        End If

        ' Save Widths
        Dim Cols As New SortedList(Of String, UserSlotColumn)
        For Each UserCol As UserSlotColumn In HQF.Settings.HQFSettings.UserSlotColumns
            Cols.Add(UserCol.Name, UserCol)
        Next

        ' Process the slot selection
        Settings.HQFSettings.UserSlotColumns.Clear()
        For Each slotItem As ListViewItem In lvwColumns.Items
            Dim OldSlot As UserSlotColumn = Cols(slotItem.Name)
            Settings.HQFSettings.UserSlotColumns.Add(New UserSlotColumn(OldSlot.Name, OldSlot.Description, OldSlot.Width, slotItem.Checked))
        Next

        ' Save the profiles
        Call DamageProfiles.SaveProfiles()
        Call DefenceProfiles.SaveProfiles()

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
        Call Me.UpdateDamageProfileOptions()
        Call Me.UpdateDefenceProfileOptions()
        Call Me.UpdateAttributeColumns()

        forceUpdate = False
        redrawColumns = True
        startUp = False

        ' Switch to the right tab
        Me.tvwSettings.Select()
        If Me.Tag IsNot Nothing Then
            If Me.Tag.ToString = "" Then
                Me.tvwSettings.SelectedNode = Me.tvwSettings.Nodes("nodeGeneral")
            Else
                If Me.tvwSettings.Nodes.ContainsKey(Me.Tag.ToString) = True Then
                    Me.tvwSettings.SelectedNode = Me.tvwSettings.Nodes(Me.Tag.ToString)
                Else
                    Me.tvwSettings.SelectedNode = Me.tvwSettings.Nodes("nodeGeneral")
                End If
            End If
        Else
            Me.tvwSettings.SelectedNode = Me.tvwSettings.Nodes("nodeGeneral")
        End If

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
        chkUseLastPilot.Checked = Settings.HQFSettings.UseLastPilot
        ' Check for protocol
        If IsProtocolInstalled(EveHQ.Core.HQ.FittingProtocol) = False Then
            lblFittingProtocolStatus.Text = "Disabled"
            btnEnableProtocol.Enabled = True
            btnDisableProtocol.Enabled = False
        Else
            lblFittingProtocolStatus.Text = "Enabled"
            btnEnableProtocol.Enabled = False
            btnDisableProtocol.Enabled = True
        End If
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
    Private Sub chkUseLastPilot_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseLastPilot.CheckedChanged
        Settings.HQFSettings.UseLastPilot = chkUseLastPilot.Checked
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
        For Each UserSlot As UserSlotColumn In Settings.HQFSettings.UserSlotColumns
            newCol = New ListViewItem(UserSlot.Description)
            newCol.Name = UserSlot.Name
            newCol.Checked = UserSlot.Active
            lvwColumns.Items.Add(newCol)
        Next
        lvwColumns.EndUpdate()
    End Sub
    Private Sub btnMoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMoveUp.Click
        ' Check we have something selected
        If lvwColumns.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select an item before trying it move it!", "Item Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        ' Find the index in the user column list
        Dim idx As Integer = lvwColumns.SelectedItems(0).Index
        If idx > 0 Then
            Dim ColToMove As UserSlotColumn = Settings.HQFSettings.UserSlotColumns(idx)
            Dim ColToSwitch As UserSlotColumn = Settings.HQFSettings.UserSlotColumns(idx - 1)
            Settings.HQFSettings.UserSlotColumns.Item(idx) = ColToSwitch
            Settings.HQFSettings.UserSlotColumns.Item(idx - 1) = ColToMove
            ' Redraw the list
            redrawColumns = True
            Call Me.RedrawSlotColumnList()
            redrawColumns = False
            lvwColumns.Items(idx - 1).Selected = True
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
        ' Find the index in the user column list
        Dim idx As Integer = lvwColumns.SelectedItems(0).Index
        If idx < lvwColumns.Items.Count - 1 Then
            Dim ColToMove As UserSlotColumn = Settings.HQFSettings.UserSlotColumns(idx)
            Dim ColToSwitch As UserSlotColumn = Settings.HQFSettings.UserSlotColumns(idx + 1)
            Settings.HQFSettings.UserSlotColumns.Item(idx) = ColToSwitch
            Settings.HQFSettings.UserSlotColumns.Item(idx + 1) = ColToMove
            ' Redraw the list
            redrawColumns = True
            Call Me.RedrawSlotColumnList()
            redrawColumns = False
            lvwColumns.Items(idx + 1).Selected = True
            lvwColumns.Select()
            forceUpdate = True
        End If
    End Sub
    Private Sub lvwColumns_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwColumns.ItemChecked
        If redrawColumns = False Then
            ' Find the index in the user column list
            Dim idx As Integer = e.Item.Index
            HQF.Settings.HQFSettings.UserSlotColumns.Item(idx).Active = e.Item.Checked
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

    Private Sub tvwSettings_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvwSettings.AfterSelect
        Dim nodeName As String = e.Node.Name
        Dim gbName As String = nodeName.TrimStart("node".ToCharArray)
        gbName = "gb" & gbName
        For Each setControl As Control In Me.pnlSettings.Controls
            If setControl.Name = "tvwSettings" Or setControl.Name = "btnClose" Or setControl.Name = gbName Then
                Me.pnlSettings.Controls(gbName).Top = 12
                Me.pnlSettings.Controls(gbName).Left = 195
                Me.pnlSettings.Controls(gbName).Width = 585
                Me.pnlSettings.Controls(gbName).Height = 500
                Me.pnlSettings.Controls(gbName).Visible = True
            Else
                setControl.Visible = False
            End If
        Next
    End Sub

    Private Sub tvwSettings_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwSettings.NodeMouseClick
        Me.tvwSettings.SelectedNode = e.Node
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
            Dim cResponse As Integer = MessageBox.Show("Are you really, really sure you wish to proceed?", "Confirm Delete ALL Fittings", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If cResponse = Windows.Forms.DialogResult.Yes Then
                Try
                    If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFFolder, "HQFFittings.bin")) = True Then
                        My.Computer.FileSystem.DeleteFile(Path.Combine(HQF.Settings.HQFFolder, "HQFFittings.bin"), FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                    End If
                    Fittings.FittingList.Clear()
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
        chkCapBoosterReloadTime.Checked = HQF.Settings.HQFSettings.IncludeCapReloadTime
        chkAmmoLoadTime.Checked = HQF.Settings.HQFSettings.IncludeAmmoReloadTime
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
    Private Sub chkCapBoosterReloadTime_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCapBoosterReloadTime.CheckedChanged
        HQF.Settings.HQFSettings.IncludeCapReloadTime = chkCapBoosterReloadTime.Checked
        forceUpdate = True
    End Sub
    Private Sub chkAmmoLoadTime_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAmmoLoadTime.CheckedChanged
        HQF.Settings.HQFSettings.IncludeAmmoReloadTime = chkAmmoLoadTime.Checked
        forceUpdate = True
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

        MessageBox.Show("Total traversed items: " & itemcount, "Tree Walk Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)

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

#Region "Protocol Check Routines"

    Private Function IsProtocolInstalled(ByVal protocol As String) As Boolean
        Dim rk As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(protocol)
        If rk IsNot Nothing Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub btnEnableProtocol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnableProtocol.Click
        ' Ask if we want to install the protocol
        Dim msg As String = "Would you like to associate the '" & EveHQ.Core.HQ.FittingProtocol & "://' protocol with EveHQ?"
        Dim reply As Integer = MessageBox.Show(msg, "Install Protocol", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = DialogResult.Yes Then
            Call Me.InstallProtocol(EveHQ.Core.HQ.FittingProtocol)
        End If
    End Sub

    Private Sub btnDisableProtocol_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisableProtocol.Click
        ' Ask if we want to install the protocol
        Dim msg As String = "Would you like to remove the '" & EveHQ.Core.HQ.FittingProtocol & "://' protocol from use with EveHQ?"
        Dim reply As Integer = MessageBox.Show(msg, "Remove Protocol", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = DialogResult.Yes Then
            Call Me.RemoveProtocol(EveHQ.Core.HQ.FittingProtocol)
        End If
    End Sub

    Private Sub InstallProtocol(ByVal protocol As String)
        Dim rKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(protocol, True)
        Try
            If rKey Is Nothing Then
                rKey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(protocol)
                rKey.SetValue("", "URL: Eve Fitting Protocol")
                rKey.SetValue("URL Protocol", "")
                rKey = rKey.CreateSubKey("shell\open\command")
                Dim keyValue As String = ControlChars.Quote & Application.ExecutablePath & ControlChars.Quote & " " & ControlChars.Quote & "%1" & ControlChars.Quote
                rKey.SetValue("", keyValue)
            Else
                rKey.Close()
            End If
            lblFittingProtocolStatus.Text = "Enabled"
            btnEnableProtocol.Enabled = False
            btnDisableProtocol.Enabled = True
        Catch ex As System.UnauthorizedAccessException
            MessageBox.Show("You do not have the required permissions to access the registry. To install the protocol, EveHQ will need to be run in administrator mode.", "Elevated Permissions Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            lblFittingProtocolStatus.Text = "Disabled"
            btnEnableProtocol.Enabled = True
            btnDisableProtocol.Enabled = False
        End Try
    End Sub

    Private Sub RemoveProtocol(ByVal protocol As String)
        Dim rKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(protocol, True)
        Try
            If rKey IsNot Nothing Then
                Microsoft.Win32.Registry.ClassesRoot.DeleteSubKeyTree(protocol)
            Else
                rKey.Close()
            End If
            lblFittingProtocolStatus.Text = "Disabled"
            btnEnableProtocol.Enabled = True
            btnDisableProtocol.Enabled = False
        Catch ex As System.UnauthorizedAccessException
            MessageBox.Show("You do not have the required permissions to access the registry. To install the protocol, EveHQ will need to be run in administrator mode.", "Elevated Permissions Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            lblFittingProtocolStatus.Text = "Enabled"
            btnEnableProtocol.Enabled = False
            btnDisableProtocol.Enabled = True
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
            sw.Write(My.Resources.ShipBonuses.ToString)
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

#Region "Data Checking From Main Form"

    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
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
        ' Check MetaGroups
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
        Dim itemCount As Integer = 0
        Dim dataCheckList As New SortedList
        'For Each rootNode As TreeNode In frmHQF.tvwItems.Nodes
        '    SearchChildNodes(rootNode, itemCount, dataCheckList)
        'Next

        ' Write missing items to a file
        Dim sw As New IO.StreamWriter(Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyDocuments, "missingItems.csv"))
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

    Private Sub SearchChildNodes(ByRef parentNode As TreeNode, ByVal itemcount As Integer, ByVal datachecklist As SortedList)

        For Each childNode As TreeNode In parentNode.Nodes
            If childNode.Nodes.Count > 0 Then
                SearchChildNodes(childNode, itemcount, datachecklist)
            Else
                Dim groupID As String = childNode.Tag.ToString
                For Each shipMod As ShipModule In ModuleLists.moduleList.Values
                    If shipMod.MarketGroup = groupID Then
                        itemcount += 1
                        datachecklist.Add(shipMod.ID, shipMod.Name)
                    End If
                Next
            End If
        Next
    End Sub

    Private Sub SearchChildNodes1(ByRef parentNode As TreeNode, ByVal sw As IO.StreamWriter)
        For Each childNode As TreeNode In parentNode.Nodes
            If childNode.Nodes.Count > 0 Then
                SearchChildNodes1(childNode, sw)
            Else
                sw.Write(childNode.FullPath & ControlChars.CrLf)
            End If
        Next
    End Sub


#End Region

#Region "Damage Profile Options"

    Private Sub UpdateDamageProfileOptions()
        lvwProfiles.BeginUpdate()
        lvwProfiles.Items.Clear()
        Dim newItem As New ListViewItem
        For Each newProfile As DamageProfile In DamageProfiles.ProfileList.Values
            newItem = New ListViewItem
            newItem.Name = newProfile.Name
            newItem.Text = newProfile.Name
            Select Case newProfile.Type
                Case 0
                    newItem.SubItems.Add("Manual")
                Case 1
                    newItem.SubItems.Add("Fitting")
                Case 2
                    newItem.SubItems.Add("NPC")
            End Select
            lvwProfiles.Items.Add(newItem)
        Next
        lvwProfiles.EndUpdate()
    End Sub

    Private Sub lvwProfiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwProfiles.SelectedIndexChanged
        If lvwProfiles.SelectedItems.Count > 0 Then
            Dim selProfile As DamageProfile = CType(DamageProfiles.ProfileList(lvwProfiles.SelectedItems(0).Name), DamageProfile)
            lblProfileName.Text = selProfile.Name
            Select Case selProfile.Type
                Case 0
                    lblProfileType.Text = "Manual"
                    lblFittingName.Text = "n/a"
                    lblPilotName.Text = "n/a"
                    lblNPCName.Text = "n/a"
                Case 1
                    lblProfileType.Text = "Fitting"
                    lblFittingName.Text = selProfile.Fitting
                    lblPilotName.Text = selProfile.Pilot
                    lblNPCName.Text = "n/a"
                Case 2
                    lblProfileType.Text = "NPC"
                    lblFittingName.Text = "n/a"
                    lblPilotName.Text = "n/a"
                    lblNPCName.Text = ""
                    For Each NPC As String In selProfile.NPCs
                        lblNPCName.Text &= NPC & ", "
                    Next
            End Select
            lblEMDamageAmount.Text = selProfile.EM.ToString("N2")
            lblEXDamageAmount.Text = selProfile.Explosive.ToString("N2")
            lblKIDamageAmount.Text = selProfile.Kinetic.ToString("N2")
            lblTHDamageAmount.Text = selProfile.Thermal.ToString("N2")
            Dim total As Double = selProfile.EM + selProfile.Explosive + selProfile.Kinetic + selProfile.Thermal
            lblTotalDamageAmount.Text = total.ToString("N2")
            lblEMDamagePercentage.Text = "= " & (selProfile.EM / total * 100).ToString("N2") & "%"
            lblEXDamagePercentage.Text = "= " & (selProfile.Explosive / total * 100).ToString("N2") & "%"
            lblKIDamagePercentage.Text = "= " & (selProfile.Kinetic / total * 100).ToString("N2") & "%"
            lblTHDamagePercentage.Text = "= " & (selProfile.Thermal / total * 100).ToString("N2") & "%"
            lblDPS.Text = selProfile.DPS.ToString("N2")
            If gpProfile.Visible = False Then
                gpProfile.Visible = True
            End If
        End If
    End Sub

    Private Sub btnDeleteProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteProfile.Click
        If lvwProfiles.SelectedItems.Count > 0 Then
            Dim profileName As String = lvwProfiles.SelectedItems(0).Name
            If profileName = "<Omni-Damage>" Then
                MessageBox.Show("You cannot delete this profile!", "Error Deleting Profile", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Dim reply As Integer = MessageBox.Show("Are you sure you want to delete the profile '" & profileName & "'?", "Confirm Profile Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If reply = DialogResult.No Then
                    Exit Sub
                Else
                    ' Delete the profile
                    DamageProfiles.ProfileList.Remove(profileName)
                    ' Save the profiles
                    DamageProfiles.SaveProfiles()
                    Call Me.UpdateDamageProfileOptions()
                End If
            End If
        Else
            MessageBox.Show("Please select a profile before trying to delete.", "Error Deleting Profile", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnAddProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddProfile.Click
        Dim ProfileForm As New frmAddDamageProfile
        ProfileForm.Tag = "Add"
        ProfileForm.btnAccept.Text = "Add Profile"
        ProfileForm.ShowDialog()
        ProfileForm.Dispose()
        Call Me.UpdateDamageProfileOptions()
    End Sub

    Private Sub btnEditProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditProfile.Click
        If lvwProfiles.SelectedItems.Count > 0 Then
            Dim profileName As String = lvwProfiles.SelectedItems(0).Name
            If profileName = "<Omni-Damage>" Then
                MessageBox.Show("You cannot edit this profile!", "Error Editing Profile", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Dim editProfile As DamageProfile = CType(DamageProfiles.ProfileList(profileName), DamageProfile)
                Dim ProfileForm As New frmAddDamageProfile
                ProfileForm.Tag = editProfile
                ProfileForm.btnAccept.Text = "Edit Profile"
                ProfileForm.ShowDialog()
                ProfileForm.Dispose()
                Call Me.UpdateDamageProfileOptions()
            End If
        Else
            MessageBox.Show("Please select a profile before trying to delete.", "Error Deleting Profile", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnResetProfiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetProfiles.Click
        Dim response As Integer = MessageBox.Show("This will delete all your existing profiles and re-instate the defaults. Are you sure you wish to proceed?", "Confirm Reset ALL Profiles", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If response = Windows.Forms.DialogResult.Yes Then
            Dim cResponse As Integer = MessageBox.Show("Are you really sure you wish to proceed?", "Confirm Reset ALL Profiles", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If cResponse = Windows.Forms.DialogResult.Yes Then
                Try
                    If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFFolder, "HQFProfiles.bin")) = True Then
                        My.Computer.FileSystem.DeleteFile(Path.Combine(HQF.Settings.HQFFolder, "HQFProfiles.bin"), FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                    End If
                    DamageProfiles.ResetDamageProfiles()
                    Call Me.UpdateDamageProfileOptions()
                    MessageBox.Show("Profiles successfully reset", "Profile Reset Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("Unable to delete the Damage Profiles file", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Else
                Exit Sub
            End If
        Else
            Exit Sub
        End If
    End Sub

#End Region

#Region "Defence Profile Options"

    Private Sub UpdateDefenceProfileOptions()
        lvwDefenceProfiles.BeginUpdate()
        lvwDefenceProfiles.Items.Clear()
        Dim newItem As New ListViewItem
        For Each newProfile As DefenceProfile In DefenceProfiles.ProfileList.Values
            newItem = New ListViewItem
            newItem.Name = newProfile.Name
            newItem.Text = newProfile.Name
            Select Case newProfile.Type
                Case 0
                    newItem.SubItems.Add("Manual")
                Case 1
                    newItem.SubItems.Add("Fitting")
            End Select
            lvwDefenceProfiles.Items.Add(newItem)
        Next
        lvwDefenceProfiles.EndUpdate()
    End Sub

    Private Sub lvwDefenceProfiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwDefenceProfiles.SelectedIndexChanged
        If lvwDefenceProfiles.SelectedItems.Count > 0 Then
            Dim selProfile As DefenceProfile = CType(DefenceProfiles.ProfileList(lvwDefenceProfiles.SelectedItems(0).Name), DefenceProfile)
            lblDefProfileName.Text = selProfile.Name
            Select Case selProfile.Type
                Case 0
                    lblDefProfileType.Text = "Manual"
                Case 1
                    lblDefProfileType.Text = "Fitting"
            End Select
            lblDefSEM.Text = selProfile.SEM.ToString("N2")
            lblDefSEx.Text = selProfile.SExplosive.ToString("N2")
            lblDefSKi.Text = selProfile.SKinetic.ToString("N2")
            lblDefSTh.Text = selProfile.SThermal.ToString("N2")
            lblDefAEM.Text = selProfile.AEM.ToString("N2")
            lblDefAEx.Text = selProfile.AExplosive.ToString("N2")
            lblDefAKi.Text = selProfile.AKinetic.ToString("N2")
            lblDefATh.Text = selProfile.AThermal.ToString("N2")
            lblDefHEM.Text = selProfile.HEM.ToString("N2")
            lblDefHEx.Text = selProfile.HExplosive.ToString("N2")
            lblDefHKi.Text = selProfile.HKinetic.ToString("N2")
            lblDefHTh.Text = selProfile.HThermal.ToString("N2")
            If gpDefenceProfile.Visible = False Then
                gpDefenceProfile.Visible = True
            End If
        End If
    End Sub

    Private Sub btnDeleteDefenceProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteDefenceProfile.Click
        If lvwDefenceProfiles.SelectedItems.Count > 0 Then
            Dim profileName As String = lvwDefenceProfiles.SelectedItems(0).Name
            Dim reply As Integer = MessageBox.Show("Are you sure you want to delete the profile '" & profileName & "'?", "Confirm Profile Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If reply = DialogResult.No Then
                Exit Sub
            Else
                ' Delete the profile
                DefenceProfiles.ProfileList.Remove(profileName)
                ' Save the profiles
                DefenceProfiles.SaveProfiles()
                Call Me.UpdateDefenceProfileOptions()
            End If
        Else
            MessageBox.Show("Please select a profile before trying to delete.", "Error Deleting Profile", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnAddDefenceProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddDefenceProfile.Click
        Dim ProfileForm As New frmAddDefenceProfile
        ProfileForm.Tag = "Add"
        ProfileForm.btnAccept.Text = "Add Profile"
        ProfileForm.ShowDialog()
        ProfileForm.Dispose()
        Call Me.UpdateDefenceProfileOptions()
    End Sub

    Private Sub btnEditDefenceProfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditDefenceProfile.Click
        If lvwDefenceProfiles.SelectedItems.Count > 0 Then
            Dim profileName As String = lvwDefenceProfiles.SelectedItems(0).Name
            Dim editProfile As DefenceProfile = CType(DefenceProfiles.ProfileList(profileName), DefenceProfile)
            Dim ProfileForm As New frmAddDefenceProfile()
            ProfileForm.Tag = editProfile
            ProfileForm.btnAccept.Text = "Edit Profile"
            ProfileForm.ShowDialog()
            ProfileForm.Dispose()
            Call Me.UpdateDefenceProfileOptions()
        Else
            MessageBox.Show("Please select a profile before trying to delete.", "Error Deleting Profile", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub btnResetDefenceProfiles_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetDefenceProfiles.Click
        Dim response As Integer = MessageBox.Show("This will delete all your existing profiles and re-instate the defaults. Are you sure you wish to proceed?", "Confirm Reset ALL Profiles", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If response = Windows.Forms.DialogResult.Yes Then
            Dim cResponse As Integer = MessageBox.Show("Are you really sure you wish to proceed?", "Confirm Reset ALL Profiles", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If cResponse = Windows.Forms.DialogResult.Yes Then
                Try
                    If My.Computer.FileSystem.FileExists(Path.Combine(HQF.Settings.HQFFolder, "HQFDefenceProfiles.bin")) = True Then
                        My.Computer.FileSystem.DeleteFile(Path.Combine(HQF.Settings.HQFFolder, "HQFDefenceProfiles.bin"), FileIO.UIOption.OnlyErrorDialogs, FileIO.RecycleOption.SendToRecycleBin)
                    End If
                    DefenceProfiles.ResetDefenceProfiles()
                    Call Me.UpdateDefenceProfileOptions()
                    MessageBox.Show("Profiles successfully reset", "Profile Reset Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("Unable to delete the Defence Profiles file", "Deletion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Else
                Exit Sub
            End If
        Else
            Exit Sub
        End If
    End Sub

#End Region

    Private Sub btnGenerateIconList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerateIconList.Click
        Dim IconPath1 As String = "Z:\My Downloads\Incursion_1.4_imgs_Icons\Icons\items\32_32"
        Dim IconPath2 As String = "Z:\_HQFIcons"
        Dim IconList As New List(Of String)
        For Each sMod As ShipModule In ModuleLists.moduleList.Values
            If IconList.Contains(sMod.Icon) = False Then
                IconList.Add(sMod.Icon)
                Dim SourceFile As String = Path.Combine(IconPath1, "icon" & sMod.Icon & ".png")
                If My.Computer.FileSystem.FileExists(SourceFile) = True Then
                    Dim DestFile As String = Path.Combine(IconPath2, sMod.Icon & ".png")
                    My.Computer.FileSystem.CopyFile(SourceFile, DestFile)
                End If
            End If
        Next
        MessageBox.Show(IconList.Count.ToString)
    End Sub

    Private Sub btnGetIconImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetIconImage.Click
        pbIcon.Image = ImageHandler.IconImage24("02_02", 5)
    End Sub

#Region "Attribute Column Options"

    Private Sub UpdateAttributeColumns()
        adtAttributeColumns.BeginUpdate()
        adtAttributeColumns.Nodes.Clear()
        For Each attID As String In HQF.Settings.HQFSettings.IgnoredAttributeColumns
            Dim NewNode As New Node
            NewNode.Name = attID
            NewNode.Text = attID
            NewNode.Cells.Add(New Cell(CStr(Attributes.AttributeQuickList(attID))))
            adtAttributeColumns.Nodes.Add(NewNode)
        Next
        EveHQ.Core.AdvTreeSorter.Sort(adtAttributeColumns, 1, False, True)
        adtAttributeColumns.EndUpdate()
    End Sub

    Private Sub adtAttributeColumns_SelectionChanged(sender As Object, e As System.EventArgs) Handles adtAttributeColumns.SelectionChanged
        If adtAttributeColumns.SelectedNodes.Count > 0 Then
            btnRemoveAttribute.Enabled = True
        Else
            btnRemoveAttribute.Enabled = False
        End If
    End Sub

    Private Sub btnRemoveAttribute_Click(sender As System.Object, e As System.EventArgs) Handles btnRemoveAttribute.Click
        If adtAttributeColumns.SelectedNodes.Count > 0 Then
            Dim attID As String = adtAttributeColumns.SelectedNodes(0).Name
            If HQF.Settings.HQFSettings.IgnoredAttributeColumns.Contains(attID) = True Then
                HQF.Settings.HQFSettings.IgnoredAttributeColumns.Remove(attID)
                Call Me.UpdateAttributeColumns()
            End If
        End If
    End Sub

    Private Sub btnClearAttributes_Click(sender As System.Object, e As System.EventArgs) Handles btnClearAttributes.Click
        Dim reply As DialogResult = MessageBox.Show("Are you sure you wish to clear all the ignored attribute columns?", "Confirm Clear Columns", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If reply = Windows.Forms.DialogResult.Yes Then
            HQF.Settings.HQFSettings.IgnoredAttributeColumns.Clear()
            Call Me.UpdateAttributeColumns()
        Else
            Exit Sub
        End If
    End Sub
#End Region

   
End Class
