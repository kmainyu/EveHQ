<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SkillQueueControl
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
        Me.components = New System.ComponentModel.Container
        Me.panelSkillQueue = New DevComponents.DotNetBar.PanelEx
        Me.tmrUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'panelSkillQueue
        '
        Me.panelSkillQueue.AutoScroll = True
        Me.panelSkillQueue.CanvasColor = System.Drawing.SystemColors.Control
        Me.panelSkillQueue.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005
        Me.panelSkillQueue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelSkillQueue.Location = New System.Drawing.Point(0, 0)
        Me.panelSkillQueue.Name = "panelSkillQueue"
        Me.panelSkillQueue.Size = New System.Drawing.Size(406, 166)
        Me.panelSkillQueue.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.panelSkillQueue.Style.BackColor1.Color = System.Drawing.Color.Black
        Me.panelSkillQueue.Style.BackColor2.Color = System.Drawing.Color.Black
        Me.panelSkillQueue.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.panelSkillQueue.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.panelSkillQueue.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.panelSkillQueue.Style.GradientAngle = 90
        Me.panelSkillQueue.TabIndex = 0
        '
        'tmrUpdate
        '
        Me.tmrUpdate.Enabled = True
        Me.tmrUpdate.Interval = 5000
        '
        'SkillQueueControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.panelSkillQueue)
        Me.DoubleBuffered = True
        Me.Name = "SkillQueueControl"
        Me.Size = New System.Drawing.Size(406, 166)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents panelSkillQueue As DevComponents.DotNetBar.PanelEx
    Friend WithEvents tmrUpdate As System.Windows.Forms.Timer

End Class
