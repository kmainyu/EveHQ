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
Imports DevComponents.AdvTree

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
            adtShips.BeginUpdate()
            adtShips.Nodes.Clear()
            For Each newShip As ShipData In ShipInfo.Values
                Dim newShipNode As New Node
                newShipNode.Text = newShip.Ship & ", " & newShip.Fitting
                newShipNode.Tag = newShip.Modules
                adtShips.Nodes.Add(newShipNode)
                STT.SetSuperTooltip(newShipNode, New DevComponents.DotNetBar.SuperTooltipInfo("Fitting Info", newShip.Fitting, newShip.Modules, Nothing, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
                newShipNode.Cells.Add(New Cell(newShip.EHP.ToString("N0")))
                newShipNode.Cells.Add(New Cell(newShip.Tank.ToString("N0")))
                If newShip.Capacitor > 0 Then
                    newShipNode.Cells.Add(New Cell("Stable at " & newShip.Capacitor.ToString("N0") & "%"))
                Else
                    newShipNode.Cells.Add(New Cell("Lasts " & EveHQ.Core.SkillFunctions.TimeToString(-newShip.Capacitor)))
                End If
                newShipNode.Cells(3).Tag = newShip.Capacitor
                newShipNode.Cells.Add(New Cell(newShip.Volley.ToString("N0")))
                newShipNode.Cells.Add(New Cell(newShip.DPS.ToString("N0")))
                newShipNode.Cells.Add(New Cell(newShip.SEM.ToString("N2")))
                newShipNode.Cells.Add(New Cell(newShip.SEx.ToString("N2")))
                newShipNode.Cells.Add(New Cell(newShip.SKi.ToString("N2")))
                newShipNode.Cells.Add(New Cell(newShip.STh.ToString("N2")))
                newShipNode.Cells.Add(New Cell(newShip.AEM.ToString("N2")))
                newShipNode.Cells.Add(New Cell(newShip.AEx.ToString("N2")))
                newShipNode.Cells.Add(New Cell(newShip.AKi.ToString("N2")))
                newShipNode.Cells.Add(New Cell(newShip.ATh.ToString("N2")))
            Next
            EveHQ.Core.AdvTreeSorter.Sort(adtShips, 1, True, True)
            adtShips.EndUpdate()
        End If
    End Sub

    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Dim buffer As New System.Text.StringBuilder
        buffer.AppendLine("HQF Ship Comparison Table")
        For i As Integer = 0 To adtShips.Columns.Count - 1
            buffer.Append(adtShips.Columns(i).Text)
            buffer.Append(ControlChars.Tab)
        Next
        buffer.Append("Fitting List")
        buffer.Append(ControlChars.CrLf)
        For i As Integer = 0 To adtShips.Nodes.Count - 1
            For j As Integer = 0 To adtShips.Nodes(0).Cells.Count - 1
                If adtShips.Nodes(i).Cells(j) IsNot Nothing Then
                    buffer.Append(adtShips.Nodes(i).Cells(j).Text)
                    buffer.Append(ControlChars.Tab)
                End If
            Next
            buffer.Append(adtShips.Nodes(i).Cells(0).Tag.ToString.Replace(ControlChars.CrLf, ", ").Replace(", , ", ", "))
            buffer.Append(ControlChars.CrLf)
        Next
        My.Computer.Clipboard.SetText(buffer.ToString)
    End Sub

    Private Sub adtShips_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles adtShips.ColumnHeaderMouseDown
        Dim CH As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
        EveHQ.Core.AdvTreeSorter.Sort(CH, True, False)
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