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
Imports Microsoft.Win32
Imports System.Data
Imports System.Data.OleDb
Imports System.Data.Odbc
Imports System.Data.SqlClient
Imports System.Xml
Imports System.IO
Imports System.Net

Public Class frmSettings
    Dim redrawColumns As Boolean = False
    Dim startup As Boolean = True

#Region "Form Opening & Closing"
    Private Sub frmSettings_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Call EveHQ.Core.EveHQSettingsFunctions.SaveSettings()
        If frmTraining.IsHandleCreated = True And redrawColumns = True Then
            redrawColumns = False
            Call frmTraining.RefreshAllTrainingQueues()
        End If
        Call frmEveHQ.UpdatePilotInfo()
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
        Call Me.UpdateFTPAccounts()
        Call Me.UpdateViewPilots()
        Call Me.UpdateProxyOptions()
        Call Me.UpdatePlugIns()
        Call Me.UpdateNotificationOptions()
        Call Me.UpdateTrainingOverlayOptions()
        Call Me.UpdateTrainingQueueOptions()
        Call Me.UpdateG15Options()
        Call Me.UpdateDatabaseSettings()
        Call Me.UpdateTaskBarIconOptions()

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
        For Each setControl As Control In Me.Controls
            If setControl.Name = "tvwSettings" Or setControl.Name = "btnClose" Or setControl.Name = gbName Then
                Me.Controls(gbName).Top = 12
                Me.Controls(gbName).Left = 195
                Me.Controls(gbName).Width = 700
                Me.Controls(gbName).Height = 500
                Me.Controls(gbName).Visible = True
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
        chkEncryptSettings.Checked = EveHQ.Core.HQ.EveHQSettings.EncryptSettings
        cboMDITabStyle.SelectedIndex = EveHQ.Core.HQ.EveHQSettings.MDITabStyle
        chkAutoHide.Checked = EveHQ.Core.HQ.EveHQSettings.AutoHide
        chkAutoRun.Checked = EveHQ.Core.HQ.EveHQSettings.AutoStart
        chkAutoMinimise.Checked = EveHQ.Core.HQ.EveHQSettings.AutoMinimise
        chkAutoCheck.Checked = EveHQ.Core.HQ.EveHQSettings.AutoCheck
        chkMinimiseOnExit.Checked = EveHQ.Core.HQ.EveHQSettings.MinimiseExit
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

    End Sub

    Private Sub chkAutoHide_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAutoHide.CheckedChanged
        If chkAutoHide.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.AutoHide = True
        Else
            EveHQ.Core.HQ.EveHQSettings.AutoHide = False
        End If
    End Sub

    Private Sub chkAutoRun_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAutoRun.CheckedChanged
        If chkAutoRun.Checked = True Then
            Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", True)
            RegKey.SetValue(Application.ProductName, Application.ExecutablePath.ToString)
            EveHQ.Core.HQ.EveHQSettings.AutoStart = True
        Else
            Dim RegKey As RegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", True)
            RegKey.DeleteValue(Application.ProductName, False)
            EveHQ.Core.HQ.EveHQSettings.AutoStart = False
        End If
    End Sub

    Private Sub chkAutoMinimise_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoMinimise.CheckedChanged
        If chkAutoMinimise.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.AutoMinimise = True
        Else
            EveHQ.Core.HQ.EveHQSettings.AutoMinimise = False
        End If
    End Sub

    Private Sub chkMinimiseOnExit_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkMinimiseOnExit.CheckedChanged
        If chkMinimiseOnExit.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.MinimiseExit = True
        Else
            EveHQ.Core.HQ.EveHQSettings.MinimiseExit = False
        End If
    End Sub

    Private Sub chkAutoCheck_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoCheck.CheckedChanged
        If chkAutoCheck.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.AutoCheck = True
        Else
            EveHQ.Core.HQ.EveHQSettings.AutoCheck = False
        End If
    End Sub

    Private Sub chkEncryptSettings_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEncryptSettings.CheckedChanged
        If chkEncryptSettings.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.EncryptSettings = True
        Else
            EveHQ.Core.HQ.EveHQSettings.EncryptSettings = False
        End If
    End Sub

    Private Sub cboStartupView_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboStartupView.SelectedIndexChanged
        EveHQ.Core.HQ.EveHQSettings.StartupView = CStr(cboStartupView.SelectedItem)
    End Sub

    Private Sub cboStartupPilot_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboStartupPilot.SelectedIndexChanged
        EveHQ.Core.HQ.EveHQSettings.StartupPilot = CStr(cboStartupPilot.SelectedItem)
    End Sub

    Private Sub cboMDITabStyle_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboMDITabStyle.SelectedIndexChanged
        Select Case cboMDITabStyle.SelectedIndex
            Case 0
                frmEveHQ.tabMDI.Appearance = TabAppearance.FlatButtons
            Case 1
                frmEveHQ.tabMDI.Appearance = TabAppearance.Buttons
            Case 2
                frmEveHQ.tabMDI.Appearance = TabAppearance.Normal
        End Select
        EveHQ.Core.HQ.EveHQSettings.MDITabStyle = cboMDITabStyle.SelectedIndex
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

#End Region

