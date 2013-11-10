Imports System.Drawing
Imports System.Windows.Forms.DataVisualization.Charting
Imports System.Drawing.Drawing2D

Namespace CoreReports

    Public Class PieCollectedDataHelper

#Region "Fields"

        ''' <summary>
        ''' Specifies the percentage of the total series values. This value determines 
        ''' if the data point value is a "small" value and should be shown as collected.
        ''' </summary>
        Public CollectedPercentage As Double = 5.0

        ''' <summary>
        ''' Position in relative coordinates ( 0,0 - top left corner; 100,100 - bottom right corner)
        ''' where original and supplemental pie charts should be placed.
        ''' </summary>
        Public ChartAreaPosition As RectangleF = New RectangleF(5.0F, 5.0F, 90.0F, 90.0F)

        ''' <summary>
        ''' Indicates if small segments should be shown as one "collected" segment in the original series
        ''' </summary>
        Public ShowCollectedDataAsOneSlice As Boolean = False

        ''' <summary>
        ''' Spacing between the original and supplemental chart areas in percentage
        ''' </summary>
        Public ChartAreaSpacing As Single = 5.0F

        ''' <summary>
        ''' Size ratio between the original and supplemental chart areas.
        ''' Value of 1.0f indicates that same area size will be used.
        ''' </summary>
        Public SupplementedAreaSizeRatio As Single = 0.9F

        ''' <summary>
        ''' Color of the connection lines
        ''' </summary>
        Public ConnectionLinesColor As Color = Color.FromArgb(64, 64, 64)

        ''' <summary>
        ''' Collected pie segment label
        ''' </summary>
        Public CollectedLabel As String = "Other"


        ' Reference to the parameters
        Private ReadOnly _chartControl As Chart = Nothing
        Private _series As Series = Nothing

        ' Internal use fields
        Private _supplementalSeries As Series = Nothing
        Private _originalChartArea As ChartArea = Nothing
        Private _supplementalChartArea As ChartArea = Nothing
        Private _collectedPieSliceAngle As Single = 0.0F

#End Region   ' Fields

#Region "Constructor"

        ''' <summary>
        ''' Public constructor.
        ''' </summary>
        ''' <param name="chartControl">Reference to the chart control.</param>
        Public Sub New(ByVal chartControl As Chart)
            chartControl = chartControl

            ' Handle chart PostPaint event to draw the "connection" between the 
            ' collected pie slice and supplemental chart.
            AddHandler chartControl.PostPaint, AddressOf chart_PostPaint
        End Sub

#End Region   ' Constructor

