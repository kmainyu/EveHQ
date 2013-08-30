<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MarketItemMetrics
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.MetricsPanel = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me._minimumPrice = New System.Windows.Forms.Label()
        Me._maximumPrice = New System.Windows.Forms.Label()
        Me._averagePrice = New System.Windows.Forms.Label()
        Me._percentilePrice = New System.Windows.Forms.Label()
        Me._stdDeviation = New System.Windows.Forms.Label()
        Me._medianPrice = New System.Windows.Forms.Label()
        Me.label26 = New System.Windows.Forms.Label()
        Me.label25 = New System.Windows.Forms.Label()
        Me.label24 = New System.Windows.Forms.Label()
        Me._volume = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.MetricsPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'MetricsPanel
        '
        Me.MetricsPanel.CanvasColor = System.Drawing.SystemColors.Control
        Me.MetricsPanel.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.MetricsPanel.Controls.Add(Me._volume)
        Me.MetricsPanel.Controls.Add(Me.Label11)
        Me.MetricsPanel.Controls.Add(Me._percentilePrice)
        Me.MetricsPanel.Controls.Add(Me._stdDeviation)
        Me.MetricsPanel.Controls.Add(Me._medianPrice)
        Me.MetricsPanel.Controls.Add(Me.label26)
        Me.MetricsPanel.Controls.Add(Me.label25)
        Me.MetricsPanel.Controls.Add(Me.label24)
        Me.MetricsPanel.Controls.Add(Me._averagePrice)
        Me.MetricsPanel.Controls.Add(Me._maximumPrice)
        Me.MetricsPanel.Controls.Add(Me._minimumPrice)
        Me.MetricsPanel.Controls.Add(Me.Label3)
        Me.MetricsPanel.Controls.Add(Me.Label2)
        Me.MetricsPanel.Controls.Add(Me.Label1)
        Me.MetricsPanel.Location = New System.Drawing.Point(4, 3)
        Me.MetricsPanel.Name = "MetricsPanel"
        Me.MetricsPanel.Size = New System.Drawing.Size(347, 121)
        '
        '
        '
        Me.MetricsPanel.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.MetricsPanel.Style.BackColorGradientAngle = 90
        Me.MetricsPanel.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.MetricsPanel.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.MetricsPanel.Style.BorderBottomWidth = 1
        Me.MetricsPanel.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.MetricsPanel.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.MetricsPanel.Style.BorderLeftWidth = 1
        Me.MetricsPanel.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.MetricsPanel.Style.BorderRightWidth = 1
        Me.MetricsPanel.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.MetricsPanel.Style.BorderTopWidth = 1
        Me.MetricsPanel.Style.CornerDiameter = 4
        Me.MetricsPanel.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.MetricsPanel.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.MetricsPanel.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.MetricsPanel.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.MetricsPanel.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.MetricsPanel.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.MetricsPanel.TabIndex = 34
        Me.MetricsPanel.Text = "GroupPanel1"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Location = New System.Drawing.Point(14, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Minimum"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Location = New System.Drawing.Point(14, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(51, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Maximum"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Location = New System.Drawing.Point(14, 77)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(47, 13)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Average"
        '
        '_minimumPrice
        '
        Me._minimumPrice.AutoSize = True
        Me._minimumPrice.BackColor = System.Drawing.Color.Transparent
        Me._minimumPrice.Location = New System.Drawing.Point(68, 30)
        Me._minimumPrice.Name = "_minimumPrice"
        Me._minimumPrice.Size = New System.Drawing.Size(0, 13)
        Me._minimumPrice.TabIndex = 3
        '
        '_maximumPrice
        '
        Me._maximumPrice.AutoSize = True
        Me._maximumPrice.BackColor = System.Drawing.Color.Transparent
        Me._maximumPrice.Location = New System.Drawing.Point(68, 55)
        Me._maximumPrice.Name = "_maximumPrice"
        Me._maximumPrice.Size = New System.Drawing.Size(0, 13)
        Me._maximumPrice.TabIndex = 4
        '
        '_averagePrice
        '
        Me._averagePrice.AutoSize = True
        Me._averagePrice.BackColor = System.Drawing.Color.Transparent
        Me._averagePrice.Location = New System.Drawing.Point(68, 77)
        Me._averagePrice.Name = "_averagePrice"
        Me._averagePrice.Size = New System.Drawing.Size(0, 13)
        Me._averagePrice.TabIndex = 5
        '
        '_percentilePrice
        '
        Me._percentilePrice.AutoSize = True
        Me._percentilePrice.BackColor = System.Drawing.Color.Transparent
        Me._percentilePrice.Location = New System.Drawing.Point(232, 77)
        Me._percentilePrice.Name = "_percentilePrice"
        Me._percentilePrice.Size = New System.Drawing.Size(0, 13)
        Me._percentilePrice.TabIndex = 11
        '
        '_stdDeviation
        '
        Me._stdDeviation.AutoSize = True
        Me._stdDeviation.BackColor = System.Drawing.Color.Transparent
        Me._stdDeviation.Location = New System.Drawing.Point(232, 55)
        Me._stdDeviation.Name = "_stdDeviation"
        Me._stdDeviation.Size = New System.Drawing.Size(0, 13)
        Me._stdDeviation.TabIndex = 10
        '
        '_medianPrice
        '
        Me._medianPrice.AutoSize = True
        Me._medianPrice.BackColor = System.Drawing.Color.Transparent
        Me._medianPrice.Location = New System.Drawing.Point(232, 30)
        Me._medianPrice.Name = "_medianPrice"
        Me._medianPrice.Size = New System.Drawing.Size(0, 13)
        Me._medianPrice.TabIndex = 9
        '
        'label26
        '
        Me.label26.AutoSize = True
        Me.label26.BackColor = System.Drawing.Color.Transparent
        Me.label26.Location = New System.Drawing.Point(178, 77)
        Me.label26.Name = "label26"
        Me.label26.Size = New System.Drawing.Size(54, 13)
        Me.label26.TabIndex = 8
        Me.label26.Text = "Percentile"
        '
        'label25
        '
        Me.label25.AutoSize = True
        Me.label25.BackColor = System.Drawing.Color.Transparent
        Me.label25.Location = New System.Drawing.Point(178, 55)
        Me.label25.Name = "label25"
        Me.label25.Size = New System.Drawing.Size(52, 13)
        Me.label25.TabIndex = 7
        Me.label25.Text = "Std. Dev."
        '
        'label24
        '
        Me.label24.AutoSize = True
        Me.label24.BackColor = System.Drawing.Color.Transparent
        Me.label24.Location = New System.Drawing.Point(178, 30)
        Me.label24.Name = "label24"
        Me.label24.Size = New System.Drawing.Size(42, 13)
        Me.label24.TabIndex = 6
        Me.label24.Text = "Median"
        '
        '_volume
        '
        Me._volume.AutoSize = True
        Me._volume.BackColor = System.Drawing.Color.Transparent
        Me._volume.Location = New System.Drawing.Point(150, 9)
        Me._volume.Name = "_volume"
        Me._volume.Size = New System.Drawing.Size(0, 13)
        Me._volume.TabIndex = 13
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Location = New System.Drawing.Point(96, 9)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(42, 13)
        Me.Label11.TabIndex = 12
        Me.Label11.Text = "Volume"
        '
        'MarketItemMetrics
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.MetricsPanel)
        Me.Name = "MarketItemMetrics"
        Me.Size = New System.Drawing.Size(358, 128)
        Me.MetricsPanel.ResumeLayout(False)
        Me.MetricsPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents label26 As System.Windows.Forms.Label
    Friend WithEvents label25 As System.Windows.Forms.Label
    Friend WithEvents label24 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Public WithEvents MetricsPanel As DevComponents.DotNetBar.Controls.GroupPanel
    Private WithEvents _volume As System.Windows.Forms.Label
    Private WithEvents _percentilePrice As System.Windows.Forms.Label
    Private WithEvents _stdDeviation As System.Windows.Forms.Label
    Private WithEvents _medianPrice As System.Windows.Forms.Label
    Private WithEvents _averagePrice As System.Windows.Forms.Label
    Private WithEvents _maximumPrice As System.Windows.Forms.Label
    Private WithEvents _minimumPrice As System.Windows.Forms.Label

End Class
