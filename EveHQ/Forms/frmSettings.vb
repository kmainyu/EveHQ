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
Imports Microsoft.Win32
Imports System.Data
Imports System.Xml
Imports System.IO
Imports System.Net
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar
Imports System.Text

Public Class frmSettings
    Dim redrawColumns As Boolean = False
    Dim startup As Boolean = True
    Public Property DoNotRecalculatePilots As Boolean = False

#Region "Form Opening & Closing"
    Private Sub frmSettings_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Call EveHQ.Core.EveHQSettingsFunctions.SaveSettings()
        If DoNotRecalculatePilots = False Then
            If frmTraining.IsHandleCreated = True And redrawColumns = True Then
                redrawColumns = False
                Call frmTraining.RefreshAllTrainingQueues()
            End If
            Call frmEveHQ.UpdatePilotInfo()
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Set the startup flag
        startup = True

        Call Me.UpdateGeneralSettings()
        Call Me.UpdateColourOptions()
        Call Me.UpdateEveServerSettings()
        Call Me.UpdateIGBSettings()
        Call Me.UpdateAccounts()
        Call Me.UpdatePilots()
        Call Me.UpdateEveFolderOptions()
        Call Me.UpdateViewPilots()
        Call Me.UpdateProxyOptions()
        Call Me.UpdatePlugIns()
        Call Me.UpdateNotificationOptions()
        Call Me.UpdateTrainingQueueOptions()
        Call Me.UpdateG15Options()
        Call Me.UpdateDatabaseSettings()
        Call Me.UpdateTaskBarIconOptions()
        Call Me.UpdateDashboardOptions()

        ' Set the flag to indicate end of the startup
        startup = False

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
#End Region

#Region "Treeview Routines"

    Private Sub tvwSettings_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvwSettings.AfterSelect
        Dim nodeName As String = e.Node.Name
        Dim gbName As String = nodeName.TrimStart("node".ToCharArray)
        gbName = "gb" & gbName
        For Each setControl As Control In Me.panelSettings.Controls
            If setControl.Name = "gpNav" Or setControl.Name = "tvwSettings" Or setControl.Name = "btnClose" Or setControl.Name = gbName Then
                Me.panelSettings.Controls(gbName).Top = 12
                Me.panelSettings.Controls(gbName).Left = 195
                Me.panelSettings.Controls(gbName).Width = 700
                Me.panelSettings.Controls(gbName).Height = 500
                Me.panelSettings.Controls(gbName).Visible = True
            Else
                setControl.Visible = False
            End If
        Next
    End Sub
    Private Sub tvwSettings_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvwSettings.NodeMouseClick
        Me.tvwSettings.SelectedNode = e.Node
    End Sub
#End Region

#Region "General Settings"

    Private Sub UpdateGeneralSettings()
        chkAutoHide.Checked = EveHQ.Core.HQ.EveHQSettings.AutoHide
        chkAutoRun.Checked = EveHQ.Core.HQ.EveHQSettings.AutoStart
        chkAutoMinimise.Checked = EveHQ.Core.HQ.EveHQSettings.AutoMinimise
        chkMinimiseOnExit.Checked = EveHQ.Core.HQ.EveHQSettings.MinimiseExit
        chkDisableAutoConnections.Checked = EveHQ.Core.HQ.EveHQSettings.DisableAutoWebConnections
        chkDisableTrainingBar.Checked = EveHQ.Core.HQ.EveHQSettings.DisableTrainingBar
        If cboStartupView.Items.Contains(EveHQ.Core.HQ.EveHQSettings.StartupView) = True Then
            cboStartupView.SelectedItem = EveHQ.Core.HQ.EveHQSettings.StartupView
        Else
            cboStartupView.SelectedIndex = 0
        End If
        txtUpdateLocation.Text = EveHQ.Core.HQ.EveHQSettings.UpdateURL
        txtUpdateLocation.Enabled = False
        chkErrorReporting.Checked = EveHQ.Core.HQ.EveHQSettings.ErrorReportingEnabled
        txtErrorRepName.Text = EveHQ.Core.HQ.EveHQSettings.ErrorReportingName
        txtErrorRepEmail.Text = EveHQ.Core.HQ.EveHQSettings.ErrorReportingEmail
        If EveHQ.Core.HQ.EveHQSettings.MDITabPosition IsNot Nothing Then
            cboMDITabPosition.SelectedItem = EveHQ.Core.HQ.EveHQSettings.MDITabPosition
        Else
            cboMDITabPosition.SelectedItem = "Top"
        End If
    End Sub

    Private Sub chkAutoHide_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAutoHide.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.AutoHide = chkAutoHide.Checked
    End Sub

    Private Sub chkAutoRun_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAutoRun.CheckedChanged
        If chkAutoRun.Checked = True Then
            Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", True)
            If EveHQ.Core.HQ.IsUsingLocalFolders = False Then
                RegKey.SetValue(Application.ProductName, """" & Application.ExecutablePath.ToString & """")
            Else
                RegKey.SetValue(Application.ProductName, """" & Application.ExecutablePath.ToString & """" & " /local")
            End If
            EveHQ.Core.HQ.EveHQSettings.AutoStart = True
        Else
            Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", True)
            RegKey.DeleteValue(Application.ProductName, False)
            EveHQ.Core.HQ.EveHQSettings.AutoStart = False
        End If
    End Sub

    Private Sub chkAutoMinimise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoMinimise.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.AutoMinimise = chkAutoMinimise.Checked
    End Sub

    Private Sub chkMinimiseOnExit_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMinimiseOnExit.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.MinimiseExit = chkMinimiseOnExit.Checked
    End Sub

    Private Sub chkDisableAutoConnections_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDisableAutoConnections.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.DisableAutoWebConnections = chkDisableAutoConnections.Checked
        If EveHQ.Core.HQ.EveHQSettings.DisableAutoWebConnections = True Then
            chkAutoAPI.Checked = False
            chkAutoAPI.Enabled = False
            chkAutoMailAPI.Checked = False
            chkAutoMailAPI.Enabled = False
        Else
            chkAutoAPI.Enabled = True
            chkAutoMailAPI.Enabled = True
        End If
    End Sub

    Private Sub chkDisableTrainingBar_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDisableTrainingBar.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.DisableTrainingBar = chkDisableTrainingBar.Checked
        If EveHQ.Core.HQ.EveHQSettings.DisableTrainingBar = False Then
            If EveHQ.Core.HQ.EveHQSettings.TrainingBarDockPosition = DevComponents.DotNetBar.eDockSide.None Then
                EveHQ.Core.HQ.EveHQSettings.TrainingBarDockPosition = DevComponents.DotNetBar.eDockSide.Bottom
            End If
            frmEveHQ.Bar1.DockSide = CType(EveHQ.Core.HQ.EveHQSettings.TrainingBarDockPosition, DevComponents.DotNetBar.eDockSide)
            frmEveHQ.DockContainerItem1.Height = EveHQ.Core.HQ.EveHQSettings.TrainingBarHeight
            frmEveHQ.DockContainerItem1.Width = EveHQ.Core.HQ.EveHQSettings.TrainingBarWidth
        Else
            frmEveHQ.Bar1.Visible = False
            ' Clear old event handlers and controls
            For c As Integer = frmEveHQ.pdc1.Controls.Count - 1 To 0 Step -1
                Dim cb As CharacterTrainingBlock = CType(frmEveHQ.pdc1.Controls(c), CharacterTrainingBlock)
                RemoveHandler cb.lblSkill.Click, AddressOf frmEveHQ.TrainingStatusLabelClick
                RemoveHandler cb.lblTime.Click, AddressOf frmEveHQ.TrainingStatusLabelClick
                RemoveHandler cb.lblQueue.Click, AddressOf frmEveHQ.TrainingStatusLabelClick
                cb.Dispose()
            Next
            ' Clear items - just to make sure
            frmEveHQ.pdc1.Controls.Clear()
        End If
    End Sub

    Private Sub cboStartupView_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboStartupView.SelectedIndexChanged
        EveHQ.Core.HQ.EveHQSettings.StartupView = CStr(cboStartupView.SelectedItem)
    End Sub

    Private Sub cboStartupPilot_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboStartupPilot.SelectedIndexChanged
        EveHQ.Core.HQ.EveHQSettings.StartupPilot = CStr(cboStartupPilot.SelectedItem)
    End Sub

    Private Sub UpdateViewPilots()
        cboStartupPilot.Items.Clear()
        Dim myPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
        For Each myPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If myPilot.Active = True Then
                cboStartupPilot.Items.Add(myPilot.Name)
            End If
        Next
        If cboStartupPilot.Items.Contains(EveHQ.Core.HQ.EveHQSettings.StartupPilot) = False Then
            If cboStartupPilot.Items.Count > 0 Then
                cboStartupPilot.SelectedIndex = 0
            End If
        Else
            cboStartupPilot.SelectedItem = EveHQ.Core.HQ.EveHQSettings.StartupPilot
        End If
    End Sub

    Private Sub lblUpdateLocation_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblUpdateLocation.DoubleClick
        txtUpdateLocation.Enabled = True
    End Sub

    Private Sub txtUpdateLocation_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUpdateLocation.TextChanged
        EveHQ.Core.HQ.EveHQSettings.UpdateURL = txtUpdateLocation.Text
    End Sub

    Private Sub chkErrorReporting_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkErrorReporting.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.ErrorReportingEnabled = chkErrorReporting.Checked
        If EveHQ.Core.HQ.EveHQSettings.ErrorReportingEnabled = True Then
            lblErrorRepEmail.Enabled = True
            lblErrorRepName.Enabled = True
            txtErrorRepEmail.Enabled = True
            txtErrorRepName.Enabled = True
        Else
            lblErrorRepEmail.Enabled = False
            lblErrorRepName.Enabled = False
            txtErrorRepEmail.Enabled = False
            txtErrorRepName.Enabled = False
        End If
    End Sub

    Private Sub txtErrorRepName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtErrorRepName.TextChanged
        EveHQ.Core.HQ.EveHQSettings.ErrorReportingName = txtErrorRepName.Text
    End Sub

    Private Sub txtErrorRepEmail_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtErrorRepEmail.TextChanged
        EveHQ.Core.HQ.EveHQSettings.ErrorReportingEmail = txtErrorRepEmail.Text
    End Sub

    Private Sub cboMDITabPosition_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboMDITabPosition.SelectedIndexChanged
        EveHQ.Core.HQ.EveHQSettings.MDITabPosition = cboMDITabPosition.SelectedItem.ToString
        Select Case EveHQ.Core.HQ.EveHQSettings.MDITabPosition
            Case "Top"
                frmEveHQ.tabEveHQMDI.Dock = DockStyle.Top
                frmEveHQ.tabEveHQMDI.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Top
            Case "Bottom"
                frmEveHQ.tabEveHQMDI.Dock = DockStyle.Bottom
                frmEveHQ.tabEveHQMDI.TabAlignment = DevComponents.DotNetBar.eTabStripAlignment.Bottom
        End Select
    End Sub

#End Region

#Region "Colour Settings"
    Private Sub UpdateColourOptions()
        ' Update the pilot colours
        Call Me.UpdatePBPilotColours()
        chkDisableVisualStyles.Checked = EveHQ.Core.HQ.EveHQSettings.DisableVisualStyles
        txtCSVSeparator.Text = EveHQ.Core.HQ.EveHQSettings.CSVSeparatorChar
    End Sub

    Private Sub UpdatePBPilotColours()
        pbPilotCurrent.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotCurrentTrainSkillColor))
        pbPilotLevel5.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotLevel5SkillColor))
        pbPilotPartial.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotPartTrainedSkillColor))
        pbPilotStandard.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotStandardSkillColor))
        pbPilotGroupBG.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotGroupBackgroundColor))
        pbPilotGroupText.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotGroupTextColor))
        pbPilotSkillText.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotSkillTextColor))
        pbPilotSkillHighlight.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotSkillHighlightColor))
    End Sub

    Private Sub pbPilotColours_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbPilotGroupBG.Click, pbPilotGroupText.Click, pbPilotSkillText.Click, pbPilotSkillHighlight.Click, pbPilotCurrent.Click, pbPilotLevel5.Click, pbPilotPartial.Click, pbPilotStandard.Click
        Dim thisPB As PictureBox = CType(sender, PictureBox)
        Dim dlgResult As Integer = 0
        With cd1
            .AllowFullOpen = True
            .AnyColor = True
            .FullOpen = True
            Select Case thisPB.Name
                Case "pbPilotCurrent"
                    .Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotCurrentTrainSkillColor))
                Case "pbPilotLevel5"
                    .Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotLevel5SkillColor))
                Case "pbPilotPartial"
                    .Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotPartTrainedSkillColor))
                Case "pbPilotStandard"
                    .Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotStandardSkillColor))
                Case "pbPilotGroupBG"
                    .Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotGroupBackgroundColor))
                Case "pbPilotGroupText"
                    .Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotGroupTextColor))
                Case "pbPilotSkillText"
                    .Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotSkillTextColor))
                Case "pbPilotSkillHighlight"
                    .Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PilotSkillHighlightColor))
            End Select
            dlgResult = .ShowDialog()
        End With
        If dlgResult = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        Else
            thisPB.BackColor = cd1.Color
            Select Case thisPB.Name
                Case "pbPilotCurrent"
                    EveHQ.Core.HQ.EveHQSettings.PilotCurrentTrainSkillColor = cd1.Color.ToArgb
                Case "pbPilotLevel5"
                    EveHQ.Core.HQ.EveHQSettings.PilotLevel5SkillColor = cd1.Color.ToArgb
                Case "pbPilotPartial"
                    EveHQ.Core.HQ.EveHQSettings.PilotPartTrainedSkillColor = cd1.Color.ToArgb
                Case "pbPilotStandard"
                    EveHQ.Core.HQ.EveHQSettings.PilotStandardSkillColor = cd1.Color.ToArgb
                Case "pbPilotGroupBG"
                    EveHQ.Core.HQ.EveHQSettings.PilotGroupBackgroundColor = cd1.Color.ToArgb
                Case "pbPilotGroupText"
                    EveHQ.Core.HQ.EveHQSettings.PilotGroupTextColor = cd1.Color.ToArgb
                Case "pbPilotSkillText"
                    EveHQ.Core.HQ.EveHQSettings.PilotSkillTextColor = cd1.Color.ToArgb
                Case "pbPilotSkillHighlight"
                    EveHQ.Core.HQ.EveHQSettings.PilotSkillHighlightColor = cd1.Color.ToArgb
            End Select
            ' Update the colours
            frmPilot.adtSkills.Refresh()
        End If
    End Sub

    Private Sub btnResetPilotColours_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetPilotColours.Click
        ' Resets the panel colours to the default values
        EveHQ.Core.HQ.EveHQSettings.PilotCurrentTrainSkillColor = System.Drawing.Color.LimeGreen.ToArgb
        EveHQ.Core.HQ.EveHQSettings.PilotLevel5SkillColor = System.Drawing.Color.Thistle.ToArgb
        EveHQ.Core.HQ.EveHQSettings.PilotPartTrainedSkillColor = System.Drawing.Color.Gold.ToArgb
        EveHQ.Core.HQ.EveHQSettings.PilotStandardSkillColor = System.Drawing.Color.White.ToArgb
        EveHQ.Core.HQ.EveHQSettings.PilotGroupBackgroundColor = System.Drawing.Color.DimGray.ToArgb
        EveHQ.Core.HQ.EveHQSettings.PilotGroupTextColor = System.Drawing.Color.White.ToArgb
        EveHQ.Core.HQ.EveHQSettings.PilotSkillTextColor = System.Drawing.Color.Black.ToArgb
        EveHQ.Core.HQ.EveHQSettings.PilotSkillHighlightColor = System.Drawing.Color.DodgerBlue.ToArgb
        ' Update the colours
        frmPilot.adtSkills.Refresh()
        ' Update the PBPilot Colours
        Call Me.UpdatePBPilotColours()
    End Sub

    Private Sub chkDisableVisualStyles_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDisableVisualStyles.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.DisableVisualStyles = chkDisableVisualStyles.Checked
        If chkDisableVisualStyles.Checked = True Then
            Application.VisualStyleState = VisualStyles.VisualStyleState.NoneEnabled
        Else
            Application.VisualStyleState = VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled
        End If
    End Sub

    Private Sub txtCSVSeparator_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCSVSeparator.TextChanged
        EveHQ.Core.HQ.EveHQSettings.CSVSeparatorChar = txtCSVSeparator.Text
    End Sub
