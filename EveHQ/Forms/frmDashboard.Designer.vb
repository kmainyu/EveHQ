Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class FrmDashboard
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmDashboard))
            Me.ctxDashboard = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.mnuConfigureDB = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
            Me.mnuRefreshDB = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
            Me.mnuClearDashboard = New System.Windows.Forms.ToolStripMenuItem()
            Me.panelDB = New DevComponents.DotNetBar.PanelEx()
            Me.ctxDashboard.SuspendLayout()
            Me.SuspendLayout()
            '
            'ctxDashboard
            '
            Me.ctxDashboard.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.ctxDashboard.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuConfigureDB, Me.ToolStripMenuItem1, Me.mnuRefreshDB, Me.ToolStripMenuItem2, Me.mnuClearDashboard})
            Me.ctxDashboard.Name = "ctxDashboard"
            Me.ctxDashboard.Size = New System.Drawing.Size(177, 82)
            '
            'mnuConfigureDB
            '
            Me.mnuConfigureDB.Name = "mnuConfigureDB"
            Me.mnuConfigureDB.Size = New System.Drawing.Size(176, 22)
            Me.mnuConfigureDB.Text = "Configure Dashboard"
            '
            'ToolStripMenuItem1
            '
            Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
            Me.ToolStripMenuItem1.Size = New System.Drawing.Size(173, 6)
            '
            'mnuRefreshDB
            '
            Me.mnuRefreshDB.Name = "mnuRefreshDB"
            Me.mnuRefreshDB.Size = New System.Drawing.Size(176, 22)
            Me.mnuRefreshDB.Text = "Refresh Dashboard"
            '
            'ToolStripMenuItem2
            '
            Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
            Me.ToolStripMenuItem2.Size = New System.Drawing.Size(173, 6)
            '
            'mnuClearDashboard
            '
            Me.mnuClearDashboard.Name = "mnuClearDashboard"
            Me.mnuClearDashboard.Size = New System.Drawing.Size(176, 22)
            Me.mnuClearDashboard.Text = "Clear Dashboard"
            '
            'panelDB
            '
            Me.panelDB.AutoScroll = True
            Me.panelDB.CanvasColor = System.Drawing.SystemColors.Control
            Me.panelDB.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.panelDB.ContextMenuStrip = Me.ctxDashboard
            Me.panelDB.Dock = System.Windows.Forms.DockStyle.Fill
            Me.panelDB.Location = New System.Drawing.Point(0, 0)
            Me.panelDB.Name = "panelDB"
            Me.panelDB.Size = New System.Drawing.Size(717, 580)
            Me.panelDB.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.panelDB.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.panelDB.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.panelDB.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.panelDB.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.panelDB.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.panelDB.Style.GradientAngle = 90
            Me.panelDB.TabIndex = 1
            '
            'frmDashboard
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(717, 580)
            Me.Controls.Add(Me.panelDB)
            Me.DoubleBuffered = True
            Me.EnableGlass = False
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.Name = "frmDashboard"
            Me.Text = "EveHQ Dashboard"
            Me.ctxDashboard.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents ctxDashboard As System.Windows.Forms.ContextMenuStrip
        Friend WithEvents mnuConfigureDB As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mnuRefreshDB As System.Windows.Forms.ToolStripMenuItem
        Friend WithEvents panelDB As DevComponents.DotNetBar.PanelEx
        Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
        Friend WithEvents mnuClearDashboard As System.Windows.Forms.ToolStripMenuItem
    End Class
End NameSpace