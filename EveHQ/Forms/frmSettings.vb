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
Imports DevComponents.DotNetBar
Imports EveHQ.Core
Imports DevComponents.AdvTree
Imports System.Net.Mail
Imports System.IO
Imports EveHQ.Market
Imports EveHQ.Market.MarketServices
Imports EveHQ.Common.Extensions
Imports Microsoft.Win32
Imports System.Windows.Forms.VisualStyles
Imports System.Net
Imports System.Reflection

Public Class frmSettings
    Dim redrawColumns As Boolean = False
    Dim startup As Boolean = True
    Public Property DoNotRecalculatePilots As Boolean = False

#Region "Form Opening & Closing"

    Private Sub frmSettings_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        Call SaveMarketSettings()
        Call EveHQSettingsFunctions.SaveSettings()
        If DoNotRecalculatePilots = False Then
            If frmTraining.IsHandleCreated = True And redrawColumns = True Then
                redrawColumns = False
                Call frmTraining.RefreshAllTrainingQueues()
            End If
            Call frmEveHQ.UpdatePilotInfo()
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub frmSettings_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
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
        Call UpdateMarketSettings()

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

    Private Sub tvwSettings_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs) _
        Handles tvwSettings.AfterSelect
        Dim nodeName As String = e.Node.Name
        Dim gbName As String = nodeName.TrimStart("node".ToCharArray)
        gbName = "gb" & gbName

        For Each setControl As Control In Me.panelSettings.Controls
            If _
                setControl.Name = "gpNav" Or setControl.Name = "tvwSettings" Or setControl.Name = "btnClose" Or
                setControl.Name = gbName Then
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

    Private Sub tvwSettings_NodeMouseClick(ByVal sender As Object, ByVal e As TreeNodeMouseClickEventArgs) _
        Handles tvwSettings.NodeMouseClick
        Me.tvwSettings.SelectedNode = e.Node
    End Sub

#End Region

