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
Imports System.Windows.Forms

Public Class frmAddDamageProfile

    Private Sub cboProfileType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboProfileType.SelectedIndexChanged
        Select Case cboProfileType.SelectedIndex
            Case 0 ' Manual
                lblFittingName.Enabled = False : cboFittingName.Enabled = False
                lblPilotName.Enabled = False : cboPilotName.Enabled = False
                lblNPCs.Enabled = False : lvwNPCs.Enabled = False : cboNPC.Enabled = False : btnAddNPC.Enabled = False
                lblEmDamage.Enabled = True : txtEMDamage.Enabled = True
                lblEXDamage.Enabled = True : txtEXDamage.Enabled = True
                lblKIDamage.Enabled = True : txtKIDamage.Enabled = True
                lblTHDamage.Enabled = True : txtTHDamage.Enabled = True
                lblDPS.Enabled = True : txtDPS.Enabled = True
                lblDamageInfo.Enabled = True
            Case 1 ' Fitting
                lblFittingName.Enabled = True : cboFittingName.Enabled = True
                lblPilotName.Enabled = True : cboPilotName.Enabled = True
                lblNPCs.Enabled = False : lvwNPCs.Enabled = False : cboNPC.Enabled = False : btnAddNPC.Enabled = False
                lblEmDamage.Enabled = False : txtEMDamage.Enabled = False
                lblEXDamage.Enabled = False : txtEXDamage.Enabled = False
                lblKIDamage.Enabled = False : txtKIDamage.Enabled = False
                lblTHDamage.Enabled = False : txtTHDamage.Enabled = False
                lblDPS.Enabled = False : txtDPS.Enabled = False
                lblDamageInfo.Enabled = False
                Call Me.GetFittingDetails()
            Case 2 ' NPC
                lblFittingName.Enabled = False : cboFittingName.Enabled = False
                lblPilotName.Enabled = False : cboPilotName.Enabled = False
                lblNPCs.Enabled = True : lvwNPCs.Enabled = True : cboNPC.Enabled = True : btnAddNPC.Enabled = True
                lblEmDamage.Enabled = False : txtEMDamage.Enabled = False
                lblEXDamage.Enabled = False : txtEXDamage.Enabled = False
                lblKIDamage.Enabled = False : txtKIDamage.Enabled = False
                lblTHDamage.Enabled = False : txtTHDamage.Enabled = False
                lblDPS.Enabled = False : txtDPS.Enabled = False
                lblDamageInfo.Enabled = False
                Call Me.GetNPCDetails()
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
            If DamageProfiles.ProfileList.Contains(txtProfileName.Text.Trim) = True Then
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
                    Case 2 ' NPCs
                        If lvwNPCs.Items.Count = 0 Then
                            MessageBox.Show("You need to select at least one NPC. Please try again.", "NPC Required", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Exit Sub
                        End If
                End Select
                ' Assume all is OK so lets save the information
                Dim newProfile As New DamageProfile
                newProfile.Name = txtProfileName.Text.Trim
                newProfile.Type = cboProfileType.SelectedIndex
                newProfile.EM = CDbl(txtEMDamage.Text)
                newProfile.Explosive = CDbl(txtEXDamage.Text)
                newProfile.Kinetic = CDbl(txtKIDamage.Text)
                newProfile.Thermal = CDbl(txtTHDamage.Text)
                newProfile.DPS = CDbl(txtDPS.Text)
                Select Case newProfile.Type
                    Case 0 ' Manual
                        newProfile.Fitting = "" : newProfile.Pilot = "" : newProfile.NPCs.Clear()
                    Case 1 ' Fitting
                        newProfile.Fitting = cboFittingName.SelectedItem.ToString : newProfile.Pilot = cboPilotName.SelectedItem.ToString : newProfile.NPCs.Clear()
                    Case 2 ' NPCs
                        newProfile.Fitting = "" : newProfile.Pilot = ""
                        For Each NPCItem As ListViewItem In lvwNPCs.Items
                            newProfile.NPCs.Add(NPCItem.Text)
                        Next
                End Select
                DamageProfiles.ProfileList.Add(newProfile.Name, newProfile)
                Call Settings.HQFSettings.SaveProfiles()
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
        For Each cPilot As EveHQ.Core.Pilot In EveHQ.Core.HQ.Pilots
            cboPilotName.Items.Add(cPilot.Name)
        Next
        cboPilotName.EndUpdate()

        ' Load up the NPC Data
        Me.cboNPC.Items.Clear()
        cboNPC.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        cboNPC.AutoCompleteSource = AutoCompleteSource.CustomSource
        cboNPC.BeginUpdate()
        For Each NPC As String In NPCs.NPCList.Keys
            cboNPC.AutoCompleteCustomSource.Add(NPC)
            Me.cboNPC.Items.Add(NPC)
        Next
        Me.cboNPC.EndUpdate()

        cboProfileType.SelectedIndex = 0
    End Sub

    Private Sub cboFittingName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFittingName.SelectedIndexChanged
        Call Me.GetFittingDetails()
    End Sub

    Private Sub cboPilotName_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilotName.SelectedIndexChanged
        Call Me.GetFittingDetails()
    End Sub

    Private Sub GetFittingDetails()
        ' Check if we have a fitting and a pilot and if so, generate some data
        If cboFittingName.SelectedItem IsNot Nothing And cboPilotName.SelectedItem IsNot Nothing Then
            ' Let's try and generate a fitting and get some damage info
            Dim shipFit As String = cboFittingName.SelectedItem.ToString
            Dim fittingSep As Integer = shipFit.IndexOf(", ")
            Dim shipName As String = shipFit.Substring(0, fittingSep)
            Dim fittingName As String = shipFit.Substring(fittingSep + 2)
            Dim pShip As Ship = CType(ShipLists.shipList(shipName), Ship).Clone
            pShip = Engine.UpdateShipDataFromFittingList(pShip, CType(Fittings.FittingList(shipFit), ArrayList))
            Dim pPilot As HQFPilot = CType(HQFPilotCollection.HQFPilots(cboPilotName.SelectedItem.ToString), HQFPilot)
            Dim profileShip As Ship = Engine.ApplyFitting(pShip, pPilot)
            ' Place details of ship damage and DPS into text boxes
            txtEMDamage.Text = FormatNumber(profileShip.Attributes("10055").ToString, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            txtEXDamage.Text = FormatNumber(profileShip.Attributes("10056").ToString, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            txtKIDamage.Text = FormatNumber(profileShip.Attributes("10057").ToString, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            txtTHDamage.Text = FormatNumber(profileShip.Attributes("10058").ToString, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            txtDPS.Text = FormatNumber(profileShip.Attributes("10029").ToString, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        Else
            txtEMDamage.Text = FormatNumber(0, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            txtEXDamage.Text = FormatNumber(0, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            txtKIDamage.Text = FormatNumber(0, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            txtTHDamage.Text = FormatNumber(0, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            txtDPS.Text = FormatNumber(0, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        End If
    End Sub

    Private Sub btnAddNPC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddNPC.Click
        ' Check that the data in the text box is a valid NPC
        If NPCs.NPCList.Contains(cboNPC.Text) = True Then
            ' Add it to the list
            lvwNPCs.Items.Add(cboNPC.Text)
            ' Go through the list and get some estimation of the damage profile
            Call Me.GetNPCDetails()
        Else
            MessageBox.Show("'" & cboNPC.Text & "' is not a valid NPC. Please try again.", "NPC Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub
    Private Sub GetNPCDetails()
        ' Go through the list and get some estimation of the damage profile
        Dim NPCData As NPC
        Dim EM, EX, KI, TH, DPS As Double
        For Each NPCItem As ListViewItem In lvwNPCs.Items
            NPCData = CType(NPCs.NPCList(NPCItem.Text), NPC)
            EM += NPCData.EM
            EX += NPCData.Explosive
            KI += NPCData.Kinetic
            TH += NPCData.Thermal
            DPS += NPCData.DPS
        Next
        txtEMDamage.Text = FormatNumber(EM, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        txtEXDamage.Text = FormatNumber(EX, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        txtKIDamage.Text = FormatNumber(KI, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        txtTHDamage.Text = FormatNumber(TH, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
        txtDPS.Text = FormatNumber(DPS, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
    End Sub
End Class