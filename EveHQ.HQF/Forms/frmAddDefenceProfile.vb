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

Public Class frmAddDefenceProfile

    Dim startUp As Boolean = True

    Private Sub cboProfileType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboProfileType.SelectedIndexChanged
        Select Case cboProfileType.SelectedIndex
            Case 0 ' Manual
                lblFittingName.Enabled = False : cboFittingName.Enabled = False
                lblPilotName.Enabled = False : cboPilotName.Enabled = False
                lblEmDamage.Enabled = True
                lblEXDamage.Enabled = True
                lblKIDamage.Enabled = True
                lblTHDamage.Enabled = True
                txtSEM.Enabled = True
                txtSEx.Enabled = True
                txtSKi.Enabled = True
                txtSTh.Enabled = True
                txtAEM.Enabled = True
                txtAEx.Enabled = True
                txtAKi.Enabled = True
                txtATh.Enabled = True
                txtHEM.Enabled = True
                txtHEx.Enabled = True
                txtHKi.Enabled = True
                txtHTh.Enabled = True
                lblResistInfo.Enabled = True
            Case 1 ' Fitting
                lblFittingName.Enabled = True : cboFittingName.Enabled = True
                lblPilotName.Enabled = True : cboPilotName.Enabled = True
                lblEmDamage.Enabled = False
                lblEXDamage.Enabled = False
                lblKIDamage.Enabled = False
                lblTHDamage.Enabled = False
                txtSEM.Enabled = False
                txtSEx.Enabled = False
                txtSKi.Enabled = False
                txtSTh.Enabled = False
                txtAEM.Enabled = False
                txtAEx.Enabled = False
                txtAKi.Enabled = False
                txtATh.Enabled = False
                txtHEM.Enabled = False
                txtHEx.Enabled = False
                txtHKi.Enabled = False
                txtHTh.Enabled = False
                lblResistInfo.Enabled = False
                Call Me.GetFittingDetails()
        End Select
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        ' Check we have a profile name
        If txtProfileName.Text.Trim = "" Then
            MessageBox.Show("You need to enter a valid profile name. Please try again.", "Profile Name Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        Else
            ' Check the name isn't in use
            If DamageProfiles.ProfileList.Contains(txtProfileName.Text.Trim) And txtProfileName.Text.Trim <> txtProfileName.Tag.ToString.Trim Then
                MessageBox.Show("This Profile name is in use. Please select another.", "Duplicate Profile Name Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            Else
                ' Check we have all the required info
                Select Case cboProfileType.SelectedIndex
                    Case 0 ' Manual
                        If IsNumeric(txtSEM.Text) = False Or IsNumeric(txtSEx.Text) = False Or IsNumeric(txtSKi.Text) = False Or IsNumeric(txtSTh.Text) = False Then
                            MessageBox.Show("Shield Resist amounts contain non numeric characters. Please try again.", "Resistance Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                        If IsNumeric(txtAEM.Text) = False Or IsNumeric(txtAEx.Text) = False Or IsNumeric(txtAKi.Text) = False Or IsNumeric(txtATh.Text) = False Then
                            MessageBox.Show("Armor Resist amounts contain non numeric characters. Please try again.", "Resistance Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                        If IsNumeric(txtHEM.Text) = False Or IsNumeric(txtHEx.Text) = False Or IsNumeric(txtHKi.Text) = False Or IsNumeric(txtHTh.Text) = False Then
                            MessageBox.Show("Hull Resist amounts contain non numeric characters. Please try again.", "Resistance Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If

                        If CDbl(txtSEM.Text) < 0 Or CDbl(txtSEx.Text) < 0 Or CDbl(txtSKi.Text) < 0 Or CDbl(txtSTh.Text) < 0 Or CDbl(txtSEM.Text) > 100 Or CDbl(txtSEx.Text) > 100 Or CDbl(txtSKi.Text) > 100 Or CDbl(txtSTh.Text) > 100 Then
                            MessageBox.Show("Shield Resist amounts must be greater than or equal to zero and less than 100. Please try again.", "Damage Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                        If CDbl(txtAEM.Text) < 0 Or CDbl(txtAEx.Text) < 0 Or CDbl(txtAKi.Text) < 0 Or CDbl(txtATh.Text) < 0 Or CDbl(txtAEM.Text) > 100 Or CDbl(txtAEx.Text) > 100 Or CDbl(txtAKi.Text) > 100 Or CDbl(txtATh.Text) > 100 Then
                            MessageBox.Show("Armor Resist amounts must be greater than or equal to zero and less than 100. Please try again.", "Damage Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                        If CDbl(txtHEM.Text) < 0 Or CDbl(txtHEx.Text) < 0 Or CDbl(txtHKi.Text) < 0 Or CDbl(txtHTh.Text) < 0 Or CDbl(txtHEM.Text) > 100 Or CDbl(txtHEx.Text) > 100 Or CDbl(txtHKi.Text) > 100 Or CDbl(txtHTh.Text) > 100 Then
                            MessageBox.Show("Hull Resist amounts must be greater than or equal to zero and less than 100. Please try again.", "Damage Value Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                Dim newProfile As New DefenceProfile
                newProfile.Name = txtProfileName.Text.Trim
                newProfile.Type = cboProfileType.SelectedIndex
                newProfile.SEM = CDbl(txtSEM.Text)
                newProfile.SExplosive = CDbl(txtSEx.Text)
                newProfile.SKinetic = CDbl(txtSKi.Text)
                newProfile.SThermal = CDbl(txtSTh.Text)
                newProfile.AEM = CDbl(txtAEM.Text)
                newProfile.AExplosive = CDbl(txtAEx.Text)
                newProfile.AKinetic = CDbl(txtAKi.Text)
                newProfile.AThermal = CDbl(txtATh.Text)
                newProfile.HEM = CDbl(txtHEM.Text)
                newProfile.HExplosive = CDbl(txtHEx.Text)
                newProfile.HKinetic = CDbl(txtHKi.Text)
                newProfile.HThermal = CDbl(txtHTh.Text)
                Select Case newProfile.Type
                    Case 0 ' Manual
                        newProfile.Fitting = "" : newProfile.Pilot = ""
                    Case 1 ' Fitting
                        newProfile.Fitting = cboFittingName.SelectedItem.ToString : newProfile.Pilot = cboPilotName.SelectedItem.ToString
                End Select
                ' If Editing, delete the old profile
                If Me.Tag.ToString <> "Add" Then
                    DefenceProfiles.ProfileList.Remove(txtProfileName.Tag.ToString)
                End If
                ' Add the profile
                DefenceProfiles.ProfileList.Add(newProfile.Name, newProfile)
                Call DefenceProfiles.SaveProfiles()
                Me.Close()
            End If
        End If
    End Sub

    Private Sub frmAddDamageProfile_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Load details into the combo boxes
        cboFittingName.BeginUpdate()
        cboFittingName.Items.Clear()
        For Each fitting As String In Fittings.FittingList.Keys
            cboFittingName.Items.Add(fitting)
        Next
        cboFittingName.EndUpdate()

        cboPilotName.BeginUpdate()
        cboPilotName.Items.Clear()
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.EveHQSettings.Pilots
            cboPilotName.Items.Add(cPilot.Name)
        Next
        cboPilotName.EndUpdate()

        If Me.Tag.ToString = "Add" Then
            cboProfileType.SelectedIndex = 0
            Me.txtProfileName.Tag = ""
            txtSEM.Text = CDbl(0).ToString("N2")
            txtSEx.Text = CDbl(0).ToString("N2")
            txtSKi.Text = CDbl(0).ToString("N2")
            txtSTh.Text = CDbl(0).ToString("N2")
            txtAEM.Text = CDbl(0).ToString("N2")
            txtAEx.Text = CDbl(0).ToString("N2")
            txtAKi.Text = CDbl(0).ToString("N2")
            txtATh.Text = CDbl(0).ToString("N2")
            txtHEM.Text = CDbl(0).ToString("N2")
            txtHEx.Text = CDbl(0).ToString("N2")
            txtHKi.Text = CDbl(0).ToString("N2")
            txtHTh.Text = CDbl(0).ToString("N2")
            startUp = False
        Else
            Dim editProfile As DefenceProfile = CType(Me.Tag, DefenceProfile)
            Me.txtProfileName.Tag = editProfile.Name
            ' Populate the bits and pieces with the relevant data
            Me.txtProfileName.Text = editProfile.Name
            Select Case editProfile.Type
                Case 0 ' Manual
                    Me.txtSEM.Text = editProfile.SEM.ToString("N2")
                    Me.txtSEx.Text = editProfile.SExplosive.ToString("N2")
                    Me.txtSKi.Text = editProfile.SKinetic.ToString("N2")
                    Me.txtSTh.Text = editProfile.SThermal.ToString("N2")
                    Me.txtAEM.Text = editProfile.AEM.ToString("N2")
                    Me.txtAEx.Text = editProfile.AExplosive.ToString("N2")
                    Me.txtAKi.Text = editProfile.AKinetic.ToString("N2")
                    Me.txtATh.Text = editProfile.AThermal.ToString("N2")
                    Me.txtHEM.Text = editProfile.HEM.ToString("N2")
                    Me.txtHEx.Text = editProfile.HExplosive.ToString("N2")
                    Me.txtHKi.Text = editProfile.HKinetic.ToString("N2")
                    Me.txtHTh.Text = editProfile.HThermal.ToString("N2")
                Case 1 ' Fitting
                    Me.cboFittingName.SelectedItem = editProfile.Fitting
                    Me.cboPilotName.SelectedItem = editProfile.Pilot
            End Select
            ' End the startup and select the right combo index to trigger an update to the form visibility
            startUp = False
            Me.cboProfileType.SelectedIndex = editProfile.Type
        End If

    End Sub

    Private Sub cboFittingName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFittingName.SelectedIndexChanged
        Call Me.GetFittingDetails()
    End Sub

    Private Sub cboPilotName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilotName.SelectedIndexChanged
        Call Me.GetFittingDetails()
    End Sub

    Private Sub GetFittingDetails()
        ' Check if we have a fitting and a pilot and if so, generate some data
        If startUp = False Then
            If cboFittingName.SelectedItem IsNot Nothing And cboPilotName.SelectedItem IsNot Nothing Then
                ' Let's try and generate a fitting and get some damage info
                Dim shipFit As String = cboFittingName.SelectedItem.ToString
                Dim pPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboPilotName.SelectedItem.ToString), HQFPilot)
                Dim NewFit As Fitting = Fittings.FittingList(shipFit).Clone
                NewFit.UpdateBaseShipFromFitting()
                NewFit.PilotName = pPilot.PilotName
                NewFit.ApplyFitting(BuildType.BuildEverything)
                Dim profileShip As Ship = NewFit.FittedShip
                ' Place details of ship damage and DPS into text boxes
                txtSEM.Text = profileShip.ShieldEMResist.ToString("N2")
                txtSEx.Text = profileShip.ShieldExResist.ToString("N2")
                txtSKi.Text = profileShip.ShieldKiResist.ToString("N2")
                txtSTh.Text = profileShip.ShieldThResist.ToString("N2")
                txtAEM.Text = profileShip.ArmorEMResist.ToString("N2")
                txtAEx.Text = profileShip.ArmorExResist.ToString("N2")
                txtAKi.Text = profileShip.ArmorKiResist.ToString("N2")
                txtATh.Text = profileShip.ArmorThResist.ToString("N2")
                txtHEM.Text = profileShip.StructureEMResist.ToString("N2")
                txtHEx.Text = profileShip.StructureExResist.ToString("N2")
                txtHKi.Text = profileShip.StructureKiResist.ToString("N2")
                txtHTh.Text = profileShip.StructureThResist.ToString("N2")
            Else
                txtSEM.Text = CDbl(0).ToString("N2")
                txtSEx.Text = CDbl(0).ToString("N2")
                txtSKi.Text = CDbl(0).ToString("N2")
                txtSTh.Text = CDbl(0).ToString("N2")
                txtAEM.Text = CDbl(0).ToString("N2")
                txtAEx.Text = CDbl(0).ToString("N2")
                txtAKi.Text = CDbl(0).ToString("N2")
                txtATh.Text = CDbl(0).ToString("N2")
                txtHEM.Text = CDbl(0).ToString("N2")
                txtHEx.Text = CDbl(0).ToString("N2")
                txtHKi.Text = CDbl(0).ToString("N2")
                txtHTh.Text = CDbl(0).ToString("N2")
            End If
        End If
    End Sub
   
End Class