#Region "Methods"

        ''' <summary>
        ''' Shows small pie segments as supplemental pie chart series in the new chart area.
        ''' </summary>
        ''' <param name="seriesName">Series name </param>
        Public Sub ShowSmallSegmentsAsSupplementalPie(ByVal seriesName As String)
            ' Validate input
            If _chartControl Is Nothing Then
                ' ReSharper disable once NotResolvedInText
                Throw (New ArgumentNullException("_chartControl"))
            End If
            If CollectedPercentage > 100.0 OrElse CollectedPercentage < 0.0 Then
                ' ReSharper disable once NotResolvedInText
                Throw (New ArgumentException("Value must be in range from 0 to 100 percent.", "CollectedPercentage"))
            End If

            ' Initialize reference to the series
            _series = _chartControl.Series(seriesName)

            ' Check input series type
            If _series.ChartType <> SeriesChartType.Pie AndAlso _series.ChartType <> SeriesChartType.Doughnut Then
                Throw (New InvalidOperationException("Only series with Pie or Doughnut chart type can be used."))
            End If

            ' Check if specified series has data points
            If _series.Points.Count = 0 Then
                Throw (New InvalidOperationException("Cannot perform operatiuon on an empty series."))
            End If

            ' Create "collected" pie slice in original series
            _supplementalChartArea = Nothing
            If CreateCollectedPie() Then
                ' Calculate width of supplemental chart area
                Dim supplementalWidth As Single = (ChartAreaPosition.Width - ChartAreaSpacing) / 2.0F * SupplementedAreaSizeRatio

                ' Adjust position of the original chart area
                _originalChartArea = _chartControl.ChartAreas(_series.ChartArea)
                _originalChartArea.Position.X = ChartAreaPosition.X
                _originalChartArea.Position.Y = ChartAreaPosition.Y
                _originalChartArea.Position.Width = ChartAreaPosition.Width - supplementalWidth - ChartAreaSpacing
                _originalChartArea.Position.Height = ChartAreaPosition.Height

                ' Original chart area must be in 2D mode
                _originalChartArea.Area3DStyle.Enable3D = False

                ' Create and adjust position of the supplemental chart area
                _supplementalChartArea = New ChartArea()
                _supplementalChartArea.Name = _originalChartArea.Name & "_Supplemental"
                _supplementalChartArea.Position.X = _originalChartArea.Position.Right() + ChartAreaSpacing
                _supplementalChartArea.Position.Y = ChartAreaPosition.Y
                _supplementalChartArea.Position.Width = supplementalWidth
                _supplementalChartArea.Position.Height = ChartAreaPosition.Height
                _chartControl.ChartAreas.Add(_supplementalChartArea)

                ' Create supplemental pie chart series to show all the collected data
                _supplementalSeries.Name = _series.Name & "_Supplemental"
                _supplementalSeries.ChartArea = _supplementalChartArea.Name
                _chartControl.Series.Add(_supplementalSeries)

                ' Copy some attributes from the original chart area
                _supplementalChartArea.BackColor = _originalChartArea.BackColor
                _supplementalChartArea.BorderColor = _originalChartArea.BorderColor
                _supplementalChartArea.BorderWidth = _originalChartArea.BorderWidth
                _supplementalChartArea.ShadowOffset = _originalChartArea.ShadowOffset

                ' Copy some attributes from the original series
                _supplementalSeries.ChartType = _series.ChartType
                _supplementalSeries.Palette = _series.Palette
                _supplementalSeries.ShadowOffset = _series.ShadowOffset
                _supplementalSeries.BorderColor = _series.BorderColor
                _supplementalSeries.BorderWidth = _series.BorderWidth
                _supplementalSeries.IsValueShownAsLabel = _series.IsValueShownAsLabel
                _supplementalSeries.LabelBackColor = _series.LabelBackColor
                _supplementalSeries.LabelBorderColor = _series.LabelBorderColor
                _supplementalSeries.LabelBorderWidth = _series.LabelBorderWidth
                _supplementalSeries.LabelFormat = _series.LabelFormat
                _supplementalSeries.Font = _series.Font
            End If
        End Sub

        ''' <summary>
        ''' Creates the "collected" pie slice data point by re moving and accumulating all
        ''' the values of the data points which values are less then specified percentage.
        ''' </summary>
        ''' <returns>True if collected pie slice was created.</returns>
        Private Function CreateCollectedPie() As Boolean
            ' Create supplemental series
            _supplementalSeries = New Series()

            ' Calculate total vale of all point in series
            Dim total As Double = 0.0
            Dim dataPoint As DataPoint
            For Each dataPoint In _series.Points
                total += Math.Abs(dataPoint.YValues(0))
            Next dataPoint

            ' Count how many data points will be presented as collected
            Dim minValue As Double = total / 100.0 * CollectedPercentage
            Dim collectedPointsCount As Integer = 0
            Dim index As Integer = 0
            Do While index < _series.Points.Count
                Dim pointValue As Double = Math.Abs(_series.Points(index).YValues(0))
                If pointValue <= minValue Then
                    collectedPointsCount += 1
                End If
                index += 1
            Loop

            ' Do not collect data points if one or less points left in the original series
            If (_series.Points.Count - collectedPointsCount) <= 1 OrElse collectedPointsCount <= 1 Then
                Return False
            End If

            ' Add Collected data point into series before applying palette colors
            Dim colectedDataPoint As DataPoint = Nothing
            If ShowCollectedDataAsOneSlice Then
                colectedDataPoint = New DataPoint(_series)
                _series.Points.Add(colectedDataPoint)
            End If


            ' Apply pallete colors to series to save same data point colors
            ' in supplemental series.
            _chartControl.ApplyPaletteColors()
            Dim dataPoint2 As DataPoint
            For Each dataPoint2 In _series.Points
                ' Setting data point color to itself will clear the internal flag which
                ' indicates that point color should be taken from the palette again when
                ' control is rendered next time.
                dataPoint2.Color = dataPoint2.Color
            Next dataPoint2

            ' Remove points which value is less than specified percentage from total
            Dim collectedValue As Double = 0.0
            index = 0
            Do While index < _series.Points.Count
                Dim pointValue As Double = Math.Abs(_series.Points(index).YValues(0))
                If pointValue <= minValue AndAlso Not _series.Points(index) Is colectedDataPoint Then
                    ' Add point value to the collected value
                    collectedValue += pointValue

                    ' Add point to supplemental series
                    _supplementalSeries.Points.Add(_series.Points(index).Clone())

                    ' Remove point from the series
                    _series.Points.RemoveAt(index)
                    index -= 1
                End If
                index += 1
            Loop

            ' Add all collected data points at the end of the series
            If (Not ShowCollectedDataAsOneSlice) Then
                Dim dataPt As DataPoint
                For Each dataPt In _supplementalSeries.Points
                    Dim dataPointCollected As DataPoint = dataPt.Clone()
                    dataPt.IsVisibleInLegend = False
                    _series.Points.Add(dataPointCollected)

                    ' Disable labels in collected slices
                    dataPointCollected.Label = String.Empty
                    dataPointCollected.LegendText = dataPointCollected.AxisLabel
                    dataPointCollected.AxisLabel = String.Empty
                    dataPointCollected.IsValueShownAsLabel = False
                Next dataPt
            End If

            ' Check if we need to add the "collected" data point
            If collectedValue > 0.0 Then
                ' Set collected data point value and other attributes
                If ShowCollectedDataAsOneSlice Then
                    colectedDataPoint.YValues(0) = collectedValue
                    colectedDataPoint.Label = CollectedLabel
                    colectedDataPoint.IsVisibleInLegend = False

                    ' Note: Collected data point may be exploded
                    'colectedDataPoint["Exploded"] = "true";
                End If

                ' Calculate collected pie slice angle
                _collectedPieSliceAngle = CSng((360.0F / 100.0F) * (collectedValue / (total / 100)))

                ' Adjust the Pie chart start angle, so that the middle of the 
                ' collected slice looks directly at 3 o'clock.
                Dim startAngle As Integer = CInt(Math.Round(_collectedPieSliceAngle / 2.0))
                _series("PieStartAngle") = startAngle.ToString()

                Return True
            ElseIf Not colectedDataPoint Is Nothing Then
                ' Remove collected data point
                _series.Points.Remove(colectedDataPoint)
            End If

            Return False
        End Function

        ''' <summary>
        ''' Chart post paint event handler. 
        ''' Used to draw the "connection" lines between the original and supplemental pies.
        ''' </summary>
        ''' <param name="sender">Event sender.</param>
        ''' <param name="e">Event arguments.</param>
        Private Sub chart_PostPaint(ByVal sender As Object, ByVal e As ChartPaintEventArgs)
            Dim area As ChartArea = TryCast(sender, ChartArea)
            If (area IsNot Nothing) Then
                If Not _supplementalChartArea Is Nothing AndAlso area.Name = _supplementalChartArea.Name Then
                    ' Get position of the plotting areas in pixels
                    Dim originalPosition As RectangleF = GetChartAreaPlottingPosition(_originalChartArea, e.ChartGraphics)
                    Dim supplementalPosition As RectangleF = GetChartAreaPlottingPosition(_supplementalChartArea, e.ChartGraphics)

                    ' Get coordinates of the "connection" lines
                    Dim p1 As PointF = GetRotatedPlotAreaPoint(supplementalPosition, 325.0F)
                    Dim p2 As PointF = GetRotatedPlotAreaPoint(supplementalPosition, 215.0F)
                    Dim p3 As PointF = GetRotatedPlotAreaPoint(originalPosition, 90.0F - _collectedPieSliceAngle / 2.0F)
                    Dim p4 As PointF = GetRotatedPlotAreaPoint(originalPosition, 90.0F + _collectedPieSliceAngle / 2.0F)

                    ' Draw "connection lines"
                    'INSTANT VB NOTE: The following 'using' block is replaced by its pre-Visual Basic 2005 equivalent:
                    '					using(Pen pen = New Pen(ConnectionLinesColor, 1))
                    Dim pen As Pen = New Pen(ConnectionLinesColor, 1)
                    Try
                        e.ChartGraphics.Graphics.DrawLine(pen, p1, p3)
                        e.ChartGraphics.Graphics.DrawLine(pen, p2, p4)
                    Finally
                        If TypeOf pen Is IDisposable Then
                            Dim disp As IDisposable = pen
                            disp.Dispose()
                        End If
                    End Try
                End If
            End If
        End Sub

        ''' <summary>
        ''' Helper method which calculates a point on the edje of the pie chart using 
        ''' specified angle.
        ''' </summary>
        ''' <param name="areaPosition">Chart are position in pixels.</param>
        ''' <param name="angle">Point angle in degrees.</param>
        ''' <returns>Point location in pixels.</returns>
        Private Function GetRotatedPlotAreaPoint(ByVal areaPosition As RectangleF, ByVal angle As Single) As PointF
            Dim points As PointF() = New PointF(0) {}
            points(0) = New PointF(areaPosition.X + areaPosition.Width / 2.0F, areaPosition.Y)
            Dim transformMatrix As Matrix = New Matrix()
            Try
                transformMatrix.RotateAt(angle, New PointF(areaPosition.X + areaPosition.Width / 2.0F, areaPosition.Y + areaPosition.Height / 2.0F))

                transformMatrix.TransformPoints(points)
            Finally
                If TypeOf transformMatrix Is IDisposable Then
                    Dim disp As IDisposable = transformMatrix
                    disp.Dispose()
                End If
            End Try
            Return points(0)
        End Function

        ''' <summary>
        ''' Helper method which calculates chart area plotting position in pixels.
        ''' </summary>
        ''' <param name="area">Chart area to get the plotting area position.</param>
        ''' <param name="chartGraphics">Chart graphics object.</param>
        ''' <returns>Chart area ploting area position in pixels.</returns>
        Private Function GetChartAreaPlottingPosition(ByVal area As ChartArea, ByVal chartGraphics As ChartGraphics) As RectangleF
            Dim plottingRect As RectangleF = area.Position.ToRectangleF()
            plottingRect.X += (area.Position.Width / 100.0F) * area.InnerPlotPosition.X
            plottingRect.Y += (area.Position.Height / 100.0F) * area.InnerPlotPosition.Y
            plottingRect.Width = (area.Position.Width / 100.0F) * area.InnerPlotPosition.Width
            plottingRect.Height = (area.Position.Height / 100.0F) * area.InnerPlotPosition.Height
            plottingRect = chartGraphics.GetAbsoluteRectangle(plottingRect)
            Return plottingRect
        End Function

#End Region   ' Methods

    End Class

End Namespace