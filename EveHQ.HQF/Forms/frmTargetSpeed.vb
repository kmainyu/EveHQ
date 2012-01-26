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

Public Class frmTargetSpeed

    Private cShipType As Ship

    Public Property ShipType() As Ship
        Get
            Return cShipType
        End Get
        Set(ByVal value As Ship)
            cShipType = value
            ' Trigger listview update, then display form
            Call Me.DisplayTargetSpeedGraph()
        End Set
    End Property

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

    Private Sub DisplayTargetSpeedGraph()
        Dim myPane As GraphPane = zgcTargetSpeed.GraphPane

        ' Set the titles and axis labels
        myPane.Title.Text = "Targeting Speed Analysis - " & cShipType.Name & " (" & cShipType.ScanResolution.ToString("N2") & "mm)"
        myPane.XAxis.Title.Text = "Target Signature Radius (m)"
        myPane.XAxis.Scale.Min = 1
        myPane.XAxis.Scale.Max = 500
        myPane.YAxis.Title.Text = "Time to Lock (s)"
        myPane.YAxis.Scale.Min = 0
        myPane.YAxis.Scale.Max = LockTime(10)
        'myPane.YAxis.Scale.MaxAuto = True

        ' Create some points from 0 to 5000
        Dim list As New PointPairList()
        Dim list2 As New PointPairList()
        Dim listNames As New ArrayList
        Dim x As Double, y As Double

        For x = 0 To 5000 ' Signature radius
            y = LockTime(x)
            list.Add(x, y)
            Select Case x
                Case 25 ' Pod
                    list2.Add(x, y)
                    listNames.Add("Pod")
                Case 35 ' Crow
                    list2.Add(x, y)
                    listNames.Add("Crow")
                Case 49 ' Helios
                    list2.Add(x, y)
                    listNames.Add("Helios")
                Case 125 ' Zealot
                    list2.Add(x, y)
                    listNames.Add("Zealot")
                Case 180 ' Falcon
                    list2.Add(x, y)
                    listNames.Add("Falcon")
                Case 300 ' Myrmidon
                    list2.Add(x, y)
                    listNames.Add("Myrmidon")
                Case 400 ' Megathron
                    list2.Add(x, y)
                    listNames.Add("Megathron")

            End Select
        Next x

        ' Generate a blue curve with circle symbols, and "My Curve 2" in the legend
        Dim myCurve2 As LineItem = myPane.AddCurve("Specific Sig Radius", list2, Color.Black, SymbolType.XCross)
        Dim myCurve As LineItem = myPane.AddCurve("Lock Time", list, Color.Blue, SymbolType.None)
        myCurve.Line.IsAntiAlias = True
        myCurve2.Line.IsVisible = False

        For i As Integer = 0 To list2.Count - 1
            Dim pt As PointPair = myCurve2.Points(i)
            Dim Text As TextObj = New TextObj(listNames(i).ToString, pt.X + 5, pt.Y + (LockTime(10) / 50), CoordType.AxisXYScale, AlignH.Left, AlignV.Center)
            Text.ZOrder = ZOrder.A_InFront
            ' Hide the border and the fill
            Text.FontSpec.Border.IsVisible = False
            Text.FontSpec.Fill.IsVisible = False
            'text.FontSpec.Fill = new Fill( Color.FromArgb( 100, Color.White ) )
            ' Rotate the text to 90 degrees
            Text.FontSpec.Angle = 45
            myPane.GraphObjList.Add(Text)
        Next
        ' Fill the area under the curve with a white-red gradient at 45 degrees
        myCurve.Line.Fill = New Fill(Color.White, Color.Red, 45.0F)
        ' Make the symbols opaque by filling them with white
        myCurve.Symbol.Fill = New Fill(Color.White)

        ' Fill the axis background with a color gradient
        myPane.Chart.Fill = New Fill(Color.White, Color.LightSteelBlue, 45.0F)
        myPane.Legend.Fill = New Fill(Color.White, Color.LightSteelBlue, 45.0F)
        myPane.Legend.FontSpec.FontColor = Color.MidnightBlue

        ' Fill the pane background with a color gradient
        myPane.Fill = New Fill(Color.White, Color.LightSteelBlue, 45.0F)
        ' Calculate the Axis Scale Ranges
        zgcTargetSpeed.AxisChange()
        Me.ShowDialog()
    End Sub

    Private Function Asinh(ByVal x As Double) As Double
        Return Math.Log(x + Math.Sqrt(1 + x * x))
    End Function

    Private Function LockTime(ByVal sigRadius As Double) As Double
        Return ((40000 / cShipType.ScanResolution) / (Asinh(sigRadius) ^ 2)) ' Lock time
    End Function
End Class