<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmChartViewer
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmChartViewer))
        Me.ZGC1 = New ZedGraph.ZedGraphControl
        Me.lblGraphInfo = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'ZGC1
        '
        Me.ZGC1.Dock = System.Windows.Forms.DockStyle.Top
        Me.ZGC1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ZGC1.Location = New System.Drawing.Point(0, 0)
        Me.ZGC1.Name = "ZGC1"
        Me.ZGC1.ScrollGrace = 0
        Me.ZGC1.ScrollMaxX = 0
        Me.ZGC1.ScrollMaxY = 0
        Me.ZGC1.ScrollMaxY2 = 0
        Me.ZGC1.ScrollMinX = 0
        Me.ZGC1.ScrollMinY = 0
        Me.ZGC1.ScrollMinY2 = 0
        Me.ZGC1.Size = New System.Drawing.Size(753, 450)
        Me.ZGC1.TabIndex = 0
        '
        'lblGraphInfo
        '
        Me.lblGraphInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lblGraphInfo.Location = New System.Drawing.Point(0, 450)
        Me.lblGraphInfo.Name = "lblGraphInfo"
        Me.lblGraphInfo.Size = New System.Drawing.Size(753, 49)
        Me.lblGraphInfo.TabIndex = 1
        '
        'frmChartViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(753, 499)
        Me.Controls.Add(Me.lblGraphInfo)
        Me.Controls.Add(Me.ZGC1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmChartViewer"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "HQF Graph"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ZGC1 As ZedGraph.ZedGraphControl
    Friend WithEvents lblGraphInfo As System.Windows.Forms.Label
End Class
