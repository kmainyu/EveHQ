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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmDashboard))
        Me.FLP1 = New System.Windows.Forms.FlowLayoutPanel
        Me.Ticker1 = New EveHQ.Core.Ticker
        Me.ColorWithAlpha1 = New EveHQ.Core.ColorWithAlpha
        Me.ColorWithAlpha2 = New EveHQ.Core.ColorWithAlpha
        Me.SuspendLayout()
        '
        'FLP1
        '
        Me.FLP1.AllowDrop = True
        Me.FLP1.AutoScroll = True
        Me.FLP1.BackColor = System.Drawing.Color.LightSteelBlue
        Me.FLP1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FLP1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FLP1.Location = New System.Drawing.Point(0, 0)
        Me.FLP1.Name = "FLP1"
        Me.FLP1.Size = New System.Drawing.Size(717, 580)
        Me.FLP1.TabIndex = 0
        '
        'Ticker1
        '
        Me.Ticker1.BackColor = System.Drawing.Color.Black
        Me.Ticker1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Ticker1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Ticker1.Location = New System.Drawing.Point(0, 556)
        Me.Ticker1.Name = "Ticker1"
        Me.Ticker1.ScrollDistance = 1
        Me.Ticker1.ScrollNumberOfImages = 10
        Me.Ticker1.ScrollSpeed = 5
        Me.Ticker1.Size = New System.Drawing.Size(717, 24)
        Me.Ticker1.TabIndex = 0
        '
        'ColorWithAlpha1
        '
        Me.ColorWithAlpha1.Alpha = 255
        Me.ColorWithAlpha1.Color = System.Drawing.Color.White
        Me.ColorWithAlpha1.Parent = Nothing
        '
        'ColorWithAlpha2
        '
        Me.ColorWithAlpha2.Alpha = 255
        Me.ColorWithAlpha2.Color = System.Drawing.Color.Khaki
        Me.ColorWithAlpha2.Parent = Nothing
        '
        'frmDashboard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(717, 580)
        Me.Controls.Add(Me.Ticker1)
        Me.Controls.Add(Me.FLP1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmDashboard"
        Me.Text = "EveHQ Dashboard"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ColorWithAlpha1 As EveHQ.Core.ColorWithAlpha
    Friend WithEvents ColorWithAlpha2 As EveHQ.Core.ColorWithAlpha
    Friend WithEvents FLP1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents Ticker1 As EveHQ.Core.Ticker
End Class