#Region "General Settings"

    Private Sub UpdateGeneralSettings()
        chkAutoHide.Checked = HQ.EveHqSettings.AutoHide
        chkAutoRun.Checked = HQ.EveHqSettings.AutoStart
        chkAutoMinimise.Checked = HQ.EveHqSettings.AutoMinimise
        chkMinimiseOnExit.Checked = HQ.EveHqSettings.MinimiseExit
        chkDisableAutoConnections.Checked = HQ.EveHqSettings.DisableAutoWebConnections
        chkDisableTrainingBar.Checked = HQ.EveHqSettings.DisableTrainingBar
        chkEnableAutomaticSave.Checked = EveHQ.Core.HQ.EveHQSettings.EnableAutomaticSave
        nudAutomaticSaveTime.Enabled = EveHQ.Core.HQ.EveHQSettings.EnableAutomaticSave
        nudAutomaticSaveTime.Value = EveHQ.Core.HQ.EveHqSettings.AutomaticSaveTime
        If cboStartupView.Items.Contains(HQ.EveHqSettings.StartupView) = True Then
            cboStartupView.SelectedItem = HQ.EveHqSettings.StartupView
        Else
            cboStartupView.SelectedIndex = 0
        End If
        txtUpdateLocation.Text = HQ.EveHqSettings.UpdateURL
        txtUpdateLocation.Enabled = False
        chkErrorReporting.Checked = HQ.EveHqSettings.ErrorReportingEnabled
        txtErrorRepName.Text = HQ.EveHqSettings.ErrorReportingName
        txtErrorRepEmail.Text = HQ.EveHqSettings.ErrorReportingEmail
        If HQ.EveHqSettings.MDITabPosition IsNot Nothing Then
            cboMDITabPosition.SelectedItem = HQ.EveHqSettings.MDITabPosition
        Else
            cboMDITabPosition.SelectedItem = "Top"
        End If
    End Sub

    Private Sub chkAutoHide_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkAutoHide.CheckedChanged
        HQ.EveHqSettings.AutoHide = chkAutoHide.Checked
    End Sub

    Private Sub chkAutoRun_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkAutoRun.CheckedChanged
        If chkAutoRun.Checked = True Then
            Dim RegKey As RegistryKey =
                    Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", True)
            If HQ.IsUsingLocalFolders = False Then
                RegKey.SetValue(Application.ProductName, """" & Application.ExecutablePath.ToString & """")
            Else
                RegKey.SetValue(Application.ProductName, """" & Application.ExecutablePath.ToString & """" & " /local")
            End If
            HQ.EveHqSettings.AutoStart = True
        Else
            Dim RegKey As RegistryKey =
                    Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", True)
            RegKey.DeleteValue(Application.ProductName, False)
            HQ.EveHqSettings.AutoStart = False
        End If
    End Sub

    Private Sub chkAutoMinimise_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkAutoMinimise.CheckedChanged
        HQ.EveHqSettings.AutoMinimise = chkAutoMinimise.Checked
    End Sub

    Private Sub chkMinimiseOnExit_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkMinimiseOnExit.CheckedChanged
        HQ.EveHqSettings.MinimiseExit = chkMinimiseOnExit.Checked
    End Sub

    Private Sub chkDisableAutoConnections_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkDisableAutoConnections.CheckedChanged
        HQ.EveHqSettings.DisableAutoWebConnections = chkDisableAutoConnections.Checked
        If HQ.EveHqSettings.DisableAutoWebConnections = True Then
            chkAutoAPI.Checked = False
            chkAutoAPI.Enabled = False
            chkAutoMailAPI.Checked = False
            chkAutoMailAPI.Enabled = False
        Else
            chkAutoAPI.Enabled = True
            chkAutoMailAPI.Enabled = True
        End If
    End Sub

    Private Sub chkDisableTrainingBar_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkDisableTrainingBar.CheckedChanged
        HQ.EveHqSettings.DisableTrainingBar = chkDisableTrainingBar.Checked
        If HQ.EveHqSettings.DisableTrainingBar = False Then
            If HQ.EveHqSettings.TrainingBarDockPosition = eDockSide.None Then
                HQ.EveHqSettings.TrainingBarDockPosition = eDockSide.Bottom
            End If
            frmEveHQ.Bar1.DockSide = CType(HQ.EveHqSettings.TrainingBarDockPosition, eDockSide)
            frmEveHQ.DockContainerItem1.Height = HQ.EveHqSettings.TrainingBarHeight
            frmEveHQ.DockContainerItem1.Width = HQ.EveHqSettings.TrainingBarWidth
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

    Private Sub chkEnableAutomaticSave_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnableAutomaticSave.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.EnableAutomaticSave = chkEnableAutomaticSave.Checked
        nudAutomaticSaveTime.Enabled = chkEnableAutomaticSave.Checked
        If chkEnableAutomaticSave.Checked = True Then
            frmEveHQ.tmrSave.Start()
        Else
            frmEveHQ.tmrSave.Stop()
        End If
    End Sub

    Private Sub nudAutomaticSaveTime_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudAutomaticSaveTime.Click
        EveHQ.Core.HQ.EveHQSettings.AutomaticSaveTime = CInt(nudAutomaticSaveTime.Value)
        frmEveHQ.tmrSave.Interval = CInt(nudAutomaticSaveTime.Value) * 60000
    End Sub

    Private Sub nudAutomaticSaveTime_HandleDestroyed(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudAutomaticSaveTime.HandleDestroyed
        EveHQ.Core.HQ.EveHQSettings.AutomaticSaveTime = CInt(nudAutomaticSaveTime.Value)
        frmEveHQ.tmrSave.Interval = CInt(nudAutomaticSaveTime.Value) * 60000
    End Sub

    Private Sub nudAutomaticSaveTime_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles nudAutomaticSaveTime.KeyUp
        EveHQ.Core.HQ.EveHQSettings.AutomaticSaveTime = CInt(nudAutomaticSaveTime.Value)
        frmEveHQ.tmrSave.Interval = CInt(nudAutomaticSaveTime.Value) * 60000
    End Sub


    Private Sub cboStartupPilot_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles cboStartupPilot.SelectedIndexChanged
        HQ.EveHqSettings.StartupPilot = CStr(cboStartupPilot.SelectedItem)
    End Sub

    Private Sub UpdateViewPilots()
        cboStartupPilot.Items.Clear()
        Dim myPilot As Pilot = New Pilot
        For Each myPilot In HQ.EveHqSettings.Pilots
            If myPilot.Active = True Then
                cboStartupPilot.Items.Add(myPilot.Name)
            End If
        Next
        If cboStartupPilot.Items.Contains(HQ.EveHqSettings.StartupPilot) = False Then
            If cboStartupPilot.Items.Count > 0 Then
                cboStartupPilot.SelectedIndex = 0
            End If
        Else
            cboStartupPilot.SelectedItem = HQ.EveHqSettings.StartupPilot
        End If
    End Sub

    Private Sub lblUpdateLocation_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) _
        Handles lblUpdateLocation.DoubleClick
        txtUpdateLocation.Enabled = True
    End Sub

    Private Sub txtUpdateLocation_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtUpdateLocation.TextChanged
        HQ.EveHqSettings.UpdateURL = txtUpdateLocation.Text
    End Sub

    Private Sub chkErrorReporting_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkErrorReporting.CheckedChanged
        HQ.EveHqSettings.ErrorReportingEnabled = chkErrorReporting.Checked
        If HQ.EveHqSettings.ErrorReportingEnabled = True Then
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

    Private Sub txtErrorRepName_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtErrorRepName.TextChanged
        HQ.EveHqSettings.ErrorReportingName = txtErrorRepName.Text
    End Sub

    Private Sub txtErrorRepEmail_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtErrorRepEmail.TextChanged
        HQ.EveHqSettings.ErrorReportingEmail = txtErrorRepEmail.Text
    End Sub

    Private Sub cboMDITabPosition_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles cboMDITabPosition.SelectedIndexChanged
        HQ.EveHqSettings.MDITabPosition = cboMDITabPosition.SelectedItem.ToString
        Select Case HQ.EveHqSettings.MDITabPosition
            Case "Top"
                frmEveHQ.tabEveHQMDI.Dock = DockStyle.Top
                frmEveHQ.tabEveHQMDI.TabAlignment = eTabStripAlignment.Top
            Case "Bottom"
                frmEveHQ.tabEveHQMDI.Dock = DockStyle.Bottom
                frmEveHQ.tabEveHQMDI.TabAlignment = eTabStripAlignment.Bottom
        End Select
    End Sub

#End Region

#Region "Colour Settings"

    Private Sub UpdateColourOptions()
        ' Update the pilot colours
        Call Me.UpdatePBPilotColours()
        chkDisableVisualStyles.Checked = HQ.EveHqSettings.DisableVisualStyles
        txtCSVSeparator.Text = HQ.EveHqSettings.CSVSeparatorChar
    End Sub

    Private Sub UpdatePBPilotColours()
        pbPilotCurrent.BackColor = Color.FromArgb(CInt(HQ.EveHqSettings.PilotCurrentTrainSkillColor))
        pbPilotLevel5.BackColor = Color.FromArgb(CInt(HQ.EveHqSettings.PilotLevel5SkillColor))
        pbPilotPartial.BackColor = Color.FromArgb(CInt(HQ.EveHqSettings.PilotPartTrainedSkillColor))
        pbPilotStandard.BackColor = Color.FromArgb(CInt(HQ.EveHqSettings.PilotStandardSkillColor))
        pbPilotGroupBG.BackColor = Color.FromArgb(CInt(HQ.EveHqSettings.PilotGroupBackgroundColor))
        pbPilotGroupText.BackColor = Color.FromArgb(CInt(HQ.EveHqSettings.PilotGroupTextColor))
        pbPilotSkillText.BackColor = Color.FromArgb(CInt(HQ.EveHqSettings.PilotSkillTextColor))
        pbPilotSkillHighlight.BackColor = Color.FromArgb(CInt(HQ.EveHqSettings.PilotSkillHighlightColor))
    End Sub

    Private Sub pbPilotColours_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles pbPilotGroupBG.Click, pbPilotGroupText.Click, pbPilotSkillText.Click, pbPilotSkillHighlight.Click,
                pbPilotCurrent.Click, pbPilotLevel5.Click, pbPilotPartial.Click, pbPilotStandard.Click
        Dim thisPB As PictureBox = CType(sender, PictureBox)
        Dim dlgResult As Integer = 0
        With cd1
            .AllowFullOpen = True
            .AnyColor = True
            .FullOpen = True
            Select Case thisPB.Name
                Case "pbPilotCurrent"
                    .Color = Color.FromArgb(CInt(HQ.EveHqSettings.PilotCurrentTrainSkillColor))
                Case "pbPilotLevel5"
                    .Color = Color.FromArgb(CInt(HQ.EveHqSettings.PilotLevel5SkillColor))
                Case "pbPilotPartial"
                    .Color = Color.FromArgb(CInt(HQ.EveHqSettings.PilotPartTrainedSkillColor))
                Case "pbPilotStandard"
                    .Color = Color.FromArgb(CInt(HQ.EveHqSettings.PilotStandardSkillColor))
                Case "pbPilotGroupBG"
                    .Color = Color.FromArgb(CInt(HQ.EveHqSettings.PilotGroupBackgroundColor))
                Case "pbPilotGroupText"
                    .Color = Color.FromArgb(CInt(HQ.EveHqSettings.PilotGroupTextColor))
                Case "pbPilotSkillText"
                    .Color = Color.FromArgb(CInt(HQ.EveHqSettings.PilotSkillTextColor))
                Case "pbPilotSkillHighlight"
                    .Color = Color.FromArgb(CInt(HQ.EveHqSettings.PilotSkillHighlightColor))
            End Select
            dlgResult = .ShowDialog()
        End With
        If dlgResult = DialogResult.Cancel Then
            Exit Sub
        Else
            thisPB.BackColor = cd1.Color
            Select Case thisPB.Name
                Case "pbPilotCurrent"
                    HQ.EveHqSettings.PilotCurrentTrainSkillColor = cd1.Color.ToArgb
                Case "pbPilotLevel5"
                    HQ.EveHqSettings.PilotLevel5SkillColor = cd1.Color.ToArgb
                Case "pbPilotPartial"
                    HQ.EveHqSettings.PilotPartTrainedSkillColor = cd1.Color.ToArgb
                Case "pbPilotStandard"
                    HQ.EveHqSettings.PilotStandardSkillColor = cd1.Color.ToArgb
                Case "pbPilotGroupBG"
                    HQ.EveHqSettings.PilotGroupBackgroundColor = cd1.Color.ToArgb
                Case "pbPilotGroupText"
                    HQ.EveHqSettings.PilotGroupTextColor = cd1.Color.ToArgb
                Case "pbPilotSkillText"
                    HQ.EveHqSettings.PilotSkillTextColor = cd1.Color.ToArgb
                Case "pbPilotSkillHighlight"
                    HQ.EveHqSettings.PilotSkillHighlightColor = cd1.Color.ToArgb
            End Select
            ' Update the colours
            frmPilot.adtSkills.Refresh()
        End If
    End Sub

    Private Sub btnResetPilotColours_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnResetPilotColours.Click
        ' Resets the panel colours to the default values
        HQ.EveHqSettings.PilotCurrentTrainSkillColor = Color.LimeGreen.ToArgb
        HQ.EveHqSettings.PilotLevel5SkillColor = Color.Thistle.ToArgb
        HQ.EveHqSettings.PilotPartTrainedSkillColor = Color.Gold.ToArgb
        HQ.EveHqSettings.PilotStandardSkillColor = Color.White.ToArgb
        HQ.EveHqSettings.PilotGroupBackgroundColor = Color.DimGray.ToArgb
        HQ.EveHqSettings.PilotGroupTextColor = Color.White.ToArgb
        HQ.EveHqSettings.PilotSkillTextColor = Color.Black.ToArgb
        HQ.EveHqSettings.PilotSkillHighlightColor = Color.DodgerBlue.ToArgb
        ' Update the colours
        frmPilot.adtSkills.Refresh()
        ' Update the PBPilot Colours
        Call Me.UpdatePBPilotColours()
    End Sub

    Private Sub chkDisableVisualStyles_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkDisableVisualStyles.CheckedChanged
        HQ.EveHqSettings.DisableVisualStyles = chkDisableVisualStyles.Checked
        If chkDisableVisualStyles.Checked = True Then
            Application.VisualStyleState = VisualStyleState.NoneEnabled
        Else
            Application.VisualStyleState = VisualStyleState.ClientAndNonClientAreasEnabled
        End If
    End Sub

    Private Sub txtCSVSeparator_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtCSVSeparator.TextChanged
        HQ.EveHqSettings.CSVSeparatorChar = txtCSVSeparator.Text
    End Sub

#End Region

#Region "Eve Accounts Settings"

    Private Sub btnAddAccount_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddAccount.Click
        ' Clear the text boxes
        Dim myAccounts As frmModifyEveAccounts = New frmModifyEveAccounts
        With myAccounts
            .Tag = "Add"
            .txtUserIDV2.Text = ""
            .txtUserIDV2.Enabled = True
            .txtAPIKeyV2.Text = ""
            .txtAPIKeyV2.Enabled = True
            .txtAccountNameV2.Text = ""
            .txtAccountNameV2.Enabled = True
            .btnAcceptV2.Text = "OK"
            .Text = "Add New Account"
            .ShowDialog()
        End With
        Me.UpdateAccounts()
    End Sub

    Private Sub btnEditAccount_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEditAccount.Click
        Call EditAccount()
    End Sub

    Private Sub EditAccount()
        ' Check for some selection on the listview
        If adtAccounts.SelectedNodes.Count = 0 Then
            MessageBox.Show("Please select an account to edit!", "Cannot Edit", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation)
            adtAccounts.Select()
        Else
            Dim myAccounts As frmModifyEveAccounts = New frmModifyEveAccounts
            With myAccounts
                ' Load the account details into the text boxes
                Dim selAccount As EveAccount = CType(HQ.EveHqSettings.Accounts(adtAccounts.SelectedNodes(0).Name), 
                                                     EveAccount)
                Select Case selAccount.APIKeySystem
                    Case APIKeySystems.Version2
                        .txtUserIDV2.Text = selAccount.userID
                        .txtUserIDV2.Enabled = False
                        .txtAPIKeyV2.Text = selAccount.APIKey
                        .txtAPIKeyV2.Enabled = True
                        .txtAccountNameV2.Text = selAccount.FriendlyName
                        .txtAccountNameV2.Enabled = True
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

    Private Sub btnDeleteAccount_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeleteAccount.Click
        ' Check for some selection on the listview
        If adtAccounts.SelectedNodes.Count = 0 Then
            MessageBox.Show("Please select an account to delete!", "Cannot Delete Account", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation)
            adtAccounts.Select()
        Else
            Dim selAccount As String = adtAccounts.SelectedNodes(0).Name
            Dim selAccountName As String = adtAccounts.SelectedNodes(0).Text
            ' Get the list of pilots that are affected
            Dim dPilot As Pilot = New Pilot
            Dim strPilots As String = ""
            For Each dPilot In HQ.EveHqSettings.Pilots
                If dPilot.Account = selAccount Then
                    strPilots &= dPilot.Name & ControlChars.CrLf
                End If
            Next
            If strPilots = "" Then strPilots = "<none>"
            ' Confirm deletion
            Dim msg As String = ""
            If strPilots = "<none>" Then
                msg &= "Deleting the '" & selAccountName & "' account will not delete any of your existing pilots." &
                       ControlChars.CrLf & ControlChars.CrLf
            Else
                msg &= "Deleting the '" & selAccountName & "' account will delete the following pilots:" &
                       ControlChars.CrLf & strPilots & ControlChars.CrLf
            End If
            msg &= "Are you sure you wish to delete the account '" & selAccountName & "'?"
            Dim confirm As Integer = MessageBox.Show(msg, "Confirm Delete?", MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Question)
            If confirm = vbYes Then
                ' Delete the account from the accounts collection
                HQ.EveHqSettings.Accounts.Remove(adtAccounts.SelectedNodes(0).Name)
                ' Remove the item from the list
                adtAccounts.Nodes.Remove(adtAccounts.SelectedNodes(0))
                ' Remove the pilots
                For Each dPilot In HQ.EveHqSettings.Pilots
                    If dPilot.Account = selAccount Then
                        HQ.EveHqSettings.Pilots.Remove(dPilot.Name)
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

    Private Sub btnDisableAccount_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDisableAccount.Click
        ' Check for some selection on the listview
        If adtAccounts.SelectedNodes.Count = 0 Then
            MessageBox.Show("Please select an account to toggle the status of!", "Cannot Set Account Status",
                            MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            adtAccounts.Select()
        Else
            Dim SI As Node = adtAccounts.SelectedNodes(0)
            Dim UserID As String = SI.Name
            Dim cAccount As EveAccount = CType(HQ.EveHqSettings.Accounts(UserID), EveAccount)
            Select Case cAccount.APIAccountStatus
                Case APIAccountStatuses.Active, APIAccountStatuses.Disabled
                    cAccount.APIAccountStatus = APIAccountStatuses.ManualDisabled
                    btnDisableAccount.Text = "Enable Account"
                Case APIAccountStatuses.ManualDisabled
                    cAccount.APIAccountStatus = APIAccountStatuses.Active
                    btnDisableAccount.Text = "Disable Account"
            End Select
            SI.Cells(4).Text = cAccount.APIAccountStatus.ToString
        End If
    End Sub

    Private Sub btnGetData_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGetData.Click
        Call frmEveHQ.QueryMyEveServer()
    End Sub

    Public Sub UpdateAccounts()
        adtAccounts.BeginUpdate()
        adtAccounts.Nodes.Clear()
        Dim newAccount As EveAccount = New EveAccount
        For Each newAccount In HQ.EveHqSettings.Accounts
            Dim newLine As New Node
            newLine.Name = newAccount.userID
            newLine.Text = newAccount.FriendlyName
            newLine.Cells.Add(New Cell(newAccount.APIKeySystem.ToString))
            newLine.Cells.Add(New Cell(newAccount.userID))
            newLine.Cells.Add(New Cell(newAccount.APIKeyType.ToString))
            newLine.Cells.Add(New Cell(newAccount.APIAccountStatus.ToString))
            Select Case newAccount.APIKeyType
                Case APIKeyTypes.Unknown, APIKeyTypes.Limited
                    Dim TTT As String = ""
                    TTT &= "Account Creation Date: <Unknown>" & ControlChars.CrLf
                    TTT &= "Account Expiry Date: <Unknown>" & ControlChars.CrLf
                    TTT &= "Logon Count: <Unknown>" & ControlChars.CrLf
                    TTT &= "Time Online: <Unknown>"
                    Dim _
                        STI As _
                            New SuperTooltipInfo("Account Name: " & newAccount.FriendlyName,
                                                 "Eve API Account Information", TTT, Nothing, My.Resources.Info32,
                                                 eTooltipColor.Yellow)
                    STT.SetSuperTooltip(newLine, STI)
                Case APIKeyTypes.Full, APIKeyTypes.Character, APIKeyTypes.Corporation
                    Dim TTT As String = ""
                    TTT &= "Account Creation Date: " & newAccount.CreateDate.ToString & ControlChars.CrLf
                    TTT &= "Account Expiry Date: " & newAccount.PaidUntil.ToString & ControlChars.CrLf
                    TTT &= "Logon Count: " & newAccount.LogonCount.ToString("N0") & ControlChars.CrLf
                    TTT &= "Time Online: " & SkillFunctions.TimeToString(newAccount.LogonMinutes * 60, False)
                    Dim _
                        STI As _
                            New SuperTooltipInfo("Account Name: " & newAccount.FriendlyName,
                                                 "Eve API Account Information", TTT, Nothing, My.Resources.Info32,
                                                 eTooltipColor.Yellow)
                    STT.SetSuperTooltip(newLine, STI)
            End Select
            adtAccounts.Nodes.Add(newLine)
        Next
        adtAccounts.EndUpdate()
        If HQ.EveHqSettings.Accounts.Count = 0 Then
            frmEveHQ.btnQueryAPI.Enabled = False
        Else
            frmEveHQ.btnQueryAPI.Enabled = True
        End If
    End Sub

    Private Sub btnCheckAPIKeys_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCheckAPIKeys.Click
        ' Checks the API keys types by reference to the account status API
        For Each cAccount As EveAccount In HQ.EveHqSettings.Accounts
            Call cAccount.CheckAPIKey()
        Next
        Call Me.UpdateAccounts()
    End Sub

    Private Sub adtAccounts_NodeDoubleClick(sender As Object, e As TreeNodeMouseEventArgs) _
        Handles adtAccounts.NodeDoubleClick
        Call EditAccount()
    End Sub

    Private Sub adtAccounts_SelectionChanged(sender As Object, e As EventArgs) Handles adtAccounts.SelectionChanged
        If adtAccounts.SelectedNodes.Count = 1 Then
            Dim SI As Node = adtAccounts.SelectedNodes(0)
            Dim UserID As String = SI.Name
            Dim cAccount As EveAccount = CType(HQ.EveHqSettings.Accounts(UserID), EveAccount)
            Select Case cAccount.APIAccountStatus
                Case APIAccountStatuses.Active, APIAccountStatuses.Disabled
                    btnDisableAccount.Text = "Disable Account"
                    btnDisableAccount.Enabled = True
                Case APIAccountStatuses.ManualDisabled
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
        For Each newPlugIn As PlugIn In HQ.EveHqSettings.Plugins.Values
            Dim newLine As New ListViewItem
            newLine.Name = newPlugIn.Name
            newLine.Text = newPlugIn.Name & " (v" & newPlugIn.Version & ")"
            If newPlugIn.Disabled = True Then
                newLine.Checked = False
                Dim status As String = ""
                Select Case newPlugIn.Status
                    Case PlugIn.PlugInStatus.Uninitialised
                        status = "Uninitialised"
                    Case PlugIn.PlugInStatus.Loading
                        status = "Loading"
                    Case PlugIn.PlugInStatus.Failed
                        status = "Failed"
                    Case PlugIn.PlugInStatus.Active
                        status = "Active"
                End Select
                newLine.SubItems.Add("Disabled" & " (" & status & ")")
            Else
                newLine.Checked = True
                Dim status As String = ""
                Select Case newPlugIn.Status
                    Case PlugIn.PlugInStatus.Uninitialised
                        status = "Uninitialised"
                    Case PlugIn.PlugInStatus.Loading
                        status = "Loading"
                    Case PlugIn.PlugInStatus.Failed
                        status = "Failed"
                    Case PlugIn.PlugInStatus.Active
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

    Private Sub btnTidyPlugins_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTidyPlugins.Click
        Dim removePlugIns As New ArrayList
        For Each newPlugIn As PlugIn In HQ.EveHqSettings.Plugins.Values
            If newPlugIn.Available = False Then
                removePlugIns.Add(newPlugIn.Name)
            End If
        Next
        For Each Plugin As String In removePlugIns
            HQ.EveHqSettings.Plugins.Remove(Plugin)
        Next
        Call Me.UpdatePlugIns()
    End Sub

    Private Sub btnRefreshPlugins_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefreshPlugins.Click
        Call Me.UpdatePlugIns()
    End Sub

    Private Sub lvwPlugins_ItemChecked(ByVal sender As Object, ByVal e As ItemCheckedEventArgs) _
        Handles lvwPlugins.ItemChecked
        Dim pluginName As String = e.Item.Name
        Dim plugin As PlugIn = CType(HQ.EveHqSettings.Plugins(pluginName), PlugIn)
        If e.Item.Checked = True Then
            plugin.Disabled = False
        Else
            plugin.Disabled = True
        End If
    End Sub

#End Region

#Region "Eve Pilots Settings"

    Private Sub btnAddPilot_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddPilot.Click
        ' Clear the text boxes
        Dim myPilots As frmModifyEvePilots = New frmModifyEvePilots
        With myPilots
            .txtPilotName.Text = ""
            .txtPilotName.Enabled = True
            .txtPilotID.Text = ""
            .txtPilotID.Enabled = True
            .Text = "Add New Pilot"
            .ShowDialog()
        End With
        Me.UpdatePilots()
        Call frmEveHQ.UpdatePilotInfo()
    End Sub

    Private Sub btnAddPilotFromXML_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddPilotFromXML.Click
        Call PilotParseFunctions.LoadPilotFromXML()
        Call frmEveHQ.UpdatePilotInfo()
        Call Me.UpdatePilots()
    End Sub

    Private Sub btnCreateBlankPilot_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnCreateBlankPilot.Click
        Dim newCharForm As New frmCharCreate
        newCharForm.ShowDialog()
        Call Me.UpdatePilots()
        newCharForm.Dispose()
    End Sub

    Public Sub UpdatePilots()
        lvwPilots.Items.Clear()
        Dim newPilot As Pilot = New Pilot
        For Each newPilot In HQ.EveHqSettings.Pilots
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

    Private Sub btnDeletePilot_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeletePilot.Click
        ' Check for some selection on the listview
        If lvwPilots.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select a pilot to delete!", "Cannot Delete Pilot", MessageBoxButtons.OK,
                            MessageBoxIcon.Exclamation)
            lvwPilots.Select()
        Else
            Dim selPilot As String = lvwPilots.SelectedItems(0).Text
            Dim selAccount As String = lvwPilots.SelectedItems(0).SubItems(2).Text
            ' Check if the pilot is linked to an account - and therefore cannot be deleted
            If selAccount <> "" Then
                Dim msg As String = ""
                msg &= "You cannot delete pilot '" & selPilot & "' as it is currently associated with account '" &
                       selAccount & "'."
                MessageBox.Show(msg, "Cannot Delete Pilot", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            ' Confirm deletion
            Dim strMsg As String = "Are you sure you wish to delete pilot '" & selPilot & "'?"
            Dim confirm As Integer = MessageBox.Show(strMsg, "Confirm Pilot Delete?", MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Question)
            If confirm = vbYes Then
                ' Delete the account from the accounts collection
                HQ.EveHqSettings.Pilots.Remove(selPilot)
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

    Private Sub lvwPilots_ColumnClick(ByVal sender As Object, ByVal e As ColumnClickEventArgs) _
        Handles lvwPilots.ColumnClick
        If CInt(lvwPilots.Tag) = e.Column Then
            Me.lvwPilots.ListViewItemSorter = New ListViewItemComparer_Text(e.Column, SortOrder.Ascending)
            lvwPilots.Tag = -1
        Else
            Me.lvwPilots.ListViewItemSorter = New ListViewItemComparer_Text(e.Column, SortOrder.Descending)
            lvwPilots.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lvwPilots.Sort()
    End Sub

    Private Sub lvwPilots_ItemCheck(ByVal sender As Object, ByVal e As ItemCheckEventArgs) Handles lvwPilots.ItemCheck
        Dim pilotIndex As Integer = e.Index
        Dim newpilot As New Pilot
        newpilot = CType(HQ.EveHqSettings.Pilots(lvwPilots.Items(pilotIndex).Text), Pilot)
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

        nudIGBPort.Value = HQ.EveHqSettings.IGBPort
        chkStartIGBonLoad.Checked = HQ.EveHqSettings.IGBAutoStart
        Call IGB.CheckAllIGBAccessRights()

        If HQ.EveHqSettings.IGBFullMode = False Then
            rb_IGBCfgAccessMode.Checked = True
        Else
            rb_IGBFullAccessMode.Checked = True
        End If

        ' Cycle plug-ins
        For Each PlugInInfo As PlugIn In HQ.EveHqSettings.Plugins.Values
            If PlugInInfo.RunInIGB = True Then
                ' If the Plug-In is not part of the List, then add it, default to enabled
                If Not HQ.EveHqSettings.IGBAllowedData.ContainsKey(PlugInInfo.Name) Then
                    HQ.EveHqSettings.IGBAllowedData.Add(PlugInInfo.Name, True)
                End If
            End If
        Next

        clb_IGBAllowedDisplay.Items.Clear()
        For Each allowed As String In HQ.EveHqSettings.IGBAllowedData.Keys
            sp = HQ.EveHqSettings.IGBAllowedData(allowed)
            clb_IGBAllowedDisplay.Items.Add(allowed, sp)
        Next
    End Sub

    Private Sub nudIGBPort_Click(ByVal sender As Object, ByVal e As EventArgs) Handles nudIGBPort.Click
        HQ.EveHqSettings.IGBPort = CInt(nudIGBPort.Value)
    End Sub

    Private Sub nudIGBPort_HandleDestroyed(ByVal sender As Object, ByVal e As EventArgs) _
        Handles nudIGBPort.HandleDestroyed
        HQ.EveHqSettings.IGBPort = CInt(nudIGBPort.Value)
    End Sub

    Private Sub nudIGBPort_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) Handles nudIGBPort.KeyUp
        If e.KeyCode = Keys.Enter Then
            HQ.EveHqSettings.IGBPort = CInt(nudIGBPort.Value)
        End If
    End Sub

    Private Sub chkStartIGBOnLoad_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkStartIGBonLoad.CheckedChanged
        If chkStartIGBonLoad.Checked = True Then
            HQ.EveHqSettings.IGBAutoStart = True
        Else
            HQ.EveHqSettings.IGBAutoStart = False
        End If
    End Sub

    Private Sub rb_IGBInPublicMode_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles rb_IGBCfgAccessMode.CheckedChanged, rb_IGBFullAccessMode.CheckedChanged
        If startup = False Then
            If rb_IGBCfgAccessMode.Checked = True Then
                HQ.EveHqSettings.IGBFullMode = False
            Else
                HQ.EveHqSettings.IGBFullMode = True
            End If
        End If
    End Sub

#End Region

#Region "Training Queue Options"

    Private Sub clbColumns_ItemCheck(ByVal sender As Object, ByVal e As ItemCheckEventArgs)
        If e.Index > 5 Then
            If e.CurrentValue = CheckState.Checked Then
                HQ.EveHqSettings.QColumns(e.Index, 1) = CStr(False)
            Else
                HQ.EveHqSettings.QColumns(e.Index, 1) = CStr(True)
            End If
        Else
            e.NewValue = CheckState.Checked
        End If
        If startup = False Then
            redrawColumns = True
        End If
    End Sub

    Private Sub chkShowCompletedSkills_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkShowCompletedSkills.CheckedChanged
        HQ.EveHqSettings.ShowCompletedSkills = chkShowCompletedSkills.Checked
    End Sub

    Private Sub chkDeleteCompletedSkills_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkDeleteCompletedSkills.CheckedChanged
        HQ.EveHqSettings.DeleteSkills = chkDeleteCompletedSkills.Checked
    End Sub

    Private Sub chkStartWithPrimaryQueue_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkStartWithPrimaryQueue.CheckedChanged
        HQ.EveHqSettings.StartWithPrimaryQueue = chkStartWithPrimaryQueue.Checked
    End Sub

    Private Sub UpdateTrainingQueueOptions()
        ' Add the Queue columns
        Call Me.RedrawQueueColumnList()
        Me.chkDeleteCompletedSkills.Checked = HQ.EveHqSettings.DeleteSkills
        Me.chkShowCompletedSkills.Checked = HQ.EveHqSettings.ShowCompletedSkills
        Me.chkStartWithPrimaryQueue.Checked = HQ.EveHqSettings.StartWithPrimaryQueue
        Dim IColor As Color = Color.FromArgb(CInt(HQ.EveHqSettings.IsPreReqColor))
        Me.pbIsPreReqColour.BackColor = IColor
        Dim HColor As Color = Color.FromArgb(CInt(HQ.EveHqSettings.HasPreReqColor))
        Me.pbHasPreReqColour.BackColor = HColor
        Dim BColor As Color = Color.FromArgb(CInt(HQ.EveHqSettings.BothPreReqColor))
        Me.pbBothPreReqColour.BackColor = BColor
        Dim CColor As Color = Color.FromArgb(CInt(HQ.EveHqSettings.DTClashColor))
        Me.pbDowntimeClashColour.BackColor = CColor
        Dim RColor As Color = Color.FromArgb(CInt(HQ.EveHqSettings.ReadySkillColor))
        Me.pbReadySkillColour.BackColor = RColor
        Dim PColor As Color = Color.FromArgb(CInt(HQ.EveHqSettings.PartialTrainColor))
        Me.pbPartiallyTrainedColour.BackColor = PColor
    End Sub

    Private Sub pbIsPreReqColour_Click(ByVal sender As Object, ByVal e As EventArgs) Handles pbIsPreReqColour.Click
        Dim dlgResult As Integer = 0
        With cd1
            .AllowFullOpen = True
            .AnyColor = True
            .FullOpen = True
            dlgResult = .ShowDialog()
        End With
        If dlgResult = DialogResult.Cancel Then
            Exit Sub
        Else
            Me.pbIsPreReqColour.BackColor = cd1.Color
            HQ.EveHqSettings.IsPreReqColor = cd1.Color.ToArgb
            redrawColumns = True
        End If
    End Sub

    Private Sub pbHasPreReqColour_Click(ByVal sender As Object, ByVal e As EventArgs) Handles pbHasPreReqColour.Click
        Dim dlgResult As Integer = 0
        With cd1
            .AllowFullOpen = True
            .AnyColor = True
            .FullOpen = True
            dlgResult = .ShowDialog()
        End With
        If dlgResult = DialogResult.Cancel Then
            Exit Sub
        Else
            Me.pbHasPreReqColour.BackColor = cd1.Color
            HQ.EveHqSettings.HasPreReqColor = cd1.Color.ToArgb
            redrawColumns = True
        End If
    End Sub

    Private Sub pbBothPreReqColour_Click(ByVal sender As Object, ByVal e As EventArgs) Handles pbBothPreReqColour.Click
        Dim dlgResult As Integer = 0
        With cd1
            .AllowFullOpen = True
            .AnyColor = True
            .FullOpen = True
            dlgResult = .ShowDialog()
        End With
        If dlgResult = DialogResult.Cancel Then
            Exit Sub
        Else
            Me.pbBothPreReqColour.BackColor = cd1.Color
            HQ.EveHqSettings.BothPreReqColor = cd1.Color.ToArgb
            redrawColumns = True
        End If
    End Sub

    Private Sub pbDowntimeClashColour_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles pbDowntimeClashColour.Click
        Dim dlgResult As Integer = 0
        With cd1
            .AllowFullOpen = True
            .AnyColor = True
            .FullOpen = True
            dlgResult = .ShowDialog()
        End With
        If dlgResult = DialogResult.Cancel Then
            Exit Sub
        Else
            Me.pbDowntimeClashColour.BackColor = cd1.Color
            HQ.EveHqSettings.DTClashColor = cd1.Color.ToArgb
            redrawColumns = True
        End If
    End Sub

    Private Sub pbReadySkillColour_Click(ByVal sender As Object, ByVal e As EventArgs) Handles pbReadySkillColour.Click
        Dim dlgResult As Integer = 0
        With cd1
            .AllowFullOpen = True
            .AnyColor = True
            .FullOpen = True
            dlgResult = .ShowDialog()
        End With
        If dlgResult = DialogResult.Cancel Then
            Exit Sub
        Else
            Me.pbReadySkillColour.BackColor = cd1.Color
            HQ.EveHqSettings.ReadySkillColor = cd1.Color.ToArgb
            redrawColumns = True
        End If
    End Sub

    Private Sub pbPartiallyTrainedColour_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles pbPartiallyTrainedColour.Click
        Dim dlgResult As Integer = 0
        With cd1
            .AllowFullOpen = True
            .AnyColor = True
            .FullOpen = True
            dlgResult = .ShowDialog()
        End With
        If dlgResult = DialogResult.Cancel Then
            Exit Sub
        Else
            Me.pbPartiallyTrainedColour.BackColor = cd1.Color
            HQ.EveHqSettings.PartialTrainColor = cd1.Color.ToArgb
            redrawColumns = True
        End If
    End Sub

#End Region

#Region "Database Options"

    Private Sub UpdateDatabaseSettings()
        Me.cboFormat.SelectedIndex = HQ.EveHqSettings.DBFormat
        Me.chkUseAppDirForDB.Checked = HQ.EveHqSettings.UseAppDirectoryForDB
        Me.nudDBTimeout.Value = HQ.EveHqSettings.DBTimeout
    End Sub

    Private Sub nudDBTimeout_HandleDestroyed(ByVal sender As Object, ByVal e As EventArgs) _
        Handles nudDBTimeout.HandleDestroyed
        HQ.EveHqSettings.DBTimeout = CInt(nudDBTimeout.Value)
    End Sub

    Private Sub nudDBTimeout_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles nudDBTimeout.ValueChanged
        HQ.EveHqSettings.DBTimeout = CInt(nudDBTimeout.Value)
    End Sub

    Private Sub cboFormat_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles cboFormat.SelectedIndexChanged
        Select Case cboFormat.SelectedIndex
            Case 0
                gbAccess.Left = 6
                gbAccess.Top = 80
                gbAccess.Width = 500
                gbAccess.Height = 250
                txtMDBServer.Text = HQ.EveHqSettings.DBFilename
                txtMDBUsername.Text = HQ.EveHqSettings.DBUsername
                txtMDBPassword.Text = HQ.EveHqSettings.DBPassword
                gbAccess.Visible = True
                gbMSSQL.Visible = False
            Case 1, 2
                gbMSSQL.Left = 6
                gbMSSQL.Top = 80
                gbMSSQL.Width = 500
                gbMSSQL.Height = 250
                txtMSSQLServer.Text = HQ.EveHqSettings.DBServer
                txtMSSQLDatabase.Text = HQ.EveHqSettings.DBName
                txtMSSQLUsername.Text = HQ.EveHqSettings.DBUsername
                txtMSSQLPassword.Text = HQ.EveHqSettings.DBPassword
                If HQ.EveHqSettings.DBSQLSecurity = True Then
                    radMSSQLDatabase.Checked = True
                Else
                    radMSSQLWindows.Checked = True
                End If
                gbAccess.Visible = False
                gbMSSQL.Visible = True
        End Select
        HQ.EveHqSettings.DBFormat = cboFormat.SelectedIndex
        Call DataFunctions.SetEveHQConnectionString()
        Call DataFunctions.SetEveHQDataConnectionString()
    End Sub

    Private Sub btnBrowseMDB_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBrowseMDB.Click
        With ofd1
            .Title = "Select SQL CE Data file"
            .FileName = ""
            .InitialDirectory = HQ.appFolder
            .Filter = "SQL Data files (*.sdf)|*.sdf|All files (*.*)|*.*"
            .FilterIndex = 1
            .RestoreDirectory = True
            If .ShowDialog() = DialogResult.OK Then
                txtMDBServer.Text = .FileName
            End If
        End With
    End Sub

    Private Sub txtMDBUser_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtMDBUsername.TextChanged
        HQ.EveHqSettings.DBUsername = txtMDBUsername.Text
    End Sub

    Private Sub txtMDBPassword_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtMDBPassword.TextChanged
        HQ.EveHqSettings.DBPassword = txtMDBPassword.Text
    End Sub

    Private Sub txtMSSQLServer_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtMSSQLServer.TextChanged
        HQ.EveHqSettings.DBServer = txtMSSQLServer.Text
    End Sub

    Private Sub txtMSSQLUser_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtMSSQLUsername.TextChanged
        HQ.EveHqSettings.DBUsername = txtMSSQLUsername.Text
    End Sub

    Private Sub txtMSSQLPassword_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtMSSQLPassword.TextChanged
        HQ.EveHqSettings.DBPassword = txtMSSQLPassword.Text
    End Sub

    Private Sub radMSSQLWindows_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles radMSSQLWindows.CheckedChanged
        HQ.EveHqSettings.DBSQLSecurity = False
        Me.lblMSSQLUser.Visible = False
        Me.lblMSSQLPassword.Visible = False
        Me.txtMSSQLUsername.Visible = False
        Me.txtMSSQLPassword.Visible = False
    End Sub

    Private Sub radMSSQLDatabase_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles radMSSQLDatabase.CheckedChanged
        HQ.EveHqSettings.DBSQLSecurity = True
        Me.lblMSSQLUser.Visible = True
        Me.lblMSSQLPassword.Visible = True
        Me.txtMSSQLUsername.Visible = True
        Me.txtMSSQLPassword.Visible = True
    End Sub

    Private Sub txtMDBServer_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtMDBServer.TextChanged
        HQ.EveHqSettings.DBFilename = txtMDBServer.Text
    End Sub

    Private Sub txtMSSQLDatabase_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtMSSQLDatabase.TextChanged
        HQ.EveHqSettings.DBName = txtMSSQLDatabase.Text
    End Sub

    Private Sub btnTestDB_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTestDB.Click
        Call DataFunctions.CheckDatabaseConnection(False)
    End Sub

    Private Sub chkUseAppDirForDB_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkUseAppDirForDB.CheckedChanged
        HQ.EveHqSettings.UseAppDirectoryForDB = Me.chkUseAppDirForDB.Checked
    End Sub

#End Region

#Region "Proxy Server Options"

    Private Sub UpdateProxyOptions()
        chkUseProxy.Checked = HQ.EveHqSettings.ProxyRequired
        txtProxyUsername.Text = HQ.EveHqSettings.ProxyUsername
        txtProxyPassword.Text = HQ.EveHqSettings.ProxyPassword
        txtProxyServer.Text = HQ.EveHqSettings.ProxyServer
        If HQ.EveHqSettings.ProxyUseDefault = True Then
            radUseDefaultCreds.Checked = True
        Else
            radUseSpecifiedCreds.Checked = True
        End If
        chkProxyUseBasic.Checked = HQ.EveHqSettings.ProxyUseBasic
    End Sub

    Private Sub chkUseProxy_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkUseProxy.CheckedChanged
        If chkUseProxy.Checked = True Then
            gbProxyServerInfo.Visible = True
            HQ.EveHqSettings.ProxyRequired = True
        Else
            gbProxyServerInfo.Visible = False
            HQ.EveHqSettings.ProxyRequired = False
        End If
    End Sub

    Private Sub chkProxyUseBasic_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkProxyUseBasic.CheckedChanged
        HQ.EveHqSettings.ProxyUseBasic = chkProxyUseBasic.Checked
    End Sub

    Private Sub radUseDefaultCreds_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles radUseDefaultCreds.CheckedChanged
        If radUseDefaultCreds.Checked = True Then
            lblProxyUsername.Enabled = False
            lblProxyPassword.Enabled = False
            txtProxyUsername.Enabled = False
            txtProxyPassword.Enabled = False
            chkProxyUseBasic.Enabled = False
            If startup = False Then
                HQ.EveHqSettings.ProxyUseDefault = True
            End If
        End If
    End Sub

    Private Sub radUseSpecifiedCreds_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles radUseSpecifiedCreds.CheckedChanged
        If radUseSpecifiedCreds.Checked = True Then
            lblProxyUsername.Enabled = True
            lblProxyPassword.Enabled = True
            txtProxyUsername.Enabled = True
            txtProxyPassword.Enabled = True
            chkProxyUseBasic.Enabled = True
            If startup = False Then
                HQ.EveHqSettings.ProxyUseDefault = False
            End If
        End If
    End Sub

    Private Sub txtProxyServer_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtProxyServer.TextChanged
        HQ.EveHqSettings.ProxyServer = txtProxyServer.Text
    End Sub

    Private Sub txtProxyUsername_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtProxyUsername.TextChanged
        HQ.EveHqSettings.ProxyUsername = txtProxyUsername.Text
    End Sub

    Private Sub txtProxyPassword_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtProxyPassword.TextChanged
        HQ.EveHqSettings.ProxyPassword = txtProxyPassword.Text
    End Sub

#End Region

#Region "EveAPI & Server Settings"

    Private Sub UpdateEveServerSettings()
        chkEnableEveStatus.Checked = HQ.EveHqSettings.EnableEveStatus
        chkAutoAPI.Checked = HQ.EveHqSettings.AutoAPI
        chkAutoMailAPI.Checked = HQ.EveHqSettings.AutoMailAPI
        txtCCPAPIServer.Text = HQ.EveHqSettings.CCPAPIServerAddress
        txtAPIRSServer.Text = HQ.EveHqSettings.APIRSAddress
        chkUseAPIRSServer.Checked = HQ.EveHqSettings.UseAPIRS
        chkUseCCPBackup.Checked = HQ.EveHqSettings.UseCCPAPIBackup
        If HQ.EveHqSettings.UseAPIRS = False Then
            chkUseCCPBackup.Enabled = False
            txtAPIRSServer.Enabled = False
        Else
            chkUseCCPBackup.Enabled = True
            txtAPIRSServer.Enabled = True
        End If
        txtAPIFileExtension.Text = HQ.EveHqSettings.APIFileExtension
        trackServerOffset.Value = HQ.EveHqSettings.ServerOffset
        Dim offset As String = SkillFunctions.TimeToStringAll(HQ.EveHqSettings.ServerOffset)
        lblCurrentOffset.Text = "Current Offset: " & offset
    End Sub

    Private Sub trackServerOffset_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles trackServerOffset.ValueChanged
        HQ.EveHqSettings.ServerOffset = trackServerOffset.Value
        For Each newPilot As Pilot In HQ.EveHqSettings.Pilots
            newPilot.TrainingEndTime = newPilot.TrainingEndTimeActual.AddSeconds(HQ.EveHqSettings.ServerOffset)
            newPilot.TrainingStartTime = newPilot.TrainingStartTimeActual.AddSeconds(HQ.EveHqSettings.ServerOffset)
        Next
        Dim offset As String = SkillFunctions.TimeToStringAll(trackServerOffset.Value)
        lblCurrentOffset.Text = "Current Offset: " & offset
    End Sub

    Private Sub chkEnableEveStatus_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkEnableEveStatus.CheckedChanged
        If chkEnableEveStatus.Checked = True Then
            frmEveHQ.lblTQStatus.Text = "Tranquility Status: Unknown"
            HQ.EveHqSettings.EnableEveStatus = True
            frmEveHQ.tmrEve.Interval = 100
            frmEveHQ.tmrEve.Start()
        Else
            HQ.EveHqSettings.EnableEveStatus = False
            frmEveHQ.EveStatusIcon.Icon = My.Resources.EveHQ
            frmEveHQ.EveStatusIcon.Text = "EveHQ"
            frmEveHQ.tmrEve.Stop()
            frmEveHQ.lblTQStatus.Text = "Tranquility Status: Updates Disabled"
        End If
    End Sub

    Private Sub chkAutoAPI_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkAutoAPI.CheckedChanged
        If chkAutoAPI.Checked = True Then
            HQ.EveHqSettings.AutoAPI = True
            HQ.NextAutoAPITime = Now.AddMinutes(60)
        Else
            HQ.EveHqSettings.AutoAPI = False
        End If
    End Sub

    Private Sub chkAutoMailAPI_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkAutoMailAPI.CheckedChanged
        If chkAutoMailAPI.Checked = True Then
            HQ.EveHqSettings.AutoMailAPI = True
        Else
            HQ.EveHqSettings.AutoMailAPI = False
        End If
    End Sub

    Private Sub chkUseAPIRSServer_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkUseAPIRSServer.CheckedChanged
        If chkUseAPIRSServer.Checked = True Then
            HQ.EveHqSettings.UseAPIRS = True
            chkUseCCPBackup.Enabled = True
            txtAPIRSServer.Enabled = True
        Else
            HQ.EveHqSettings.UseAPIRS = False
            chkUseCCPBackup.Enabled = False
            txtAPIRSServer.Enabled = False
        End If
    End Sub

    Private Sub chkUseCCPBackup_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkUseCCPBackup.CheckedChanged
        If chkUseCCPBackup.Checked = True Then
            HQ.EveHqSettings.UseCCPAPIBackup = True
        Else
            HQ.EveHqSettings.UseCCPAPIBackup = False
        End If
    End Sub

    Private Sub txtCCPAPIServer_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtCCPAPIServer.TextChanged
        HQ.EveHqSettings.CCPAPIServerAddress = txtCCPAPIServer.Text
    End Sub

    Private Sub txtAPIRSServer_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtAPIRSServer.TextChanged
        HQ.EveHqSettings.APIRSAddress = txtAPIRSServer.Text
    End Sub

    Private Sub txtAPIFileExtension_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtAPIFileExtension.TextChanged
        HQ.EveHqSettings.APIFileExtension = txtAPIFileExtension.Text
    End Sub

#End Region

#Region "Notification Options"

    Public Sub UpdateNotificationOptions()
        Me.chkShutdownNotify.Checked = HQ.EveHqSettings.ShutdownNotify
        Me.chkNotifyToolTip.Checked = HQ.EveHqSettings.NotifyToolTip
        Me.chkNotifyDialog.Checked = HQ.EveHqSettings.NotifyDialog
        Me.chkNotifyNow.Checked = HQ.EveHqSettings.NotifyNow
        Me.chkNotifyEarly.Checked = HQ.EveHqSettings.NotifyEarly
        Me.chkNotifyEmail.Checked = HQ.EveHqSettings.NotifyEMail
        Me.chkNotifyEveMail.Checked = HQ.EveHqSettings.NotifyEveMail
        Me.chkNotifyNotification.Checked = HQ.EveHqSettings.NotifyEveNotification
        If HQ.EveHqSettings.NotifySound = True Then
            Me.chkNotifySound.Checked = True
            btnSelectSoundFile.Enabled = True
            btnSoundTest.Enabled = True
        Else
            Me.chkNotifySound.Checked = False
            btnSelectSoundFile.Enabled = False
            btnSoundTest.Enabled = False
        End If
        Me.lblSoundFile.Text = HQ.EveHqSettings.NotifySoundFile
        If HQ.EveHqSettings.UseSMTPAuth = True Then
            Me.chkSMTPAuthentication.Checked = True
            lblEmailUsername.Enabled = True
            lblEmailPassword.Enabled = True
            txtEmailUsername.Enabled = True
            txtEmailPassword.Enabled = True
        Else
            Me.chkSMTPAuthentication.Checked = False
            lblEmailUsername.Enabled = False
            lblEmailPassword.Enabled = False
            txtEmailUsername.Enabled = False
            txtEmailPassword.Enabled = False
        End If
        Me.chkUseSSL.Checked = HQ.EveHqSettings.UseSSL
        Me.txtSMTPServer.Text = HQ.EveHqSettings.EMailServer
        Me.txtSMTPPort.Text = CStr(HQ.EveHqSettings.EMailPort)
        Me.txtEmailAddress.Text = HQ.EveHqSettings.EMailAddress
        Me.txtEmailUsername.Text = HQ.EveHqSettings.EMailUsername
        Me.txtEmailPassword.Text = HQ.EveHqSettings.EMailPassword
        Me.txtSenderAddress.Text = HQ.EveHqSettings.EmailSenderAddress
        Me.sldNotifyOffset.Value = HQ.EveHqSettings.NotifyOffset
        Dim offset As String = SkillFunctions.TimeToStringAll(sldNotifyOffset.Value)
        lblNotifyOffset.Text = "Early Notification Offset: " & offset
        Me.nudShutdownNotifyPeriod.Value = HQ.EveHqSettings.ShutdownNotifyPeriod
        Me.chkIgnoreLastMessage.Checked = HQ.EveHqSettings.IgnoreLastMessage
        Me.chkNotifyAccountTime.Checked = HQ.EveHqSettings.NotifyAccountTime
        Me.chkNotifyInsuffClone.Checked = EveHQ.Core.HQ.EveHqSettings.NotifyInsuffClone
        Me.nudAccountTimeLimit.Enabled = HQ.EveHqSettings.NotifyAccountTime
        Me.nudAccountTimeLimit.Value = HQ.EveHqSettings.AccountTimeLimit
    End Sub

    Private Sub chkShutdownNotify_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkShutdownNotify.CheckedChanged
        If chkShutdownNotify.Checked = True Then
            HQ.EveHqSettings.ShutdownNotify = True
        Else
            HQ.EveHqSettings.ShutdownNotify = False
        End If
    End Sub

    Private Sub nudShutdownNotifyPeriod_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles nudShutdownNotifyPeriod.Click
        HQ.EveHqSettings.ShutdownNotifyPeriod = CInt(nudShutdownNotifyPeriod.Value)
    End Sub

    Private Sub nudShutdownNotifyPeriod_HandleDestroyed(ByVal sender As Object, ByVal e As EventArgs) _
        Handles nudShutdownNotifyPeriod.HandleDestroyed
        HQ.EveHqSettings.ShutdownNotifyPeriod = CInt(nudShutdownNotifyPeriod.Value)
    End Sub

    Private Sub nudShutdownNotifyPeriod_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) _
        Handles nudShutdownNotifyPeriod.KeyUp
        HQ.EveHqSettings.ShutdownNotifyPeriod = CInt(nudShutdownNotifyPeriod.Value)
    End Sub

    Private Sub chkNotifyNow_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkNotifyNow.CheckedChanged
        HQ.EveHqSettings.NotifyNow = chkNotifyNow.Checked
    End Sub

    Private Sub chkNotifyEarly_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkNotifyEarly.CheckedChanged
        HQ.EveHqSettings.NotifyEarly = chkNotifyEarly.Checked
    End Sub

    Private Sub chkNotifyToolTip_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkNotifyToolTip.CheckedChanged
        HQ.EveHqSettings.NotifyToolTip = chkNotifyToolTip.Checked
    End Sub

    Private Sub chkNotifyDialog_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkNotifyDialog.CheckedChanged
        HQ.EveHqSettings.NotifyDialog = chkNotifyDialog.Checked
    End Sub

    Private Sub chkNotifyEmail_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkNotifyEmail.CheckedChanged
        HQ.EveHqSettings.NotifyEMail = chkNotifyEmail.Checked
    End Sub

    Private Sub chkNotifySound_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkNotifySound.CheckedChanged
        If chkNotifySound.Checked = True Then
            HQ.EveHqSettings.NotifySound = True
            btnSelectSoundFile.Enabled = True
            btnSoundTest.Enabled = True
        Else
            HQ.EveHqSettings.NotifySound = False
            btnSelectSoundFile.Enabled = False
            btnSoundTest.Enabled = False
        End If
    End Sub

    Private Sub chkNotifyEveMail_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkNotifyEveMail.CheckedChanged
        HQ.EveHqSettings.NotifyEveMail = chkNotifyEveMail.Checked
    End Sub

    Private Sub chkNotifyNotification_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkNotifyNotification.CheckedChanged
        HQ.EveHqSettings.NotifyEveNotification = chkNotifyNotification.Checked
    End Sub

    Private Sub sldNotifyOffset_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles sldNotifyOffset.ValueChanged
        HQ.EveHqSettings.NotifyOffset = sldNotifyOffset.Value
        Dim offset As String = SkillFunctions.TimeToStringAll(sldNotifyOffset.Value)
        lblNotifyOffset.Text = "Early Notification Offset: " & offset
    End Sub

    Private Sub chkSMTPAuthentication_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkSMTPAuthentication.CheckedChanged
        If chkSMTPAuthentication.Checked = True Then
            HQ.EveHqSettings.UseSMTPAuth = True
            lblEmailUsername.Enabled = True
            lblEmailPassword.Enabled = True
            txtEmailUsername.Enabled = True
            txtEmailPassword.Enabled = True
        Else
            HQ.EveHqSettings.UseSMTPAuth = False
            lblEmailUsername.Enabled = False
            lblEmailPassword.Enabled = False
            txtEmailUsername.Enabled = False
            txtEmailPassword.Enabled = False
        End If
    End Sub

    Private Sub chkUseSSL_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkUseSSL.CheckedChanged
        HQ.EveHqSettings.UseSSL = chkUseSSL.Checked
    End Sub

    Private Sub txtSMTPServer_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtSMTPServer.TextChanged
        HQ.EveHqSettings.EMailServer = txtSMTPServer.Text
    End Sub

    Private Sub txtEmailAddress_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtEmailAddress.TextChanged
        HQ.EveHqSettings.EMailAddress = txtEmailAddress.Text
    End Sub

    Private Sub txtEmailUsername_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtEmailUsername.TextChanged
        HQ.EveHqSettings.EMailUsername = txtEmailUsername.Text
    End Sub

    Private Sub txtEmailPassword_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtEmailPassword.TextChanged
        HQ.EveHqSettings.EMailPassword = txtEmailPassword.Text
    End Sub

    Private Sub txtSenderAddress_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtSenderAddress.TextChanged
        HQ.EveHqSettings.EmailSenderAddress = txtSenderAddress.Text
    End Sub

    Private Sub btnTestEmail_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTestEmail.Click

        ' Only do this if at least one notification is enabled
        Dim notifyText As String = ""
        For Each cPilot As Pilot In HQ.EveHqSettings.Pilots
            If cPilot.Active = True And cPilot.Training = True Then
                notifyText = ""
                Dim trainingTime As Long = SkillFunctions.CalcCurrentSkillTime(cPilot)
                Dim strTime As String = SkillFunctions.TimeToString(cPilot.TrainingCurrentTime)
                strTime = strTime.Replace("s", " seconds").Replace("m", " minutes")
                notifyText &= cPilot.Name & " has approximately " & strTime & " before training of " &
                              cPilot.TrainingSkillName & " to Level " & cPilot.TrainingSkillLevel & " completes." &
                              ControlChars.CrLf
                cPilot.TrainingNotifiedEarly = True
                cPilot.TrainingNotifiedNow = False
                ' Show the notifications
                If notifyText <> "" Then
                    ' Expand the details with some additional information
                    If cPilot.QueuedSkills.Count > 0 Then
                        notifyText &= ControlChars.CrLf
                        notifyText &= "Next skill in Eve skill queue: " &
                                      SkillFunctions.SkillIDToName(CStr(cPilot.QueuedSkills.Values(0).SkillID)) & " " &
                                      SkillFunctions.Roman(cPilot.QueuedSkills.Values(0).Level)
                        notifyText &= ControlChars.CrLf
                    Else
                        notifyText &= ControlChars.CrLf
                        notifyText &= "Next skill in Eve skill queue: No skill queued"
                        notifyText &= ControlChars.CrLf
                    End If
                    If cPilot.TrainingQueues.Count > 0 Then
                        notifyText &= ControlChars.CrLf
                        notifyText &= "EveHQ Skill Queue Info: " & ControlChars.CrLf
                        For Each sq As SkillQueue In cPilot.TrainingQueues.Values
                            Dim nq As ArrayList = SkillQueueFunctions.BuildQueue(cPilot, sq, False, True)
                            If sq.IncCurrentTraining = True Then
                                If nq.Count > 1 Then
                                    For q As Integer = 1 To nq.Count - 1
                                        If CType(nq(q), SortedQueueItem).Done = False Then
                                            notifyText &= sq.Name & ": " & CType(nq(q), SortedQueueItem).Name
                                            notifyText &= " (" &
                                                          SkillFunctions.Roman(
                                                              CInt(CType(nq(q), SortedQueueItem).FromLevel))
                                            notifyText &= " to " &
                                                          SkillFunctions.Roman(
                                                              CInt(CType(nq(q), SortedQueueItem).FromLevel) + 1) & ")" &
                                                          ControlChars.CrLf
                                            Exit For
                                        End If
                                    Next
                                End If
                            Else
                                If nq.Count > 0 Then
                                    For q As Integer = 0 To nq.Count - 1
                                        If CType(nq(q), SortedQueueItem).Done = False Then
                                            notifyText &= sq.Name & ": " & CType(nq(q), SortedQueueItem).Name
                                            notifyText &= " (" &
                                                          SkillFunctions.Roman(
                                                              CInt(CType(nq(q), SortedQueueItem).FromLevel))
                                            notifyText &= " to " &
                                                          SkillFunctions.Roman(
                                                              CInt(CType(nq(q), SortedQueueItem).FromLevel) + 1) & ")" &
                                                          ControlChars.CrLf
                                            Exit For
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End If
                    Dim eveHQMail As New SmtpClient
                    Try
                        eveHQMail.Host = HQ.EveHqSettings.EMailServer
                        eveHQMail.Port = HQ.EveHqSettings.EMailPort
                        eveHQMail.EnableSsl = HQ.EveHqSettings.UseSSL
                        If HQ.EveHqSettings.UseSMTPAuth = True Then
                            Dim newCredentials As New NetworkCredential
                            newCredentials.UserName = HQ.EveHqSettings.EMailUsername
                            newCredentials.Password = HQ.EveHqSettings.EMailPassword
                            eveHQMail.Credentials = newCredentials
                        End If
                        Dim recList As String =
                                HQ.EveHqSettings.EMailAddress.Replace(ControlChars.CrLf, "").Replace(" ", "").Replace(
                                    ";", ",")
                        Dim eveHQMsg As New MailMessage(HQ.EveHqSettings.EmailSenderAddress, recList)
                        eveHQMsg.Subject = "Eve Training Notification: " & cPilot.Name & " (" & cPilot.TrainingSkillName &
                                           " " & SkillFunctions.Roman(cPilot.TrainingSkillLevel) & ")"
                        eveHQMsg.Body = notifyText
                        eveHQMail.Send(eveHQMsg)
                        MessageBox.Show("Test message sent successfully. Please check your inbox for confirmation.",
                                        "EveHQ Test Email Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        ' Exit after the first mail
                        Exit Sub
                    Catch ex As Exception
                        MessageBox.Show(
                            "The mail sending process failed. Please check that the server, address, username and password are correct." &
                            ControlChars.CrLf & ControlChars.CrLf & "The error was: " & ex.Message,
                            "EveHQ Test Email Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        ' Exit after the first mail
                        Exit Sub
                    End Try
                End If
            End If
        Next
    End Sub

    Private Sub btnSelectSoundFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSelectSoundFile.Click
        With ofd1
            .Title = "Please select a valid .wav file"
            .FileName = ""
            .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            .Filter = "Wave files (*.wav)|*.wav|All files (*.*)|*.*"
            .FilterIndex = 1
            .RestoreDirectory = True
            If .ShowDialog() = DialogResult.OK Then
                lblSoundFile.Text = .FileName
            End If
            HQ.EveHqSettings.NotifySoundFile = .FileName
        End With
    End Sub

    Private Sub btnSoundTest_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSoundTest.Click
        Try
            My.Computer.Audio.Play(HQ.EveHqSettings.NotifySoundFile, AudioPlayMode.Background)
        Catch ex As Exception
            MessageBox.Show("Unable to play sound file." & ControlChars.CrLf & "Error: " & ex.Message,
                            "Error with Wave File", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

    Private Sub txtSMTPPort_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtSMTPPort.TextChanged
        If IsNumeric(txtSMTPPort.Text) = True Then
            HQ.EveHqSettings.EMailPort = CInt(txtSMTPPort.Text)
        Else
            HQ.EveHqSettings.EMailPort = 0
        End If
    End Sub

    Private Sub chkIgnoreLastMessage_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkIgnoreLastMessage.CheckedChanged
        HQ.EveHqSettings.IgnoreLastMessage = chkIgnoreLastMessage.Checked
    End Sub

    Private Sub chkNotifyInsuffClone_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotifyInsuffClone.CheckedChanged
        EveHQ.Core.HQ.EveHqSettings.NotifyInsuffClone = chkNotifyInsuffClone.Checked
    End Sub

    Private Sub chkNotifyAccountTime_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkNotifyAccountTime.CheckedChanged
        HQ.EveHqSettings.NotifyAccountTime = chkNotifyAccountTime.Checked
        nudAccountTimeLimit.Enabled = chkNotifyAccountTime.Checked
    End Sub

    Private Sub nudAccountTimeLimit_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles nudAccountTimeLimit.Click
        HQ.EveHqSettings.AccountTimeLimit = CInt(nudAccountTimeLimit.Value)
    End Sub

    Private Sub nudAccountTimeLimit_HandleDestroyed(ByVal sender As Object, ByVal e As EventArgs) _
        Handles nudAccountTimeLimit.HandleDestroyed
        HQ.EveHqSettings.AccountTimeLimit = CInt(nudAccountTimeLimit.Value)
    End Sub

    Private Sub nudAccountTimeLimit_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs) _
        Handles nudAccountTimeLimit.KeyUp
        HQ.EveHqSettings.AccountTimeLimit = CInt(nudAccountTimeLimit.Value)
    End Sub

#End Region

#Region "G15 Routines"

    Private Sub UpdateG15Options()
        If HQ.EveHqSettings.ActivateG15 = True Then
            chkActivateG15.Checked = True
        Else
            chkActivateG15.Checked = False
        End If
        If HQ.EveHqSettings.CycleG15Pilots = True Then
            chkCyclePilots.Checked = True
        Else
            chkCyclePilots.Checked = False
        End If
        nudCycleTime.Value = HQ.EveHqSettings.CycleG15Time
    End Sub

    Private Sub chkActivateG15_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkActivateG15.CheckedChanged
        If chkActivateG15.Checked = True Then
            If HQ.EveHqSettings.ActivateG15 = False Then
                HQ.EveHqSettings.ActivateG15 = True
                'Init the LCD
                Try
                    G15LCDv2.InitLCD()
                    ' Check if the LCD will cycle chars
                    If HQ.IsG15LCDActive = True And HQ.EveHqSettings.CycleG15Pilots = True Then
                        G15LCDv2.tmrLCDChar.Interval = (1000 * HQ.EveHqSettings.CycleG15Time)
                        G15LCDv2.tmrLCDChar.Enabled = True
                    End If
                Catch ex As Exception
                    MessageBox.Show(
                        "Unable to start G15 Display. Please ensure you have the keyboard and drivers correctly installed. The error was: " &
                        ex.Message, "Error Starting G15", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    chkActivateG15.Checked = False
                End Try
            End If
        Else
            If HQ.EveHqSettings.ActivateG15 = True Then
                HQ.EveHqSettings.ActivateG15 = False
                ' Close the LCD
                Try
                    G15LCDv2.CloseLCD()
                Catch ex As Exception
                    MessageBox.Show("Unable to close G15 Display: " & ex.Message, "Error Closing G15",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                End Try
            End If
        End If
    End Sub

    Private Sub chkCyclePilots_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkCyclePilots.CheckedChanged
        If chkCyclePilots.Checked = True Then
            HQ.EveHqSettings.CycleG15Pilots = True
            G15LCDv2.tmrLCDChar.Interval = (1000 * HQ.EveHqSettings.CycleG15Time)
            If HQ.EveHqSettings.ActivateG15 = True Then
                G15LCDv2.tmrLCDChar.Enabled = True
            End If
        Else
            HQ.EveHqSettings.CycleG15Pilots = False
            G15LCDv2.tmrLCDChar.Enabled = False
        End If
    End Sub

    Private Sub nudCycleTime_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles nudCycleTime.ValueChanged
        HQ.EveHqSettings.CycleG15Time = CInt(nudCycleTime.Value)
        If HQ.EveHqSettings.CycleG15Time > 0 Then
            G15LCDv2.tmrLCDChar.Interval = CInt((nudCycleTime.Value * 1000))
        Else
            G15LCDv2.tmrLCDChar.Interval = CInt(1000)
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
            lblEveDir.Text = HQ.EveHqSettings.EveFolder(folder)
            If My.Computer.FileSystem.DirectoryExists(HQ.EveHqSettings.EveFolder(folder)) = True Then
                chkLUA.Enabled = True
                txtFName.Enabled = True
                txtFName.Text = HQ.EveHqSettings.EveFolderLabel(folder)
            Else
                chkLUA.Enabled = False
                txtFName.Enabled = False
                txtFName.Text = ""
            End If
            If HQ.EveHqSettings.EveFolderLUA(folder) = True Then
                chkLUA.Checked = True
            Else
                chkLUA.Checked = False
                If chkLUA.Enabled = True Then
                    'lblCacheSize.Text &= " (shared)"
                End If
            End If

        Next
    End Sub

    Private Sub btnEveDirClick(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnEveDir1.Click, btnEveDir2.Click, btnEveDir3.Click, btnEveDir4.Click
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
                MessageBox.Show("This folder does not contain the Eve.exe file.", "Directory Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                lblEveDir.Text = .SelectedPath
                HQ.EveHqSettings.EveFolder(folder) = .SelectedPath
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

    Private Sub btnClearClick(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnClear1.Click, btnClear2.Click, btnClear3.Click, btnClear4.Click
        Dim btnEveDir As New Button
        btnEveDir = CType(sender, Button)
        Dim folder As Integer = CInt(btnEveDir.Name.Remove(0, 8))
        Dim gbFolderHost As GroupBox = CType(Me.gbEveFolders.Controls("gbLocation" & CStr(folder).Trim), GroupBox)
        Dim lblEveDir As Label = CType(gbFolderHost.Controls("lblEveDir" & CStr(folder).Trim), Label)
        lblEveDir.Text = ""
        Dim chkLUA As CheckBox = CType(gbFolderHost.Controls("chkLUA" & CStr(folder).Trim), CheckBox)
        chkLUA.Checked = False
        chkLUA.Enabled = False
        Dim lblCacheSize As Label = CType(gbFolderHost.Controls("lblCacheSize" & CStr(folder).Trim), Label)
        lblCacheSize.Text = ""
        HQ.EveHqSettings.EveFolder(folder) = ""
    End Sub

    Private Sub chkLUA_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkLUA1.CheckedChanged, chkLUA2.CheckedChanged, chkLUA3.CheckedChanged, chkLUA4.CheckedChanged
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
            HQ.EveHqSettings.EveFolderLUA(folder) = True
            ' Check program files
            If startup = False Then
                Dim cacheDir As String = Path.Combine(HQ.EveHqSettings.EveFolder(folder), "cache")
                Dim settingsDir As String = Path.Combine(cacheDir, "settings")
                Dim prefsFile As String = Path.Combine(cacheDir, "prefs.ini")
                Dim browserDir As String = Path.Combine(cacheDir, "browser")
                Dim machoDIR As String = Path.Combine(cacheDir, "machonet")
                If _
                    My.Computer.FileSystem.DirectoryExists(cacheDir) = True And
                    My.Computer.FileSystem.FileExists(prefsFile) = True Then
                    MessageBox.Show("Confirmed /LUA:off active on this folder.", "LUA Result", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information)
                Else
                    MessageBox.Show("Warning: /LUA:off does not appear active on this folder.", "LUA Result",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        Else
            HQ.EveHqSettings.EveFolderLUA(folder) = False
            ' Check the application directory for the user
            If startup = False Then
                Dim EveAppFolder As String = HQ.EveHqSettings.EveFolder(folder)
                EveAppFolder = EveAppFolder.Replace("\", "_").Replace(":", "").Replace(" ", "_").ToLower
                EveAppFolder &= "_tranquility"
                Dim eveDir As String =
                        Path.Combine(
                            Path.Combine(
                                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                                             "CCP"), "Eve"), EveAppFolder)
                Dim cacheDir As String = Path.Combine(eveDir, "cache")
                Dim settingsDir As String = Path.Combine(eveDir, "settings")
                Dim prefsFile As String = Path.Combine(settingsDir, "prefs.ini")
                Dim browserDir As String = Path.Combine(cacheDir, "browser")
                Dim machoDIR As String = Path.Combine(cacheDir, "prefs.machonet")
                If _
                    My.Computer.FileSystem.DirectoryExists(cacheDir) = True And
                    My.Computer.FileSystem.FileExists(prefsFile) = True Then
                    MessageBox.Show("Confirmed shared settings are active.", "LUA Result", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information)
                Else
                    MessageBox.Show("No shared settings found.", "LUA Result", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information)
                End If
            End If
        End If
    End Sub

    Private Sub txtFriendlyName_TextChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles txtFriendlyName1.TextChanged, txtFriendlyName2.TextChanged, txtFriendlyName3.TextChanged,
                txtFriendlyName4.TextChanged
        Dim txtFName As TextBox = CType(sender, TextBox)
        Dim idx As Integer = CInt(txtFName.Name.Substring(txtFName.Name.Length - 1, 1))
        HQ.EveHqSettings.EveFolderLabel(idx) = txtFName.Text
        Dim gbFolderHost As GroupBox = CType(Me.gbEveFolders.Controls("gbLocation" & CStr(idx).Trim), GroupBox)
        If HQ.EveHqSettings.EveFolderLabel(idx) <> "" Then
            gbFolderHost.Text = "Eve Location " & idx & " (" & HQ.EveHqSettings.EveFolderLabel(idx) & ")"
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
        cboTaskbarIconMode.SelectedIndex = HQ.EveHqSettings.TaskbarIconMode
    End Sub

    Private Sub cboTaskbarIconMode_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles cboTaskbarIconMode.SelectedIndexChanged
        HQ.EveHqSettings.TaskbarIconMode = cboTaskbarIconMode.SelectedIndex
        Select Case HQ.EveHqSettings.TaskbarIconMode
            Case 0 ' Simple
                Select Case HQ.myTQServer.Status
                    Case EveServer.ServerStatus.Down
                        frmEveHQ.EveStatusIcon.Text = HQ.myTQServer.StatusText
                    Case EveServer.ServerStatus.Starting
                        frmEveHQ.EveStatusIcon.Text = HQ.myTQServer.StatusText
                    Case EveServer.ServerStatus.Shutting
                        frmEveHQ.EveStatusIcon.Text = HQ.myTQServer.StatusText
                    Case EveServer.ServerStatus.Full
                        frmEveHQ.EveStatusIcon.Text = HQ.myTQServer.StatusText
                    Case EveServer.ServerStatus.ProxyDown
                        frmEveHQ.EveStatusIcon.Text = HQ.myTQServer.StatusText
                    Case EveServer.ServerStatus.Unknown
                        frmEveHQ.EveStatusIcon.Text = HQ.myTQServer.StatusText
                    Case EveServer.ServerStatus.Up
                        Dim msg As String = HQ.myTQServer.ServerName & ":" & vbCrLf
                        msg = msg & "Version: " & HQ.myTQServer.Version & vbCrLf
                        msg = msg & "Players: " & HQ.myTQServer.Players
                        If msg.Length > 50 Then
                            frmEveHQ.EveStatusIcon.Text = HQ.myTQServer.ServerName & ":" & vbCrLf &
                                                          "Server currently initialising"
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
        For Each cWidget As String In HQ.Widgets.Keys
            cboWidgets.Items.Add(cWidget)
        Next
        cboWidgets.EndUpdate()
    End Sub

    Private Sub UpdateWidgets()
        lvWidgets.BeginUpdate()
        lvWidgets.Items.Clear()
        For Each config As SortedList(Of String, Object) In HQ.EveHqSettings.DashboardConfiguration
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
        chkShowPriceTicker.Checked = HQ.EveHqSettings.DBTicker
        If HQ.EveHqSettings.DBTickerLocation = "" Then
            HQ.EveHqSettings.DBTickerLocation = "Bottom"
        End If
        cboTickerLocation.SelectedItem = HQ.EveHqSettings.DBTickerLocation
    End Sub

    Private Sub btnAddWidget_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddWidget.Click
        ' Check we have a selected widget type
        If cboWidgets.SelectedItem IsNot Nothing Then
            Dim WidgetName As String = cboWidgets.SelectedItem.ToString
            ' Determine the type of control to add
            Dim ClassName As String = HQ.Widgets(WidgetName)
            Dim myType As Type = Assembly.GetExecutingAssembly.GetType(ClassName)
            Dim newWidget As Object = Activator.CreateInstance(myType)
            Dim pi As PropertyInfo = myType.GetProperty("ControlConfigForm")
            Dim myConfigFormName As String = pi.GetValue(newWidget, Nothing).ToString
            If myConfigFormName <> "" Then
                Dim ConfigType As Type = Assembly.GetExecutingAssembly.GetType(myConfigFormName)
                Dim ConfigForm As Form = CType(Activator.CreateInstance(ConfigType), Form)
                Dim fi As PropertyInfo = ConfigForm.GetType().GetProperty("DBWidget")
                fi.SetValue(ConfigForm, newWidget, Nothing)
                ConfigForm.ShowDialog()
                If ConfigForm.DialogResult = DialogResult.OK Then
                    ' Save the Widget
                    Dim ci As PropertyInfo = myType.GetProperty("ControlConfiguration")
                    Dim myConfig As SortedList(Of String, Object) = CType(ci.GetValue(newWidget, Nothing), 
                                                                          SortedList(Of String, Object))
                    HQ.EveHqSettings.DashboardConfiguration.Add(myConfig)
                    Call Me.UpdateWidgets()
                    ' Update the dashboard
                    frmDashboard.UpdateWidgets()
                Else
                    ' Process Aborted
                    MessageBox.Show("Widget configuration aborted - information not saved.", "Addition Cancelled",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                ConfigForm.Dispose()
            Else
                ' Save the Widget
                Dim ci As PropertyInfo = myType.GetProperty("ControlConfiguration")
                Dim myConfig As SortedList(Of String, Object) = CType(ci.GetValue(newWidget, Nothing), 
                                                                      SortedList(Of String, Object))
                HQ.EveHqSettings.DashboardConfiguration.Add(myConfig)
                Call Me.UpdateWidgets()
                ' Update the dashboard
                frmDashboard.UpdateWidgets()
            End If
        Else
            ' Need a widget type before proceeding
            MessageBox.Show("Please select a Widget type before proceeding.", "Widget Type Required",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
    End Sub

    Private Sub btnRemoveWidget_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemoveWidget.Click
        ' Check for an item selection
        If lvWidgets.SelectedItems.Count > 0 Then
            Dim index As Integer = lvWidgets.SelectedItems(0).Index
            HQ.EveHqSettings.DashboardConfiguration.RemoveAt(index)
            lvWidgets.SelectedItems(0).Remove()
            ' Update the dashboard
            frmDashboard.UpdateWidgets()
        Else
            MessageBox.Show("Please select a Widget to remove before proceeding.", "Widget Selection Required",
                            MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
    End Sub

    Private Sub lvWidgets_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles lvWidgets.DoubleClick
        ' Edit a widget
        If lvWidgets.SelectedItems.Count > 0 Then
            Dim index As Integer = lvWidgets.SelectedItems(0).Index
            Dim WidgetName As String = lvWidgets.SelectedItems(0).Text
            ' Determine the type of control to add
            Select Case WidgetName
                Case "Pilot Information"
                    Dim newWidget As New DBCPilotInfo
                    newWidget.ControlConfiguration = CType(HQ.EveHqSettings.DashboardConfiguration.Item(index), 
                                                           SortedList(Of String, Object))
                    Dim newWidgetConfig As New DBCPilotInfoConfig
                    newWidgetConfig.DBWidget = newWidget
                    newWidgetConfig.ShowDialog()
                    lvWidgets.Items(index).SubItems(1).Text = "Default Pilot: " &
                                                              CStr(newWidget.ControlConfiguration("DefaultPilotName"))
                Case "Skill Queue Information"
                    Dim newWidget As New DBCSkillQueueInfo
                    newWidget.ControlConfiguration = CType(HQ.EveHqSettings.DashboardConfiguration.Item(index), 
                                                           SortedList(Of String, Object))
                    Dim newWidgetConfig As New DBCSkillQueueInfoConfig
                    newWidgetConfig.DBWidget = newWidget
                    newWidgetConfig.ShowDialog()
                    If CBool(newWidget.ControlConfiguration("EveQueue")) = True Then
                        lvWidgets.Items(index).SubItems(1).Text = "Default Pilot: " &
                                                                  CStr(newWidget.ControlConfiguration("DefaultPilotName")) &
                                                                  ", Eve Queue"
                    Else
                        lvWidgets.Items(index).SubItems(1).Text = "Default Pilot: " &
                                                                  CStr(newWidget.ControlConfiguration("DefaultPilotName")) &
                                                                  ", EveHQ Queue (" &
                                                                  CStr(newWidget.ControlConfiguration("DefaultQueueName")) &
                                                                  ")"
                    End If
            End Select
            ' Update the dashboard
            frmDashboard.UpdateWidgets()
        End If
    End Sub

    Private Sub chkShowPriceTicker_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles chkShowPriceTicker.CheckedChanged
        HQ.EveHqSettings.DBTicker = chkShowPriceTicker.Checked
        frmDashboard.Ticker1.Visible = HQ.EveHqSettings.DBTicker
    End Sub

    Private Sub cboTickerLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) _
        Handles cboTickerLocation.SelectedIndexChanged
        HQ.EveHqSettings.DBTickerLocation = cboTickerLocation.SelectedItem.ToString
        If frmDashboard IsNot Nothing Then
            Try
                Select Case HQ.EveHqSettings.DBTickerLocation
                    Case "Top"
                        frmDashboard.Ticker1.Dock = DockStyle.Top
                    Case "Bottom"
                        frmDashboard.Ticker1.Dock = DockStyle.Bottom
                End Select

            Catch ex As Exception
                ' just supressing any errors for now... 
            End Try
        End If
    End Sub

#End Region

#Region "Market Data Settings"

    Private Sub UpdateMarketSettings()
        UpdateMarketProviderList()
        UpdateDefaultMetrics()
        UpdateDataSourceList()
        UpdateItemOverrideControls()

    End Sub

    Private Sub UpdateMarketProviderList()

        _marketDataProvider.Items.Add(EveHqMarketDataProvider.Name)
        _marketDataProvider.Items.Add(EveCentralMarketDataProvider.Name)


        ' Set selected to the current setting.
        _marketDataProvider.SelectedItem = HQ.MarketStatDataProvider.ProviderName

        enableMarketDataUpload.Checked = HQ.EveHqSettings.MarketDataUploadEnabled
    End Sub

    Private Sub UpdateDefaultMetrics()

        Select Case HQ.EveHqSettings.MarketDefaultTransactionType
            Case MarketTransactionKind.All
                _defaultAll.Checked = True
            Case MarketTransactionKind.Buy
                _defaultBuy.Checked = True
            Case Else
                _defaultSell.Checked = True
        End Select


        Dim metric As String = HQ.EveHqSettings.MarketDefaultMetric.ToString()
        If _useMiniumPrice.Text = metric Then
            _useMiniumPrice.Checked = True
        End If

        If _useMaximumPrice.Text = metric Then
            _useMaximumPrice.Checked = True
        End If

        If _useAveragePrice.Text = metric Then
            _useAveragePrice.Checked = True
        End If

        If _useMedianPrice.Text = metric Then
            _useMedianPrice.Checked = True
        End If

        If _usePercentile.Text = metric Then
            _usePercentile.Checked = True
        End If
    End Sub

    Private Sub UpdateDataSourceList()
        If _regionList IsNot Nothing Then
            If _regionList.Items IsNot Nothing Then
                _regionList.Items.Clear()
            End If


            If (HQ.MarketStatDataProvider.LimitedRegionSelection = False) Then
                For Each region As EveGalaticRegion In HQ.Regions.Values
                    _regionList.Items.Add(region.Name)
                Next
            Else

                For Each regionId As Integer In HQ.MarketStatDataProvider.SupportedRegions
                    Dim temp As EveGalaticRegion = (From region In HQ.Regions.Values Where region.Id = regionId Select region).FirstOrDefault()
                    If (temp IsNot Nothing) Then
                        _regionList.Items.Add(temp.Name)
                    End If
                Next
            End If
        End If

        If _systemList IsNot Nothing Then
            If _systemList.Items IsNot Nothing Then
                _systemList.Items.Clear()
            End If

            If (HQ.MarketStatDataProvider.LimitedSystemSelection = False) Then
                For Each system As SolarSystem In HQ.SolarSystemsByName.Values
                    _systemList.Items.Add(system.Name)
                Next

            Else
                For Each systemId As Integer In HQ.MarketStatDataProvider.SupportedSystems
                    Dim temp As SolarSystem
                    If (HQ.SolarSystemsById.TryGetValue(systemId.ToInvariantString(), temp)) Then
                        _systemList.Items.Add(temp.Name)
                    End If
                Next
            End If
        End If

        If (_systemList IsNot Nothing And _regionList IsNot Nothing) Then
            If HQ.EveHqSettings.MarketUseRegionMarket = True Then
                _useRegionData.Checked = True
                _useSystemPrice.Checked = False
                _regionList.Enabled = True
                _systemList.Enabled = False

                'Get the selected regions from settings and find them in the collection
                For Each regionID As Integer In Core.HQ.EveHqSettings.MarketRegions
                    Dim marketRegion As EveGalaticRegion = (From galRegion In Core.HQ.Regions _
                                                             Where galRegion.Value.Id = regionID
                                                             Select galRegion.Value).FirstOrDefault()
                    If marketRegion IsNot Nothing Then
                        _regionList.SelectedItems.Add(marketRegion.Name)
                    End If
                Next

            Else
                _useSystemPrice.Checked = True
                _useRegionData.Checked = False
                _regionList.Enabled = False
                _systemList.Enabled = True

                'Find the select system based on id
                Dim marketSystem As SolarSystem = (From system In Core.HQ.SolarSystemsById Where system.Value.Id = Core.HQ.EveHqSettings.MarketSystem Select system.Value).FirstOrDefault()
                If marketSystem IsNot Nothing Then
                    _systemList.SelectedItem = marketSystem.Name
                End If
            End If
        End If
    End Sub

    Private Sub OnUseSystemPriceChecked(sender As System.Object, e As System.EventArgs) Handles _useSystemPrice.Click
        _regionList.Enabled = False
        _systemList.Enabled = True
    End Sub

    Private Sub OnUseRegionPriceChecked(sender As System.Object, e As System.EventArgs) Handles _useRegionData.Click
        _regionList.Enabled = True
        _systemList.Enabled = False
    End Sub

    Private Sub SaveMarketSettings()
        If _marketDataProvider.SelectedItem Is Nothing Then
            HQ.EveHqSettings.MarketDataProvider = MarketProviders.EveCentral.ToString()
        Else
            HQ.EveHqSettings.MarketDataProvider = _marketDataProvider.SelectedItem.ToString()
        End If
        HQ.MarketStatDataProvider = Nothing 'this will cause an update on next use.
        If _useMiniumPrice.Checked Then
            HQ.EveHqSettings.MarketDefaultMetric = MarketMetric.Minimum
        End If

        If _useMaximumPrice.Checked Then
            HQ.EveHqSettings.MarketDefaultMetric = MarketMetric.Maximum
        End If

        If _useAveragePrice.Checked Then
            HQ.EveHqSettings.MarketDefaultMetric = MarketMetric.Average
        End If

        If _useMedianPrice.Checked Then
            HQ.EveHqSettings.MarketDefaultMetric = MarketMetric.Median
        End If

        If _usePercentile.Checked Then
            HQ.EveHqSettings.MarketDefaultMetric = MarketMetric.Percentile
        End If

        If (_defaultAll.Checked) Then
            HQ.EveHqSettings.MarketDefaultTransactionType = MarketTransactionKind.All
        ElseIf _defaultBuy.Checked Then
            HQ.EveHqSettings.MarketDefaultTransactionType = MarketTransactionKind.Buy
        Else
            HQ.EveHqSettings.MarketDefaultTransactionType = MarketTransactionKind.Sell

        End If

        If _useSystemPrice.Checked Then
            HQ.EveHqSettings.MarketUseRegionMarket = False
        Else
            HQ.EveHqSettings.MarketUseRegionMarket = True
        End If

        If _systemList.SelectedItem IsNot Nothing Then
            HQ.EveHqSettings.MarketSystem = HQ.SolarSystemsByName(_systemList.SelectedItem.ToString).Id
        End If

        If _regionList.SelectedItems IsNot Nothing And _regionList.SelectedItems.Count > 0 Then
            HQ.EveHqSettings.MarketRegions = (From marketRegion In _regionList.SelectedItems Select HQ.Regions(marketRegion.ToString).Id).ToList()
        End If

        HQ.EveHqSettings.MarketDataUploadEnabled = enableMarketDataUpload.Checked

        If EveHQ.Core.HQ.EveHqSettings.MarketDataUploadEnabled = True Then
            Core.HQ.MarketCacheUploader.Start()
        Else
            Core.HQ.MarketCacheUploader.Stop() ' It should be stopped already, but never hurts to set it so again.
        End If
    End Sub


    Private Sub UpdateItemOverrideControls()
        If (_itemOverrideItemList.Items.Count = 0) Then
            _itemOverrideItemList.Items.AddRange((From item In Core.HQ.itemData.Values Where item.MarketGroup <> 0 Select item.Name).ToArray())
        End If

        'set radio buttons to defaults
        SetOverrideMetricRadioButton(HQ.EveHqSettings.MarketDefaultMetric)

        SetOverrideTransactionTypeRadioButton(HQ.EveHqSettings.MarketDefaultTransactionType)

        UpdateActiveOverrides()
    End Sub

    Private Sub SetOverrideMetricRadioButton(metric As MarketMetric)
        Select Case metric
            Case MarketMetric.Average
                _itemOverrideAvgPrice.Checked = True
            Case MarketMetric.Maximum
                _itemOverrideMaxPrice.Checked = True
            Case MarketMetric.Median
                _itemOverrideMedianPrice.Checked = True
            Case MarketMetric.Minimum
                _itemOverrideMinPrice.Checked = True
            Case MarketMetric.Percentile
                _itemOverridePercentPrice.Checked = True
            Case MarketMetric.Default
                _itemOverrideMinPrice.Checked = True

        End Select
    End Sub

    Private Sub SetOverrideTransactionTypeRadioButton(type As MarketTransactionKind)
        Select Case type
            Case MarketTransactionKind.All
                _itemOverrideAllOrders.Checked = True
            Case MarketTransactionKind.Buy
                _itemOverrideBuyOrders.Checked = True
            Case MarketTransactionKind.Sell
                _itemOverrideSellOrders.Checked = True
            Case MarketTransactionKind.Default
                _itemOverrideSellOrders.Checked = True
        End Select
    End Sub

    Private Sub OnOverrideItemListIndexChanged(sender As System.Object, e As System.EventArgs) Handles _itemOverrideItemList.SelectedIndexChanged

        Dim item As String
        Dim itemId As Integer
        Dim activeStat As MarketMetric = HQ.EveHqSettings.MarketDefaultMetric
        Dim activeTransactionType As MarketTransactionKind = HQ.EveHqSettings.MarketDefaultTransactionType
        If (HQ.itemList.TryGetValue(_itemOverrideItemList.SelectedItem.ToString, item)) And Integer.TryParse(item, itemId) Then

            ' see if the item is in the override list, otherwise set values to default
            Dim itemOverride As ItemMarketOverride
            If HQ.EveHqSettings.MarketStatOverrides.Count > 0 And HQ.EveHqSettings.MarketStatOverrides.TryGetValue(itemId, itemOverride) Then
                If (itemOverride IsNot Nothing) Then
                    activeStat = itemOverride.MarketStat
                    activeTransactionType = itemOverride.TransactionType
                End If
            End If

        End If

        SetOverrideMetricRadioButton(activeStat)

        SetOverrideTransactionTypeRadioButton(activeTransactionType)



    End Sub


    Private Sub OnAddUpdateItemOverrideClick(sender As System.Object, e As System.EventArgs) Handles _itemOverrideAddOverride.Click
        Dim item As String
        Dim itemID As Integer


        If _itemOverrideItemList Is Nothing Then
            Return
        End If

        If _itemOverrideItemList.SelectedItem Is Nothing Then
            Return
        End If


        If (HQ.itemList.TryGetValue(_itemOverrideItemList.SelectedItem.ToString, item) = False) Or (Integer.TryParse(item, itemID) = False) Then
            Return 'not a real item
        End If

        Dim override As New ItemMarketOverride()
        override.ItemId = itemID


        If (_itemOverrideAllOrders.Checked) Then
            override.TransactionType = MarketTransactionKind.All
        ElseIf _itemOverrideBuyOrders.Checked Then
            override.TransactionType = MarketTransactionKind.Buy
        Else
            override.TransactionType = MarketTransactionKind.Sell
        End If

        If (_itemOverrideAvgPrice.Checked) Then
            override.MarketStat = MarketMetric.Average
        ElseIf _itemOverrideMaxPrice.Checked Then
            override.MarketStat = MarketMetric.Maximum
        ElseIf _itemOverrideMedianPrice.Checked Then
            override.MarketStat = MarketMetric.Median
        ElseIf _itemOverridePercentPrice.Checked Then
            override.MarketStat = MarketMetric.Percentile
        Else
            override.MarketStat = MarketMetric.Minimum
        End If

        Dim existing As ItemMarketOverride
        If HQ.EveHqSettings.MarketStatOverrides.TryGetValue(override.ItemId, existing) Then
            HQ.EveHqSettings.MarketStatOverrides(override.ItemId) = override
        Else
            HQ.EveHqSettings.MarketStatOverrides.Add(override.ItemId, override)
        End If

        UpdateActiveOverrides()
    End Sub

    Private Sub OnOverrideGridNodeClick(sender As System.Object, e As DevComponents.AdvTree.TreeNodeMouseEventArgs) Handles _itemOverridesActiveGrid.NodeClick


        If (e.Node IsNot Nothing) Then
            _itemOverrideItemList.SelectedItem = e.Node.Text
        End If
    End Sub

    Private Sub UpdateActiveOverrides()
        _itemOverridesActiveGrid.Nodes.Clear()

        For Each override As ItemMarketOverride In HQ.EveHqSettings.MarketStatOverrides.Values
            Dim node As New Node()
            node.Text = HQ.itemData(override.ItemId.ToInvariantString).Name
            node.Cells.Add(New Cell(override.ItemId.ToInvariantString))
            node.Cells.Add(New Cell(override.TransactionType.ToString))
            node.Cells.Add(New Cell(override.MarketStat.ToString))

            _itemOverridesActiveGrid.Nodes.Add(node)
        Next

    End Sub



    Private Sub OnRemoveOverrideClick(sender As System.Object, e As System.EventArgs) Handles _itemOverrideRemoveOverride.Click
        Dim item As String
        Dim itemID As Integer

        If _itemOverrideItemList Is Nothing Then
            Return
        End If

        If _itemOverrideItemList.SelectedItem Is Nothing Then
            Return
        End If

        If (HQ.itemList.TryGetValue(_itemOverrideItemList.SelectedItem.ToString, item) = False) Or (Integer.TryParse(item, itemID) = False) Then
            Return 'not a real item
        End If

        HQ.EveHqSettings.MarketStatOverrides.Remove(itemID)
        UpdateActiveOverrides()

    End Sub



    Private Sub OnMarketProviderChanged(sender As System.Object, e As System.EventArgs) Handles _marketDataProvider.SelectedIndexChanged
        ChangeMarketProvider(_marketDataProvider.SelectedItem.ToString())
        UpdateDataSourceList()
    End Sub

    Private Sub ChangeMarketProvider(providerName As String)
        If (providerName = EveHqMarketDataProvider.Name) Then
            HQ.MarketStatDataProvider = HQ.GetEveHqMarketInstance(HQ.AppDataFolder)
        ElseIf providerName = EveCentralMarketDataProvider.Name Then
            HQ.MarketStatDataProvider = HQ.GetEveCentralMarketInstance(HQ.AppDataFolder)
        End If
    End Sub

#End Region


    Private Sub RedrawQueueColumnList()
        ' Setup the listview
        Dim newCol As New ListViewItem
        lvwColumns.BeginUpdate()
        lvwColumns.Items.Clear()
        For Each slot As String In HQ.EveHqSettings.UserQueueColumns
            For Each stdSlot As ListViewItem In HQ.EveHqSettings.StandardQueueColumns
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

    Private Sub btnMoveUp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMoveUp.Click
        ' Check we have something selected
        If lvwColumns.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select an item before trying it move it!", "Item Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        ' Save the selected item
        ' Get the slot name of the item selected
        Dim slotName As String = lvwColumns.SelectedItems(0).Name
        Dim selName As String = slotName
        ' Find the index in the user column list
        Dim idx As Integer = HQ.EveHqSettings.UserQueueColumns.IndexOf(slotName & "0")
        If idx = -1 Then
            idx = HQ.EveHqSettings.UserQueueColumns.IndexOf(slotName & "1")
        End If
        ' Switch with the one above if the index is not zero
        If idx <> 0 Then
            slotName = CStr(HQ.EveHqSettings.UserQueueColumns(idx - 1))
            HQ.EveHqSettings.UserQueueColumns(idx - 1) = HQ.EveHqSettings.UserQueueColumns(idx)
            HQ.EveHqSettings.UserQueueColumns(idx) = slotName
            ' Redraw the list
            redrawColumns = True
            Call Me.RedrawQueueColumnList()
            redrawColumns = False
            lvwColumns.Items(selName).Selected = True
            lvwColumns.Select()
        End If
    End Sub

    Private Sub btnMoveDown_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnMoveDown.Click
        ' Check we have something selected
        If lvwColumns.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select an item before trying it move it!", "Item Required", MessageBoxButtons.OK,
                            MessageBoxIcon.Information)
            Exit Sub
        End If
        ' Get the slot name of the item selected
        Dim slotName As String = lvwColumns.SelectedItems(0).Name
        Dim selName As String = slotName
        ' Find the index in the user column list
        Dim idx As Integer = HQ.EveHqSettings.UserQueueColumns.IndexOf(slotName & "0")
        If idx = -1 Then
            idx = HQ.EveHqSettings.UserQueueColumns.IndexOf(slotName & "1")
        End If
        ' Switch with the one above if the index is not the last
        If idx <> HQ.EveHqSettings.UserQueueColumns.Count - 1 Then
            slotName = CStr(HQ.EveHqSettings.UserQueueColumns(idx + 1))
            HQ.EveHqSettings.UserQueueColumns(idx + 1) = HQ.EveHqSettings.UserQueueColumns(idx)
            HQ.EveHqSettings.UserQueueColumns(idx) = slotName
            ' Redraw the list
            redrawColumns = True
            Call Me.RedrawQueueColumnList()
            redrawColumns = False
            lvwColumns.Items(selName).Selected = True
            lvwColumns.Select()
        End If
    End Sub

    Private Sub lvwColumns_ItemChecked(ByVal sender As Object, ByVal e As ItemCheckedEventArgs) _
        Handles lvwColumns.ItemChecked
        If redrawColumns = False Then
            ' Get the slot name of the ticked item
            Dim slotName As String = e.Item.Name
            ' Find the index in the user column list
            Dim idx As Integer = HQ.EveHqSettings.UserQueueColumns.IndexOf(slotName & "0")
            If idx = -1 Then
                idx = HQ.EveHqSettings.UserQueueColumns.IndexOf(slotName & "1")
            End If
            If e.Item.Checked = False Then
                HQ.EveHqSettings.UserQueueColumns(idx) = slotName & "0"
            Else
                HQ.EveHqSettings.UserQueueColumns(idx) = slotName & "1"
            End If
        End If
    End Sub

    Public Sub FinaliseAPIServerUpdate()
        btnGetData.Enabled = True
        Call Me.UpdatePilots()
    End Sub

    Private Sub clb_IGBAllowedDisplay_ItemCheck(ByVal sender As Object, ByVal e As ItemCheckEventArgs) _
        Handles clb_IGBAllowedDisplay.ItemCheck
        If startup = False Then
            Dim cbx As String = clb_IGBAllowedDisplay.SelectedItem.ToString()
            Dim chkSt As Boolean

            If (e.NewValue = CheckState.Checked) Then
                chkSt = True
            Else
                chkSt = False
            End If

            HQ.EveHqSettings.IGBAllowedData(cbx) = chkSt
        End If
    End Sub



End Class