#Region "Colour Settings"
    Private Sub UpdateColourOptions()
        ' Update the panel colours
        Call Me.UpdatePBPanelColours()
        ' Update the pilot colours
        Call Me.UpdatePBPilotColours()
    End Sub
    Private Sub UpdatePBPanelColours()
        pbPanelBackground.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelBackgroundColor))
        pbPanelBottomRight.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelBottomRightColor))
        pbPanelHighlight.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelHighlightColor))
        pbPanelLeft.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelLeftColor))
        pbPanelOutline.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelOutlineColor))
        pbPanelRight.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelRightColor))
        pbPanelText.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelTextColor))
        pbPanelTopLeft.BackColor = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelTopLeftColor))
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

    Private Sub pbPanelColours_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbPanelBackground.Click, pbPanelBottomRight.Click, pbPanelHighlight.Click, pbPanelLeft.Click, pbPanelOutline.Click, pbPanelRight.Click, pbPanelText.Click, pbPanelTopLeft.Click
        Dim thisPB As PictureBox = CType(sender, PictureBox)
        Dim dlgResult As Integer = 0
        With cd1
            .AllowFullOpen = True
            .AnyColor = True
            .FullOpen = True
            Select Case thisPB.Name
                Case "pbPanelBackground"
                    .Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelBackgroundColor))
                Case "pbPanelBottomRight"
                    .Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelBottomRightColor))
                Case "pbPanelHighlight"
                    .Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelHighlightColor))
                Case "pbPanelLeft"
                    .Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelLeftColor))
                Case "pbPanelOutline"
                    .Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelOutlineColor))
                Case "pbPanelRight"
                    .Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelRightColor))
                Case "pbPanelText"
                    .Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelTextColor))
                Case "pbPanelTopLeft"
                    .Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.PanelTopLeftColor))
            End Select
            dlgResult = .ShowDialog()
        End With
        If dlgResult = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        Else
            thisPB.BackColor = cd1.Color
            Select Case thisPB.Name
                Case "pbPanelBackground"
                    EveHQ.Core.HQ.EveHQSettings.PanelBackgroundColor = cd1.Color.ToArgb
                Case "pbPanelBottomRight"
                    EveHQ.Core.HQ.EveHQSettings.PanelBottomRightColor = cd1.Color.ToArgb
                Case "pbPanelHighlight"
                    EveHQ.Core.HQ.EveHQSettings.PanelHighlightColor = cd1.Color.ToArgb
                Case "pbPanelLeft"
                    EveHQ.Core.HQ.EveHQSettings.PanelLeftColor = cd1.Color.ToArgb
                Case "pbPanelOutline"
                    EveHQ.Core.HQ.EveHQSettings.PanelOutlineColor = cd1.Color.ToArgb
                Case "pbPanelRight"
                    EveHQ.Core.HQ.EveHQSettings.PanelRightColor = cd1.Color.ToArgb
                Case "pbPanelText"
                    EveHQ.Core.HQ.EveHQSettings.PanelTextColor = cd1.Color.ToArgb
                Case "pbPanelTopLeft"
                    EveHQ.Core.HQ.EveHQSettings.PanelTopLeftColor = cd1.Color.ToArgb
            End Select
            ' Update the colours
            Call frmEveHQ.UpdatePanelColours()
        End If
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
            frmPilot.clvSkills.Refresh()
        End If
    End Sub

    Private Sub btnResetPanelColours_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnResetPanelColours.Click
        ' Resets the panel colours to the default values
        EveHQ.Core.HQ.EveHQSettings.PanelBackgroundColor = System.Drawing.Color.Navy.ToArgb
        EveHQ.Core.HQ.EveHQSettings.PanelOutlineColor = System.Drawing.Color.SteelBlue.ToArgb
        EveHQ.Core.HQ.EveHQSettings.PanelTopLeftColor = System.Drawing.Color.LightSteelBlue.ToArgb
        EveHQ.Core.HQ.EveHQSettings.PanelBottomRightColor = System.Drawing.Color.LightSteelBlue.ToArgb
        EveHQ.Core.HQ.EveHQSettings.PanelLeftColor = System.Drawing.Color.RoyalBlue.ToArgb
        EveHQ.Core.HQ.EveHQSettings.PanelRightColor = System.Drawing.Color.LightSteelBlue.ToArgb
        EveHQ.Core.HQ.EveHQSettings.PanelTextColor = System.Drawing.Color.Black.ToArgb
        EveHQ.Core.HQ.EveHQSettings.PanelHighlightColor = System.Drawing.Color.LightSteelBlue.ToArgb
        ' Update the colours
        Call frmEveHQ.UpdatePanelColours()
        ' Update the PBPanel Colours
        Call Me.UpdatePBPanelColours()
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
        frmPilot.clvSkills.Refresh()
        ' Update the PBPilot Colours
        Call Me.UpdatePBPilotColours()
    End Sub
#End Region

#Region "Eve Accounts Settings"
    Private Sub btnAddAccount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAccount.Click
        ' Clear the text boxes
        Dim myAccounts As frmModifyEveAccounts = New frmModifyEveAccounts
        With myAccounts
            .Tag = "Add"
            .txtUserID.Text = "" : .txtUserID.Enabled = True
            .txtAPIKey.Text = "" : .txtAPIKey.Enabled = True
            .txtAccountName.Text = "" : .txtAccountName.Enabled = True
            .btnAccept.Text = "OK"
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
        If lvAccounts.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select an account to edit!", "Cannot Edit", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            lvAccounts.Select()
        Else
            Dim myAccounts As frmModifyEveAccounts = New frmModifyEveAccounts
            With myAccounts
                ' Load the account details into the text boxes
                Dim selAccount As New EveHQ.Core.EveAccount
                selAccount = CType(EveHQ.Core.HQ.EveHQSettings.Accounts(lvAccounts.SelectedItems(0).Name), Core.EveAccount)
                .txtUserID.Text = selAccount.userID : .txtUserID.Enabled = False
                .txtAPIKey.Text = selAccount.APIKey : .txtAPIKey.Enabled = True
                .txtAccountName.Text = selAccount.FriendlyName : .txtAccountName.Enabled = True
                .btnAccept.Text = "OK" : .Tag = "Edit"
                .Text = "Edit '" & selAccount.FriendlyName & "' Account Details"
                ' Disable the username text box (cannot edit this by design!!)
                .ShowDialog()
            End With
            Me.UpdateAccounts()
        End If
    End Sub

    Private Sub btnDeleteAccount_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteAccount.Click
        ' Check for some selection on the listview
        If lvAccounts.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select an account to delete!", "Cannot Delete Account", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            lvAccounts.Select()
        Else
            Dim selAccount As String = lvAccounts.SelectedItems(0).Name
            Dim selAccountName As String = lvAccounts.SelectedItems(0).Text
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
                EveHQ.Core.HQ.EveHQSettings.Accounts.Remove(lvAccounts.SelectedItems(0).Name)
                ' Remove the item from the list
                lvAccounts.Items.Remove(lvAccounts.SelectedItems(0))
                ' Remove the pilots
                For Each dPilot In EveHQ.Core.HQ.EveHQSettings.Pilots
                    If dPilot.Account = selAccount Then
                        EveHQ.Core.HQ.EveHQSettings.Pilots.Remove(dPilot.Name)
                        If dPilot.Name = EveHQ.Core.HQ.myPilot.Name Then
                            EveHQ.Core.HQ.myPilot = New EveHQ.Core.Pilot
                            Call frmPilot.UpdatePilotInfo()
                        End If
                    End If
                Next
                Call frmEveHQ.UpdatePilotInfo()
                Call Me.UpdatePilots()
            Else
                lvAccounts.Select()
                Exit Sub
            End If
        End If
    End Sub

    Private Sub btnGetData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetData.Click
        Call frmEveHQ.QueryMyEveServer()
        Call Me.UpdatePilots()
    End Sub

    Public Sub UpdateAccounts()
        lvAccounts.Items.Clear()
        Dim newAccount As EveHQ.Core.EveAccount = New EveHQ.Core.EveAccount
        For Each newAccount In EveHQ.Core.HQ.EveHQSettings.Accounts
            Dim newLine As New ListViewItem
            newLine.Name = newAccount.userID
            newLine.Text = newAccount.FriendlyName
            newLine.SubItems.Add(newAccount.userID)
            lvAccounts.Items.Add(newLine)
        Next
        If EveHQ.Core.HQ.EveHQSettings.Accounts.Count = 0 Then
            frmEveHQ.mnuToolsGetAccountInfo.Enabled = False
        Else
            frmEveHQ.mnuToolsGetAccountInfo.Enabled = True
        End If
    End Sub

    Private Sub lvAccounts_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvAccounts.ColumnClick
        If CInt(lvAccounts.Tag) = e.Column Then
            Me.lvAccounts.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, Windows.Forms.SortOrder.Ascending)
            lvAccounts.Tag = -1
        Else
            Me.lvAccounts.ListViewItemSorter = New EveHQ.Core.ListViewItemComparer_Text(e.Column, Windows.Forms.SortOrder.Descending)
            lvAccounts.Tag = e.Column
        End If
        ' Call the sort method to manually sort.
        lvAccounts.Sort()
    End Sub

    Private Sub lvAccounts_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvAccounts.DoubleClick
        Call EditAccount()
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
            lvAccounts.Select()
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
                lvAccounts.Select()
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
        nudIGBPort.Value = EveHQ.Core.HQ.EveHQSettings.IGBPort
        chkStartIGBonLoad.Checked = EveHQ.Core.HQ.EveHQSettings.IGBAutoStart
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
#End Region

