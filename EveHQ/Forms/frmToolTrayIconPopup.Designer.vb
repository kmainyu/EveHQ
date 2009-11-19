<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmToolTrayIconPopup
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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmToolTrayIconPopup))
        Me.AGP1 = New EveHQ.Core.AlphaGradientPanel
        Me.ColorWithAlpha1 = New EveHQ.Core.ColorWithAlpha
        Me.ColorWithAlpha2 = New EveHQ.Core.ColorWithAlpha
        Me.displayTimer = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'AGP1
        '
        Me.AGP1.AutoSize = True
        Me.AGP1.BackColor = System.Drawing.Color.Transparent
        Me.AGP1.Border = True
        Me.AGP1.BorderColor = System.Drawing.Color.MidnightBlue
        Me.AGP1.Colors.Add(Me.ColorWithAlpha1)
        Me.AGP1.Colors.Add(Me.ColorWithAlpha2)
        Me.AGP1.ContentPadding = New System.Windows.Forms.Padding(10)
        Me.AGP1.CornerRadius = 10
        Me.AGP1.Corners = CType(((EveHQ.Core.Corner.TopLeft Or EveHQ.Core.Corner.TopRight) _
                    Or EveHQ.Core.Corner.BottomLeft), EveHQ.Core.Corner)
        Me.AGP1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AGP1.Gradient = True
        Me.AGP1.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        Me.AGP1.GradientOffset = 1.0!
        Me.AGP1.GradientSize = New System.Drawing.Size(0, 0)
        Me.AGP1.GradientWrapMode = System.Drawing.Drawing2D.WrapMode.Tile
        Me.AGP1.Grayscale = False
        Me.AGP1.Image = CType(resources.GetObject("AGP1.Image"), System.Drawing.Image)
        Me.AGP1.ImageAlpha = 75
        Me.AGP1.ImagePadding = New System.Windows.Forms.Padding(5, 5, -10, -10)
        Me.AGP1.ImagePosition = EveHQ.Core.ImagePosition.BottomRight
        Me.AGP1.ImageSize = New System.Drawing.Size(64, 64)
        Me.AGP1.Location = New System.Drawing.Point(0, 0)
        Me.AGP1.Name = "AGP1"
        Me.AGP1.Rounded = True
        Me.AGP1.Size = New System.Drawing.Size(357, 210)
        Me.AGP1.TabIndex = 0
        '
        'ColorWithAlpha1
        '
        Me.ColorWithAlpha1.Alpha = 255
        Me.ColorWithAlpha1.Color = System.Drawing.Color.LightSteelBlue
        Me.ColorWithAlpha1.Parent = Me.AGP1
        '
        'ColorWithAlpha2
        '
        Me.ColorWithAlpha2.Alpha = 128
        Me.ColorWithAlpha2.Color = System.Drawing.Color.White
        Me.ColorWithAlpha2.Parent = Me.AGP1
        '
        'displayTimer
        '
        Me.displayTimer.Interval = 1000
        '
        'frmToolTrayIconPopup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(357, 210)
        Me.ControlBox = False
        Me.Controls.Add(Me.AGP1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmToolTrayIconPopup"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "frmToolTrayIconPopup"
        Me.TopMost = True
        Me.TransparencyKey = System.Drawing.Color.White
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents AGP1 As EveHQ.Core.AlphaGradientPanel
    Friend WithEvents ColorWithAlpha1 As EveHQ.Core.ColorWithAlpha
    Friend WithEvents ColorWithAlpha2 As EveHQ.Core.ColorWithAlpha
    Private WithEvents displayTimer As System.Windows.Forms.Timer
End Class
