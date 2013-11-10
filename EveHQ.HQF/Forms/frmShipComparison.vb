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
Imports System.Text
Imports System.Windows.Forms
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar
Imports EveHQ.Core

Namespace Forms

    Public Class FrmShipComparison

        Dim _startUp As Boolean = True
        Dim _shipList As New SortedList

        Public Property ShipList() As SortedList
            Get
                Return _shipList
            End Get
            Set(ByVal value As SortedList)
                _shipList = value
            End Set
        End Property

        Private Sub frmShipComparison_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load

            ' Populte the Pilots combobox
            Call LoadPilots()

            ' Populate the Profiles combobox
            Call LoadProfiles()

            ' Look at the settings for default pilot
            If cboPilots.Items.Count > 0 Then
                If PluginSettings.HQFSettings.DefaultPilot <> "" Then
                    cboPilots.SelectedItem = PluginSettings.HQFSettings.DefaultPilot
                Else
                    cboPilots.SelectedIndex = 0
                End If
            End If

           ' Set the default damage profile
            cboProfiles.SelectedItem = "<Omni-Damage>"

            _startUp = False

            Call UpdateShipData()

        End Sub

        Private Sub LoadPilots()
            cboPilots.BeginUpdate()
            cboPilots.Items.Clear()
            For Each hPilot As FittingPilot In FittingPilots.HQFPilots.Values
                cboPilots.Items.Add(hPilot.PilotName)
            Next
            cboPilots.EndUpdate()
        End Sub

        Private Sub LoadProfiles()
            cboProfiles.BeginUpdate()
            cboProfiles.Items.Clear()
            For Each newProfile As HQFDamageProfile In HQFDamageProfiles.ProfileList.Values
                cboProfiles.Items.Add(newProfile.Name)
            Next
            cboProfiles.EndUpdate()
        End Sub

        Private Sub cboPilots_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboPilots.SelectedIndexChanged
            If _startUp = False Then
                Call UpdateShipData()
            End If
        End Sub

        Private Sub cboProfiles_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboProfiles.SelectedIndexChanged
            If _startUp = False Then
                Call UpdateShipData()
            End If
        End Sub

        Private Sub UpdateShipData()
            Dim shipInfo As SortedList
            ' Create a sortedlist of holding results
            If cboPilots.SelectedItem IsNot Nothing And cboProfiles.SelectedItem IsNot Nothing Then
                Using compareWorker As New FrmShipComparisonWorker

                    compareWorker.Pilot = FittingPilots.HQFPilots(cboPilots.SelectedItem.ToString)
                    compareWorker.Profile = HQFDamageProfiles.ProfileList(cboProfiles.SelectedItem.ToString)
                    compareWorker.ShipList = ShipList
                    compareWorker.ShowDialog()
                    shipInfo = compareWorker.ShipInfo
                End Using

                ' Add the details to the listview
                adtShips.BeginUpdate()
                adtShips.Nodes.Clear()
                For Each newShip As ShipData In shipInfo.Values
                    Dim newShipNode As New Node
                    newShipNode.Text = newShip.Ship & ", " & newShip.Fitting
                    newShipNode.Tag = newShip.Modules
                    adtShips.Nodes.Add(newShipNode)
                    STT.SetSuperTooltip(newShipNode, New SuperTooltipInfo("Fitting Info", newShip.Fitting, newShip.Modules, Nothing, Nothing, eTooltipColor.Yellow))
                    newShipNode.Cells.Add(New Cell(newShip.Ehp.ToString("N0")))
                    newShipNode.Cells.Add(New Cell(newShip.Tank.ToString("N0")))
                    If newShip.Capacitor > 0 Then
                        newShipNode.Cells.Add(New Cell("Stable at " & newShip.Capacitor.ToString("N0") & "%"))
                    Else
                        newShipNode.Cells.Add(New Cell("Lasts " & SkillFunctions.TimeToString(-newShip.Capacitor)))
                    End If
                    newShipNode.Cells(3).Tag = newShip.Capacitor
                    newShipNode.Cells.Add(New Cell(newShip.Volley.ToString("N0")))
                    newShipNode.Cells.Add(New Cell(newShip.DPS.ToString("N0")))
                    newShipNode.Cells.Add(New Cell(newShip.SEm.ToString("N2")))
                    newShipNode.Cells.Add(New Cell(newShip.SEx.ToString("N2")))
                    newShipNode.Cells.Add(New Cell(newShip.SKi.ToString("N2")))
                    newShipNode.Cells.Add(New Cell(newShip.STh.ToString("N2")))
                    newShipNode.Cells.Add(New Cell(newShip.AEm.ToString("N2")))
                    newShipNode.Cells.Add(New Cell(newShip.AEx.ToString("N2")))
                    newShipNode.Cells.Add(New Cell(newShip.AKi.ToString("N2")))
                    newShipNode.Cells.Add(New Cell(newShip.ATh.ToString("N2")))
                Next
                AdvTreeSorter.Sort(adtShips, 1, True, True)
                adtShips.EndUpdate()
            End If
        End Sub

        Private Sub btnCopy_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCopy.Click
            Dim buffer As New StringBuilder
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

        Private Sub adtShips_ColumnHeaderMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles adtShips.ColumnHeaderMouseDown
            Dim ch As DevComponents.AdvTree.ColumnHeader = CType(sender, DevComponents.AdvTree.ColumnHeader)
            AdvTreeSorter.Sort(ch, True, False)
        End Sub

    End Class

    Public Class ShipData
        Public Ship As String
        Public Fitting As String
        Public Modules As String
        Public Ehp As Double
        Public Tank As Double
        Public Capacitor As Double
        Public Volley As Double
        Public DPS As Double
        Public SEm As Double
        Public SEx As Double
        Public SKi As Double
        Public STh As Double
        Public AEm As Double
        Public AEx As Double
        Public AKi As Double
        Public ATh As Double
        Public Speed As Double
    End Class
End NameSpace