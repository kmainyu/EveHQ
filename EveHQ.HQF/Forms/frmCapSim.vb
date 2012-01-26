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
Imports ZedGraph
Imports System.Drawing
Imports System.Text
Imports DevComponents.AdvTree
Imports DevComponents.DotNetBar

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

        Call Me.UpdateCapData()

        Dim HiSlotStyle As ElementStyle = adtModules.Styles("SlotStyle").Copy
        Dim MidSlotStyle As ElementStyle = adtModules.Styles("SlotStyle").Copy
        Dim LowSlotStyle As ElementStyle = adtModules.Styles("SlotStyle").Copy
        Dim RigSlotStyle As ElementStyle = adtModules.Styles("SlotStyle").Copy
        Dim SubSlotStyle As ElementStyle = adtModules.Styles("SlotStyle").Copy
        Dim SelSlotStyle As ElementStyle = adtModules.Styles("SlotStyle").Copy

        ' Add modules
        adtModules.BeginUpdate()
        adtModules.Nodes.Clear()
        For Each CM As CapacitorModule In CSR.Modules
            Dim NewMod As New Node
            NewMod.Text = CM.Name
            NewMod.Cells.Add(New Cell(CM.CycleTime.ToString("N2")))
            NewMod.Cells.Add(New Cell(CM.CapAmount.ToString("N2")))
            NewMod.Cells.Add(New Cell((CM.CapAmount / CM.CycleTime).ToString("N2")))
            NewMod.CheckBoxThreeState = False
            NewMod.CheckBoxVisible = True
            NewMod.Checked = CM.IsActive
            NewMod.Tag = CM
            ' Set Style
            Select Case CM.SlotType
                Case SlotTypes.High
                    NewMod.Style = HiSlotStyle
                    NewMod.Style.BackColor2 = Color.FromArgb(CInt(HQF.Settings.HQFSettings.HiSlotColour))
                Case SlotTypes.Mid
                    NewMod.Style = MidSlotStyle
                    NewMod.Style.BackColor2 = Color.FromArgb(CInt(HQF.Settings.HQFSettings.MidSlotColour))
                Case SlotTypes.Low
                    NewMod.Style = LowSlotStyle
                    NewMod.Style.BackColor2 = Color.FromArgb(CInt(HQF.Settings.HQFSettings.LowSlotColour))
            End Select
            NewMod.Style.BackColor = Color.FromArgb(255, 255, 255)
            NewMod.StyleSelected = NewMod.Style
            adtModules.Nodes.Add(NewMod)
        Next
        adtModules.EndUpdate()

        ' Set Filter limits
        Call Me.ResetTimeFilter()

    End Sub

    Private Sub adtModules_AfterCheck(sender As Object, e As DevComponents.AdvTree.AdvTreeCellEventArgs) Handles adtModules.AfterCheck
        For Each CheckNode As Node In adtModules.Nodes
            CType(CheckNode.Tag, CapacitorModule).IsActive = CheckNode.Checked
        Next
        Capacitor.RecalculateCapStatistics(CapShip, True, CSR)
        Call Me.UpdateCapData()
        UpdateEventList()
    End Sub

    Private Sub iiStartTime_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iiStartTime.ValueChanged
        ' Set the minimum end time to 1 second after
        iiEndTime.MinValue = iiStartTime.Value + 1
    End Sub

    Private Sub iiEndTime_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles iiEndTime.ValueChanged
        ' Set the maximum start time to 1 second before
        iiStartTime.MaxValue = iiEndTime.Value - 1
    End Sub

    Private Sub UpdateCapData()
        ' Determine the maximum sim time
        If CSR.CapIsDrained = True Then
            MaxSimTime = CSR.TimeToDrain
        Else
            MaxSimTime = CSR.SimulationTime
        End If
        Me.Text = "Capacitor Simulation Results - Max Time: " & EveHQ.Core.SkillFunctions.TimeToString(MaxSimTime, False)
        ' Populate the Summary Labels
        lblCapacity.Text = "Capacity: " & CapShip.CapCapacity & " GJ"
        lblRecharge.Text = "Recharge Time: " & CapShip.CapRecharge & " s"
        lblPeakRate.Text = "Peak Recharge Rate: " & (HQF.Settings.HQFSettings.CapRechargeConstant * CapShip.CapCapacity / CapShip.CapRecharge).ToString("N2") & " GJ/s"
        Dim PI As Double = (CDbl(CapShip.Attributes("10050")) * -1) + (HQF.Settings.HQFSettings.CapRechargeConstant * CapShip.CapCapacity / CapShip.CapRecharge)
        Dim PO As Double = CDbl(CapShip.Attributes("10049"))
        lblPeakIn.Text = "Peak In: " & PI.ToString("N2") & " GJ"
        lblPeakOut.Text = "Peak Out: " & PO.ToString("N2") & " GJ"
        lblPeakDelta.Text = "Peak Delta: " & (PI - PO).ToString("N2") & " GJ"
        If CSR.CapIsDrained = False Then
            lblStability.Text = "Stability: Stable at " & (CSR.MinimumCap / CapShip.CapCapacity * 100).ToString("N2") & "%"
        Else
            lblStability.Text = "Stability: Lasts " & EveHQ.Core.SkillFunctions.TimeToString(CSR.TimeToDrain, False)
        End If
    End Sub

    Private Sub UpdateEventList()
        adtResults.BeginUpdate()
        adtResults.Nodes.Clear()
        For Each CE As CapacitorEvent In CSR.Events
            If CE.SimTime >= iiStartTime.Value And CE.SimTime <= iiEndTime.Value Then
                Dim NewEvent As New Node
                NewEvent.Text = CE.SimTime.ToString("N2")
                NewEvent.Cells.Add(New Cell(CE.ModuleName))
                NewEvent.Cells.Add(New Cell(CE.StartingCap.ToString("N2")))
                NewEvent.Cells.Add(New Cell((-CE.ActivationCost).ToString("N2")))
                Dim EndCap As Double = Math.Min(Math.Max(CE.StartingCap - CE.ActivationCost, 0), CapShip.CapCapacity)
                NewEvent.Cells.Add(New Cell(EndCap.ToString("N2")))
                NewEvent.Cells.Add(New Cell((EndCap / CapShip.CapCapacity * 100).ToString("N2") & "%"))
                NewEvent.Cells.Add(New Cell(CE.RechargeRate.ToString("N2")))
                adtResults.Nodes.Add(NewEvent)
            End If
        Next
        adtResults.EndUpdate()
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
        Call Me.ExportToClipboard("Capacitor Simulation Results", adtResults, ControlChars.Tab)
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