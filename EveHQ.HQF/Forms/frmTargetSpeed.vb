' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2012-2013 EveHQ Development Team
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
Imports System.Windows.Forms.DataVisualization.Charting

Namespace Forms

    Public Class FrmTargetSpeed

        Private _cShipType As Ship

        Public Property ShipType() As Ship
            Get
                Return _cShipType
            End Get
            Set(ByVal value As Ship)
                _cShipType = value
                ' Trigger listview update, then display form
                Call DisplayTargetSpeedGraph()
            End Set
        End Property

        Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
            Close()
        End Sub

        Private Sub DisplayTargetSpeedGraph()

            ' Create series
            Chart1.Series.Clear()
            Chart1.Series.Add("LockTime")
            Chart1.Series.Add("Ships")

            ' Set chart type and styles
            Chart1.Series("LockTime").ChartType = SeriesChartType.FastLine
            Chart1.Series("Ships").ChartType = SeriesChartType.Point
            Chart1.Series("Ships").IsValueShownAsLabel = False
            Chart1.Series("Ships")("LabelStyle") = "TopRight"
            Chart1.Series("Ships").MarkerSize = 15
            Chart1.Series("Ships").MarkerStyle = MarkerStyle.Cross
            Chart1.Series("LockTime").XAxisType = AxisType.Primary
            Chart1.Series("Ships").XAxisType = AxisType.Secondary
            Chart1.ChartAreas("Default").AxisX.Minimum = 0
            Chart1.ChartAreas("Default").AxisX.Maximum = 500
            Chart1.ChartAreas("Default").AxisY.Minimum = 0
            Chart1.ChartAreas("Default").AxisY.Maximum = Int(LockTime(10)) + 1
            Chart1.ChartAreas("Default").AxisX2.Minimum = 0
            Chart1.ChartAreas("Default").AxisX2.Maximum = 500
            Chart1.ChartAreas("Default").AxisY2.Minimum = 0
            Chart1.ChartAreas("Default").AxisY2.Maximum = Int(LockTime(10)) + 1

            Chart1.Titles(0).Text = "Targeting Speed Analysis - " & _cShipType.Name & " (" & _cShipType.ScanResolution.ToString("N2") & "mm)"

            Dim p As Double, x As Double, y As Double
            For p = 1 To 10000 ' Signature radius
                x = p / 5
                y = LockTime(x)

                Chart1.Series("LockTime").Points.AddXY(x, y)

                Select Case x
                    Case 25 ' Pod
                        Dim sp As New Windows.Forms.DataVisualization.Charting.DataPoint
                        sp.SetValueXY(x, y)
                        sp.Label = "Pod"
                        Chart1.Series("Ships").Points.Add(sp)
                    Case 35 ' Crow
                        Dim sp As New Windows.Forms.DataVisualization.Charting.DataPoint
                        sp.SetValueXY(x, y)
                        sp.Label = "Crow"
                        Chart1.Series("Ships").Points.Add(sp)
                    Case 49 ' Helios
                        Dim sp As New Windows.Forms.DataVisualization.Charting.DataPoint
                        sp.SetValueXY(x, y)
                        sp.Label = "Helios"
                        Chart1.Series("Ships").Points.Add(sp)
                    Case 125 ' Zealot
                        Dim sp As New Windows.Forms.DataVisualization.Charting.DataPoint
                        sp.SetValueXY(x, y)
                        sp.Label = "Zealot"
                        Chart1.Series("Ships").Points.Add(sp)
                    Case 180 ' Falcon
                        Dim sp As New Windows.Forms.DataVisualization.Charting.DataPoint
                        sp.SetValueXY(x, y)
                        sp.Label = "Falcon"
                        Chart1.Series("Ships").Points.Add(sp)
                    Case 300 ' Myrmidon
                        Dim sp As New Windows.Forms.DataVisualization.Charting.DataPoint
                        sp.SetValueXY(x, y)
                        sp.Label = "Myrmisdon"
                        Chart1.Series("Ships").Points.Add(sp)
                    Case 400 ' Megathron
                        Dim sp As New Windows.Forms.DataVisualization.Charting.DataPoint
                        sp.SetValueXY(x, y)
                        sp.Label = "Megathron"
                        Chart1.Series("Ships").Points.Add(sp)
                End Select
            Next
            ShowDialog()
        End Sub

        Private Function Asinh(ByVal x As Double) As Double
            Return Math.Log(x + Math.Sqrt(1 + x * x))
        End Function

        Private Function LockTime(ByVal sigRadius As Double) As Double
            Return ((40000 / _cShipType.ScanResolution) / (Asinh(sigRadius) ^ 2)) ' Lock time
        End Function

    End Class
End NameSpace