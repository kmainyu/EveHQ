Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class FrmTargetSpeed
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
            btnClose = New DevComponents.DotNetBar.ButtonX()
            Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
            CType(Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
            SuspendLayout()
            '
            'btnClose
            '
            btnClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            btnClose.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            btnClose.Location = New System.Drawing.Point(660, 464)
            btnClose.Name = "btnClose"
            btnClose.Size = New System.Drawing.Size(75, 23)
            btnClose.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            btnClose.TabIndex = 15
            btnClose.Text = "Close"
            '
            'Chart1
            '
            Chart1.BackColor = System.Drawing.Color.WhiteSmoke
            Chart1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom
            Chart1.BackSecondaryColor = System.Drawing.Color.White
            Chart1.BorderlineColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(105, Byte), Integer))
            Chart1.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
            Chart1.BorderlineWidth = 2
            ChartArea1.Area3DStyle.Inclination = 15
            ChartArea1.Area3DStyle.IsClustered = True
            ChartArea1.Area3DStyle.IsRightAngleAxes = False
            ChartArea1.Area3DStyle.Perspective = 10
            ChartArea1.Area3DStyle.Rotation = 10
            ChartArea1.Area3DStyle.WallWidth = 0
            ChartArea1.AxisX.LabelAutoFitStyle = CType(((System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.IncreaseFont Or System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.DecreaseFont) _
                                                        Or System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.WordWrap), System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles)
            ChartArea1.AxisX.LabelStyle.Font = New System.Drawing.Font("Trebuchet MS", 8.25!, System.Drawing.FontStyle.Bold)
            ChartArea1.AxisX.LineColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
            ChartArea1.AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
            ChartArea1.AxisX.ScrollBar.LineColor = System.Drawing.Color.Black
            ChartArea1.AxisX.ScrollBar.Size = 10.0R
            ChartArea1.AxisX.Title = "Target Signature Radius (m)"
            ChartArea1.AxisY.LabelStyle.Font = New System.Drawing.Font("Trebuchet MS", 8.25!, System.Drawing.FontStyle.Bold)
            ChartArea1.AxisY.LineColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
            ChartArea1.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
            ChartArea1.AxisY.ScrollBar.LineColor = System.Drawing.Color.Black
            ChartArea1.AxisY.ScrollBar.Size = 10.0R
            ChartArea1.AxisY.Title = "Time to Lock (s)"
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
            Chart1.ChartAreas.Add(ChartArea1)
            Legend1.BackColor = System.Drawing.Color.Transparent
            Legend1.Enabled = False
            Legend1.Font = New System.Drawing.Font("Trebuchet MS", 8.25!, System.Drawing.FontStyle.Bold)
            Legend1.IsTextAutoFit = False
            Legend1.Name = "Default"
            Chart1.Legends.Add(Legend1)
            Chart1.Location = New System.Drawing.Point(12, 12)
            Chart1.Name = "Chart1"
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
            Chart1.Series.Add(Series1)
            Chart1.Series.Add(Series2)
            Chart1.Size = New System.Drawing.Size(723, 446)
            Chart1.TabIndex = 16
            Title1.Font = New System.Drawing.Font("Trebuchet MS", 12.0!, System.Drawing.FontStyle.Bold)
            Title1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(105, Byte), Integer))
            Title1.Name = "Title1"
            Title1.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
            Title1.ShadowOffset = 3
            Title1.Text = "Targeting Speed"
            Chart1.Titles.Add(Title1)
            '
            'frmTargetSpeed
            '
            AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            ClientSize = New System.Drawing.Size(747, 499)
            Controls.Add(Chart1)
            Controls.Add(btnClose)
            DoubleBuffered = True
            EnableGlass = False
            Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            MaximizeBox = False
            MinimizeBox = False
            Name = "frmTargetSpeed"
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Text = "Targeting Speed Analysis"
            CType(Chart1, System.ComponentModel.ISupportInitialize).EndInit()
            ResumeLayout(False)

        End Sub
        Friend WithEvents btnClose As DevComponents.DotNetBar.ButtonX
        Private WithEvents Chart1 As System.Windows.Forms.DataVisualization.Charting.Chart
    End Class
End NameSpace