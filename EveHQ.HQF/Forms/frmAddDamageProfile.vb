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

Public Class frmAddDamageProfile

    Dim _startUp As Boolean = True

    Private Sub cboProfileType_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboProfileType.SelectedIndexChanged
        Select Case cboProfileType.SelectedIndex
            Case 0 ' Manual
                lblFittingName.Enabled = False : cboFittingName.Enabled = False
                lblPilotName.Enabled = False : cboPilotName.Enabled = False
                lblEmDamage.Enabled = True : txtEMDamage.Enabled = True
                lblEXDamage.Enabled = True : txtEXDamage.Enabled = True
                lblKIDamage.Enabled = True : txtKIDamage.Enabled = True
                lblTHDamage.Enabled = True : txtTHDamage.Enabled = True
                lblDPS.Enabled = True : txtDPS.Enabled = True
                lblDamageInfo.Enabled = True
            Case 1 ' Fitting
                lblFittingName.Enabled = True : cboFittingName.Enabled = True
                lblPilotName.Enabled = True : cboPilotName.Enabled = True
                lblEmDamage.Enabled = False : txtEMDamage.Enabled = False
                lblEXDamage.Enabled = False : txtEXDamage.Enabled = False
                lblKIDamage.Enabled = False : txtKIDamage.Enabled = False
                lblTHDamage.Enabled = False : txtTHDamage.Enabled = False
                lblDPS.Enabled = False : txtDPS.Enabled = False
                lblDamageInfo.Enabled = False
                Call GetFittingDetails()
        End Select
    End Sub

    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAccept.Click
        ' Check we have a profile name
        If txtProfileName.Text.Trim = "" Then
            MessageBox.Show("You need to enter a valid profile name. Please try again.", "Profile Name Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            ' Check the name isn't in use
            If HQFDamageProfiles.ProfileList.ContainsKey(txtProfileName.Text.Trim) And txtProfileName.Text.Trim <> txtProfileName.Tag.ToString.Trim Then
                MessageBox.Show("This Profile name is in use. Please select another.", "Duplicate Profile Name Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                ' Check we have all the required info
                Select Case cboProfileType.SelectedIndex
                    Case 0 ' Manual
                        If IsNumeric(txtEMDamage.Text) = False Or IsNumeric(txtEXDamage.Text) = False Or IsNumeric(txtKIDamage.Text) = False Or IsNumeric(txtTHDamage.Text) = False Or IsNumeric(txtDPS.Text) = False Then
                            MessageBox.Show("Damage amounts contain non numeric characters. Please try again.", "Damage Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                        If CDbl(txtEMDamage.Text) < 0 Or CDbl(txtEXDamage.Text) < 0 Or CDbl(txtKIDamage.Text) < 0 Or CDbl(txtTHDamage.Text) < 0 Or CDbl(txtDPS.Text) < 0 Then
                            MessageBox.Show("Damage amounts must be greater than or equal to zero. Please try again.", "Damage Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                    Case 1 ' Fitting
                        If cboFittingName.SelectedItem Is Nothing Then
                            MessageBox.Show("A valid Fitting needs to be selected. Please try again.", "Fitting Required", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                        If cboPilotName.SelectedItem Is Nothing Then
                            MessageBox.Show("A Pilot needs to be selected. Please try again.", "Pilot Required", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                End Select
                ' Assume all is OK so lets save the information
                Dim newProfile As New HQFDamageProfile
                newProfile.Name = txtProfileName.Text.Trim
                newProfile.Type = CType(cboProfileType.SelectedIndex, ProfileTypes)
                newProfile.EM = CDbl(txtEMDamage.Text)
                newProfile.Explosive = CDbl(txtEXDamage.Text)
                newProfile.Kinetic = CDbl(txtKIDamage.Text)
                newProfile.Thermal = CDbl(txtTHDamage.Text)
                newProfile.DPS = CDbl(txtDPS.Text)
                Select Case newProfile.Type
                    Case ProfileTypes.Manual  ' Manual
                        newProfile.Fitting = "" : newProfile.Pilot = ""
                    Case ProfileTypes.Fitting  ' Fitting
                        newProfile.Fitting = cboFittingName.SelectedItem.ToString : newProfile.Pilot = cboPilotName.SelectedItem.ToString
                End Select
                ' If Editing, delete the old profile
                If Tag.ToString <> "Add" Then
                    HQFDamageProfiles.ProfileList.Remove(txtProfileName.Tag.ToString)
                End If
                ' Add the profile
                HQFDamageProfiles.ProfileList.Add(newProfile.Name, newProfile)
                Call HQFDamageProfiles.Save()
                Close()
            End If
        End If
    End Sub

    Private Sub frmAddDamageProfile_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ' Load details into the combo boxes
        cboFittingName.BeginUpdate()
        cboFittingName.Items.Clear()
        For Each fitting As String In Fittings.FittingList.Keys
            cboFittingName.Items.Add(fitting)
        Next
        cboFittingName.EndUpdate()

        cboPilotName.BeginUpdate()
        cboPilotName.Items.Clear()
        For Each cPilot As Core.EveHQPilot In Core.HQ.Settings.Pilots.Values
            cboPilotName.Items.Add(cPilot.Name)
        Next
        cboPilotName.EndUpdate()

        If Tag.ToString = "Add" Then
            cboProfileType.SelectedIndex = 0
            txtProfileName.Tag = ""
            txtEMDamage.Text = CDbl(0).ToString("N2")
            txtEXDamage.Text = CDbl(0).ToString("N2")
            txtKIDamage.Text = CDbl(0).ToString("N2")
            txtTHDamage.Text = CDbl(0).ToString("N2")
            txtDPS.Text = CDbl(0).ToString("N2")
        Else
            Dim editProfile As HQFDamageProfile = CType(Tag, HQFDamageProfile)
            txtProfileName.Tag = editProfile.Name
            ' Populate the bits and pieces with the relevant data
            txtProfileName.Text = editProfile.Name
            Select Case editProfile.Type
                Case ProfileTypes.Manual  ' Manual
                    txtEMDamage.Text = editProfile.EM.ToString("N2")
                    txtEXDamage.Text = editProfile.Explosive.ToString("N2")
                    txtKIDamage.Text = editProfile.Kinetic.ToString("N2")
                    txtTHDamage.Text = editProfile.Thermal.ToString("N2")
                    txtDPS.Text = editProfile.DPS.ToString("N2")
                Case ProfileTypes.Fitting  ' Fitting
                    cboFittingName.SelectedItem = editProfile.Fitting
                    cboPilotName.SelectedItem = editProfile.Pilot
            End Select
            ' End the startup and select the right combo index to trigger an update to the form visibility
            _startUp = False
            cboProfileType.SelectedIndex = editProfile.Type
        End If

    End Sub

    Private Sub cboFittingName_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboFittingName.SelectedIndexChanged
        Call GetFittingDetails()
    End Sub

    Private Sub cboPilotName_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPilotName.SelectedIndexChanged
        Call GetFittingDetails()
    End Sub

    Private Sub GetFittingDetails()
        ' Check if we have a fitting and a pilot and if so, generate some data
        If _startUp = False Then
            If cboFittingName.SelectedItem IsNot Nothing And cboPilotName.SelectedItem IsNot Nothing Then
                ' Let's try and generate a fitting and get some damage info
                Dim shipFit As String = cboFittingName.SelectedItem.ToString
                Dim pPilot As FittingPilot = FittingPilots.HQFPilots(cboPilotName.SelectedItem.ToString)
                Dim newFit As Fitting = Fittings.FittingList(shipFit).Clone
                newFit.UpdateBaseShipFromFitting()
                newFit.PilotName = pPilot.PilotName
                newFit.ApplyFitting(BuildType.BuildEverything)
                Dim profileShip As Ship = newFit.FittedShip
                ' Place details of ship damage and DPS into text boxes
                txtEMDamage.Text = profileShip.Attributes("10055").ToString("N2")
                txtEXDamage.Text = profileShip.Attributes("10056").ToString("N2")
                txtKIDamage.Text = profileShip.Attributes("10057").ToString("N2")
                txtTHDamage.Text = profileShip.Attributes("10058").ToString("N2")
                txtDPS.Text = profileShip.Attributes("10029").ToString("N2")
            Else
                txtEMDamage.Text = CDbl(0).ToString("N2")
                txtEXDamage.Text = CDbl(0).ToString("N2")
                txtKIDamage.Text = CDbl(0).ToString("N2")
                txtTHDamage.Text = CDbl(0).ToString("N2")
                txtDPS.Text = CDbl(0).ToString("N2")
            End If
        End If
    End Sub

End Class