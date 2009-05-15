<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDashboard
    Inherits System.Windows.Forms.Form

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
        Me.FLP1 = New System.Windows.Forms.FlowLayoutPanel
        Me.AGP1 = New EveHQ.Core.AlphaGradientPanel
        Me.ColorWithAlpha1 = New EveHQ.Core.ColorWithAlpha
        Me.ColorWithAlpha2 = New EveHQ.Core.ColorWithAlpha
        Me.Label1 = New System.Windows.Forms.Label
        Me.AGP2 = New EveHQ.Core.AlphaGradientPanel
        Me.Label2 = New System.Windows.Forms.Label
        Me.AGP3 = New EveHQ.Core.AlphaGradientPanel
        Me.Label3 = New System.Windows.Forms.Label
        Me.AGP4 = New EveHQ.Core.AlphaGradientPanel
        Me.Label4 = New System.Windows.Forms.Label
        Me.FLP1.SuspendLayout()
        Me.AGP1.SuspendLayout()
        Me.AGP2.SuspendLayout()
        Me.AGP3.SuspendLayout()
        Me.AGP4.SuspendLayout()
        Me.SuspendLayout()
        '
        'FLP1
        '
        Me.FLP1.AllowDrop = True
        Me.FLP1.BackColor = System.Drawing.Color.LightSteelBlue
        Me.FLP1.Controls.Add(Me.AGP1)
        Me.FLP1.Controls.Add(Me.AGP2)
        Me.FLP1.Controls.Add(Me.AGP3)
        Me.FLP1.Controls.Add(Me.AGP4)
        Me.FLP1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FLP1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FLP1.Location = New System.Drawing.Point(0, 0)
        Me.FLP1.Name = "FLP1"
        Me.FLP1.Size = New System.Drawing.Size(717, 580)
        Me.FLP1.TabIndex = 0
        '
        'AGP1
        '
        Me.AGP1.BackColor = System.Drawing.Color.Transparent
        Me.AGP1.Border = True
        Me.AGP1.BorderColor = System.Drawing.Color.DarkGreen
        Me.AGP1.Colors.Add(Me.ColorWithAlpha1)
        Me.AGP1.Colors.Add(Me.ColorWithAlpha2)
        Me.AGP1.ContentPadding = New System.Windows.Forms.Padding(0)
        Me.AGP1.Controls.Add(Me.Label1)
        Me.AGP1.CornerRadius = 25
        Me.AGP1.Corners = EveHQ.Core.Corner.BottomRight
        Me.AGP1.Gradient = True
        Me.AGP1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical
        Me.AGP1.GradientOffset = 1.0!
        Me.AGP1.GradientSize = New System.Drawing.Size(0, 0)
        Me.AGP1.GradientWrapMode = System.Drawing.Drawing2D.WrapMode.Tile
        Me.AGP1.Grayscale = False
        Me.AGP1.Image = Nothing
        Me.AGP1.ImageAlpha = 75
        Me.AGP1.ImagePadding = New System.Windows.Forms.Padding(5)
        Me.AGP1.ImagePosition = EveHQ.Core.ImagePosition.BottomRight
        Me.AGP1.ImageSize = New System.Drawing.Size(48, 48)
        Me.AGP1.Location = New System.Drawing.Point(5, 5)
        Me.AGP1.Margin = New System.Windows.Forms.Padding(5)
        Me.AGP1.Name = "AGP1"
        Me.AGP1.Rounded = True
        Me.AGP1.Size = New System.Drawing.Size(267, 200)
        Me.AGP1.TabIndex = 0
        '
        'ColorWithAlpha1
        '
        Me.ColorWithAlpha1.Alpha = 255
        Me.ColorWithAlpha1.Color = System.Drawing.Color.White
        Me.ColorWithAlpha1.Parent = Me.AGP4
        '
        'ColorWithAlpha2
        '
        Me.ColorWithAlpha2.Alpha = 255
        Me.ColorWithAlpha2.Color = System.Drawing.Color.MintCream
        Me.ColorWithAlpha2.Parent = Me.AGP4
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(114, 90)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Label1"
        '
        'AGP2
        '
        Me.AGP2.BackColor = System.Drawing.Color.Transparent
        Me.AGP2.Border = True
        Me.AGP2.BorderColor = System.Drawing.Color.DarkGreen
        Me.AGP2.Colors.Add(Me.ColorWithAlpha1)
        Me.AGP2.Colors.Add(Me.ColorWithAlpha2)
        Me.AGP2.ContentPadding = New System.Windows.Forms.Padding(0)
        Me.AGP2.Controls.Add(Me.Label2)
        Me.AGP2.CornerRadius = 25
        Me.AGP2.Corners = EveHQ.Core.Corner.BottomRight
        Me.AGP2.Gradient = True
        Me.AGP2.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical
        Me.AGP2.GradientOffset = 1.0!
        Me.AGP2.GradientSize = New System.Drawing.Size(0, 0)
        Me.AGP2.GradientWrapMode = System.Drawing.Drawing2D.WrapMode.Tile
        Me.AGP2.Grayscale = False
        Me.AGP2.Image = Nothing
        Me.AGP2.ImageAlpha = 75
        Me.AGP2.ImagePadding = New System.Windows.Forms.Padding(5)
        Me.AGP2.ImagePosition = EveHQ.Core.ImagePosition.BottomRight
        Me.AGP2.ImageSize = New System.Drawing.Size(48, 48)
        Me.AGP2.Location = New System.Drawing.Point(5, 215)
        Me.AGP2.Margin = New System.Windows.Forms.Padding(5)
        Me.AGP2.Name = "AGP2"
        Me.AGP2.Rounded = True
        Me.AGP2.Size = New System.Drawing.Size(267, 200)
        Me.AGP2.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(114, 90)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(39, 13)
        Me.Label2.TabIndex = 0
        Me.Label2.Text = "Label2"
        '
        'AGP3
        '
        Me.AGP3.BackColor = System.Drawing.Color.Transparent
        Me.AGP3.Border = True
        Me.AGP3.BorderColor = System.Drawing.Color.DarkGreen
        Me.AGP3.Colors.Add(Me.ColorWithAlpha1)
        Me.AGP3.Colors.Add(Me.ColorWithAlpha2)
        Me.AGP3.ContentPadding = New System.Windows.Forms.Padding(0)
        Me.AGP3.Controls.Add(Me.Label3)
        Me.AGP3.CornerRadius = 25
        Me.AGP3.Corners = EveHQ.Core.Corner.BottomRight
        Me.AGP3.Gradient = True
        Me.AGP3.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical
        Me.AGP3.GradientOffset = 1.0!
        Me.AGP3.GradientSize = New System.Drawing.Size(0, 0)
        Me.AGP3.GradientWrapMode = System.Drawing.Drawing2D.WrapMode.Tile
        Me.AGP3.Grayscale = False
        Me.AGP3.Image = Nothing
        Me.AGP3.ImageAlpha = 75
        Me.AGP3.ImagePadding = New System.Windows.Forms.Padding(5)
        Me.AGP3.ImagePosition = EveHQ.Core.ImagePosition.BottomRight
        Me.AGP3.ImageSize = New System.Drawing.Size(48, 48)
        Me.AGP3.Location = New System.Drawing.Point(282, 5)
        Me.AGP3.Margin = New System.Windows.Forms.Padding(5)
        Me.AGP3.Name = "AGP3"
        Me.AGP3.Rounded = True
        Me.AGP3.Size = New System.Drawing.Size(267, 200)
        Me.AGP3.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(114, 90)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 13)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Label3"
        '
        'AGP4
        '
        Me.AGP4.BackColor = System.Drawing.Color.Transparent
        Me.AGP4.Border = True
        Me.AGP4.BorderColor = System.Drawing.Color.DarkGreen
        Me.AGP4.Colors.Add(Me.ColorWithAlpha1)
        Me.AGP4.Colors.Add(Me.ColorWithAlpha2)
        Me.AGP4.ContentPadding = New System.Windows.Forms.Padding(0)
        Me.AGP4.Controls.Add(Me.Label4)
        Me.AGP4.CornerRadius = 25
        Me.AGP4.Corners = EveHQ.Core.Corner.BottomRight
        Me.AGP4.Gradient = True
        Me.AGP4.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical
        Me.AGP4.GradientOffset = 1.0!
        Me.AGP4.GradientSize = New System.Drawing.Size(0, 0)
        Me.AGP4.GradientWrapMode = System.Drawing.Drawing2D.WrapMode.Tile
        Me.AGP4.Grayscale = False
        Me.AGP4.Image = Nothing
        Me.AGP4.ImageAlpha = 75
        Me.AGP4.ImagePadding = New System.Windows.Forms.Padding(5)
        Me.AGP4.ImagePosition = EveHQ.Core.ImagePosition.BottomRight
        Me.AGP4.ImageSize = New System.Drawing.Size(48, 48)
        Me.AGP4.Location = New System.Drawing.Point(282, 215)
        Me.AGP4.Margin = New System.Windows.Forms.Padding(5)
        Me.AGP4.Name = "AGP4"
        Me.AGP4.Rounded = True
        Me.AGP4.Size = New System.Drawing.Size(267, 200)
        Me.AGP4.TabIndex = 3
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(114, 90)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(39, 13)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Label4"
        '
        'frmDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(717, 580)
        Me.Controls.Add(Me.FLP1)
        Me.Name = "frmDashboard"
        Me.Text = "EveHQ Dashboard"
        Me.FLP1.ResumeLayout(False)
        Me.AGP1.ResumeLayout(False)
        Me.AGP1.PerformLayout()
        Me.AGP2.ResumeLayout(False)
        Me.AGP2.PerformLayout()
        Me.AGP3.ResumeLayout(False)
        Me.AGP3.PerformLayout()
        Me.AGP4.ResumeLayout(False)
        Me.AGP4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents FLP1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents AGP1 As EveHQ.Core.AlphaGradientPanel
    Friend WithEvents ColorWithAlpha1 As EveHQ.Core.ColorWithAlpha
    Friend WithEvents ColorWithAlpha2 As EveHQ.Core.ColorWithAlpha
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents AGP4 As EveHQ.Core.AlphaGradientPanel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents AGP2 As EveHQ.Core.AlphaGradientPanel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents AGP3 As EveHQ.Core.AlphaGradientPanel
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
