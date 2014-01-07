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
            Me.components = New System.ComponentModel.Container()
            Dim ChartArea1 As System.Windows.Forms.DataVisualization.Charting.ChartArea = New System.Windows.Forms.DataVisualization.Charting.ChartArea()
            Dim Legend1 As System.Windows.Forms.DataVisualization.Charting.Legend = New System.Windows.Forms.DataVisualization.Charting.Legend()
            Dim Series1 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
            Dim Series2 As System.Windows.Forms.DataVisualization.Charting.Series = New System.Windows.Forms.DataVisualization.Charting.Series()
            Dim Title1 As System.Windows.Forms.DataVisualization.Charting.Title = New System.Windows.Forms.DataVisualization.Charting.Title()
            Me.btnClose = New DevComponents.DotNetBar.ButtonX()
            Me.Chart1 = New System.Windows.Forms.DataVisualization.Charting.Chart()
            Me.ctxGraph = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.mnuResetZoom = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
            Me.mnuSaveImage = New System.Windows.Forms.ToolStripMenuItem()
            CType(Me.Chart1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.ctxGraph.SuspendLayout()
            Me.SuspendLayout()
            '
            'btnClose
            '
            Me.btnClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnClose.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnClose.Location = New System.Drawing.Point(660, 464)
            Me.btnClose.Name = "btnClose"
            Me.btnClose.Size = New System.Drawing.Size(75, 23)
            Me.btnClose.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnClose.TabIndex = 15
            Me.btnClose.Text = "Close"
            '
            'Chart1
            '
            Me.Chart1.BackColor = System.Drawing.Color.WhiteSmoke
            Me.Chart1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom
            Me.Chart1.BackSecondaryColor = System.Drawing.Color.White
            Me.Chart1.BorderlineColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(105, Byte), Integer))
            Me.Chart1.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid
            Me.Chart1.BorderlineWidth = 2
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
            Me.Chart1.ChartAreas.Add(ChartArea1)
            Me.Chart1.ContextMenuStrip = Me.ctxGraph
            Legend1.BackColor = System.Drawing.Color.Transparent
            Legend1.Enabled = False
            Legend1.Font = New System.Drawing.Font("Trebuchet MS", 8.25!, System.Drawing.FontStyle.Bold)
            Legend1.IsTextAutoFit = False
            Legend1.Name = "Default"
            Me.Chart1.Legends.Add(Legend1)
            Me.Chart1.Location = New System.Drawing.Point(12, 12)
            Me.Chart1.Name = "Chart1"
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
            Me.Chart1.Series.Add(Series1)
            Me.Chart1.Series.Add(Series2)
            Me.Chart1.Size = New System.Drawing.Size(723, 446)
            Me.Chart1.TabIndex = 16
            Title1.Font = New System.Drawing.Font("Trebuchet MS", 12.0!, System.Drawing.FontStyle.Bold)
            Title1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(26, Byte), Integer), CType(CType(59, Byte), Integer), CType(CType(105, Byte), Integer))
            Title1.Name = "Title1"
            Title1.ShadowColor = System.Drawing.Color.FromArgb(CType(CType(32, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
            Title1.ShadowOffset = 3
            Title1.Text = "Targeting Speed"
            Me.Chart1.Titles.Add(Title1)
            '
            'ctxGraph
            '
            Me.ctxGraph.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuResetZoom, Me.ToolStripMenuItem1, Me.mnuSaveImage})
            Me.ctxGraph.Name = "ctxGraph"
            Me.ctxGraph.Size = New System.Drawing.Size(151, 54)
            '
            'mnuResetZoom
            '
            Me.mnuResetZoom.Name = "mnuResetZoom"
            Me.mnuResetZoom.Size = New System.Drawing.Size(150, 22)
            Me.mnuResetZoom.Text = "Reset Zoom"
            '
            'ToolStripMenuItem1
            '
            Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
            Me.ToolStripMenuItem1.Size = New System.Drawing.Size(147, 6)
            '
            'mnuSaveImage
            '
            Me.mnuSaveImage.Name = "mnuSaveImage"
            Me.mnuSaveImage.Size = New System.Drawing.Size(150, 22)
            Me.mnuSaveImage.Text = "Save As Image"
            '
            'FrmTargetSpeed
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(747, 499)
            Me.Controls.Add(Me.Chart1)
            Me.Controls.Add(Me.btnClose)
            Me.DoubleBuffered = True
            Me.EnableGlass = False
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "FrmTargetSpeed"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Targeting Speed Analysis"
            CType(Me.Chart1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ctxGraph.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents btnClose As DevComponents.DotNetBar.ButtonX
        Private WithEvents Chart1 As System.Windows.Forms.DataVisualization.Charting.Chart
        Friend WithEvents ctxGraph As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents mnuResetZoom As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mnuSaveImage As System.Windows.Forms.ToolStripMenuItem
    End Class
End NameSpace