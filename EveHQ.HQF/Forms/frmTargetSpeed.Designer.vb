<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTargetSpeed
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
        Me.btnClose = New System.Windows.Forms.Button
        Me.zgcTargetSpeed = New ZedGraph.ZedGraphControl
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(660, 464)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 12
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
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
        'frmTargetSpeed
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(747, 499)
        Me.Controls.Add(Me.zgcTargetSpeed)
        Me.Controls.Add(Me.btnClose)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmTargetSpeed"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Targeting Speed Analysis"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents zgcTargetSpeed As ZedGraph.ZedGraphControl
End Class
