'==============================================================================
'
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2014  EveHQ Development Team
'
' This file is part of EveHQ.
'
' The source code for EveHQ is free and you may redistribute 
' it and/or modify it under the terms of the MIT License. 
'
' Refer to the NOTICES file in the root folder of EVEHQ source
' project for details of 3rd party components that are covered
' under their own, separate licenses.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the MIT 
' license below for details.
'
' ------------------------------------------------------------------------------
'
' The MIT License (MIT)
'
' Copyright © 2005-2014  EveHQ Development Team
'
' Permission is hereby granted, free of charge, to any person obtaining a copy
' of this software and associated documentation files (the "Software"), to deal
' in the Software without restriction, including without limitation the rights
' to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
' copies of the Software, and to permit persons to whom the Software is
' furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in
' all copies or substantial portions of the Software.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
' IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
' FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
' AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
' LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
' OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
' THE SOFTWARE.
'
' ==============================================================================

Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Windows.Forms

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

            If _cShipType.ScanResolution > 0 Then

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
                Chart1.Series("Ships").XAxisType = AxisType.Primary
                Chart1.ChartAreas("Default").AxisX.Minimum = 0
                Chart1.ChartAreas("Default").AxisX.Maximum = 500
                Chart1.ChartAreas("Default").AxisY.Minimum = 0
                Chart1.ChartAreas("Default").AxisY.Maximum = Int(LockTime(10)) + 1
                Chart1.ChartAreas("Default").AxisX2.Minimum = 0
                Chart1.ChartAreas("Default").AxisX2.Maximum = 500
                Chart1.ChartAreas("Default").AxisY2.Minimum = 0
                Chart1.ChartAreas("Default").AxisY2.Maximum = Int(LockTime(10)) + 1
                Chart1.ChartAreas("Default").AxisX.LabelStyle.Format = "N2"
                Chart1.ChartAreas("Default").AxisY.LabelStyle.Format = "N2"
                Chart1.ChartAreas("Default").CursorX.Interval = 0
                Chart1.ChartAreas("Default").CursorY.Interval = 0
                Chart1.Series("LockTime").ToolTip = "Sig Radius: #VALXm\nLock Time: #VALYs"

                Chart1.Titles(0).Text = "Targeting Speed Analysis - " & _cShipType.Name & " (" & _cShipType.ScanResolution.ToString("N2") & "mm)"

                Dim p As Double, x As Double, y As Double
                For p = 1 To 10000 ' Signature radius
                    x = p / 5
                    y = LockTime(x)

                    Chart1.Series("LockTime").Points.AddXY(x, y)

                    Select Case x
                        Case 25 ' Pod
                            Dim sp As New DataPoint
                            sp.SetValueXY(x, y)
                            sp.Label = "Pod"
                            Chart1.Series("Ships").Points.Add(sp)
                        Case 35 ' Crow
                            Dim sp As New DataPoint
                            sp.SetValueXY(x, y)
                            sp.Label = "Crow"
                            Chart1.Series("Ships").Points.Add(sp)
                        Case 49 ' Helios
                            Dim sp As New DataPoint
                            sp.SetValueXY(x, y)
                            sp.Label = "Helios"
                            Chart1.Series("Ships").Points.Add(sp)
                        Case 125 ' Zealot
                            Dim sp As New DataPoint
                            sp.SetValueXY(x, y)
                            sp.Label = "Zealot"
                            Chart1.Series("Ships").Points.Add(sp)
                        Case 180 ' Falcon
                            Dim sp As New DataPoint
                            sp.SetValueXY(x, y)
                            sp.Label = "Falcon"
                            Chart1.Series("Ships").Points.Add(sp)
                        Case 300 ' Myrmidon
                            Dim sp As New DataPoint
                            sp.SetValueXY(x, y)
                            sp.Label = "Myrmidon"
                            Chart1.Series("Ships").Points.Add(sp)
                        Case 400 ' Megathron
                            Dim sp As New DataPoint
                            sp.SetValueXY(x, y)
                            sp.Label = "Megathron"
                            Chart1.Series("Ships").Points.Add(sp)
                    End Select
                Next
            Else
                Chart1.Titles(0).Text = "Targeting Speed Analysis - " & _cShipType.Name & " (Unable to target)"
            End If
            ShowDialog()
        End Sub

        Private Function Asinh(ByVal x As Double) As Double
            Return Math.Log(x + Math.Sqrt(1 + x * x))
        End Function

        Private Function LockTime(ByVal sigRadius As Double) As Double
            If _cShipType.ScanResolution <> 0 Then
                Return Math.Round(((40000 / _cShipType.ScanResolution) / (Asinh(sigRadius) ^ 2)), 2) ' Lock time
            Else
                Return 0
            End If
        End Function

        Private Sub mnuResetZoom_Click(sender As System.Object, e As EventArgs) Handles mnuResetZoom.Click
            Chart1.ChartAreas("Default").AxisX.ScaleView.ZoomReset(0)
            Chart1.ChartAreas("Default").AxisY.ScaleView.ZoomReset(0)
            Chart1.ChartAreas("Default").CursorX.SelectionStart = Double.NaN
            Chart1.ChartAreas("Default").CursorY.SelectionEnd = Double.NaN
        End Sub

        Private Sub mnuSaveImage_Click(sender As System.Object, e As EventArgs) Handles mnuSaveImage.Click
            Try
                Chart1.SaveImage(IO.Path.Combine(Core.HQ.ReportFolder, Chart1.Titles(0).Text & ".png"), Drawing.Imaging.ImageFormat.Png)
                MessageBox.Show("Image successfully saved into the EveHQ report folder.", "Save Successful", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("There was an error saving the image: " & ex.Message, "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub Chart1_AxisScrollBarClicked(sender As Object, e As DataVisualization.Charting.ScrollBarEventArgs) Handles Chart1.AxisScrollBarClicked
            If (e.ButtonType = ScrollBarButtonType.ZoomReset) Then
                e.IsHandled = True
                Chart1.ChartAreas("Default").AxisX.ScaleView.ZoomReset(0)
                Chart1.ChartAreas("Default").AxisY.ScaleView.ZoomReset(0)
                Chart1.ChartAreas("Default").CursorX.SelectionStart = Double.NaN
                Chart1.ChartAreas("Default").CursorY.SelectionEnd = Double.NaN
            End If
        End Sub

    End Class
End NameSpace