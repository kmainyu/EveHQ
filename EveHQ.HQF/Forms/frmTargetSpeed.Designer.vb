<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTargetSpeed
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
        Me.components = New System.ComponentModel.Container
        Me.zgcTargetSpeed = New ZedGraph.ZedGraphControl
        Me.btnClose = New DevComponents.DotNetBar.ButtonX
        Me.SuspendLayout()
        '
        'zgcTargetSpeed
        '
        Me.zgcTargetSpeed.IsEnableVPan = False
        Me.zgcTargetSpeed.IsEnableVZoom = False
        Me.zgcTargetSpeed.IsShowPointValues = True
        Me.zgcTargetSpeed.Location = New System.Drawing.Point(12, 12)
        Me.zgcTargetSpeed.Name = "zgcTargetSpeed"
        Me.zgcTargetSpeed.ScrollGrace = 0
        Me.zgcTargetSpeed.ScrollMaxX = 0
        Me.zgcTargetSpeed.ScrollMaxY = 0
        Me.zgcTargetSpeed.ScrollMaxY2 = 0
        Me.zgcTargetSpeed.ScrollMinX = 0
        Me.zgcTargetSpeed.ScrollMinY = 0
        Me.zgcTargetSpeed.ScrollMinY2 = 0
        Me.zgcTargetSpeed.Size = New System.Drawing.Size(723, 446)
        Me.zgcTargetSpeed.TabIndex = 14
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
        'frmTargetSpeed
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(747, 499)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.zgcTargetSpeed)
        Me.DoubleBuffered = True
        Me.EnableGlass = False
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTargetSpeed"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Targeting Speed Analysis"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents zgcTargetSpeed As ZedGraph.ZedGraphControl
    Friend WithEvents btnClose As DevComponents.DotNetBar.ButtonX
End Class
