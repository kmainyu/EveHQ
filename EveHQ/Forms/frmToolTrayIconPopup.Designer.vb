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
        Me.ColorWithAlpha1 = New EveHQ.Core.ColorWithAlpha
        Me.ColorWithAlpha2 = New EveHQ.Core.ColorWithAlpha
        Me.displayTimer = New System.Windows.Forms.Timer(Me.components)
        Me.AGP1 = New System.Windows.Forms.FlowLayoutPanel
        Me.SuspendLayout()
        '
        'ColorWithAlpha1
        '
        Me.ColorWithAlpha1.Alpha = 255
        Me.ColorWithAlpha1.Color = System.Drawing.Color.LightSteelBlue
        '
        'ColorWithAlpha2
        '
        Me.ColorWithAlpha2.Alpha = 128
        Me.ColorWithAlpha2.Color = System.Drawing.Color.White
        '
        'displayTimer
        '
        Me.displayTimer.Interval = 1000
        '
        'AGP1
        '
        Me.AGP1.AutoSize = True
        Me.AGP1.BackColor = System.Drawing.SystemColors.Info
        Me.AGP1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.AGP1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AGP1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.AGP1.Location = New System.Drawing.Point(0, 0)
        Me.AGP1.Name = "AGP1"
        Me.AGP1.Size = New System.Drawing.Size(300, 80)
        Me.AGP1.TabIndex = 0
        '
        'frmToolTrayIconPopup
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(300, 80)
        Me.ControlBox = False
        Me.Controls.Add(Me.AGP1)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmToolTrayIconPopup"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "frmToolTrayIconPopup"
        Me.TransparencyKey = System.Drawing.Color.White
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ColorWithAlpha1 As EveHQ.Core.ColorWithAlpha
    Friend WithEvents ColorWithAlpha2 As EveHQ.Core.ColorWithAlpha
    Private WithEvents displayTimer As System.Windows.Forms.Timer
    Friend WithEvents AGP1 As System.Windows.Forms.FlowLayoutPanel
End Class