#End Region

#Region "Eve Accounts Settings"

    Private Sub btnAddAccount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAccount.Click
        ' Clear the text boxes
        Dim myAccounts As frmModifyEveAccounts = New frmModifyEveAccounts
        With myAccounts
            .Tag = "Add"
            .txtUserIDV2.Text = "" : .txtUserIDV2.Enabled = True
            .txtAPIKeyV2.Text = "" : .txtAPIKeyV2.Enabled = True
            .txtAccountNameV2.Text = "" : .txtAccountNameV2.Enabled = True
            .btnAcceptV2.Text = "OK"
            .Text = "Add New Account"
            .ShowDialog()
        End With
        Me.UpdateAccounts()
    End Sub

    Private Sub btnEditAccount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditAccount.Click
        Call EditAccount()
    End Sub

    Private Sub EditAccount()
        ' Check for some selection on the listview
        If adtAccounts.SelectedNodes.Count = 0 Then
            MessageBox.Show("Please select an account to edit!", "Cannot Edit", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            adtAccounts.Select()
        Else
            Dim myAccounts As frmModifyEveAccounts = New frmModifyEveAccounts
            With myAccounts
                ' Load the account details into the text boxes
                Dim selAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts(adtAccounts.SelectedNodes(0).Name), Core.EveAccount)
                Select Case selAccount.APIKeySystem
                    Case Core.APIKeySystems.Version2
                        .txtUserIDV2.Text = selAccount.userID : .txtUserIDV2.Enabled = False
                        .txtAPIKeyV2.Text = selAccount.APIKey : .txtAPIKeyV2.Enabled = True
                        .txtAccountNameV2.Text = selAccount.FriendlyName : .txtAccountNameV2.Enabled = True
                        .lblAPIKeyTypeV2.Text = selAccount.APIKeyType.ToString
                        .lblAPIAccessMask.Text = selAccount.AccessMask.ToString
                        .btnAcceptV2.Text = "OK"
                    Case Else
                        ' Ignore
                End Select
                .Tag = "Edit"
                .Text = "Edit '" & selAccount.FriendlyName & "' Account Details"
                ' Disable the username text box (cannot edit this by design!!)
                .ShowDialog()
            End With
            Me.UpdateAccounts()
        End If
    End Sub

    Private Sub btnDeleteAccount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteAccount.Click
        ' Check for some selection on the listview
        If adtAccounts.SelectedNodes.Count = 0 Then
            MessageBox.Show("Please select an account to delete!", "Cannot Delete Account", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            adtAccounts.Select()
        Else
            Dim selAccount As String = adtAccounts.SelectedNodes(0).Name
            Dim selAccountName As String = adtAccounts.SelectedNodes(0).Text
            ' Get the list of pilots that are affected
            Dim dPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
            Dim strPilots As String = ""
            For Each dPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                If dPilot.Account = selAccount Then
                    strPilots &= dPilot.Name & ControlChars.CrLf
                End If
            Next
            If strPilots = "" Then strPilots = "<none>"
            ' Confirm deletion
            Dim msg As String = ""
            If strPilots = "<none>" Then
                msg &= "Deleting the '" & selAccountName & "' account will not delete any of your existing pilots." & ControlChars.CrLf & ControlChars.CrLf
            Else
                msg &= "Deleting the '" & selAccountName & "' account will delete the following pilots:" & ControlChars.CrLf & strPilots & ControlChars.CrLf
            End If
            msg &= "Are you sure you wish to delete the account '" & selAccountName & "'?"
            Dim confirm As Integer = MessageBox.Show(msg, "Confirm Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If confirm = vbYes Then
                ' Delete the account from the accounts collection
                EveHQ.Core.HQ.EveHQSettings.Accounts.Remove(adtAccounts.SelectedNodes(0).Name)
                ' Remove the item from the list
                adtAccounts.Nodes.Remove(adtAccounts.SelectedNodes(0))
                ' Remove the pilots
                For Each dPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                    If dPilot.Account = selAccount Then
                        EveHQ.Core.HQ.EveHQSettings.Pilots.Remove(dPilot.Name)
                    End If
                Next
                Call frmEveHQ.UpdatePilotInfo()
                Call Me.UpdatePilots()
            Else
                adtAccounts.Select()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub btnDisableAccount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDisableAccount.Click
        ' Check for some selection on the listview
        If adtAccounts.SelectedNodes.Count = 0 Then
            MessageBox.Show("Please select an account to toggle the status of!", "Cannot Set Account Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            adtAccounts.Select()
        Else
            Dim SI As Node = adtAccounts.SelectedNodes(0)
            Dim UserID As String = SI.Name
            Dim cAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts(UserID), Core.EveAccount)
            Select Case cAccount.APIAccountStatus
                Case Core.APIAccountStatuses.Active, Core.APIAccountStatuses.Disabled
                    cAccount.APIAccountStatus = Core.APIAccountStatuses.ManualDisabled
                    btnDisableAccount.Text = "Enable Account"
                Case Core.APIAccountStatuses.ManualDisabled
                    cAccount.APIAccountStatus = Core.APIAccountStatuses.Active
                    btnDisableAccount.Text = "Disable Account"
            End Select
            SI.Cells(4).Text = cAccount.APIAccountStatus.ToString
        End If
    End Sub

    Private Sub btnGetData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Call frmEveHQ.QueryMyEveServer()
    End Sub

    Public Sub UpdateAccounts()
        adtAccounts.BeginUpdate()
        adtAccounts.Nodes.Clear()
        Dim newAccount As EveHQ.Core.EveAccount = New EveHQ.Core.EveAccount
        For Each newAccount In EveHQ.Core.HQ.EveHQSettings.Accounts
            Dim newLine As New Node
            newLine.Name = newAccount.userID
            newLine.Text = newAccount.FriendlyName
            newLine.Cells.Add(New Cell(newAccount.APIKeySystem.ToString))
            newLine.Cells.Add(New Cell(newAccount.userID))
            newLine.Cells.Add(New Cell(newAccount.APIKeyType.ToString))
            newLine.Cells.Add(New Cell(newAccount.APIAccountStatus.ToString))
            Select Case newAccount.APIKeyType
                Case Core.APIKeyTypes.Unknown, Core.APIKeyTypes.Limited
                    Dim TTT As String = ""
                    TTT &= "Account Creation Date: <Unknown>" & ControlChars.CrLf
                    TTT &= "Account Expiry Date: <Unknown>" & ControlChars.CrLf
                    TTT &= "Logon Count: <Unknown>" & ControlChars.CrLf
                    TTT &= "Time Online: <Unknown>"
                    Dim STI As New SuperTooltipInfo("Account Name: " & newAccount.FriendlyName, "Eve API Account Information", TTT, Nothing, My.Resources.Info32, eTooltipColor.Yellow)
                    STT.SetSuperTooltip(newLine, STI)
                Case Core.APIKeyTypes.Full, Core.APIKeyTypes.Character, Core.APIKeyTypes.Corporation
                    Dim TTT As String = ""
                    TTT &= "Account Creation Date: " & newAccount.CreateDate.ToString & ControlChars.CrLf
                    TTT &= "Account Expiry Date: " & newAccount.PaidUntil.ToString & ControlChars.CrLf
                    TTT &= "Logon Count: " & newAccount.LogonCount.ToString("N0") & ControlChars.CrLf
                    TTT &= "Time Online: " & EveHQ.Core.SkillFunctions.TimeToString(newAccount.LogonMinutes * 60, False)
                    Dim STI As New SuperTooltipInfo("Account Name: " & newAccount.FriendlyName, "Eve API Account Information", TTT, Nothing, My.Resources.Info32, eTooltipColor.Yellow)
                    STT.SetSuperTooltip(newLine, STI)
            End Select
            adtAccounts.Nodes.Add(newLine)
        Next
        adtAccounts.EndUpdate()
        If EveHQ.Core.HQ.EveHQSettings.Accounts.Count = 0 Then
            frmEveHQ.btnQueryAPI.Enabled = False
        Else
            frmEveHQ.btnQueryAPI.Enabled = True
        End If
    End Sub

    Private Sub btnCheckAPIKeys_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckAPIKeys.Click
        ' Checks the API keys types by reference to the account status API
        For Each cAccount As EveHQ.Core.EveAccount In EveHQ.Core.HQ.EveHQSettings.Accounts
            Call cAccount.CheckAPIKey()
        Next
        Call Me.UpdateAccounts()
    End Sub

    Private Sub adtAccounts_NodeDoubleClick(sender As Object, e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles adtAccounts.NodeDoubleClick
        Call EditAccount()
    End Sub

    Private Sub adtAccounts_SelectionChanged(sender As Object, e As System.EventArgs) Handles adtAccounts.SelectionChanged
        If adtAccounts.SelectedNodes.Count = 1 Then
            Dim SI As Node = adtAccounts.SelectedNodes(0)
            Dim UserID As String = SI.Name
            Dim cAccount As EveHQ.Core.EveAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts(UserID), Core.EveAccount)
            Select Case cAccount.APIAccountStatus
                Case Core.APIAccountStatuses.Active, Core.APIAccountStatuses.Disabled
                    btnDisableAccount.Text = "Disable Account"
                    btnDisableAccount.Enabled = True
                Case Core.APIAccountStatuses.ManualDisabled
                    btnDisableAccount.Text = "Enable Account"
                    btnDisableAccount.Enabled = True
            End Select
        Else
            btnDisableAccount.Text = "Disable Account"
            btnDisableAccount.Enabled = False
        End If
    End Sub

#End Region

#Region "Plug-ins Settings"
    Public Sub UpdatePlugIns()
        lvwPlugins.Items.Clear()
        For Each newPlugIn As EveHQ.Core.PlugIn In EveHQ.Core.HQ.EveHQSettings.Plugins.Values
            Dim newLine As New ListViewItem
            newLine.Name = newPlugIn.Name
            newLine.Text = newPlugIn.Name & " (v" & newPlugIn.Version & ")"
            If newPlugIn.Disabled = True Then
                newLine.Checked = False
                Dim status As String = ""
                Select Case newPlugIn.Status
                    Case EveHQ.Core.PlugIn.PlugInStatus.Uninitialised
                        status = "Uninitialised"
                    Case EveHQ.Core.PlugIn.PlugInStatus.Loading
                        status = "Loading"
                    Case EveHQ.Core.PlugIn.PlugInStatus.Failed
                        status = "Failed"
                    Case EveHQ.Core.PlugIn.PlugInStatus.Active
                        status = "Active"
                End Select
                newLine.SubItems.Add("Disabled" & " (" & status & ")")
            Else
                newLine.Checked = True
                Dim status As String = ""
                Select Case newPlugIn.Status
                    Case EveHQ.Core.PlugIn.PlugInStatus.Uninitialised
                        status = "Uninitialised"
                    Case EveHQ.Core.PlugIn.PlugInStatus.Loading
                        status = "Loading"
                    Case EveHQ.Core.PlugIn.PlugInStatus.Failed
                        status = "Failed"
                    Case EveHQ.Core.PlugIn.PlugInStatus.Active
                        status = "Active"
                End Select
                newLine.SubItems.Add("Enabled" & " (" & status & ")")
            End If
            If newPlugIn.Available = False Then
                newLine.SubItems(1).Text = "Unavailable"
            End If
            lvwPlugins.Items.Add(newLine)
        Next
    End Sub
    Private Sub btnTidyPlugins_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTidyPlugins.Click
        Dim removePlugIns As New ArrayList
        For Each newPlugIn As EveHQ.Core.PlugIn In EveHQ.Core.HQ.EveHQSettings.Plugins.Values
            If newPlugIn.Available = False Then
                removePlugIns.Add(newPlugIn.Name)
            End If
        Next
        For Each Plugin As String In removePlugIns
            EveHQ.Core.HQ.EveHQSettings.Plugins.Remove(Plugin)
        Next
        Call Me.UpdatePlugIns()
    End Sub
    Private Sub btnRefreshPlugins_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefreshPlugins.Click
        Call Me.UpdatePlugIns()
    End Sub
    Private Sub lvwPlugins_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwPlugins.ItemChecked
        Dim pluginName As String = e.Item.Name
        Dim plugin As EveHQ.Core.PlugIn = CType(EveHQ.Core.HQ.EveHQSettings.Plugins(pluginName), Core.PlugIn)
        If e.Item.Checked = True Then
            plugin.Disabled = False
        Else
            plugin.Disabled = True
        End If
    End Sub
#End Region

#Region "Eve Pilots Settings"
    Private Sub btnAddPilot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPilot.Click
        ' Clear the text boxes
        Dim myPilots As frmModifyEvePilots = New frmModifyEvePilots
        With myPilots
            .txtPilotName.Text = "" : .txtPilotName.Enabled = True
            .txtPilotID.Text = "" : .txtPilotID.Enabled = True
            .Text = "Add New Pilot"
            .ShowDialog()
        End With
        Me.UpdatePilots()
        Call frmEveHQ.UpdatePilotInfo()
    End Sub

    Private Sub btnAddPilotFromXML_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPilotFromXML.Click
        Call EveHQ.Core.PilotParseFunctions.LoadPilotFromXML()
        Call frmEveHQ.UpdatePilotInfo()
        Call Me.UpdatePilots()
    End Sub

    Private Sub btnCreateBlankPilot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateBlankPilot.Click
        Dim newCharForm As New frmCharCreate
        newCharForm.ShowDialog()
        Call Me.UpdatePilots()
        newCharForm.Dispose()
    End Sub

    Public Sub UpdatePilots()
        lvwPilots.Items.Clear()
        Dim newPilot As EveHQ.Core.Pilot = New EveHQ.Core.Pilot
        For Each newPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            Dim newLine As New ListViewItem
            newLine.Text = newPilot.Name
            newLine.SubItems.Add(newPilot.ID)
            newLine.SubItems.Add(newPilot.Account)
            If newPilot.Active = True Then
                newLine.Checked = True
            Else
                newLine.Checked = False
            End If
            lvwPilots.Items.Add(newLine)
        Next
    End Sub

    Private Sub btnDeletePilot_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeletePilot.Click
        ' Check for some selection on the listview
        If lvwPilots.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select a pilot to delete!", "Cannot Delete Pilot", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            lvwPilots.Select()
        Else
            Dim selPilot As String = lvwPilots.SelectedItems(0).Text
            Dim selAccount As String = lvwPilots.SelectedItems(0).SubItems(2).Text
            ' Check if the pilot is linked to an account - and therefore cannot be deleted
            If selAccount <> "" Then
                Dim msg As String = ""
                msg &= "You cannot delete pilot '" & selPilot & "' as it is currently associated with account '" & selAccount & "'."
                MessageBox.Show(msg, "Cannot Delete Pilot", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            ' Confirm deletion
            Dim strMsg As String = "Are you sure you wish to delete pilot '" & selPilot & "'?"
            Dim confirm As Integer = MessageBox.Show(strMsg, "Confirm Pilot Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If confirm = vbYes Then
                ' Delete the account from the accounts collection
                EveHQ.Core.HQ.EveHQSettings.Pilots.Remove(selPilot)
                ' Update the settings view
                Call Me.UpdatePilots()
                ' Update the list of pilots in the main form
                Call frmEveHQ.UpdatePilotInfo()
            Else
                lvwPilots.Select()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub lvwPilots_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvwPilots.ColumnClick
        If CInt(lvwPilots.Tag) = e.Column Then
            Me.lvwPilots.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, Windows.Forms.SortOrder.Ascending)
            lvwPilots.Tag = -1
        Else
            Me.lvwPilots.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, Windows.Forms.SortOrder.Descending)
            lvwPilots.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lvwPilots.Sort()
    End Sub

    Private Sub lvwPilots_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles lvwPilots.ItemCheck
        Dim pilotIndex As Integer = e.Index
        Dim newpilot As New EveHQ.Core.Pilot
        newpilot = CType(EveHQ.Core.HQ.EveHQSettings.Pilots(lvwPilots.Items(pilotIndex).Text), Core.Pilot)
        If lvwPilots.Items(pilotIndex).Checked = False Then
            newpilot.Active = True
        Else
            newpilot.Active = False
        End If
    End Sub

#End Region

#Region "IGB Settings"

    Private Sub UpdateIGBSettings()
        Dim sp As Boolean = False

        nudIGBPort.Value = EveHQ.Core.HQ.EveHQSettings.IGBPort
        chkStartIGBonLoad.Checked = EveHQ.Core.HQ.EveHQSettings.IGBAutoStart
        Call EveHQ.Core.IGB.CheckAllIGBAccessRights()

        If EveHQ.Core.HQ.EveHQSettings.IGBFullMode = False Then
            rb_IGBCfgAccessMode.Checked = True
        Else
            rb_IGBFullAccessMode.Checked = True
        End If

        ' Cycle plug-ins
        For Each PlugInInfo As EveHQ.Core.PlugIn In EveHQ.Core.HQ.EveHQSettings.Plugins.Values
            If PlugInInfo.RunInIGB = True Then
                ' If the Plug-In is not part of the List, then add it, default to enabled
                If Not EveHQ.Core.HQ.EveHQSettings.IGBAllowedData.ContainsKey(PlugInInfo.Name) Then
                    EveHQ.Core.HQ.EveHQSettings.IGBAllowedData.Add(PlugInInfo.Name, True)
                End If
            End If
        Next

        clb_IGBAllowedDisplay.Items.Clear()
        For Each allowed As String In EveHQ.Core.HQ.EveHQSettings.IGBAllowedData.Keys
            sp = EveHQ.Core.HQ.EveHQSettings.IGBAllowedData(allowed)
            clb_IGBAllowedDisplay.Items.Add(allowed, sp)
        Next

    End Sub

    Private Sub nudIGBPort_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudIGBPort.Click
        EveHQ.Core.HQ.EveHQSettings.IGBPort = CInt(nudIGBPort.Value)
    End Sub

    Private Sub nudIGBPort_HandleDestroyed(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudIGBPort.HandleDestroyed
        EveHQ.Core.HQ.EveHQSettings.IGBPort = CInt(nudIGBPort.Value)
    End Sub

    Private Sub nudIGBPort_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles nudIGBPort.KeyUp
        If e.KeyCode = Keys.Enter Then
            EveHQ.Core.HQ.EveHQSettings.IGBPort = CInt(nudIGBPort.Value)
        End If
    End Sub

    Private Sub chkStartIGBOnLoad_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkStartIGBonLoad.CheckedChanged
        If chkStartIGBonLoad.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.IGBAutoStart = True
        Else
            EveHQ.Core.HQ.EveHQSettings.IGBAutoStart = False
        End If
    End Sub

    Private Sub rb_IGBInPublicMode_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_IGBCfgAccessMode.CheckedChanged, rb_IGBFullAccessMode.CheckedChanged
        If startup = False Then
            If rb_IGBCfgAccessMode.Checked = True Then
                EveHQ.Core.HQ.EveHQSettings.IGBFullMode = False
            Else
                EveHQ.Core.HQ.EveHQSettings.IGBFullMode = True
            End If
        End If
    End Sub

#End Region

#Region "Training Queue Options"
    Private Sub clbColumns_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs)
        If e.Index > 5 Then
            If e.CurrentValue = CheckState.Checked Then
                EveHQ.Core.HQ.EveHQSettings.QColumns(e.Index, 1) = CStr(False)
            Else
                EveHQ.Core.HQ.EveHQSettings.QColumns(e.Index, 1) = CStr(True)
            End If
        Else
            e.NewValue = CheckState.Checked
        End If
        If startup = False Then
            redrawColumns = True
        End If
    End Sub

    Private Sub chkShowCompletedSkills_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowCompletedSkills.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.ShowCompletedSkills = chkShowCompletedSkills.Checked
    End Sub

    Private Sub chkDeleteCompletedSkills_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDeleteCompletedSkills.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.DeleteSkills = chkDeleteCompletedSkills.Checked
    End Sub

    Private Sub chkStartWithPrimaryQueue_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkStartWithPrimaryQueue.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.StartWithPrimaryQueue = chkStartWithPrimaryQueue.Checked
    End Sub

    Private Sub UpdateTrainingQueueOptions()
        ' Add the Queue columns
        Call Me.RedrawQueueColumnList()
        Me.chkDeleteCompletedSkills.Checked = EveHQ.Core.HQ.EveHQSettings.DeleteSkills
        Me.chkShowCompletedSkills.Checked = EveHQ.Core.HQ.EveHQSettings.ShowCompletedSkills
        Me.chkStartWithPrimaryQueue.Checked = EveHQ.Core.HQ.EveHQSettings.StartWithPrimaryQueue
        Dim IColor As Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.IsPreReqColor))
        Me.pbIsPreReqColour.BackColor = IColor
        Dim HColor As Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.HasPreReqColor))
        Me.pbHasPreReqColour.BackColor = HColor
        Dim BColor As Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.BothPreReqColor))
        Me.pbBothPreReqColour.BackColor = BColor
        Dim CColor As Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.DTClashColor))
        Me.pbDowntimeClashColour.BackColor = CColor
        Dim RColor As Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.ReadySkillColor))
        Me.pbReadySkillColour.BackColor = RColor
        Dim PColor As Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PartialTrainColor))
        Me.pbPartiallyTrainedColour.BackColor = PColor
    End Sub

    Private Sub pbIsPreReqColour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbIsPreReqColour.Click
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
            Me.pbIsPreReqColour.BackColor = cd1.Color
            EveHQ.Core.HQ.EveHQSettings.IsPreReqColor = cd1.Color.ToArgb
            redrawColumns = True
        End If
    End Sub

    Private Sub pbHasPreReqColour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbHasPreReqColour.Click
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
            Me.pbHasPreReqColour.BackColor = cd1.Color
            EveHQ.Core.HQ.EveHQSettings.HasPreReqColor = cd1.Color.ToArgb
            redrawColumns = True
        End If
    End Sub

    Private Sub pbBothPreReqColour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbBothPreReqColour.Click
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
            Me.pbBothPreReqColour.BackColor = cd1.Color
            EveHQ.Core.HQ.EveHQSettings.BothPreReqColor = cd1.Color.ToArgb
            redrawColumns = True
        End If
    End Sub

    Private Sub pbDowntimeClashColour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbDowntimeClashColour.Click
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
            Me.pbDowntimeClashColour.BackColor = cd1.Color
            EveHQ.Core.HQ.EveHQSettings.DTClashColor = cd1.Color.ToArgb
            redrawColumns = True
        End If
    End Sub

    Private Sub pbReadySkillColour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbReadySkillColour.Click
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
            Me.pbReadySkillColour.BackColor = cd1.Color
            EveHQ.Core.HQ.EveHQSettings.ReadySkillColor = cd1.Color.ToArgb
            redrawColumns = True
        End If
    End Sub

    Private Sub pbPartiallyTrainedColour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbPartiallyTrainedColour.Click
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
            Me.pbPartiallyTrainedColour.BackColor = cd1.Color
            EveHQ.Core.HQ.EveHQSettings.PartialTrainColor = cd1.Color.ToArgb
            redrawColumns = True
        End If
    End Sub
