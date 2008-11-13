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
Imports DotNetLib.Windows.Forms

Public Class frmShipComparison

    Dim StartUp As Boolean = True
    Dim cShipList As New SortedList

    Public Property ShipList() As SortedList
        Get
            Return cShipList
        End Get
        Set(ByVal value As SortedList)
            cShipList = value
        End Set
    End Property

    Private Sub frmShipComparison_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Populte the Pilots combobox
        Call Me.LoadPilots()

        ' Populate the Profiles combobox
        Call Me.LoadProfiles()

        ' Look at the settings for default pilot
        If cboPilots.Items.Count > 0 Then
            If HQF.Settings.HQFSettings.DefaultPilot <> "" Then
                cboPilots.SelectedItem = HQF.Settings.HQFSettings.DefaultPilot
            Else
                cboPilots.SelectedIndex = 0
            End If
        End If

        Call Me.LoadProfiles()

        ' Set the default damage profile
        cboProfiles.SelectedItem = "<Omni-Damage>"

        StartUp = False

        Call Me.UpdateShipData()

    End Sub

    Private Sub LoadPilots()
        cboPilots.BeginUpdate()
        cboPilots.Items.Clear()
        For Each hPilot As HQFPilot In HQFPilotCollection.HQFPilots.Values
            cboPilots.Items.Add(hPilot.PilotName)
        Next
        cboPilots.EndUpdate()
    End Sub

    Private Sub LoadProfiles()
        cboProfiles.BeginUpdate()
        cboProfiles.Items.Clear()
        For Each newProfile As DamageProfile In DamageProfiles.ProfileList.Values
            cboProfiles.Items.Add(newProfile.Name)
        Next
        cboProfiles.EndUpdate()
    End Sub

    Private Sub cboPilots_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPilots.SelectedIndexChanged
        If StartUp = False Then
            Call Me.UpdateShipData()
        End If
    End Sub

    Private Sub cboProfiles_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboProfiles.SelectedIndexChanged
        If StartUp = False Then
            Call Me.UpdateShipData()
        End If
    End Sub

    Private Sub UpdateShipData()
        Dim ShipInfo As New SortedList
        ' Create a sortedlist of holding results
        If cboPilots.SelectedItem IsNot Nothing And cboProfiles.SelectedItem IsNot Nothing Then
            Dim CompareWorker As New frmShipComparisonWorker

            CompareWorker.Pilot = CType(HQFPilotCollection.HQFPilots(cboPilots.SelectedItem.ToString), HQFPilot)
            CompareWorker.Profile = CType(HQF.DamageProfiles.ProfileList(cboProfiles.SelectedItem.ToString), DamageProfile)
            CompareWorker.ShipList = ShipList
            CompareWorker.ShowDialog()
            ShipInfo = CompareWorker.ShipInfo
            CompareWorker.Dispose()

            ' Add the details to the listview
            clvShips.BeginUpdate()
            clvShips.Items.Clear()
            For Each newShip As ShipData In ShipInfo.Values
                Dim newCLV As New ContainerListViewItem
                newCLV.Text = newShip.Ship & ", " & newShip.Fitting
                clvShips.Items.Add(newCLV)
                Dim pb As New Windows.Forms.PictureBox
                pb.Height = 16 : pb.Width = 16 : pb.Image = My.Resources.imgInfo2 : pb.SizeMode = Windows.Forms.PictureBoxSizeMode.StretchImage
                ToolTip1.SetToolTip(pb, newShip.Modules)
                newCLV.SubItems(0).ItemControl = pb
                newCLV.SubItems(0).Tag = newShip.Modules
                newCLV.SubItems(1).Text = newShip.Ship & ", " & newShip.Fitting
                newCLV.SubItems(2).Text = FormatNumber(newShip.EHP, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLV.SubItems(3).Text = FormatNumber(newShip.Tank, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLV.SubItems(4).Tag = newShip.Capacitor
                If newShip.Capacitor > 0 Then
                    newCLV.SubItems(4).Text = "Stable at " & FormatNumber(newShip.Capacitor, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault) & "%"
                Else
                    newCLV.SubItems(4).Text = "Lasts " & EveHQ.Core.SkillFunctions.TimeToString(-newShip.Capacitor)
                End If
                newCLV.SubItems(5).Text = FormatNumber(newShip.Volley, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLV.SubItems(6).Text = FormatNumber(newShip.DPS, 0, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLV.SubItems(7).Text = FormatNumber(newShip.SEM, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLV.SubItems(8).Text = FormatNumber(newShip.SEx, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLV.SubItems(9).Text = FormatNumber(newShip.SKi, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLV.SubItems(10).Text = FormatNumber(newShip.STh, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLV.SubItems(11).Text = FormatNumber(newShip.AEM, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLV.SubItems(12).Text = FormatNumber(newShip.AEx, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLV.SubItems(13).Text = FormatNumber(newShip.AKi, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
                newCLV.SubItems(14).Text = FormatNumber(newShip.ATh, 2, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault)
            Next
            clvShips.EndUpdate()
        End If
    End Sub

    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Dim buffer As New System.Text.StringBuilder
        buffer.AppendLine("HQF Ship Comparison Table")
        For i As Integer = 1 To clvShips.Columns.Count - 1
            buffer.Append(clvShips.Columns(i).Text)
            buffer.Append(ControlChars.Tab)
        Next
        buffer.Append("Fitting List")
        buffer.Append(ControlChars.CrLf)
        For i As Integer = 0 To clvShips.Items.Count - 1
            For j As Integer = 1 To clvShips.Items(0).SubItems.Count - 1
                If clvShips.Items(i).SubItems(j) IsNot Nothing Then
                    buffer.Append(clvShips.Items(i).SubItems(j).Text)
                    buffer.Append(ControlChars.Tab)
                End If
            Next
            buffer.Append(clvShips.Items(i).SubItems(0).Tag.ToString.Replace(ControlChars.CrLf, ", ").Replace(", , ", ", "))
            buffer.Append(ControlChars.CrLf)
        Next
        My.Computer.Clipboard.SetText(buffer.ToString)
    End Sub
End Class

Public Class ShipData
    Public Ship As String
    Public Fitting As String
    Public Modules As String
    Public EHP As Double
    Public Tank As Double
    Public Capacitor As Double
    Public Volley As Double
    Public DPS As Double
    Public SEM As Double
    Public SEx As Double
    Public SKi As Double
    Public STh As Double
    Public AEM As Double
    Public AEx As Double
    Public AKi As Double
    Public ATh As Double
    Public Speed As Double
End Class