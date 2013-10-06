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
            Dim DataPoint1 As System.Windows.Forms.DataVisualization.Charting.DataPoint = New System.Windows.Forms.DataVisualization.Charting.DataPoint(0.0R, 65.62R)
            Dim DataPoint2 As System.Windows.Forms.DataVisualization.Charting.DataPoint = New System.Windows.Forms.DataVisualization.Charting.DataPoint(0.0R, 75.54R)
            Dim DataPoint3 As System.Windows.Forms.DataVisualization.Charting.DataPoint = New System.Windows.Forms.DataVisualization.Charting.DataPoint(0.0R, 60.45R)
            Dim DataPoint4 As System.Windows.Forms.DataVisualization.Charting.DataPoint = New System.Windows.Forms.DataVisualization.Charting.DataPoint(0.0R, 55.73R)
            Dim DataPoint5 As System.Windows.Forms.DataVisualization.Charting.DataPoint = New System.Windows.Forms.DataVisualization.Charting.DataPoint(0.0R, 70.42R)
            Dim Title1 As System.Windows.Forms.DataVisualization.Charting.Title = New System.Windows.Forms.DataVisualization.Charting.Title()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmChartViewer))
            Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
            CType(Me.Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'Chart1
            '
            Me.Chart1.BackColor = System.Drawing.Color.WhiteSmoke
            Me.Chart1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom
            Me.Chart1.BackSecondaryColor = System.Drawing.Color.White
            Me.Chart1.BorderlineColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(105, Byte), Integer))
            Me.Chart1.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
            Me.Chart1.BorderlineWidth = 2
            Me.Chart1.BorderSkin.SkinStyle = System.Windows.Forms.DataVisualization.Charting.BorderSkinStyle.Emboss
            ChartArea1.Area3DStyle.IsClustered = True
            ChartArea1.Area3DStyle.IsRightAngleAxes = False
            ChartArea1.Area3DStyle.Perspective = 10
            ChartArea1.Area3DStyle.PointGapDepth = 0
            ChartArea1.Area3DStyle.Rotation = 0
            ChartArea1.Area3DStyle.WallWidth = 0
            ChartArea1.AxisX.LabelStyle.Font = New System.Drawing.Font("Trebuchet MS", 8.25!, System.Drawing.FontStyle.Bold)
            ChartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
            ChartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
            ChartArea1.AxisY.LabelStyle.Font = New System.Drawing.Font("Trebuchet MS", 8.25!, System.Drawing.FontStyle.Bold)
            ChartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
            ChartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
            ChartArea1.BackColor = System.Drawing.Color.Transparent
            ChartArea1.BackSecondaryColor = System.Drawing.Color.Transparent
            ChartArea1.BorderColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
            ChartArea1.BorderWidth = 0
            ChartArea1.Name = "Default"
            ChartArea1.ShadowColor = System.Drawing.Color.Transparent
            Me.Chart1.ChartAreas.Add(ChartArea1)
            Me.Chart1.Dock = System.Windows.Forms.DockStyle.Fill
            Legend1.Alignment = System.Drawing.StringAlignment.Center
            Legend1.BackColor = System.Drawing.Color.Transparent
            Legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom
            Legend1.Enabled = False
            Legend1.Font = New System.Drawing.Font("Trebuchet MS", 8.25!, System.Drawing.FontStyle.Bold)
            Legend1.IsTextAutoFit = False
            Legend1.Name = "Default"
            Me.Chart1.Legends.Add(Legend1)
            Me.Chart1.Location = New System.Drawing.Point(0, 0)
            Me.Chart1.Name = "Chart1"
            Me.Chart1.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel
            Series1.BorderColor = System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(26, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(105, Byte), Integer))
            Series1.ChartArea = "Default"
            Series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut
            Series1.Color = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(65, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(240, Byte), Integer))
            Series1.CustomProperties = "DoughnutRadius=60, PieDrawingStyle=SoftEdge, PieLabelStyle=Disabled"
            Series1.Legend = "Default"
            Series1.Name = "Default"
            DataPoint1.CustomProperties = "Exploded=false"
            DataPoint1.Label = "France"
            DataPoint2.CustomProperties = "Exploded=false"
            DataPoint2.Label = "Canada"
            DataPoint3.CustomProperties = "Exploded=false"
            DataPoint3.Label = "UK"
            DataPoint4.CustomProperties = "Exploded=false"
            DataPoint4.Label = "USA"
            DataPoint5.CustomProperties = "Exploded=false"
            DataPoint5.Label = "Italy"
            Series1.Points.Add(DataPoint1)
            Series1.Points.Add(DataPoint2)
            Series1.Points.Add(DataPoint3)
            Series1.Points.Add(DataPoint4)
            Series1.Points.Add(DataPoint5)
            Series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[String]
            Series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.[Double]
            Me.Chart1.Series.Add(Series1)
            Me.Chart1.Size = New System.Drawing.Size(725, 512)
            Me.Chart1.TabIndex = 1
            Title1.Font = New System.Drawing.Font("Trebuchet MS", 14.25!, System.Drawing.FontStyle.Bold)
            Title1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(105, Byte), Integer))
            Title1.Name = "Title1"
            Title1.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
            Title1.ShadowOffset = 3
            Title1.Text = "Doughnut Chart"
            Me.Chart1.Titles.Add(Title1)
            '
            'FrmChartViewer
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(725, 512)
            Me.Controls.Add(Me.Chart1)
            Me.DoubleBuffered = True
            Me.EnableGlass = False
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.Name = "FrmChartViewer"
            Me.Text = "frmChartViewer"
            CType(Me.Chart1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Public WithEvents Chart1 As System.Windows.Forms.DataVisualization.Charting.Chart
    End Class
End NameSpace