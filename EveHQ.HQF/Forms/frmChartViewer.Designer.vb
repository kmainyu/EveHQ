Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class FrmChartViewer
        Inherits DevComponents.DotNetBar.Office2007Form

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
            Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
            Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
            Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
            Dim Title1 As System.Windows.Forms.DataVisualization.Charting.Title = New System.Windows.Forms.DataVisualization.Charting.Title()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmChartViewer))
            Me.pnlInfo = New DevComponents.DotNetBar.PanelEx()
            Me.lblGraphInfo = New DevComponents.DotNetBar.LabelX()
            Me.chartHQF = New System.Windows.Forms.DataVisualization.Charting.Chart()
            Me.pnlInfo.SuspendLayout()
            CType(Me.chartHQF, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlInfo
            '
            Me.pnlInfo.CanvasColor = System.Drawing.SystemColors.Control
            Me.pnlInfo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.pnlInfo.Controls.Add(Me.lblGraphInfo)
            Me.pnlInfo.Controls.Add(Me.chartHQF)
            Me.pnlInfo.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlInfo.Location = New System.Drawing.Point(0, 0)
            Me.pnlInfo.Name = "pnlInfo"
            Me.pnlInfo.Size = New System.Drawing.Size(753, 499)
            Me.pnlInfo.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.pnlInfo.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.pnlInfo.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.pnlInfo.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.pnlInfo.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.pnlInfo.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.pnlInfo.Style.GradientAngle = 90
            Me.pnlInfo.TabIndex = 1
            '
            'lblGraphInfo
            '
            '
            '
            '
            Me.lblGraphInfo.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblGraphInfo.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblGraphInfo.Location = New System.Drawing.Point(0, 450)
            Me.lblGraphInfo.Name = "lblGraphInfo"
            Me.lblGraphInfo.Size = New System.Drawing.Size(753, 49)
            Me.lblGraphInfo.TabIndex = 0
            '
            'chartHQF
            '
            Me.chartHQF.BackColor = System.Drawing.Color.WhiteSmoke
            Me.chartHQF.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom
            Me.chartHQF.BackSecondaryColor = System.Drawing.Color.White
            Me.chartHQF.BorderlineColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(105, Byte), Integer))
            Me.chartHQF.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
            Me.chartHQF.BorderlineWidth = 2
            ChartArea1.Area3DStyle.Inclination = 15
            ChartArea1.Area3DStyle.IsClustered = True
            ChartArea1.Area3DStyle.IsRightAngleAxes = False
            ChartArea1.Area3DStyle.Perspective = 10
            ChartArea1.Area3DStyle.Rotation = 10
            ChartArea1.Area3DStyle.WallWidth = 0
            ChartArea1.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.[True]
            ChartArea1.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount
            ChartArea1.AxisX.IsLabelAutoFit = False
            ChartArea1.AxisX.IsStartedFromZero = False
            ChartArea1.AxisX.LabelAutoFitMaxFontSize = 7
            ChartArea1.AxisX.LabelAutoFitStyle = CType(((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont Or System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont) _
                Or System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.WordWrap), System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles)
            ChartArea1.AxisX.LabelStyle.Font = New System.Drawing.Font("Trebuchet MS", 8.25!, System.Drawing.FontStyle.Bold)
            ChartArea1.AxisX.LabelStyle.Format = "N0"
            ChartArea1.AxisX.LabelStyle.Interval = 0.0R
            ChartArea1.AxisX.LabelStyle.IntervalOffset = 0.0R
            ChartArea1.AxisX.LabelStyle.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.[Auto]
            ChartArea1.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.[Auto]
            ChartArea1.AxisX.LabelStyle.IsStaggered = True
            ChartArea1.AxisX.LabelStyle.TruncatedLabels = True
            ChartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
            ChartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
            ChartArea1.AxisX.MajorTickMark.Interval = 0.0R
            ChartArea1.AxisX.MajorTickMark.IntervalOffset = 0.0R
            ChartArea1.AxisX.MajorTickMark.IntervalOffsetType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.[Auto]
            ChartArea1.AxisX.MajorTickMark.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.[Auto]
            ChartArea1.AxisX.ScrollBar.LineColor = System.Drawing.Color.Black
            ChartArea1.AxisX.ScrollBar.Size = 10.0R
            ChartArea1.AxisX.Title = "Range (m)"
            ChartArea1.AxisX.TitleFont = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            ChartArea1.AxisY.IsLabelAutoFit = False
            ChartArea1.AxisY.LabelStyle.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            ChartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
            ChartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
            ChartArea1.AxisY.ScrollBar.LineColor = System.Drawing.Color.Black
            ChartArea1.AxisY.ScrollBar.Size = 10.0R
            ChartArea1.AxisY.Title = "Hit Chance (%)"
            ChartArea1.BackColor = System.Drawing.Color.Gainsboro
            ChartArea1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom
            ChartArea1.BackSecondaryColor = System.Drawing.Color.White
            ChartArea1.BorderColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
            ChartArea1.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
            ChartArea1.CursorX.IsUserEnabled = True
            ChartArea1.CursorX.IsUserSelectionEnabled = True
            ChartArea1.CursorY.IsUserEnabled = True
            ChartArea1.CursorY.IsUserSelectionEnabled = True
            ChartArea1.Name = "Default"
            ChartArea1.ShadowColor = System.Drawing.Color.Transparent
            Me.chartHQF.ChartAreas.Add(ChartArea1)
            Me.chartHQF.Dock = System.Windows.Forms.DockStyle.Top
            Legend1.Alignment = System.Drawing.StringAlignment.Center
            Legend1.BackColor = System.Drawing.Color.Transparent
            Legend1.DockedToChartArea = "Default"
            Legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom
            Legend1.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
            Legend1.IsTextAutoFit = False
            Legend1.Name = "Default"
            Legend1.TitleFont = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.chartHQF.Legends.Add(Legend1)
            Me.chartHQF.Location = New System.Drawing.Point(0, 0)
            Me.chartHQF.Name = "chartHQF"
            Series1.BorderColor = System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(26, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(105, Byte), Integer))
            Series1.ChartArea = "Default"
            Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine
            Series1.Legend = "Default"
            Series1.Name = "Series1"
            Series1.ShadowColor = System.Drawing.Color.Black
            Series2.BorderColor = System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(26, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(105, Byte), Integer))
            Series2.ChartArea = "Default"
            Series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine
            Series2.Color = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(10, Byte), Integer))
            Series2.Legend = "Default"
            Series2.Name = "Series2"
            Series2.ShadowColor = System.Drawing.Color.Black
            Me.chartHQF.Series.Add(Series1)
            Me.chartHQF.Series.Add(Series2)
            Me.chartHQF.Size = New System.Drawing.Size(753, 450)
            Me.chartHQF.TabIndex = 18
            Title1.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Title1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(105, Byte), Integer))
            Title1.Name = "Title1"
            Title1.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
            Title1.ShadowOffset = 3
            Title1.Text = "Range vs Hit Chance"
            Me.chartHQF.Titles.Add(Title1)
            '
            'FrmChartViewer
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(753, 499)
            Me.Controls.Add(Me.pnlInfo)
            Me.DoubleBuffered = True
            Me.EnableGlass = False
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.Name = "FrmChartViewer"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "HQF Graph"
            Me.TopMost = True
            Me.pnlInfo.ResumeLayout(False)
            CType(Me.chartHQF, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents pnlInfo As DevComponents.DotNetBar.PanelEx
        Friend WithEvents lblGraphInfo As DevComponents.DotNetBar.LabelX
        Friend WithEvents chartHQF As System.Windows.Forms.DataVisualization.Charting.Chart
    End Class
End NameSpace