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
Imports ZedGraph
Imports System.Drawing
Imports System.Text

Public Class frmCapSim

    Dim CSR As CapSimResults
    Dim CapShip As Ship
    Dim MaxSimTime As Double = 0

    Public Sub New(ByVal CalcShip As Ship)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        CSR = Capacitor.CalculateCapStatistics(CalcShip, True)
        CapShip = CalcShip

        ' Determine the maximum sim time
        If CSR.CapIsDrained = True Then
            MaxSimTime = CSR.TimeToDrain
        Else
            MaxSimTime = CSR.SimulationTime
        End If

        Me.Text = "Capacitor Simulation Results - Max Time: " & EveHQ.Core.SkillFunctions.TimeToString(MaxSimTime, False)

        ' Populate the Summary Labels
        lblCapacity.Text = "Capacity: " & CalcShip.CapCapacity & " GJ"
        lblRecharge.Text = "Recharge Time: " & CalcShip.CapRecharge & " s"
        lblPeakRate.Text = "Peak Recharge Rate: " & FormatNumber(HQF.Settings.HQFSettings.CapRechargeConstant * CalcShip.CapCapacity / CalcShip.CapRecharge, 2) & " GJ/s"
        Dim PI As Double = (CDbl(CalcShip.Attributes("10050")) * -1) + (HQF.Settings.HQFSettings.CapRechargeConstant * CalcShip.CapCapacity / CalcShip.CapRecharge)
        Dim PO As Double = CDbl(CalcShip.Attributes("10049"))
        lblPeakIn.Text = "Peak In: " & FormatNumber(PI, 2) & " GJ"
        lblPeakOut.Text = "Peak Out: " & FormatNumber(PO, 2) & " GJ"
        lblPeakDelta.Text = "Peak Delta: " & FormatNumber(PI - PO, 2) & " GJ"
        If CSR.CapIsDrained = False Then
            lblStability.Text = "Stability: Stable at " & FormatNumber(CSR.MinimumCap / CalcShip.CapCapacity * 100, 2) & "%"
        Else
            lblStability.Text = "Stability: Lasts " & EveHQ.Core.SkillFunctions.TimeToString(CSR.TimeToDrain, False)
        End If

        ' Add modules
        lvwModules.BeginUpdate()
        lvwModules.Items.Clear()
        For Each CM As CapacitorModule In CSR.Modules
            Dim NewMod As New ListViewItem
            NewMod.Text = CM.Name
            NewMod.SubItems.Add(CM.CycleTime.ToString("N2"))
            NewMod.SubItems.Add(CM.CapAmount.ToString("N2"))
            NewMod.SubItems.Add((CM.CapAmount / CM.CycleTime).ToString("N2"))
            lvwModules.Items.Add(NewMod)
        Next
        lvwModules.EndUpdate()

        ' Set Filter limits
        Call Me.ResetTimeFilter()

    End Sub

    Private Sub iiStartTime_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iiStartTime.ValueChanged
        ' Set the minimum end time to 1 second after
        iiEndTime.MinValue = iiStartTime.Value + 1
    End Sub

    Private Sub iiEndTime_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iiEndTime.ValueChanged
        ' Set the maximum start time to 1 second before
        iiStartTime.MaxValue = iiEndTime.Value - 1
    End Sub

    Private Sub UpdateEventList()
        lvwResults.BeginUpdate()
        lvwResults.Items.Clear()
        For Each CE As CapacitorEvent In CSR.Events
            If CE.SimTime >= iiStartTime.Value And CE.SimTime <= iiEndTime.Value Then
                Dim NewEvent As New ListViewItem
                NewEvent.Text = CE.SimTime.ToString("N2")
                NewEvent.SubItems.Add(CE.ModuleName)
                NewEvent.SubItems.Add(CE.StartingCap.ToString("N2"))
                NewEvent.SubItems.Add((-CE.ActivationCost).ToString("N2"))
                Dim EndCap As Double = Math.Min(Math.Max(CE.StartingCap - CE.ActivationCost, 0), CapShip.CapCapacity)
                NewEvent.SubItems.Add(EndCap.ToString("N2"))
                NewEvent.SubItems.Add((EndCap / CapShip.CapCapacity * 100).ToString("N2") & "%")
                NewEvent.SubItems.Add(CE.RechargeRate.ToString("N2"))
                lvwResults.Items.Add(NewEvent)
            End If
        Next
        lvwResults.EndUpdate()
        Call Me.UpdateCapGraph()
    End Sub

    Private Sub UpdateCapGraph()
        Dim myPane As GraphPane = zgcCapacitor.GraphPane
        myPane.CurveList.Clear()

        ' Set the titles and axis labels
        myPane.Title.Text = "Capacitor Simulation - " & CapShip.Name
        myPane.XAxis.Title.Text = "Time (s)"
        myPane.XAxis.Scale.Min = iiStartTime.Value
        myPane.XAxis.Scale.Max = iiEndTime.Value
        myPane.YAxis.Title.Text = "Cap (%)"
        myPane.YAxis.Scale.Min = 0
        myPane.YAxis.Scale.Max = 100
        'myPane.YAxis.Scale.MaxAuto = True

        ' Calculate the points for a graph taking into account the lowest value in each time reference
        Dim Cap As New SortedList(Of Double, Double)
        For Each CE As CapacitorEvent In CSR.Events
            If CE.SimTime >= iiStartTime.Value And CE.SimTime <= iiEndTime.Value Then
                Dim EndCap As Double = (Math.Min(Math.Max(CE.StartingCap - CE.ActivationCost, 0), CapShip.CapCapacity)) / CapShip.CapCapacity * 100
                If Cap.ContainsKey(CE.SimTime) = True Then
                    Cap(CE.SimTime) = Math.Min(Cap(CE.SimTime), EndCap)
                Else
                    Cap.Add(CE.SimTime, EndCap)
                End If
            End If
        Next

        ' Create the actual points
        Dim list As New PointPairList()
        For Each time As Double In Cap.Keys
            list.Add(time, Cap(time))
        Next

        Dim myCurve As LineItem = myPane.AddCurve("Cap Level", list, Color.DarkGreen, SymbolType.None)
        myCurve.Line.IsAntiAlias = True
        ' Fill the area under the curve with a white-green gradient at 45 degrees
        myCurve.Line.Fill = New Fill(Color.White, Color.YellowGreen, 90.0F)
        ' Make the symbols opaque by filling them with white
        myCurve.Symbol.Fill = New Fill(Color.White)

        ' Fill the axis background with a color gradient
        myPane.Chart.Fill = New Fill(Color.White, Color.LightGray, 45.0F)
        myPane.Legend.Fill = New Fill(Color.White, Color.LightGray, 45.0F)
        myPane.Legend.FontSpec.FontColor = Color.MidnightBlue

        ' Fill the pane background with a color gradient
        myPane.Fill = New Fill(Color.White, Color.LightGray, 45.0F)
        ' Calculate the Axis Scale Ranges
        zgcCapacitor.AxisChange()
        zgcCapacitor.Refresh()

    End Sub

    Private Sub btnUpdateEvents_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateEvents.Click
        Call Me.UpdateEventList()
    End Sub

    Private Sub btnReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Call Me.ResetTimeFilter()
    End Sub

    Private Sub ResetTimeFilter()
        iiStartTime.MinValue = 0
        iiStartTime.MaxValue = CInt(MaxSimTime) - 1
        iiStartTime.Value = 0
        iiEndTime.MinValue = iiStartTime.Value + 1
        iiEndTime.MaxValue = CInt(MaxSimTime)
        iiEndTime.Value = CInt(Math.Min(MaxSimTime, 1800))
        ' Add Events
        Call Me.UpdateEventList()
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Call Me.ExportToClipboard("Capacitor Simulation Results", lvwResults, ControlChars.Tab)
    End Sub

    Private Sub ExportToClipboard(ByVal title As String, ByVal sourceList As ListView, ByVal sepChar As String)
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
        For Each req As ListViewItem In sourceList.Items
            For c As Integer = 0 To sourceList.Columns.Count - 2
                str.Append(req.SubItems(c).Text & sepChar)
            Next
            str.AppendLine(req.SubItems(sourceList.Columns.Count - 1).Text)
        Next
        ' Copy to the clipboard
        Try
            Clipboard.SetText(str.ToString)
        Catch ex As Exception
            MessageBox.Show("Unable to copy Capacitor data to the clipboard.", "Clipboard Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

End Class