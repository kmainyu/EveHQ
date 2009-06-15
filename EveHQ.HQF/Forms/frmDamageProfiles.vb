﻿' ========================================================================
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
Imports System.Windows.Forms
Imports System.IO

Public Class frmDamageProfiles

    Private Sub frmDamageProfiles_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call Me.UpdateProfileList()
    End Sub

    Private Sub UpdateProfileList()
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
            lblEMDamageAmount.Text = FormatNumber(selProfile.EM, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            lblEXDamageAmount.Text = FormatNumber(selProfile.Explosive, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            lblKIDamageAmount.Text = FormatNumber(selProfile.Kinetic, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            lblTHDamageAmount.Text = FormatNumber(selProfile.Thermal, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            Dim total As Double = selProfile.EM + selProfile.Explosive + selProfile.Kinetic + selProfile.Thermal
            lblTotalDamageAmount.Text = FormatNumber(total, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            lblEMDamagePercentage.Text = "= " & FormatNumber(selProfile.EM / total * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
            lblEXDamagePercentage.Text = "= " & FormatNumber(selProfile.Explosive / total * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
            lblKIDamagePercentage.Text = "= " & FormatNumber(selProfile.Kinetic / total * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
            lblTHDamagePercentage.Text = "= " & FormatNumber(selProfile.Thermal / total * 100, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
            lblDPS.Text = FormatNumber(selProfile.DPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            If gbProfileInfo.Visible = False Then
                gbProfileInfo.Visible = True
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
                    Settings.HQFSettings.SaveProfiles()
                    Call Me.UpdateProfileList()
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
        Me.UpdateProfileList()
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
                Call Me.UpdateProfileList()
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
                    DamageProfiles.ProfileList.Clear()
                    Settings.HQFSettings.LoadProfiles()
                    Call Me.UpdateProfileList()
                    MessageBox.Show("Profiles successfully reset", "Profile Reset Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
End Class