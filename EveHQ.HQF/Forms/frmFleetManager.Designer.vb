<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFleetManager
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFleetManager))
        Me.ctxDashboard = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuRefreshDB = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.mnuClearDashboard = New System.Windows.Forms.ToolStripMenuItem()
        Me.panelDB = New DevComponents.DotNetBar.PanelEx()
        Me.FleetDashboard1 = New EveHQ.HQF.FleetDashboard()
        Me.ctxDashboard.SuspendLayout()
        Me.SuspendLayout()
        '
        'ctxDashboard
        '
        Me.ctxDashboard.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ctxDashboard.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuRefreshDB, Me.ToolStripMenuItem2, Me.mnuClearDashboard})
        Me.ctxDashboard.Name = "ctxDashboard"
        Me.ctxDashboard.Size = New System.Drawing.Size(168, 54)
        '
        'mnuRefreshDB
        '
        Me.mnuRefreshDB.Name = "mnuRefreshDB"
        Me.mnuRefreshDB.Size = New System.Drawing.Size(167, 22)
        Me.mnuRefreshDB.Text = "Refresh Dashboard"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(164, 6)
        '
        'mnuClearDashboard
        '
        Me.mnuClearDashboard.Name = "mnuClearDashboard"
        Me.mnuClearDashboard.Size = New System.Drawing.Size(167, 22)
        Me.mnuClearDashboard.Text = "Clear Dashboard"
        '
        'panelDB
        '
        Me.panelDB.CanvasColor = System.Drawing.SystemColors.Control
        Me.panelDB.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.panelDB.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelDB.Location = New System.Drawing.Point(0, 655)
        Me.panelDB.Name = "panelDB"
        Me.panelDB.Size = New System.Drawing.Size(1232, 169)
        Me.panelDB.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.panelDB.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.panelDB.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.panelDB.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.panelDB.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.panelDB.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.panelDB.Style.GradientAngle = 90
        Me.panelDB.TabIndex = 1
        Me.panelDB.Text = "PanelEx1"
        '
        'FleetDashboard1
        '
        Me.FleetDashboard1.BackColor = System.Drawing.SystemColors.ControlDark
        Me.FleetDashboard1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FleetDashboard1.Location = New System.Drawing.Point(0, 0)
        Me.FleetDashboard1.Name = "FleetDashboard1"
        Me.FleetDashboard1.Size = New System.Drawing.Size(1232, 655)
        Me.FleetDashboard1.TabIndex = 2
        '
        'frmFleetManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1232, 824)
        Me.Controls.Add(Me.FleetDashboard1)
        Me.Controls.Add(Me.panelDB)
        Me.DoubleBuffered = True
        Me.EnableGlass = False
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmFleetManager"
        Me.Text = "HQF Fleet Manager"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.ctxDashboard.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ctxDashboard As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuRefreshDB As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuClearDashboard As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents panelDB As DevComponents.DotNetBar.PanelEx
    Friend WithEvents FleetDashboard1 As EveHQ.HQF.FleetDashboard
End Class