#Region "FTP Settings"
    Private Sub btnAddFTP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddFTP.Click
        ' Clear the text boxes
        Dim myFTPAccounts As frmModifyFTPAccounts = New frmModifyFTPAccounts
        With myFTPAccounts
            .txtFTPName.Text = "" : .txtFTPName.Enabled = True
            .txtServer.Text = "" : .txtServer.Enabled = True
            .txtPort.Value = 21 : .txtPort.Enabled = True
            .txtPath.Text = "" : .txtPath.Enabled = True
            .txtUsername.Text = "" : .txtUsername.Enabled = True
            .txtPassword.Text = "" : .txtPassword.Enabled = True
            .btnAccept.Text = "Add"
            .Text = "Add New FTP Account"
            .ShowDialog()
        End With
        Me.UpdateFTPAccounts()
    End Sub

    Private Sub btnEditFTP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditFTP.Click
        ' Check for some selection on the listview
        If lvwFTP.SelectedItems.Count = 0 Then
            lvwFTP.Select()
            MessageBox.Show("Please select an FTP account to edit!", "Cannot Edit", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        Else
            Dim myFTPAccounts As frmModifyFTPAccounts = New frmModifyFTPAccounts
            With myFTPAccounts
                ' Load the account details into the text boxes
                Dim selAccount As New EveHQ.Core.FTPAccount
                selAccount = CType(EveHQ.Core.HQ.EveHQSettings.FTPAccounts(lvwFTP.SelectedItems(0).Text), Core.FTPAccount)
                .txtFTPName.Text = selAccount.FTPName : .txtFTPName.Enabled = False
                .txtServer.Text = selAccount.Server : .txtServer.Enabled = True
                .txtPort.Value = selAccount.Port : .txtPort.Enabled = True
                .txtPath.Text = selAccount.Path : .txtPath.Enabled = True
                .txtUsername.Text = selAccount.Username : .txtUsername.Enabled = True
                .txtPassword.Text = selAccount.Password : .txtPassword.Enabled = True
                .btnAccept.Text = "Edit"
                .Text = "Edit '" & selAccount.FTPName & "' FTP Account Details"
                ' Disable the FTPName text box (cannot edit this by design!!)
                .ShowDialog()
            End With
            Me.UpdateFTPAccounts()
        End If
    End Sub

    Private Sub btnDeleteFTP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteFTP.Click
        ' Check for some selection on the listview
        If lvwFTP.SelectedItems.Count = 0 Then
            MessageBox.Show("Please select an FTP account to delete!", "Cannot Delete Account", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            lvAccounts.Select()
        Else
            Dim selAccount As String = lvwFTP.SelectedItems(0).Text
            ' Confirm deletion
            Dim msg As String = ""
            msg &= "Are you sure you wish to delete the account '" & selAccount & "'?"
            Dim confirm As Integer = MessageBox.Show(msg, "Confirm Delete?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            If confirm = vbYes Then
                ' Delete the account from the accounts collection
                EveHQ.Core.HQ.EveHQSettings.FTPAccounts.Remove(lvwFTP.SelectedItems(0).Text)
                ' Remove the item from the list
                lvwFTP.Items.Remove(lvwFTP.SelectedItems(0))
            Else
                lvwFTP.Select()
                Exit Sub
            End If
        End If
    End Sub

    Public Sub UpdateFTPAccounts()
        lvwFTP.Items.Clear()
        Dim newAccount As EveHQ.Core.FTPAccount = New EveHQ.Core.FTPAccount
        For Each newAccount In EveHQ.Core.HQ.EveHQSettings.FTPAccounts
            Dim newFTP As ListViewItem = New ListViewItem
            newFTP.Text = newAccount.FTPName
            newFTP.SubItems.Add(newAccount.Server)
            lvwFTP.Items.Add(newFTP)
        Next
    End Sub

    Private Sub btnTestUpload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTestUpload.Click
        If lvwFTP.SelectedItems.Count = 0 Then
            MessageBox.Show("You need to select an FTP account first", "Upload Error!", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        ' Load the account details into the text boxes
        Dim selAccount As New EveHQ.Core.FTPAccount
        selAccount = CType(EveHQ.Core.HQ.EveHQSettings.FTPAccounts(lvwFTP.SelectedItems(0).Text), Core.FTPAccount)

        Dim fileName As String = "CharSheet (Vessper).html"
        Dim localPath As String = EveHQ.Core.HQ.reportFolder

        Dim myFTP As EveFTP = New EveFTP
        myFTP.UploadFile(localPath & "\" & fileName, selAccount.Server, selAccount.Path)
    End Sub
#End Region

#Region "Training Queue Options"
    Private Sub clbColumns_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles clbColumns.ItemCheck
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

    Private Sub chkContinueTraining_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkContinueTraining.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.ContinueTraining = chkContinueTraining.Checked
    End Sub

    Private Sub chkShowCompletedSkills_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowCompletedSkills.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.ShowCompletedSkills = chkShowCompletedSkills.Checked
    End Sub

    Private Sub chkDeleteCompletedSkills_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDeleteCompletedSkills.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.DeleteSkills = chkDeleteCompletedSkills.Checked
    End Sub

    Private Sub UpdateTrainingQueueOptions()
        ' Add the Queue columns
        Me.clbColumns.Items.Clear()
        For a As Integer = 0 To 16
            Select Case EveHQ.Core.HQ.EveHQSettings.QColumns(a, 0)
                Case "Name"
                    Me.clbColumns.Items.Add("Skill Name")
                Case "Curr"
                    Me.clbColumns.Items.Add("Current Level")
                Case "From"
                    Me.clbColumns.Items.Add("From Level")
                Case "Tole"
                    Me.clbColumns.Items.Add("To Level")
                Case "Perc"
                    Me.clbColumns.Items.Add("% Complete")
                Case "Trai"
                    Me.clbColumns.Items.Add("Training Time")
                Case "Date"
                    Me.clbColumns.Items.Add("Date Completed")
                Case "Rank"
                    Me.clbColumns.Items.Add("Skill Rank")
                Case "PAtt"
                    Me.clbColumns.Items.Add("Primary Attribute")
                Case "SAtt"
                    Me.clbColumns.Items.Add("Secondary Atribute")
                Case "SPRH"
                    Me.clbColumns.Items.Add("SP Rate/Hour")
                Case "SPRD"
                    Me.clbColumns.Items.Add("SP Rate/Day")
                Case "SPRW"
                    Me.clbColumns.Items.Add("SP Rate/Week")
                Case "SPRM"
                    Me.clbColumns.Items.Add("SP Rate/Month")
                Case "SPRY"
                    Me.clbColumns.Items.Add("SP Rate/Year")
                Case "SPAd"
                    Me.clbColumns.Items.Add("SP Earned")
                Case "SPTo"
                    Me.clbColumns.Items.Add("SP Total")
            End Select
            If CBool(EveHQ.Core.HQ.EveHQSettings.QColumns(a, 1)) = True Then
                Me.clbColumns.SetItemChecked(a, True)
            Else
                Me.clbColumns.SetItemChecked(a, False)
            End If
        Next
        Me.chkContinueTraining.Checked = EveHQ.Core.HQ.EveHQSettings.ContinueTraining
        Me.chkDeleteCompletedSkills.Checked = EveHQ.Core.HQ.EveHQSettings.DeleteSkills
        Me.chkShowCompletedSkills.Checked = EveHQ.Core.HQ.EveHQSettings.ShowCompletedSkills
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
                gbAccess.Left = 6 : gbAccess.Top = 80 : gbAccess.Width = 400 : gbAccess.Height = 175
                txtMDBServer.Text = EveHQ.Core.HQ.EveHQSettings.DBFilename
                txtMDBServer2.Text = EveHQ.Core.HQ.EveHQSettings.DBDataFilename
                txtMDBUsername.Text = EveHQ.Core.HQ.EveHQSettings.DBUsername
                txtMDBPassword.Text = EveHQ.Core.HQ.EveHQSettings.DBPassword
                gbAccess.Visible = True : gbMSSQL.Visible = False
            Case 1, 2
                gbMSSQL.Left = 6 : gbMSSQL.Top = 80 : gbMSSQL.Width = 400 : gbMSSQL.Height = 175
                txtMSSQLServer.Text = EveHQ.Core.HQ.EveHQSettings.DBServer
                txtMSSQLDatabase.Text = EveHQ.Core.HQ.EveHQSettings.DBName
                txtMSSQLDatabase2.Text = EveHQ.Core.HQ.EveHQSettings.DBDataName
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
            .Title = "Select Access Data file"
            .FileName = ""
            .InitialDirectory = "c:\"
            .Filter = "Access Data files (*.mdb)|*.mdb|All files (*.*)|*.*"
            .FilterIndex = 1
            .RestoreDirectory = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                txtMDBServer.Text = .FileName
            End If
        End With
    End Sub

    Private Sub btnBrowseMDB2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseMDB2.Click
        With ofd1
            .Title = "Select Access Data file"
            .FileName = ""
            .InitialDirectory = "c:\"
            .Filter = "Access Data files (*.mdb)|*.mdb|All files (*.*)|*.*"
            .FilterIndex = 1
            .RestoreDirectory = True
            If .ShowDialog() = Windows.Forms.DialogResult.OK Then
                txtMDBServer2.Text = .FileName
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

    Private Sub txtMDBServer2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMDBServer2.TextChanged
        EveHQ.Core.HQ.EveHQSettings.DBDataFilename = txtMDBServer2.Text
    End Sub

    Private Sub txtMSSQLDatabase_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMSSQLDatabase.TextChanged
        EveHQ.Core.HQ.EveHQSettings.DBName = txtMSSQLDatabase.Text
    End Sub

    Private Sub txtMSSQLDatabase2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMSSQLDatabase2.TextChanged
        EveHQ.Core.HQ.EveHQSettings.DBDataName = txtMSSQLDatabase2.Text
    End Sub

    Private Sub btnTestDB_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTestDB.Click
        Dim strConnection As String = ""
        Select Case EveHQ.Core.HQ.EveHQSettings.DBFormat
            Case 0
                strConnection = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & EveHQ.Core.HQ.EveHQSettings.DBFilename & ";"
                Dim connection As New OleDbConnection(strConnection)
                Try
                    connection.Open()
                    connection.Close()
                    MessageBox.Show("Connected successfully to Access database", "Connection Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "Error Opening Access Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Case 1
                strConnection = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer
                If EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = True Then
                    strConnection += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBName & "; User ID=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & "; Password=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
                Else
                    strConnection += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBName & "; Integrated Security = SSPI;"
                End If
                Dim connection As New SqlConnection(strConnection)
                Try
                    connection.Open()
                    connection.Close()
                    MessageBox.Show("Connected successfully to MS SQL database", "Connection Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Catch ex As Exception
                    MessageBox.Show(ex.Message, "Error Opening MS SQL Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            Case 2
                strConnection = "Server=" & EveHQ.Core.HQ.EveHQSettings.DBServer & "\SQLEXPRESS"
                If EveHQ.Core.HQ.EveHQSettings.DBSQLSecurity = True Then
                    strConnection += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBName & "; User ID=" & EveHQ.Core.HQ.EveHQSettings.DBUsername & "; Password=" & EveHQ.Core.HQ.EveHQSettings.DBPassword & ";"
                Else
                    strConnection += "; Database = " & EveHQ.Core.HQ.EveHQSettings.DBName & "; Integrated Security = SSPI;"
                End If
                Dim connection As New SqlConnection(strConnection)
                Try
                    connection.Open()
                    connection.Close()
                    MessageBox.Show("Connected successfully to MS SQL EXPRESS database", "Connection Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "Error Opening MS SQL EXPRESS Database", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
        End Select
    End Sub

    Private Sub chkUseAppDirForDB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseAppDirForDB.CheckedChanged
        EveHQ.Core.HQ.EveHQSettings.UseAppDirectoryForDB = Me.chkUseAppDirForDB.Checked
    End Sub

#End Region

#Region "Proxy Server Options"

    Private Sub UpdateProxyOptions()
        If EveHQ.Core.HQ.EveHQSettings.ProxyRequired = True Then
            chkUseProxy.Checked = True
        Else
            chkUseProxy.Checked = False
        End If
        txtProxyUsername.Text = EveHQ.Core.HQ.EveHQSettings.ProxyUsername
        txtProxyPassword.Text = EveHQ.Core.HQ.EveHQSettings.ProxyPassword
        txtProxyServer.Text = EveHQ.Core.HQ.EveHQSettings.ProxyServer
        If EveHQ.Core.HQ.EveHQSettings.ProxyUseDefault = True Then
            radUseDefaultCreds.Checked = True
        Else
            radUseSpecifiedCreds.Checked = True
        End If
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
    Private Sub radUseDefaultCreds_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radUseDefaultCreds.CheckedChanged

        If radUseDefaultCreds.Checked = True Then
            lblProxyUsername.Enabled = False
            lblProxyPassword.Enabled = False
            txtProxyUsername.Enabled = False
            txtProxyPassword.Enabled = False
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
        trackServerOffset.Value = EveHQ.Core.HQ.EveHQSettings.ServerOffset
        chkAutoAPI.Checked = EveHQ.Core.HQ.EveHQSettings.AutoAPI
        If System.Net.HttpListener.IsSupported Then
            nudAPIRSPort.Value = EveHQ.Core.HQ.EveHQSettings.APIRSPort
            chkAPIRSAutoStart.Checked = EveHQ.Core.HQ.EveHQSettings.APIRSAutoStart
            If EveHQ.Core.HQ.APIRSActive = True Then
                chkActivateAPIRS.Checked = True
            End If
        Else
            chkActivateAPIRS.Checked = False
            chkActivateAPIRS.Enabled = False
            nudAPIRSPort.Enabled = False
            chkAPIRSAutoStart.Enabled = False
        End If
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
    End Sub
    Private Sub trackServerOffset_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles trackServerOffset.ValueChanged
        EveHQ.Core.HQ.EveHQSettings.ServerOffset = trackServerOffset.Value
        For Each newPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            newPilot.TrainingEndTime = newPilot.TrainingEndTimeActual.AddSeconds(EveHQ.Core.HQ.EveHQSettings.ServerOffset)
            newPilot.TrainingStartTime = newPilot.TrainingStartTimeActual.AddSeconds(EveHQ.Core.HQ.EveHQSettings.ServerOffset)
        Next
        Call UpdateTimeOffset()
    End Sub
    Public Sub UpdateTimeOffset()
        Dim offset As String = EveHQ.Core.SkillFunctions.TimeToStringAll(trackServerOffset.Value)
        Dim skillOffset As String = ""
        If EveHQ.Core.HQ.myPilot.Training = True Then
            skillOffset = " (Skill Time Remaining: " & EveHQ.Core.SkillFunctions.TimeToString(EveHQ.Core.HQ.myPilot.TrainingCurrentTime) & ")"
        Else
            skillOffset = ""
        End If
        lblCurrentOffset.Text = "Current Offset: " & offset & skillOffset
    End Sub
    Private Sub chkEnableEveStatus_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnableEveStatus.CheckedChanged
        If chkEnableEveStatus.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.EnableEveStatus = True
            frmEveHQ.tmrEve.Interval = 100
            frmEveHQ.tmrEve.Enabled = True
        Else
            EveHQ.Core.HQ.EveHQSettings.EnableEveStatus = False
            frmEveHQ.EveStatusIcon.Icon = My.Resources.EveHQ
            frmEveHQ.EveStatusIcon.Text = "EveHQ"
            frmEveHQ.tmrEve.Enabled = False
        End If
    End Sub
    Private Sub chkAutoAPI_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAutoAPI.CheckedChanged
        If chkAutoAPI.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.AutoAPI = True
        Else
            EveHQ.Core.HQ.EveHQSettings.AutoAPI = False
        End If
    End Sub
    Private Sub chkActivateAPIRS_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkActivateAPIRS.CheckedChanged
        If chkActivateAPIRS.Checked = True Then
            If EveHQ.Core.HQ.APIRSActive = False Then
                If frmEveHQ.APIRSWorker.CancellationPending = True Then
                    MessageBox.Show("The API Relay Server is still shutting down. Please wait a few moments", "API Relay Server Busy", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    frmEveHQ.APIRSWorker.Dispose()
                    frmEveHQ.APIRSWorker = New System.ComponentModel.BackgroundWorker
                    frmEveHQ.APIRSWorker.WorkerSupportsCancellation = True
                Else
                    frmEveHQ.APIRSWorker = New System.ComponentModel.BackgroundWorker
                    frmEveHQ.APIRSWorker.WorkerSupportsCancellation = True
                    frmEveHQ.APIRSWorker.RunWorkerAsync()
                    EveHQ.Core.HQ.APIRSActive = True
                End If
            End If
        Else
            frmEveHQ.APIRSWorker.CancelAsync()
            EveHQ.Core.HQ.APIRSActive = False
        End If
    End Sub
    Private Sub nudAPIRSPort_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudAPIRSPort.Click
        EveHQ.Core.HQ.EveHQSettings.APIRSPort = CInt(nudAPIRSPort.Value)
    End Sub
    Private Sub nudAPIRSPort_HandleDestroyed(ByVal sender As Object, ByVal e As System.EventArgs) Handles nudAPIRSPort.HandleDestroyed
        EveHQ.Core.HQ.EveHQSettings.APIRSPort = CInt(nudAPIRSPort.Value)
    End Sub
    Private Sub nudAPIRSPort_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles nudAPIRSPort.KeyUp
        If e.KeyCode = Keys.Enter Then
            EveHQ.Core.HQ.EveHQSettings.APIRSPort = CInt(nudAPIRSPort.Value)
        End If
    End Sub
    Private Sub chkAPIRSAutoStart_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAPIRSAutoStart.CheckedChanged
        If chkAPIRSAutoStart.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.APIRSAutoStart = True
        Else
            EveHQ.Core.HQ.EveHQSettings.APIRSAutoStart = False
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
        If EveHQ.Core.HQ.EveHQSettings.ShutdownNotify = True Then
            Me.chkShutdownNotify.Checked = True
        Else
            Me.chkShutdownNotify.Checked = False
        End If
        If EveHQ.Core.HQ.EveHQSettings.NotifyToolTip = True Then
            Me.chkNotifyToolTip.Checked = True
        Else
            Me.chkNotifyToolTip.Checked = False
        End If
        If EveHQ.Core.HQ.EveHQSettings.NotifyDialog = True Then
            Me.chkNotifyDialog.Checked = True
        Else
            Me.chkNotifyDialog.Checked = False
        End If
        If EveHQ.Core.HQ.EveHQSettings.NotifySound = True Then
            Me.chkNotifySound.Checked = True
            btnSelectSoundFile.Enabled = True
            btnSoundTest.Enabled = True
        Else
            Me.chkNotifySound.Checked = False
            btnSelectSoundFile.Enabled = False
            btnSoundTest.Enabled = False
        End If
        If EveHQ.Core.HQ.EveHQSettings.NotifyNow = True Then
            Me.chkNotifyNow.Checked = True
        Else
            Me.chkNotifyNow.Checked = False
        End If
        If EveHQ.Core.HQ.EveHQSettings.NotifyEarly = True Then
            Me.chkNotifyEarly.Checked = True
        Else
            Me.chkNotifyEarly.Checked = False
        End If
        Me.lblSoundFile.Text = EveHQ.Core.HQ.EveHQSettings.NotifySoundFile
        If EveHQ.Core.HQ.EveHQSettings.NotifyEMail = True Then
            Me.chkNotifyEmail.Checked = True
            gbEmailOptions.Visible = True
        Else
            Me.chkNotifyEmail.Checked = False
            gbEmailOptions.Visible = False
        End If
        If EveHQ.Core.HQ.EveHQSettings.UseSMTPAuth = True Then
            Me.chkSMTPAuthentication.Checked = True
            lblEmailUsername.Enabled = True : lblEmailPassword.Enabled = True
            txtEmailUsername.Enabled = True : txtEmailPassword.Enabled = True
        Else
            Me.chkSMTPAuthentication.Checked = False
            lblEmailUsername.Enabled = False : lblEmailPassword.Enabled = False
            txtEmailUsername.Enabled = False : txtEmailPassword.Enabled = False
        End If
        Me.txtSMTPServer.Text = EveHQ.Core.HQ.EveHQSettings.EMailServer
        Me.txtSMTPPort.Text = CStr(EveHQ.Core.HQ.EveHQSettings.EMailPort)
        Me.txtEmailAddress.Text = EveHQ.Core.HQ.EveHQSettings.EMailAddress
        Me.txtEmailUsername.Text = EveHQ.Core.HQ.EveHQSettings.EMailUsername
        Me.txtEmailPassword.Text = EveHQ.Core.HQ.EveHQSettings.EMailPassword
        Me.trackNotifyOffset.Value = EveHQ.Core.HQ.EveHQSettings.NotifyOffset
        Dim offset As String = EveHQ.Core.SkillFunctions.TimeToStringAll(trackNotifyOffset.Value)
        lblNotifyOffset.Text = "Early Notification Offset: " & offset
        Me.nudShutdownNotifyPeriod.Value = EveHQ.Core.HQ.EveHQSettings.ShutdownNotifyPeriod
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
        If chkNotifyNow.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.NotifyNow = True
        Else
            EveHQ.Core.HQ.EveHQSettings.NotifyNow = False
        End If
    End Sub

    Private Sub chkNotifyEarly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotifyEarly.CheckedChanged
        If chkNotifyEarly.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.NotifyEarly = True
        Else
            EveHQ.Core.HQ.EveHQSettings.NotifyEarly = False
        End If
    End Sub

    Private Sub chkNotifyToolTip_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotifyToolTip.CheckedChanged
        If chkNotifyToolTip.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.NotifyToolTip = True
        Else
            EveHQ.Core.HQ.EveHQSettings.NotifyToolTip = False
        End If
    End Sub

    Private Sub chkNotifyDialog_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotifyDialog.CheckedChanged
        If chkNotifyDialog.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.NotifyDialog = True
        Else
            EveHQ.Core.HQ.EveHQSettings.NotifyDialog = False
        End If
    End Sub

    Private Sub chkNotifyEmail_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNotifyEmail.CheckedChanged
        If chkNotifyEmail.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.NotifyEMail = True
            gbEmailOptions.Visible = True
        Else
            EveHQ.Core.HQ.EveHQSettings.NotifyEMail = False
            gbEmailOptions.Visible = False
        End If
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

    Private Sub trackNotifyOffset_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles trackNotifyOffset.ValueChanged
        EveHQ.Core.HQ.EveHQSettings.NotifyOffset = trackNotifyOffset.Value
        Dim offset As String = EveHQ.Core.SkillFunctions.TimeToStringAll(trackNotifyOffset.Value)
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

    Private Sub btnTestEmail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTestEmail.Click
        Dim eveHQMail As New System.Net.Mail.SmtpClient
        Try
            eveHQMail.Host = EveHQ.Core.HQ.EveHQSettings.EMailServer
            eveHQMail.Port = EveHQ.Core.HQ.EveHQSettings.EMailPort
            If EveHQ.Core.HQ.EveHQSettings.UseSMTPAuth = True Then
                Dim newCredentials As New System.Net.NetworkCredential
                newCredentials.UserName = EveHQ.Core.HQ.EveHQSettings.EMailUsername
                newCredentials.Password = EveHQ.Core.HQ.EveHQSettings.EMailPassword
                eveHQMail.Credentials = newCredentials
            End If
            Dim eveHQMsg As New System.Net.Mail.MailMessage("notifications@evehq.net", EveHQ.Core.HQ.EveHQSettings.EMailAddress)
            eveHQMsg.Subject = "Test Mail From EveHQ"
            eveHQMsg.Body = "This confirms that EveHQ is correctly setup to deliver skill completion notifcations."
            eveHQMail.Send(eveHQMsg)
            MessageBox.Show("Test message sent successfully. Please check your inbox for confirmation.", "EveHQ Test Email Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("The mail sending process failed. Please check that the server, address, username and password are correct." & ControlChars.CrLf & ControlChars.CrLf & "The error was: " & ex.Message, "EveHQ Test Email Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub btnSelectSoundFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectSoundFile.Click
        With ofd1
            .Title = "Please select a valid .wav file"
            .FileName = ""
            .InitialDirectory = "c:\"
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
        EveHQ.Core.HQ.EveHQSettings.EMailPort = CInt(txtSMTPPort.Text)
    End Sub

#End Region

#Region "Training Overlay Routines"

    Public Sub UpdateTrainingOverlayOptions()
        Dim bColor As Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.OverlayBorderColor))
        pbBorderColour.BackColor = bColor
        Dim pColor As Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.OverlayPanelColor))
        pbPanelColour.BackColor = pColor
        Dim fColor As Color = Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.OverlayFontColor))
        pbFontColour.BackColor = fColor
        tbTransparancy.Value = EveHQ.Core.HQ.EveHQSettings.OverlayTransparancy
        lblTransparancyValue.Text = tbTransparancy.Value & "%"
        nudOverlayXOffset.Value = EveHQ.Core.HQ.EveHQSettings.OverlayXOffset
        nudOverlayYOffset.Value = EveHQ.Core.HQ.EveHQSettings.OverlayYOffset
        Select Case EveHQ.Core.HQ.EveHQSettings.OverlayPosition
            Case 0
                radTopLeft.Checked = True
            Case 1
                radTopRight.Checked = True
            Case 2
                radBottomLeft.Checked = True
            Case 3
                radBottomRight.Checked = True
            Case Else
                radBottomRight.Checked = True
        End Select
        If EveHQ.Core.HQ.EveHQSettings.OverlayStartup = True Then
            chkShowOverlayOnStartup.Checked = True
        Else
            chkShowOverlayOnStartup.Checked = False
        End If
        If EveHQ.Core.HQ.EveHQSettings.OverlayClickThru = True Then
            chkClickThroughOverlay.Checked = True
        Else
            chkClickThroughOverlay.Checked = False
        End If
    End Sub

    Private Sub pbBorderColour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbBorderColour.Click
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
            pbBorderColour.BackColor = cd1.Color
            EveHQ.Core.HQ.EveHQSettings.OverlayBorderColor = cd1.Color.ToArgb
            If frmTrainingInfo.IsHandleCreated = True Then
                frmTrainingInfo.BackColor = (Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.OverlayBorderColor)))
            End If
        End If
    End Sub

    Private Sub pbFontColour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbFontColour.Click
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
            pbFontColour.BackColor = cd1.Color
            EveHQ.Core.HQ.EveHQSettings.OverlayFontColor = cd1.Color.ToArgb
            If frmTrainingInfo.IsHandleCreated = True Then
                frmTrainingInfo.lblTrainingStatus.ForeColor = (Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.OverlayFontColor)))
                frmTrainingInfo.lblPilot.ForeColor = (Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.OverlayFontColor)))
            End If
        End If
    End Sub

    Private Sub pbPanelColour_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbPanelColour.Click
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
            pbPanelColour.BackColor = cd1.Color
            EveHQ.Core.HQ.EveHQSettings.OverlayPanelColor = cd1.Color.ToArgb
            If frmTrainingInfo.IsHandleCreated = True Then
                frmTrainingInfo.Panel1.BackColor = (Color.FromArgb(CInt(EveHQ.Core.HQ.EveHQSettings.OverlayPanelColor)))
            End If
        End If
    End Sub

    Private Sub tbTransparancy_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tbTransparancy.ValueChanged
        lblTransparancyValue.Text = tbTransparancy.Value & "%"
        EveHQ.Core.HQ.EveHQSettings.OverlayTransparancy = tbTransparancy.Value
        If frmTrainingInfo.IsHandleCreated = True Then
            frmTrainingInfo.Opacity = (100 - EveHQ.Core.HQ.EveHQSettings.OverlayTransparancy) / 100
        End If
    End Sub

    Private Sub radTopLeft_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radTopLeft.CheckedChanged
        If radTopLeft.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.OverlayPosition = 0
        End If
        Call Me.RedrawOverlayFormPosition()
    End Sub

    Private Sub radTopRight_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radTopRight.CheckedChanged
        If radTopRight.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.OverlayPosition = 1
        End If
        Call Me.RedrawOverlayFormPosition()
    End Sub

    Private Sub radBottomLeft_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radBottomLeft.CheckedChanged
        If radBottomLeft.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.OverlayPosition = 2
        End If
        Call Me.RedrawOverlayFormPosition()
    End Sub

    Private Sub radBottomRight_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles radBottomRight.CheckedChanged
        If radBottomRight.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.OverlayPosition = 3
        End If
        Call Me.RedrawOverlayFormPosition()
    End Sub

    Private Sub RedrawOverlayFormPosition()
        If frmTrainingInfo.IsHandleCreated = True Then
            Dim wx As Integer = Screen.PrimaryScreen.WorkingArea.Right
            Dim wy As Integer = Screen.PrimaryScreen.WorkingArea.Bottom
            Dim x As Integer = 0
            Dim y As Integer = 0
            Select Case EveHQ.Core.HQ.EveHQSettings.OverlayPosition
                Case 0
                    x = EveHQ.Core.HQ.EveHQSettings.OverlayXOffset
                    y = EveHQ.Core.HQ.EveHQSettings.OverlayYOffset
                Case 1
                    x = wx - frmTrainingInfo.Width - EveHQ.Core.HQ.EveHQSettings.OverlayXOffset
                    y = EveHQ.Core.HQ.EveHQSettings.OverlayYOffset
                Case 2
                    x = EveHQ.Core.HQ.EveHQSettings.OverlayXOffset
                    y = wy - frmTrainingInfo.Height - EveHQ.Core.HQ.EveHQSettings.OverlayYOffset
                Case Else
                    x = wx - frmTrainingInfo.Width - EveHQ.Core.HQ.EveHQSettings.OverlayXOffset
                    y = wy - frmTrainingInfo.Height - EveHQ.Core.HQ.EveHQSettings.OverlayYOffset
            End Select
            frmTrainingInfo.Left = x
            frmTrainingInfo.Top = y
        End If
    End Sub

    Private Sub nudOverlayXOffset_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudOverlayXOffset.ValueChanged
        EveHQ.Core.HQ.EveHQSettings.OverlayXOffset = CInt(nudOverlayXOffset.Value)
        Call Me.RedrawOverlayFormPosition()
    End Sub

    Private Sub nudOverlayYOffset_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudOverlayYOffset.ValueChanged
        EveHQ.Core.HQ.EveHQSettings.OverlayYOffset = CInt(nudOverlayYOffset.Value)
        Call Me.RedrawOverlayFormPosition()
    End Sub

    Private Sub chkShowOverlayOnStartup_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowOverlayOnStartup.CheckedChanged
        If chkShowOverlayOnStartup.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.OverlayStartup = True
        Else
            EveHQ.Core.HQ.EveHQSettings.OverlayStartup = False
        End If
    End Sub

    Private Sub chkClickThroughOverlay_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkClickThroughOverlay.CheckedChanged
        If chkClickThroughOverlay.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.OverlayClickThru = True
        Else
            EveHQ.Core.HQ.EveHQSettings.OverlayClickThru = False
        End If
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
                EveHQ.Core.HQ.EveHQLCD.InitLCD("EveHQ LCD Display")
                If EveHQ.Core.HQ.IsG15LCDActive = False Then
                    MessageBox.Show("Unable to start G15 Display. Please ensure you have the keyboard and drivers correctly installed.", "Error Starting G15", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    chkActivateG15.Checked = False
                Else
                    'With the LCD initialised, draw the opening screen
                    EveHQ.Core.HQ.EveHQLCD.DrawIntroScreen()
                    ' Check if the LCD will cycle chars
                    If EveHQ.Core.HQ.EveHQSettings.CycleG15Pilots = True Then
                        EveHQ.Core.HQ.EveHQLCD.tmrLCDChar.Interval = (1000 * EveHQ.Core.HQ.EveHQSettings.CycleG15Time)
                        EveHQ.Core.HQ.EveHQLCD.tmrLCDChar.Enabled = True
                    End If
                End If
            End If
        Else
            If EveHQ.Core.HQ.EveHQSettings.ActivateG15 = True Then
                EveHQ.Core.HQ.EveHQSettings.ActivateG15 = False
                ' Close the LCD
                EveHQ.Core.HQ.EveHQLCD.CloseLCD()
            End If
        End If
    End Sub
    Private Sub chkCyclePilots_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCyclePilots.CheckedChanged
        If chkCyclePilots.Checked = True Then
            EveHQ.Core.HQ.EveHQSettings.CycleG15Pilots = True
            EveHQ.Core.HQ.EveHQLCD.tmrLCDChar.Interval = (1000 * EveHQ.Core.HQ.EveHQSettings.CycleG15Time)
            EveHQ.Core.HQ.EveHQLCD.tmrLCDChar.Enabled = True
        Else
            EveHQ.Core.HQ.EveHQSettings.CycleG15Pilots = False
            EveHQ.Core.HQ.EveHQLCD.tmrLCDChar.Enabled = False
        End If
    End Sub
    Private Sub nudCycleTime_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudCycleTime.ValueChanged
        EveHQ.Core.HQ.EveHQSettings.CycleG15Time = CInt(nudCycleTime.Value)
        If EveHQ.Core.HQ.EveHQSettings.CycleG15Time > 0 Then
            EveHQ.Core.HQ.EveHQLCD.tmrLCDChar.Interval = CInt((nudCycleTime.Value * 1000))
        Else
            EveHQ.Core.HQ.EveHQLCD.tmrLCDChar.Interval = CInt(1000)
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
            'lblCacheSize = CType(gbFolderHost.Controls("lblCacheSize" & CStr(folder).Trim), Label)
            lblEveDir.Text = EveHQ.Core.HQ.EveHQSettings.EveFolder(folder)
            If My.Computer.FileSystem.DirectoryExists(EveHQ.Core.HQ.EveHQSettings.EveFolder(folder)) = True Then
                'lblCacheSize.Text = "Cache Size: " & FormatNumber(Me.CheckCacheSize(folder) / 1024 / 1024, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "Mb"
                chkLUA.Enabled = True
                txtFName.Enabled = True
                txtFName.Text = EveHQ.Core.HQ.EveHQSettings.EveFolderLabel(folder)
            Else
                'lblCacheSize.Text = ""
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
            If My.Computer.FileSystem.FileExists(.SelectedPath & "\eve.exe") = False Then
                MessageBox.Show("This folder does not contain the Eve.exe file.", "Directory Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                lblEveDir.Text = .SelectedPath
                EveHQ.Core.HQ.EveHQSettings.EveFolder(folder) = .SelectedPath
                Dim chkLUA As CheckBox = CType(gbFolderHost.Controls("chkLUA" & CStr(folder).Trim), CheckBox)
                chkLUA.Enabled = True
                Dim txtFName As TextBox = CType(gbFolderHost.Controls("txtFriendlyName" & CStr(folder).Trim), TextBox)
                txtFName.Enabled = True
                Dim lblCacheSize As Label = CType(gbFolderHost.Controls("lblCacheSize" & CStr(folder).Trim), Label)
                'lblCacheSize.Text = "Cache Size: " & FormatNumber(Me.CheckCacheSize(folder) / 1024 / 1024, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "Mb"
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
        'lblCacheSize.Text = "Cache Size: " & FormatNumber(Me.CheckCacheSize(folder) / 1024 / 1024, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "Mb"
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
                Dim cacheDir As String = EveHQ.Core.HQ.EveHQSettings.EveFolder(folder) & "\cache"
                Dim settingsDir As String = cacheDir & "\settings"
                Dim prefsFile As String = cacheDir & "\prefs.ini"
                Dim browserDir As String = cacheDir & "\browser"
                Dim machoDIR As String = cacheDir & "\machonet"
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
                Dim cacheDir As String = (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\CCP\EVE\cache").Replace("\\", "\")
                Dim settingsDir As String = (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\CCP\EVE\settings").Replace("\\", "\")
                Dim prefsFile As String = settingsDir & "\prefs.ini"
                Dim browserDir As String = cacheDir & "\browser"
                Dim machoDIR As String = cacheDir & "\machonet"
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


End Class
