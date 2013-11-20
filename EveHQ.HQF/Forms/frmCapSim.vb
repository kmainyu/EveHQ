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
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Windows.Forms.DataVisualization.Charting
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar
Imports EveHQ.Core

Namespace Forms

    Public Class FrmCapSim

        Dim _csr As CapSimResults
        ReadOnly _capShip As Ship
        Dim _maxSimTime As Double = 0

        Public Sub New(ByVal calcShip As Ship)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            _csr = Capacitor.CalculateCapStatistics(CalcShip, True)
            _capShip = CalcShip

            Call UpdateCapData()

            Dim hiSlotStyle As ElementStyle = adtModules.Styles("SlotStyle").Copy
            Dim midSlotStyle As ElementStyle = adtModules.Styles("SlotStyle").Copy
            Dim lowSlotStyle As ElementStyle = adtModules.Styles("SlotStyle").Copy

            ' Add modules
            adtModules.BeginUpdate()
            adtModules.Nodes.Clear()
            For Each cm As CapacitorModule In _csr.Modules
                Dim newMod As New Node
                newMod.Text = cm.Name
                newMod.Cells.Add(New Cell(cm.CycleTime.ToString("N2")))
                newMod.Cells.Add(New Cell(cm.CapAmount.ToString("N2")))
                newMod.Cells.Add(New Cell((cm.CapAmount / cm.CycleTime).ToString("N2")))
                newMod.CheckBoxThreeState = False
                newMod.CheckBoxVisible = True
                newMod.Checked = cm.IsActive
                newMod.Tag = cm
                ' Set Style
                Select Case cm.SlotType
                    Case SlotTypes.High
                        newMod.Style = hiSlotStyle
                        newMod.Style.BackColor2 = Color.FromArgb(CInt(PluginSettings.HQFSettings.HiSlotColour))
                    Case SlotTypes.Mid
                        newMod.Style = midSlotStyle
                        newMod.Style.BackColor2 = Color.FromArgb(CInt(PluginSettings.HQFSettings.MidSlotColour))
                    Case SlotTypes.Low
                        newMod.Style = lowSlotStyle
                        newMod.Style.BackColor2 = Color.FromArgb(CInt(PluginSettings.HQFSettings.LowSlotColour))
                End Select
                newMod.Style.BackColor = Color.FromArgb(255, 255, 255)
                newMod.StyleSelected = newMod.Style
                adtModules.Nodes.Add(newMod)
            Next
            adtModules.EndUpdate()

            ' Set Filter limits
            Call ResetTimeFilter()

        End Sub

        Private Sub adtModules_AfterCheck(sender As Object, e As AdvTreeCellEventArgs) Handles adtModules.AfterCheck
            For Each checkNode As Node In adtModules.Nodes
                CType(checkNode.Tag, CapacitorModule).IsActive = checkNode.Checked
            Next
            Capacitor.RecalculateCapStatistics(_capShip, True, _csr)
            Call UpdateCapData()
            UpdateEventList()
        End Sub

        Private Sub iiStartTime_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles iiStartTime.ValueChanged
            ' Set the minimum end time to 1 second after
            iiEndTime.MinValue = iiStartTime.Value + 1
        End Sub

        Private Sub iiEndTime_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles iiEndTime.ValueChanged
            ' Set the maximum start time to 1 second before
            iiStartTime.MaxValue = iiEndTime.Value - 1
        End Sub

        Private Sub UpdateCapData()
            ' Determine the maximum sim time
            If _csr.CapIsDrained = True Then
                _maxSimTime = _csr.TimeToDrain
            Else
                _maxSimTime = _csr.SimulationTime
            End If
            Text = "Capacitor Simulation Results - Max Time: " & SkillFunctions.TimeToString(_maxSimTime, False)
            ' Populate the Summary Labels
            lblCapacity.Text = "Capacity: " & _capShip.CapCapacity & " GJ"
            lblRecharge.Text = "Recharge Time: " & _capShip.CapRecharge & " s"
            lblPeakRate.Text = "Peak Recharge Rate: " & (PluginSettings.HQFSettings.CapRechargeConstant * _capShip.CapCapacity / _capShip.CapRecharge).ToString("N2") & " GJ/s"
            Dim pi As Double = (CDbl(_capShip.Attributes(10050)) * -1) + (PluginSettings.HQFSettings.CapRechargeConstant * _capShip.CapCapacity / _capShip.CapRecharge)
            Dim po As Double = CDbl(_capShip.Attributes(10049))
            lblPeakIn.Text = "Peak In: " & pi.ToString("N2") & " GJ"
            lblPeakOut.Text = "Peak Out: " & po.ToString("N2") & " GJ"
            lblPeakDelta.Text = "Peak Delta: " & (pi - po).ToString("N2") & " GJ"
            If _csr.CapIsDrained = False Then
                lblStability.Text = "Stability: Stable at " & (_csr.MinimumCap / _capShip.CapCapacity * 100).ToString("N2") & "%"
            Else
                lblStability.Text = "Stability: Lasts " & SkillFunctions.TimeToString(_csr.TimeToDrain, False)
            End If
        End Sub

        Private Sub UpdateEventList()
            adtResults.BeginUpdate()
            adtResults.Nodes.Clear()
            For Each ce As CapacitorEvent In _csr.Events
                If ce.SimTime >= iiStartTime.Value And ce.SimTime <= iiEndTime.Value Then
                    Dim newEvent As New Node
                    newEvent.Text = ce.SimTime.ToString("N2")
                    newEvent.Cells.Add(New Cell(ce.ModuleName))
                    newEvent.Cells.Add(New Cell(ce.StartingCap.ToString("N2")))
                    newEvent.Cells.Add(New Cell((-ce.ActivationCost).ToString("N2")))
                    Dim endCap As Double = Math.Min(Math.Max(ce.StartingCap - ce.ActivationCost, 0), _capShip.CapCapacity)
                    newEvent.Cells.Add(New Cell(endCap.ToString("N2")))
                    newEvent.Cells.Add(New Cell((endCap / _capShip.CapCapacity * 100).ToString("N2") & "%"))
                    newEvent.Cells.Add(New Cell(ce.RechargeRate.ToString("N2")))
                    adtResults.Nodes.Add(newEvent)
                End If
            Next
            adtResults.EndUpdate()
            Call UpdateCapGraph()
        End Sub

        Private Sub UpdateCapGraph()
            ' Create series
            chartCap.Series.Clear()
            chartCap.Series.Add("Cap")

            ' Set chart type and styles
            chartCap.Titles(0).Text = "Capacitor Simulation - " & _capShip.Name
            chartCap.Series("Cap").ChartType = SeriesChartType.FastLine
            chartCap.Series("Cap").XAxisType = AxisType.Primary

            chartCap.ChartAreas("Default").AxisX.Minimum = iiStartTime.Value
            chartCap.ChartAreas("Default").AxisX.Maximum = iiEndTime.Value
            chartCap.ChartAreas("Default").AxisY.Minimum = 0
            chartCap.ChartAreas("Default").AxisY.Maximum = 100

            ' Calculate the points for a graph taking into account the lowest value in each time reference
            Dim cap As New SortedList(Of Double, Double)
            For Each ce As CapacitorEvent In _csr.Events
                If ce.SimTime >= iiStartTime.Value And ce.SimTime <= iiEndTime.Value Then
                    Dim endCap As Double = (Math.Min(Math.Max(ce.StartingCap - ce.ActivationCost, 0), _capShip.CapCapacity)) / _capShip.CapCapacity * 100
                    If cap.ContainsKey(ce.SimTime) = True Then
                        cap(ce.SimTime) = Math.Min(cap(ce.SimTime), endCap)
                    Else
                        cap.Add(ce.SimTime, endCap)
                    End If
                End If
            Next

            ' Create the actual points
            For Each time As Double In cap.Keys
                chartCap.Series("Cap").Points.AddXY(time, cap(time))
            Next

        End Sub

        Private Sub btnUpdateEvents_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdateEvents.Click
            Call UpdateEventList()
        End Sub

        Private Sub btnReset_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReset.Click
            Call ResetTimeFilter()
        End Sub

        Private Sub ResetTimeFilter()
            iiStartTime.MinValue = 0
            iiStartTime.MaxValue = CInt(_maxSimTime) - 1
            iiStartTime.Value = 0
            iiEndTime.MinValue = iiStartTime.Value + 1
            iiEndTime.MaxValue = CInt(_maxSimTime)
            iiEndTime.Value = CInt(Math.Min(_maxSimTime, 1800))
            ' Add Events
            Call UpdateEventList()
        End Sub

        Private Sub btnExport_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExport.Click
            Call ExportToClipboard("Capacitor Simulation Results", adtResults, ControlChars.Tab)
        End Sub

        Private Sub ExportToClipboard(ByVal title As String, ByVal sourceList As AdvTree, ByVal sepChar As String)
            Dim str As New StringBuilder
            ' Add a line for the current build job
            str.AppendLine(title)
            str.AppendLine("")
            ' Add some headings
            For c As Integer = 0 To sourceList.Columns.Count - 2
                str.Append(sourceList.Columns(c).Text & sepChar)
            Next
            str.AppendLine(sourceList.Columns(sourceList.Columns.Count - 1).Text)
            ' Add the details
            For Each req As Node In sourceList.Nodes
                For c As Integer = 0 To sourceList.Columns.Count - 2
                    str.Append(req.Cells(c).Text & sepChar)
                Next
                str.AppendLine(req.Cells(sourceList.Columns.Count - 1).Text)
            Next
            ' Copy to the clipboard
            Try
                Clipboard.SetText(str.ToString)
            Catch ex As Exception
                MessageBox.Show("Unable to copy Capacitor data to the clipboard.", "Clipboard Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Try
        End Sub

    End Class
End NameSpace