#End Region

#Region "Database Options"

    Private Sub UpdateDatabaseSettings()
        Me.cboFormat.SelectedIndex = EveHQ.Core.HQ.EveHQSettings.DBFormat
        Me.chkUseAppDirForDB.Checked = EveHQ.Core.HQ.EveHQSettings.UseAppDirectoryForDB
        Me.nudDBTimeout.Value = EveHQ.Core.HQ.EveHQSettings.DBTimeout
    End Sub

    Private Sub nudDBTimeout_HandleDestroyed(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudDBTimeout.HandleDestroyed
        EveHQ.Core.HQ.EveHQSettings.DBTimeout = CInt(nudDBTimeout.Value)
    End Sub

    Private Sub nudDBTimeout_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudDBTimeout.ValueChanged
        EveHQ.Core.HQ.EveHQSettings.DBTimeout = CInt(nudDBTimeout.Value)
    End Sub

    Private Sub cboFormat_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFormat.SelectedIndexChanged
        Select Case cboFormat.SelectedIndex
            Case 0
                gbAccess.Left = 6 : gbAccess.Top = 80 : gbAccess.Width = 500 : gbAccess.Height = 250
                txtMDBServer.Text = EveHQ.Core.HQ.EveHQSettings.DBFilename
                txtMDBUsername.Text = EveHQ.Core.HQ.EveHQSettings.DBUsername
                txtMDBPassword.Text = EveHQ.Core.HQ.EveHQSettings.DBPassword
                gbAccess.Visible = True : gbMSSQL.Visible = False
            Case 1, 2
                gbMSSQL.Left = 6 : gbMSSQL.Top = 80 : gbMSSQL.Width = 500 : gbMSSQL.Height = 250
                txtMSSQLServer.Text = EveHQ.Core.HQ.EveHQSettings.DBServer
                txtMSSQLDatabase.Text = EveHQ.Core.HQ.EveHQSettings.DBName
                txtMSSQLUsername.Text = EveHQ.Core.HQ.EveHQSettings.DBUsername
                txtMSSQLPassword.Text = EveHQ.Core.HQ.EveHQSettings.DBPassword
                If EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = True Then
                    radMSSQLDatabase.Checked = True
                Else
                    radMSSQLWindows.Checked = True
                End If
                gbAccess.Visible = False : gbMSSQL.Visible = True
        End Select
        EveHQ.Core.HQ.EveHQSettings.DBFormat = cboFormat.SelectedIndex
        Call EveHQ.Core.DataFunctions.SetEveHQConnectionString()
        Call EveHQ.Core.DataFunctions.SetEveHQDataConnectionString()
    End Sub

    Private Sub btnBrowseMDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseMDB.Click
        With ofd1
            .Title = "Select SQL CE Data file"
            .FileName = ""
            .InitialDirectory = EveHQ.Core.HQ.appFolder
            .Filter = "SQL Data files (*.sdf)|*.sdf|All files (*.*)|*.*"
            .FilterIndex = 1
            .RestoreDirectory = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                txtMDBServer.Text = .FileName
            End If
        End With
    End Sub

    Private Sub txtMDBUser_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMDBUsername.TextChanged
        EveHQ.Core.HQ.EveHQSettings.DBUsername = txtMDBUsername.Text
    End Sub

    Private Sub txtMDBPassword_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMDBPassword.TextChanged
        EveHQ.Core.HQ.EveHQSettings.DBPassword = txtMDBPassword.Text
    End Sub

    Private Sub txtMSSQLServer_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMSSQLServer.TextChanged
        EveHQ.Core.HQ.EveHQSettings.DBServer = txtMSSQLServer.Text
    End Sub

    Private Sub txtMSSQLUser_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMSSQLUsername.TextChanged
        EveHQ.Core.HQ.EveHQSettings.DBUsername = txtMSSQLUsername.Text
    End Sub

    Private Sub txtMSSQLPassword_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMSSQLPassword.TextChanged
        EveHQ.Core.HQ.EveHQSettings.DBPassword = txtMSSQLPassword.Text
    End Sub

    Private Sub radMSSQLWindows_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radMSSQLWindows.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = False
        Me.lblMSSQLUser.Visible = False
        Me.lblMSSQLPassword.Visible = False
        Me.txtMSSQLUsername.Visible = False
        Me.txtMSSQLPassword.Visible = False
    End Sub

    Private Sub radMSSQLDatabase_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radMSSQLDatabase.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = True
        Me.lblMSSQLUser.Visible = True
        Me.lblMSSQLPassword.Visible = True
        Me.txtMSSQLUsername.Visible = True
        Me.txtMSSQLPassword.Visible = True
    End Sub

    Private Sub txtMDBServer_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMDBServer.TextChanged
        EveHQ.Core.HQ.EveHQSettings.DBFilename = txtMDBServer.Text
    End Sub

    Private Sub txtMSSQLDatabase_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMSSQLDatabase.TextChanged
        EveHQ.Core.HQ.EveHQSettings.DBName = txtMSSQLDatabase.Text
    End Sub

    Private Sub btnTestDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTestDB.Click
        Call EveHQ.Core.DataFunctions.CheckDatabaseConnection(False)
    End Sub

    Private Sub chkUseAppDirForDB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseAppDirForDB.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.UseAppDirectoryForDB = Me.chkUseAppDirForDB.Checked
    End Sub

#End Region

#Region "Proxy Server Options"

    Private Sub UpdateProxyOptions()
        chkUseProxy.Checked = EveHQ.Core.HQ.EveHQSettings.ProxyRequired
        txtProxyUsername.Text = EveHQ.Core.HQ.EveHQSettings.ProxyUsername
        txtProxyPassword.Text = EveHQ.Core.HQ.EveHQSettings.ProxyPassword
        txtProxyServer.Text = EveHQ.Core.HQ.EveHQSettings.ProxyServer
        If EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault = True Then
            radUseDefaultCreds.Checked = True
        Else
            radUseSpecifiedCreds.Checked = True
        End If
        chkProxyUseBasic.Checked = EveHQ.Core.HQ.EveHQSettings.ProxyUseBasic
    End Sub
    Private Sub chkUseProxy_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseProxy.CheckedChanged
        If chkUseProxy.Checked = True Then
            gbProxyServerInfo.Visible = True
            EveHQ.Core.HQ.EveHQSettings.ProxyRequired = True
        Else
            gbProxyServerInfo.Visible = False
            EveHQ.Core.HQ.EveHQSettings.ProxyRequired = False
        End If
    End Sub
    Private Sub chkProxyUseBasic_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkProxyUseBasic.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.ProxyUseBasic = chkProxyUseBasic.Checked
    End Sub
    Private Sub radUseDefaultCreds_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radUseDefaultCreds.CheckedChanged
        If radUseDefaultCreds.Checked = True Then
            lblProxyUsername.Enabled = False
            lblProxyPassword.Enabled = False
            txtProxyUsername.Enabled = False
            txtProxyPassword.Enabled = False
            chkProxyUseBasic.Enabled = False
            If startup = False Then
                EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault = True
            End If
        End If
    End Sub
    Private Sub radUseSpecifiedCreds_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radUseSpecifiedCreds.CheckedChanged
        If radUseSpecifiedCreds.Checked = True Then
            lblProxyUsername.Enabled = True
            lblProxyPassword.Enabled = True
            txtProxyUsername.Enabled = True
            txtProxyPassword.Enabled = True
            chkProxyUseBasic.Enabled = True
            If startup = False Then
                EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault = False
            End If
        End If
    End Sub
    Private Sub txtProxyServer_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProxyServer.TextChanged
        EveHQ.Core.HQ.EveHQSettings.ProxyServer = txtProxyServer.Text
    End Sub
    Private Sub txtProxyUsername_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProxyUsername.TextChanged
        EveHQ.Core.HQ.EveHQSettings.ProxyUsername = txtProxyUsername.Text
    End Sub
    Private Sub txtProxyPassword_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtProxyPassword.TextChanged
        EveHQ.Core.HQ.EveHQSettings.ProxyPassword = txtProxyPassword.Text
    End Sub
#End Region

#Region "EveAPI & Server Settings"

    Private Sub UpdateEveServerSettings()
        chkEnableEveStatus.Checked = EveHQ.Core.HQ.EveHQSettings.EnableEveStatus
        chkAutoAPI.Checked = EveHQ.Core.HQ.EveHQSettings.AutoAPI
        chkAutoMailAPI.Checked = EveHQ.Core.HQ.EveHQSettings.AutoMailAPI
        txtCCPAPIServer.Text = EveHQ.Core.HQ.EveHQSettings.CCPAPIServerAddress
        txtAPIRSServer.Text = EveHQ.Core.HQ.EveHQSettings.APIRSAddress
        chkUseAPIRSServer.Checked = EveHQ.Core.HQ.EveHQSettings.UseAPIRS
        chkUseCCPBackup.Checked = EveHQ.Core.HQ.EveHQSettings.UseCCPAPIBackup
        If EveHQ.Core.HQ.EveHQSettings.UseAPIRS = False Then
            chkUseCCPBackup.Enabled = False
            txtAPIRSServer.Enabled = False
        Else
            chkUseCCPBackup.Enabled = True
            txtAPIRSServer.Enabled = True
        End If
        txtAPIFileExtension.Text = EveHQ.Core.HQ.EveHQSettings.APIFileExtension
        trackServerOffset.Value = EveHQ.Core.HQ.EveHQSettings.ServerOffset
        Dim offset As String = EveHQ.Core.SkillFunctions.TimeToStringAll(EveHQ.Core.HQ.EveHQSettings.ServerOffset)
        lblCurrentOffset.Text = "Current Offset: " & offset
    End Sub
    Private Sub trackServerOffset_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles trackServerOffset.ValueChanged
        EveHQ.Core.HQ.EveHQSettings.ServerOffset = trackServerOffset.Value
        For Each newPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            newPilot.TrainingEndTime = newPilot.TrainingEndTimeActual.AddSeconds(EveHQ.Core.HQ.EveHQSettings.ServerOffset)
            newPilot.TrainingStartTime = newPilot.TrainingStartTimeActual.AddSeconds(EveHQ.Core.HQ.EveHQSettings.ServerOffset)
        Next
        Dim offset As String = EveHQ.Core.SkillFunctions.TimeToStringAll(trackServerOffset.Value)
        lblCurrentOffset.Text = "Current Offset: " & offset
    End Sub
    Private Sub chkEnableEveStatus_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnableEveStatus.CheckedChanged
        If chkEnableEveStatus.Checked = True Then
            frmEveHQ.lblTQStatus.Text = "Tranquility Status: Unknown"
            EveHQ.Core.HQ.EveHQSettings.EnableEveStatus = True
            frmEveHQ.tmrEve.Interval = 100
            frmEveHQ.tmrEve.Enabled = True
        Else
            EveHQ.Core.HQ.EveHQSettings.EnableEveStatus = False
            frmEveHQ.EveStatusIcon.Icon = My.Resources.EveHQ
            frmEveHQ.EveStatusIcon.Text = "EveHQ"
            frmEveHQ.tmrEve.Enabled = False
            frmEveHQ.lblTQStatus.Text = "Tranquility Status: Updates Disabled"
        End If
    End Sub
    Private Sub chkAutoAPI_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoAPI.CheckedChanged
        If chkAutoAPI.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.AutoAPI = True
            EveHQ.Core.HQ.NextAutoAPITime = Now.AddMinutes(60)
        Else
            EveHQ.Core.HQ.EveHQSettings.AutoAPI = False
        End If
    End Sub
    Private Sub chkAutoMailAPI_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoMailAPI.CheckedChanged
        If chkAutoMailAPI.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.AutoMailAPI = True
        Else
            EveHQ.Core.HQ.EveHQSettings.AutoMailAPI = False
        End If
    End Sub
    Private Sub chkUseAPIRSServer_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseAPIRSServer.CheckedChanged
        If chkUseAPIRSServer.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.UseAPIRS = True
            chkUseCCPBackup.Enabled = True
            txtAPIRSServer.Enabled = True
        Else
            EveHQ.Core.HQ.EveHQSettings.UseAPIRS = False
            chkUseCCPBackup.Enabled = False
            txtAPIRSServer.Enabled = False
        End If
    End Sub
    Private Sub chkUseCCPBackup_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseCCPBackup.CheckedChanged
        If chkUseCCPBackup.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.UseCCPAPIBackup = True
        Else
            EveHQ.Core.HQ.EveHQSettings.UseCCPAPIBackup = False
        End If
    End Sub
    Private Sub txtCCPAPIServer_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCCPAPIServer.TextChanged
        EveHQ.Core.HQ.EveHQSettings.CCPAPIServerAddress = txtCCPAPIServer.Text
    End Sub
    Private Sub txtAPIRSServer_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAPIRSServer.TextChanged
        EveHQ.Core.HQ.EveHQSettings.APIRSAddress = txtAPIRSServer.Text
    End Sub
    Private Sub txtAPIFileExtension_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAPIFileExtension.TextChanged
        EveHQ.Core.HQ.EveHQSettings.APIFileExtension = txtAPIFileExtension.Text
    End Sub
#End Region

#Region "Notification Options"
    Public Sub UpdateNotificationOptions()
        Me.chkShutdownNotify.Checked = EveHQ.Core.HQ.EveHQSettings.ShutdownNotify
        Me.chkNotifyToolTip.Checked = EveHQ.Core.HQ.EveHQSettings.NotifyToolTip
        Me.chkNotifyDialog.Checked = EveHQ.Core.HQ.EveHQSettings.NotifyDialog
        Me.chkNotifyNow.Checked = EveHQ.Core.HQ.EveHQSettings.NotifyNow
        Me.chkNotifyEarly.Checked = EveHQ.Core.HQ.EveHQSettings.NotifyEarly
        Me.chkNotifyEmail.Checked = EveHQ.Core.HQ.EveHQSettings.NotifyEMail
        Me.chkNotifyEveMail.Checked = EveHQ.Core.HQ.EveHQSettings.NotifyEveMail
        Me.chkNotifyNotification.Checked = EveHQ.Core.HQ.EveHQSettings.NotifyEveNotification
        If EveHQ.Core.HQ.EveHQSettings.NotifySound = True Then
            Me.chkNotifySound.Checked = True
            btnSelectSoundFile.Enabled = True
            btnSoundTest.Enabled = True
        Else
            Me.chkNotifySound.Checked = False
            btnSelectSoundFile.Enabled = False
            btnSoundTest.Enabled = False
        End If
        Me.lblSoundFile.Text = EveHQ.Core.HQ.EveHQSettings.NotifySoundFile
        If EveHQ.Core.HQ.EveHQSettings.UseSMTPAuth = True Then
            Me.chkSMTPAuthentication.Checked = True
            lblEmailUsername.Enabled = True : lblEmailPassword.Enabled = True
            txtEmailUsername.Enabled = True : txtEmailPassword.Enabled = True
        Else
            Me.chkSMTPAuthentication.Checked = False
            lblEmailUsername.Enabled = False : lblEmailPassword.Enabled = False
            txtEmailUsername.Enabled = False : txtEmailPassword.Enabled = False
        End If
        Me.chkUseSSL.Checked = EveHQ.Core.HQ.EveHQSettings.UseSSL
        Me.txtSMTPServer.Text = EveHQ.Core.HQ.EveHQSettings.EMailServer
        Me.txtSMTPPort.Text = CStr(EveHQ.Core.HQ.EveHQSettings.EMailPort)
        Me.txtEmailAddress.Text = EveHQ.Core.HQ.EveHQSettings.EMailAddress
        Me.txtEmailUsername.Text = EveHQ.Core.HQ.EveHQSettings.EMailUsername
        Me.txtEmailPassword.Text = EveHQ.Core.HQ.EveHQSettings.EMailPassword
        Me.txtSenderAddress.Text = EveHQ.Core.HQ.EveHQSettings.EmailSenderAddress
        Me.sldNotifyOffset.Value = EveHQ.Core.HQ.EveHQSettings.NotifyOffset
        Dim offset As String = EveHQ.Core.SkillFunctions.TimeToStringAll(sldNotifyOffset.Value)
        lblNotifyOffset.Text = "Early Notification Offset: " & offset
        Me.nudShutdownNotifyPeriod.Value = EveHQ.Core.HQ.EveHQSettings.ShutdownNotifyPeriod
        Me.chkIgnoreLastMessage.Checked = EveHQ.Core.HQ.EveHQSettings.IgnoreLastMessage
        Me.chkNotifyInsuffClone.Checked = EveHQ.Core.HQ.EveHQSettings.NotifyInsuffClone
        Me.chkNotifyAccountTime.Checked = EveHQ.Core.HQ.EveHQSettings.NotifyAccountTime
        Me.nudAccountTimeLimit.Enabled = EveHQ.Core.HQ.EveHQSettings.NotifyAccountTime
        Me.nudAccountTimeLimit.Value = EveHQ.Core.HQ.EveHQSettings.AccountTimeLimit
    End Sub

    Private Sub chkShutdownNotify_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShutdownNotify.CheckedChanged
        If chkShutdownNotify.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.ShutdownNotify = True
        Else
            EveHQ.Core.HQ.EveHQSettings.ShutdownNotify = False
        End If
    End Sub

    Private Sub nudShutdownNotifyPeriod_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudShutdownNotifyPeriod.Click
        EveHQ.Core.HQ.EveHQSettings.ShutdownNotifyPeriod = CInt(nudShutdownNotifyPeriod.Value)
    End Sub

    Private Sub nudShutdownNotifyPeriod_HandleDestroyed(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudShutdownNotifyPeriod.HandleDestroyed
        EveHQ.Core.HQ.EveHQSettings.ShutdownNotifyPeriod = CInt(nudShutdownNotifyPeriod.Value)
    End Sub

    Private Sub nudShutdownNotifyPeriod_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles nudShutdownNotifyPeriod.KeyUp
        EveHQ.Core.HQ.EveHQSettings.ShutdownNotifyPeriod = CInt(nudShutdownNotifyPeriod.Value)
    End Sub

    Private Sub chkNotifyNow_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotifyNow.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.NotifyNow = chkNotifyNow.Checked
    End Sub

    Private Sub chkNotifyEarly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotifyEarly.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.NotifyEarly = chkNotifyEarly.Checked
    End Sub

    Private Sub chkNotifyToolTip_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotifyToolTip.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.NotifyToolTip = chkNotifyToolTip.Checked
    End Sub

    Private Sub chkNotifyDialog_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotifyDialog.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.NotifyDialog = chkNotifyDialog.Checked
    End Sub

    Private Sub chkNotifyEmail_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotifyEmail.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.NotifyEMail = chkNotifyEmail.Checked
    End Sub

    Private Sub chkNotifySound_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotifySound.CheckedChanged
        If chkNotifySound.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.NotifySound = True
            btnSelectSoundFile.Enabled = True
            btnSoundTest.Enabled = True
        Else
            EveHQ.Core.HQ.EveHQSettings.NotifySound = False
            btnSelectSoundFile.Enabled = False
            btnSoundTest.Enabled = False
        End If
    End Sub

    Private Sub chkNotifyEveMail_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotifyEveMail.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.NotifyEveMail = chkNotifyEveMail.Checked
    End Sub

    Private Sub chkNotifyNotification_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotifyNotification.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.NotifyEveNotification = chkNotifyNotification.Checked
    End Sub

    Private Sub sldNotifyOffset_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sldNotifyOffset.ValueChanged
        EveHQ.Core.HQ.EveHQSettings.NotifyOffset = sldNotifyOffset.Value
        Dim offset As String = EveHQ.Core.SkillFunctions.TimeToStringAll(sldNotifyOffset.Value)
        lblNotifyOffset.Text = "Early Notification Offset: " & offset
    End Sub

    Private Sub chkSMTPAuthentication_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSMTPAuthentication.CheckedChanged
        If chkSMTPAuthentication.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.UseSMTPAuth = True
            lblEmailUsername.Enabled = True : lblEmailPassword.Enabled = True
            txtEmailUsername.Enabled = True : txtEmailPassword.Enabled = True
        Else
            EveHQ.Core.HQ.EveHQSettings.UseSMTPAuth = False
            lblEmailUsername.Enabled = False : lblEmailPassword.Enabled = False
            txtEmailUsername.Enabled = False : txtEmailPassword.Enabled = False
        End If
    End Sub

    Private Sub chkUseSSL_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseSSL.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.UseSSL = chkUseSSL.Checked
    End Sub

    Private Sub txtSMTPServer_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSMTPServer.TextChanged
        EveHQ.Core.HQ.EveHQSettings.EMailServer = txtSMTPServer.Text
    End Sub

    Private Sub txtEmailAddress_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEmailAddress.TextChanged
        EveHQ.Core.HQ.EveHQSettings.EMailAddress = txtEmailAddress.Text
    End Sub

    Private Sub txtEmailUsername_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEmailUsername.TextChanged
        EveHQ.Core.HQ.EveHQSettings.EMailUsername = txtEmailUsername.Text
    End Sub

    Private Sub txtEmailPassword_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEmailPassword.TextChanged
        EveHQ.Core.HQ.EveHQSettings.EMailPassword = txtEmailPassword.Text
    End Sub

    Private Sub txtSenderAddress_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSenderAddress.TextChanged
        EveHQ.Core.HQ.EveHQSettings.EmailSenderAddress = txtSenderAddress.Text
    End Sub

    Private Sub btnTestEmail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTestEmail.Click

        ' Only do this if at least one notification is enabled
        Dim notifyText As String = ""
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            If cPilot.Active = True And cPilot.Training = True Then
                notifyText = ""
                Dim trainingTime As Long = EveHQ.Core.SkillFunctions.CalcCurrentSkillTime(cPilot)
                Dim strTime As String = EveHQ.Core.SkillFunctions.TimeToString(cPilot.TrainingCurrentTime)
                strTime = strTime.Replace("s", " seconds").Replace("m", " minutes")
                notifyText &= cPilot.Name & " has approximately " & strTime & " before training of " & cPilot.TrainingSkillName & " to Level " & cPilot.TrainingSkillLevel & " completes." & ControlChars.CrLf
                cPilot.TrainingNotifiedEarly = True : cPilot.TrainingNotifiedNow = False
                ' Show the notifications
                If notifyText <> "" Then
                    ' Expand the details with some additional information
                    If cPilot.QueuedSkills.Count > 0 Then
                        notifyText &= ControlChars.CrLf
                        notifyText &= "Next skill in Eve skill queue: " & EveHQ.Core.SkillFunctions.SkillIDToName(CStr(cPilot.QueuedSkills.Values(0).SkillID)) & " " & EveHQ.Core.SkillFunctions.Roman(cPilot.QueuedSkills.Values(0).Level)
                        notifyText &= ControlChars.CrLf
                    Else
                        notifyText &= ControlChars.CrLf
                        notifyText &= "Next skill in Eve skill queue: No skill queued"
                        notifyText &= ControlChars.CrLf
                    End If
                    If cPilot.TrainingQueues.Count > 0 Then
                        notifyText &= ControlChars.CrLf
                        notifyText &= "EveHQ Skill Queue Info: " & ControlChars.CrLf
                        For Each sq As EveHQ.Core.SkillQueue In cPilot.TrainingQueues.Values
                            Dim nq As ArrayList = EveHQ.Core.SkillQueueFunctions.BuildQueue(cPilot, sq, False, True)
                            If sq.IncCurrentTraining = True Then
                                If nq.Count > 1 Then
                                    For q As Integer = 1 To nq.Count - 1
                                        If CType(nq(q), EveHQ.Core.SortedQueueItem).Done = False Then
                                            notifyText &= sq.Name & ": " & CType(nq(q), EveHQ.Core.SortedQueueItem).Name
                                            notifyText &= " (" & EveHQ.Core.SkillFunctions.Roman(CInt(CType(nq(q), EveHQ.Core.SortedQueueItem).FromLevel))
                                            notifyText &= " to " & EveHQ.Core.SkillFunctions.Roman(CInt(CType(nq(q), EveHQ.Core.SortedQueueItem).FromLevel) + 1) & ")" & ControlChars.CrLf
                                            Exit For
                                        End If
                                    Next
                                End If
                            Else
                                If nq.Count > 0 Then
                                    For q As Integer = 0 To nq.Count - 1
                                        If CType(nq(q), EveHQ.Core.SortedQueueItem).Done = False Then
                                            notifyText &= sq.Name & ": " & CType(nq(q), EveHQ.Core.SortedQueueItem).Name
                                            notifyText &= " (" & EveHQ.Core.SkillFunctions.Roman(CInt(CType(nq(q), EveHQ.Core.SortedQueueItem).FromLevel))
                                            notifyText &= " to " & EveHQ.Core.SkillFunctions.Roman(CInt(CType(nq(q), EveHQ.Core.SortedQueueItem).FromLevel) + 1) & ")" & ControlChars.CrLf
                                            Exit For
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If
                    Dim eveHQMail As New System.Net.Mail.SmtpClient
                    Try
                        eveHQMail.Host = EveHQ.Core.HQ.EveHQSettings.EMailServer
                        eveHQMail.Port = EveHQ.Core.HQ.EveHQSettings.EMailPort
                        eveHQMail.EnableSsl = EveHQ.Core.HQ.EveHQSettings.UseSSL
                        If EveHQ.Core.HQ.EveHQSettings.UseSMTPAuth = True Then
                            Dim newCredentials As New System.Net.NetworkCredential
                            newCredentials.UserName = EveHQ.Core.HQ.EveHQSettings.EMailUsername
                            newCredentials.Password = EveHQ.Core.HQ.EveHQSettings.EMailPassword
                            eveHQMail.Credentials = newCredentials
                        End If
                        Dim recList As String = EveHQ.Core.HQ.EveHQSettings.EMailAddress.Replace(ControlChars.CrLf, "").Replace(" ", "").Replace(";", ",")
                        Dim eveHQMsg As New System.Net.Mail.MailMessage(EveHQ.Core.HQ.EveHQSettings.EmailSenderAddress, recList)
                        eveHQMsg.Subject = "Eve Training Notification: " & cPilot.Name & " (" & cPilot.TrainingSkillName & " " & EveHQ.Core.SkillFunctions.Roman(cPilot.TrainingSkillLevel) & ")"
                        eveHQMsg.Body = notifyText
                        eveHQMail.Send(eveHQMsg)
                        MessageBox.Show("Test message sent successfully. Please check your inbox for confirmation.", "EveHQ Test Email Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        ' Exit after the first mail
                        Exit Sub
                    Catch ex As Exception
                        MessageBox.Show("The mail sending process failed. Please check that the server, address, username and password are correct." & ControlChars.CrLf & ControlChars.CrLf & "The error was: " & ex.Message, "EveHQ Test Email Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        ' Exit after the first mail
                        Exit Sub
                    End Try
                End If
            End If
        Next
    End Sub

    Private Sub btnSelectSoundFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectSoundFile.Click
        With ofd1
            .Title = "Please select a valid .wav file"
            .FileName = ""
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            .Filter = "Wave files (*.wav)|*.wav|All files (*.*)|*.*"
            .FilterIndex = 1
            .RestoreDirectory = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                lblSoundFile.Text = .FileName
            End If
            EveHQ.Core.HQ.EveHQSettings.NotifySoundFile = .FileName
        End With
    End Sub

    Private Sub btnSoundTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSoundTest.Click
        Try
            My.Computer.Audio.Play(EveHQ.Core.HQ.EveHQSettings.NotifySoundFile, AudioPlayMode.Background)
        Catch ex As Exception
            MessageBox.Show("Unable to play sound file." & ControlChars.CrLf & "Error: " & ex.Message, "Error with Wave File", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub txtSMTPPort_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSMTPPort.TextChanged
        If IsNumeric(txtSMTPPort.Text) = True Then
            EveHQ.Core.HQ.EveHQSettings.EMailPort = CInt(txtSMTPPort.Text)
        Else
            EveHQ.Core.HQ.EveHQSettings.EMailPort = 0
        End If
    End Sub

    Private Sub chkIgnoreLastMessage_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIgnoreLastMessage.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.IgnoreLastMessage = chkIgnoreLastMessage.Checked
    End Sub

    Private Sub chkNotifyInsuffClone_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotifyInsuffClone.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.NotifyInsuffClone = chkNotifyInsuffClone.Checked
    End Sub

    Private Sub chkNotifyAccountTime_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotifyAccountTime.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.NotifyAccountTime = chkNotifyAccountTime.Checked
        nudAccountTimeLimit.Enabled = chkNotifyAccountTime.Checked
    End Sub

    Private Sub nudAccountTimeLimit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudAccountTimeLimit.Click
        EveHQ.Core.HQ.EveHQSettings.AccountTimeLimit = CInt(nudAccountTimeLimit.Value)
    End Sub

    Private Sub nudAccountTimeLimit_HandleDestroyed(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudAccountTimeLimit.HandleDestroyed
        EveHQ.Core.HQ.EveHQSettings.AccountTimeLimit = CInt(nudAccountTimeLimit.Value)
    End Sub

    Private Sub nudAccountTimeLimit_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles nudAccountTimeLimit.KeyUp
        EveHQ.Core.HQ.EveHQSettings.AccountTimeLimit = CInt(nudAccountTimeLimit.Value)
    End Sub

#End Region

#Region "G15 Routines"

    Private Sub UpdateG15Options()
        If EveHQ.Core.HQ.EveHQSettings.ActivateG15 = True Then
            chkActivateG15.Checked = True
        Else
            chkActivateG15.Checked = False
        End If
        If EveHQ.Core.HQ.EveHQSettings.CycleG15Pilots = True Then
            chkCyclePilots.Checked = True
        Else
            chkCyclePilots.Checked = False
        End If
        nudCycleTime.Value = EveHQ.Core.HQ.EveHQSettings.CycleG15Time
    End Sub
    Private Sub chkActivateG15_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkActivateG15.CheckedChanged
        If chkActivateG15.Checked = True Then
            If EveHQ.Core.HQ.EveHQSettings.ActivateG15 = False Then
                EveHQ.Core.HQ.EveHQSettings.ActivateG15 = True
                'Init the LCD
                Try
                    Core.G15LCDv2.InitLCD()
                    ' Check if the LCD will cycle chars
                    If EveHQ.Core.HQ.IsG15LCDActive = True And EveHQ.Core.HQ.EveHQSettings.CycleG15Pilots = True Then
                        Core.G15LCDv2.tmrLCDChar.Interval = (1000 * EveHQ.Core.HQ.EveHQSettings.CycleG15Time)
                        Core.G15LCDv2.tmrLCDChar.Enabled = True
                    End If
                Catch ex As Exception
                    MessageBox.Show("Unable to start G15 Display. Please ensure you have the keyboard and drivers correctly installed. The error was: " & ex.Message, "Error Starting G15", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    chkActivateG15.Checked = False
                End Try
            End If
        Else
            If EveHQ.Core.HQ.EveHQSettings.ActivateG15 = True Then
                EveHQ.Core.HQ.EveHQSettings.ActivateG15 = False
                ' Close the LCD
                Try
                    Core.G15LCDv2.CloseLCD()
                Catch ex As Exception
                    MessageBox.Show("Unable to close G15 Display: " & ex.Message, "Error Closing G15", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Try
            End If
        End If
    End Sub
    Private Sub chkCyclePilots_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCyclePilots.CheckedChanged
        If chkCyclePilots.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.CycleG15Pilots = True
            Core.G15LCDv2.tmrLCDChar.Interval = (1000 * EveHQ.Core.HQ.EveHQSettings.CycleG15Time)
            If EveHQ.Core.HQ.EveHQSettings.ActivateG15 = True Then
                Core.G15LCDv2.tmrLCDChar.Enabled = True
            End If
        Else
            EveHQ.Core.HQ.EveHQSettings.CycleG15Pilots = False
            Core.G15LCDv2.tmrLCDChar.Enabled = False
        End If
    End Sub
    Private Sub nudCycleTime_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudCycleTime.ValueChanged
        EveHQ.Core.HQ.EveHQSettings.CycleG15Time = CInt(nudCycleTime.Value)
        If EveHQ.Core.HQ.EveHQSettings.CycleG15Time > 0 Then
            Core.G15LCDv2.tmrLCDChar.Interval = CInt((nudCycleTime.Value * 1000))
        Else
            Core.G15LCDv2.tmrLCDChar.Interval = CInt(1000)
        End If
    End Sub
#End Region

#Region "Eve Folder Options"

    Private Sub UpdateEveFolderOptions()
        Dim lblEveDir As New Label
        Dim chkLUA As New CheckBox
        Dim lblCacheSize As New Label
        Dim gbFolderHost As New GroupBox
        Dim txtFName As New TextBox
        For folder As Integer = 1 To 4
            gbFolderHost = CType(Me.gbEveFolders.Controls("gbLocation" & CStr(folder).Trim), GroupBox)
            lblEveDir = CType(gbFolderHost.Controls("lblEveDir" & CStr(folder).Trim), Label)
            chkLUA = CType(gbFolderHost.Controls("chkLUA" & CStr(folder).Trim), CheckBox)
            txtFName = CType(gbFolderHost.Controls("txtFriendlyName" & CStr(folder).Trim), TextBox)
            lblEveDir.Text = EveHQ.Core.HQ.EveHQSettings.EveFolder(folder)
            If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.EveHQSettings.EveFolder(folder)) = True Then
                chkLUA.Enabled = True
                txtFName.Enabled = True
                txtFName.Text = EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(folder)
            Else
                chkLUA.Enabled = False
                txtFName.Enabled = False
                txtFName.Text = ""
            End If
            If EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(folder) = True Then
                chkLUA.Checked = True
            Else
                chkLUA.Checked = False
                If chkLUA.Enabled = True Then
                    'lblCacheSize.Text &= " (shared)"
                End If
            End If

        Next
    End Sub

    Private Sub btnEveDirClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEveDir1.Click, btnEveDir2.Click, btnEveDir3.Click, btnEveDir4.Click
        Dim btnEveDir As New Button
        btnEveDir = CType(sender, Button)
        Dim folder As Integer = CInt(btnEveDir.Name.Remove(0, 9))
        With fbd1
            .Description = "Please select the folder where the Eve executable is located..."
            .ShowNewFolderButton = False
            .RootFolder = Environment.SpecialFolder.Desktop
            .ShowDialog()
            Dim gbFolderHost As GroupBox = CType(Me.gbEveFolders.Controls("gbLocation" & CStr(folder).Trim), GroupBox)
            Dim lblEveDir As Label = CType(gbFolderHost.Controls("lblEveDir" & CStr(folder).Trim), Label)
            If My.Computer.FileSystem.FileExists(Path.Combine(.SelectedPath, "eve.exe")) = False Then
                MessageBox.Show("This folder does not contain the Eve.exe file.", "Directory Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                lblEveDir.Text = .SelectedPath
                EveHQ.Core.HQ.EveHQSettings.EveFolder(folder) = .SelectedPath
                Dim chkLUA As CheckBox = CType(gbFolderHost.Controls("chkLUA" & CStr(folder).Trim), CheckBox)
                chkLUA.Enabled = True
                Dim txtFName As TextBox = CType(gbFolderHost.Controls("txtFriendlyName" & CStr(folder).Trim), TextBox)
                txtFName.Enabled = True
                Dim lblCacheSize As Label = CType(gbFolderHost.Controls("lblCacheSize" & CStr(folder).Trim), Label)
                If chkLUA.Checked = False Then
                    'lblCacheSize.Text &= " (shared)"
                End If
            End If
        End With
    End Sub

    Private Sub btnClearClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear1.Click, btnClear2.Click, btnClear3.Click, btnClear4.Click
        Dim btnEveDir As New Button
        btnEveDir = CType(sender, Button)
        Dim folder As Integer = CInt(btnEveDir.Name.Remove(0, 8))
        Dim gbFolderHost As GroupBox = CType(Me.gbEveFolders.Controls("gbLocation" & CStr(folder).Trim), GroupBox)
        Dim lblEveDir As Label = CType(gbFolderHost.Controls("lblEveDir" & CStr(folder).Trim), Label)
        lblEveDir.Text = ""
        Dim chkLUA As CheckBox = CType(gbFolderHost.Controls("chkLUA" & CStr(folder).Trim), CheckBox)
        chkLUA.Checked = False : chkLUA.Enabled = False
        Dim lblCacheSize As Label = CType(gbFolderHost.Controls("lblCacheSize" & CStr(folder).Trim), Label)
        lblCacheSize.Text = ""
        EveHQ.Core.HQ.EveHQSettings.EveFolder(folder) = ""
    End Sub

    Private Sub chkLUA_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkLUA1.CheckedChanged, chkLUA2.CheckedChanged, chkLUA3.CheckedChanged, chkLUA4.CheckedChanged
        Dim chkLUA As New CheckBox
        chkLUA = CType(sender, CheckBox)
        Dim folder As Integer = CInt(chkLUA.Name.Remove(0, 6))
        Call CheckLUA(chkLUA, folder)
        Dim gbFolderHost As GroupBox = CType(Me.gbEveFolders.Controls("gbLocation" & CStr(folder).Trim), GroupBox)
        Dim lblCacheSize As Label = CType(gbFolderHost.Controls("lblCacheSize" & CStr(folder).Trim), Label)
        If chkLUA.Checked = False Then
            lblCacheSize.Text &= " (shared)"
        End If
    End Sub

    Private Sub CheckLUA(ByVal chkLUA As CheckBox, ByVal folder As Integer)
        ' If selected, check the program files directory for the settings, otherwise check the user directory
        If chkLUA.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(folder) = True
            ' Check program files
            If startup = False Then
                Dim cacheDir As String = Path.Combine(EveHQ.Core.HQ.EveHQSettings.EveFolder(folder), "cache")
                Dim settingsDir As String = Path.Combine(cacheDir, "settings")
                Dim prefsFile As String = Path.Combine(cacheDir, "prefs.ini")
                Dim browserDir As String = Path.Combine(cacheDir, "browser")
                Dim machoDIR As String = Path.Combine(cacheDir, "machonet")
                If My.Computer.FileSystem.DirectoryExists(cacheDir) = True And My.Computer.FileSystem.FileExists(prefsFile) = True Then
                    MessageBox.Show("Confirmed /LUA:off active on this folder.", "LUA Result", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("Warning: /LUA:off does not appear active on this folder.", "LUA Result", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        Else
            EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(folder) = False
            ' Check the application directory for the user
            If startup = False Then
                Dim EveAppFolder As String = EveHQ.Core.HQ.EveHQSettings.EveFolder(folder)
                EveAppFolder = EveAppFolder.Replace("\", "_").Replace(":", "").Replace(" ", "_").ToLower
                EveAppFolder &= "_tranquility"
                Dim eveDir As String = Path.Combine(Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CCP"), "Eve"), EveAppFolder)
                Dim cacheDir As String = Path.Combine(eveDir, "cache")
                Dim settingsDir As String = Path.Combine(eveDir, "settings")
                Dim prefsFile As String = Path.Combine(settingsDir, "prefs.ini")
                Dim browserDir As String = Path.Combine(cacheDir, "browser")
                Dim machoDIR As String = Path.Combine(cacheDir, "prefs.machonet")
                If My.Computer.FileSystem.DirectoryExists(cacheDir) = True And My.Computer.FileSystem.FileExists(prefsFile) = True Then
                    MessageBox.Show("Confirmed shared settings are active.", "LUA Result", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("No shared settings found.", "LUA Result", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        End If
    End Sub

    Private Sub txtFriendlyName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFriendlyName1.TextChanged, txtFriendlyName2.TextChanged, txtFriendlyName3.TextChanged, txtFriendlyName4.TextChanged
        Dim txtFName As TextBox = CType(sender, TextBox)
        Dim idx As Integer = CInt(txtFName.Name.Substring(txtFName.Name.Length - 1, 1))
        EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(idx) = txtFName.Text
        Dim gbFolderHost As GroupBox = CType(Me.gbEveFolders.Controls("gbLocation" & CStr(idx).Trim), GroupBox)
        If EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(idx) <> "" Then
            gbFolderHost.Text = "Eve Location " & idx & " (" & EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(idx) & ")"
        Else
            gbFolderHost.Text = "Eve Location " & idx
        End If
    End Sub

    'Private Function CheckCacheSize(ByVal folder As Integer) As Long
    '    Dim cacheDir As String = ""
    '    If EveHQ.Core.HQ.EveHQSettings.EveFolderLUA(folder) = True Then
    '        cacheDir = EveHQ.Core.HQ.EveHQSettings.EveFolder(folder) & "\cache"
    '    Else
    '        cacheDir = (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\CCP\EVE\cache").Replace("\\", "\")
    '    End If
    '    If My.Computer.FileSystem.DirectoryExists(cacheDir) = True Then
    '        Dim dirInfo As DirectoryInfo = My.Computer.FileSystem.GetDirectoryInfo(cacheDir)
    '        Dim dirSize As Long = DirectorySize(dirInfo)
    '        Return dirSize
    '    Else
    '        Return 0
    '    End If
    'End Function

    'Public Function DirectorySize(ByVal dirInfo As System.IO.DirectoryInfo) As Long
    '    Dim total As Long = 0
    '    Dim file As System.IO.FileInfo
    '    For Each file In dirInfo.GetFiles()
    '        total += file.Length
    '    Next
    '    Dim dir As System.IO.DirectoryInfo
    '    For Each dir In dirInfo.GetDirectories()
    '        total += DirectorySize(dir)
    '    Next
    '    Return total
    'End Function

#End Region

#Region "Taskbar Icon Options"
    Private Sub UpdateTaskBarIconOptions()
        cboTaskbarIconMode.SelectedIndex = EveHQ.Core.HQ.EveHQSettings.TaskbarIconMode
    End Sub

    Private Sub cboTaskbarIconMode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTaskbarIconMode.SelectedIndexChanged
        EveHQ.Core.HQ.EveHQSettings.TaskbarIconMode = cboTaskbarIconMode.SelectedIndex
        Select Case EveHQ.Core.HQ.EveHQSettings.TaskbarIconMode
            Case 0 ' Simple
                Select Case EveHQ.Core.HQ.myTQServer.Status
                    Case EveHQ.Core.EveServer.ServerStatus.Down
                        frmEveHQ.EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
                    Case EveHQ.Core.EveServer.ServerStatus.Starting
                        frmEveHQ.EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
                    Case EveHQ.Core.EveServer.ServerStatus.Shutting
                        frmEveHQ.EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
                    Case EveHQ.Core.EveServer.ServerStatus.Full
                        frmEveHQ.EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
                    Case EveHQ.Core.EveServer.ServerStatus.ProxyDown
                        frmEveHQ.EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
                    Case EveHQ.Core.EveServer.ServerStatus.Unknown
                        frmEveHQ.EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.StatusText
                    Case EveHQ.Core.EveServer.ServerStatus.Up
                        Dim msg As String = EveHQ.Core.HQ.myTQServer.ServerName & ":" & vbCrLf
                        msg = msg & "Version: " & EveHQ.Core.HQ.myTQServer.Version & vbCrLf
                        msg = msg & "Players: " & EveHQ.Core.HQ.myTQServer.Players
                        If msg.Length > 50 Then
                            frmEveHQ.EveStatusIcon.Text = EveHQ.Core.HQ.myTQServer.ServerName & ":" & vbCrLf & "Server currently initialising"
                        Else
                            frmEveHQ.EveStatusIcon.Text = msg
                        End If
                End Select
            Case 1 ' Enhanced

        End Select
    End Sub

#End Region

#Region "Dashboard Options"
    Private Sub UpdateDashboardOptions()
        ' Update the dashboard colours
        Call Me.UpdateWidgetNames()
        Call Me.UpdateWidgets()
        Call Me.UpdateDBOptions()
    End Sub

    Private Sub UpdateWidgetNames()
        cboWidgets.BeginUpdate()
        cboWidgets.Items.Clear()
        For Each cWidget As String In EveHQ.Core.HQ.Widgets.Keys
            cboWidgets.Items.Add(cWidget)
        Next
        cboWidgets.EndUpdate()
    End Sub

    Private Sub UpdateWidgets()
        lvWidgets.BeginUpdate()
        lvWidgets.Items.Clear()
        For Each config As SortedList(Of String, Object) In EveHQ.Core.HQ.EveHQSettings.DashboardConfiguration
            If config.ContainsKey("ControlConfigInfo") = False Then
                config.Add("ControlConfigInfo", "<Not Configurable>")
            End If
            Try
                Dim newWidgetLVI As New ListViewItem
                newWidgetLVI.Text = CStr(config("ControlName"))
                newWidgetLVI.SubItems.Add(CStr(config("ControlConfigInfo")))
                lvWidgets.Items.Add(newWidgetLVI)
            Catch e As Exception
                ' presumably it's an old Widget, therefore discount it
            End Try
        Next
        lvWidgets.EndUpdate()
    End Sub

    Private Sub UpdateDBOptions()
        chkShowPriceTicker.Checked = EveHQ.Core.HQ.EveHQSettings.DBTicker
        If EveHQ.Core.HQ.EveHQSettings.DBTickerLocation = "" Then
            EveHQ.Core.HQ.EveHQSettings.DBTickerLocation = "Bottom"
        End If
        cboTickerLocation.SelectedItem = EveHQ.Core.HQ.EveHQSettings.DBTickerLocation
    End Sub

    Private Sub btnAddWidget_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddWidget.Click
        ' Check we have a selected widget type
        If cboWidgets.SelectedItem IsNot Nothing Then
            Dim WidgetName As String = cboWidgets.SelectedItem.ToString
            ' Determine the type of control to add
            Dim ClassName As String = EveHQ.Core.HQ.Widgets(WidgetName)
            Dim myType As Type = Reflection.Assembly.GetExecutingAssembly.GetType(ClassName)
            Dim newWidget As Object = Activator.CreateInstance(myType)
            Dim pi As System.Reflection.PropertyInfo = myType.GetProperty("ControlConfigForm")
            Dim myConfigFormName As String = pi.GetValue(newWidget, Nothing).ToString
            If myConfigFormName <> "" Then
                Dim ConfigType As Type = Reflection.Assembly.GetExecutingAssembly.GetType(myConfigFormName)
                Dim ConfigForm As Form = CType(Activator.CreateInstance(ConfigType), Form)
                Dim fi As System.Reflection.PropertyInfo = ConfigForm.GetType().GetProperty("DBWidget")
                fi.SetValue(ConfigForm, newWidget, Nothing)
                ConfigForm.ShowDialog()
                If ConfigForm.DialogResult = Windows.Forms.DialogResult.OK Then
                    ' Save the Widget
                    Dim ci As System.Reflection.PropertyInfo = myType.GetProperty("ControlConfiguration")
                    Dim myConfig As SortedList(Of String, Object) = CType(ci.GetValue(newWidget, Nothing), Global.System.Collections.Generic.SortedList(Of String, Object))
                    EveHQ.Core.HQ.EveHQSettings.DashboardConfiguration.Add(myConfig)
                    Call Me.UpdateWidgets()
                    ' Update the dashboard
                    frmDashboard.UpdateWidgets()
                Else
                    ' Process Aborted
                    MessageBox.Show("Widget configuration aborted - information not saved.", "Addition Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                ConfigForm.Dispose()
            Else
                ' Save the Widget
                Dim ci As System.Reflection.PropertyInfo = myType.GetProperty("ControlConfiguration")
                Dim myConfig As SortedList(Of String, Object) = CType(ci.GetValue(newWidget, Nothing), Global.System.Collections.Generic.SortedList(Of String, Object))
                EveHQ.Core.HQ.EveHQSettings.DashboardConfiguration.Add(myConfig)
                Call Me.UpdateWidgets()
                ' Update the dashboard
                frmDashboard.UpdateWidgets()
            End If
        Else
            ' Need a widget type before proceeding
            MessageBox.Show("Please select a Widget type before proceeding.", "Widget Type Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
    End Sub

    Private Sub btnRemoveWidget_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveWidget.Click
        ' Check for an item selection
        If lvWidgets.SelectedItems.Count > 0 Then
            Dim index As Integer = lvWidgets.SelectedItems(0).Index
            EveHQ.Core.HQ.EveHQSettings.DashboardConfiguration.RemoveAt(index)
            lvWidgets.SelectedItems(0).Remove()
            ' Update the dashboard
            frmDashboard.UpdateWidgets()
        Else
            MessageBox.Show("Please select a Widget to remove before proceeding.", "Widget Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
    End Sub

    Private Sub lvWidgets_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvWidgets.DoubleClick
        ' Edit a widget
        If lvWidgets.SelectedItems.Count > 0 Then
            Dim index As Integer = lvWidgets.SelectedItems(0).Index
            Dim WidgetName As String = lvWidgets.SelectedItems(0).Text
            ' Determine the type of control to add
            Select Case WidgetName
                Case "Pilot Information"
                    Dim newWidget As New DBCPilotInfo
                    newWidget.ControlConfiguration = CType(EveHQ.Core.HQ.EveHQSettings.DashboardConfiguration.Item(index), SortedList(Of String, Object))
                    Dim newWidgetConfig As New DBCPilotInfoConfig
                    newWidgetConfig.DBWidget = newWidget
                    newWidgetConfig.ShowDialog()
                    lvWidgets.Items(index).SubItems(1).Text = "Default Pilot: " & CStr(newWidget.ControlConfiguration("DefaultPilotName"))
                Case "Skill Queue Information"
                    Dim newWidget As New DBCSkillQueueInfo
                    newWidget.ControlConfiguration = CType(EveHQ.Core.HQ.EveHQSettings.DashboardConfiguration.Item(index), SortedList(Of String, Object))
                    Dim newWidgetConfig As New DBCSkillQueueInfoConfig
                    newWidgetConfig.DBWidget = newWidget
                    newWidgetConfig.ShowDialog()
                    If CBool(newWidget.ControlConfiguration("EveQueue")) = True Then
                        lvWidgets.Items(index).SubItems(1).Text = "Default Pilot: " & CStr(newWidget.ControlConfiguration("DefaultPilotName")) & ", Eve Queue"
                    Else
                        lvWidgets.Items(index).SubItems(1).Text = "Default Pilot: " & CStr(newWidget.ControlConfiguration("DefaultPilotName")) & ", EveHQ Queue (" & CStr(newWidget.ControlConfiguration("DefaultQueueName")) & ")"
                    End If
            End Select
            ' Update the dashboard
            frmDashboard.UpdateWidgets()
        End If
    End Sub

    Private Sub chkShowPriceTicker_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowPriceTicker.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.DBTicker = chkShowPriceTicker.Checked
        frmDashboard.Ticker1.Visible = EveHQ.Core.HQ.EveHQSettings.DBTicker
    End Sub

    Private Sub cboTickerLocation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboTickerLocation.SelectedIndexChanged
        EveHQ.Core.HQ.EveHQSettings.DBTickerLocation = cboTickerLocation.SelectedItem.ToString
        Select Case EveHQ.Core.HQ.EveHQSettings.DBTickerLocation
            Case "Top"
                frmDashboard.Ticker1.Dock = DockStyle.Top
            Case "Bottom"
                frmDashboard.Ticker1.Dock = DockStyle.Bottom
        End Select
    End Sub

#End Region

    Private Sub RedrawQueueColumnList()
        ' Setup the listview
        Dim newCol As New ListViewItem
        lvwColumns.BeginUpdate()
        lvwColumns.Items.Clear()
        For Each slot As String In EveHQ.Core.HQ.EveHQSettings.UserQueueColumns
            For Each stdSlot As ListViewItem In EveHQ.Core.HQ.EveHQSettings.StandardQueueColumns
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
        Dim idx As Integer = EveHQ.Core.HQ.EveHQSettings.UserQueueColumns.IndexOf(slotName & "0")
        If idx = -1 Then
            idx = EveHQ.Core.HQ.EveHQSettings.UserQueueColumns.IndexOf(slotName & "1")
        End If
        ' Switch with the one above if the index is not zero
        If idx <> 0 Then
            slotName = CStr(EveHQ.Core.HQ.EveHQSettings.UserQueueColumns(idx - 1))
            EveHQ.Core.HQ.EveHQSettings.UserQueueColumns(idx - 1) = EveHQ.Core.HQ.EveHQSettings.UserQueueColumns(idx)
            EveHQ.Core.HQ.EveHQSettings.UserQueueColumns(idx) = slotName
            ' Redraw the list
            redrawColumns = True
            Call Me.RedrawQueueColumnList()
            redrawColumns = False
            lvwColumns.Items(selName).Selected = True
            lvwColumns.Select()
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
        Dim idx As Integer = EveHQ.Core.HQ.EveHQSettings.UserQueueColumns.IndexOf(slotName & "0")
        If idx = -1 Then
            idx = EveHQ.Core.HQ.EveHQSettings.UserQueueColumns.IndexOf(slotName & "1")
        End If
        ' Switch with the one above if the index is not the last
        If idx <> EveHQ.Core.HQ.EveHQSettings.UserQueueColumns.Count - 1 Then
            slotName = CStr(EveHQ.Core.HQ.EveHQSettings.UserQueueColumns(idx + 1))
            EveHQ.Core.HQ.EveHQSettings.UserQueueColumns(idx + 1) = EveHQ.Core.HQ.EveHQSettings.UserQueueColumns(idx)
            EveHQ.Core.HQ.EveHQSettings.UserQueueColumns(idx) = slotName
            ' Redraw the list
            redrawColumns = True
            Call Me.RedrawQueueColumnList()
            redrawColumns = False
            lvwColumns.Items(selName).Selected = True
            lvwColumns.Select()
        End If
    End Sub
    Private Sub lvwColumns_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwColumns.ItemChecked
        If redrawColumns = False Then
            ' Get the slot name of the ticked item
            Dim slotName As String = e.Item.Name
            ' Find the index in the user column list
            Dim idx As Integer = EveHQ.Core.HQ.EveHQSettings.UserQueueColumns.IndexOf(slotName & "0")
            If idx = -1 Then
                idx = EveHQ.Core.HQ.EveHQSettings.UserQueueColumns.IndexOf(slotName & "1")
            End If
            If e.Item.Checked = False Then
                EveHQ.Core.HQ.EveHQSettings.UserQueueColumns(idx) = slotName & "0"
            Else
                EveHQ.Core.HQ.EveHQSettings.UserQueueColumns(idx) = slotName & "1"
            End If
        End If
    End Sub

    Public Sub FinaliseAPIServerUpdate()
        btnGetData.Enabled = True
        Call Me.UpdatePilots()
    End Sub

    Private Sub clb_IGBAllowedDisplay_ItemCheck(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles clb_IGBAllowedDisplay.ItemCheck
        If startup = False Then
            Dim cbx As String = clb_IGBAllowedDisplay.SelectedItem.ToString()
            Dim chkSt As Boolean

            If (e.NewValue = System.Windows.Forms.CheckState.Checked) Then
                chkSt = True
            Else
                chkSt = False
            End If

            EveHQ.Core.HQ.EveHQSettings.IGBAllowedData(cbx) = chkSt
        End If
    End Sub

End Class
