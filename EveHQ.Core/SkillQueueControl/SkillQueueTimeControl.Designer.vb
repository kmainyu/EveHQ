Namespace SkillQueueControl
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class SkillQueueTimeControl
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
            Me.panelSQT = New DevComponents.DotNetBar.PanelEx
            Me.lblQueueRemaining = New DevComponents.DotNetBar.LabelX
            Me.lblQueueEnds = New DevComponents.DotNetBar.LabelX
            Me.tmrUpdate = New System.Windows.Forms.Timer(Me.components)
            Me.panelSQT.SuspendLayout()
            Me.SuspendLayout()
            '
            'panelSQT
            '
            Me.panelSQT.CanvasColor = System.Drawing.SystemColors.Control
            Me.panelSQT.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005
            Me.panelSQT.Controls.Add(Me.lblQueueRemaining)
            Me.panelSQT.Controls.Add(Me.lblQueueEnds)
            Me.panelSQT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.panelSQT.Location = New System.Drawing.Point(0, 0)
            Me.panelSQT.Name = "panelSQT"
            Me.panelSQT.Size = New System.Drawing.Size(300, 48)
            Me.panelSQT.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.panelSQT.Style.BackColor1.Color = System.Drawing.Color.DimGray
            Me.panelSQT.Style.BackColor2.Color = System.Drawing.Color.Black
            Me.panelSQT.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.panelSQT.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.panelSQT.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.panelSQT.Style.GradientAngle = 90
            Me.panelSQT.TabIndex = 0
            '
            'lblQueueRemaining
            '
            Me.lblQueueRemaining.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            '
            '
            '
            Me.lblQueueRemaining.BackgroundStyle.Class = ""
            Me.lblQueueRemaining.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblQueueRemaining.ForeColor = System.Drawing.Color.White
            Me.lblQueueRemaining.Location = New System.Drawing.Point(184, 0)
            Me.lblQueueRemaining.Name = "lblQueueRemaining"
            Me.lblQueueRemaining.Size = New System.Drawing.Size(110, 16)
            Me.lblQueueRemaining.TabIndex = 1
            Me.lblQueueRemaining.Text = "10d 10h 10m 10s"
            Me.lblQueueRemaining.TextAlignment = System.Drawing.StringAlignment.Far
            '
            'lblQueueEnds
            '
            Me.lblQueueEnds.AutoSize = True
            '
            '
            '
            Me.lblQueueEnds.BackgroundStyle.Class = ""
            Me.lblQueueEnds.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblQueueEnds.ForeColor = System.Drawing.Color.White
            Me.lblQueueEnds.Location = New System.Drawing.Point(3, 0)
            Me.lblQueueEnds.Name = "lblQueueEnds"
            Me.lblQueueEnds.Size = New System.Drawing.Size(45, 16)
            Me.lblQueueEnds.TabIndex = 0
            Me.lblQueueEnds.Text = "Finishes:"
            '
            'tmrUpdate
            '
            Me.tmrUpdate.Enabled = True
            Me.tmrUpdate.Interval = 2000
            '
            'SkillQueueTimeControl
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.panelSQT)
            Me.DoubleBuffered = True
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Name = "SkillQueueTimeControl"
            Me.Size = New System.Drawing.Size(300, 48)
            Me.panelSQT.ResumeLayout(False)
            Me.panelSQT.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents panelSQT As DevComponents.DotNetBar.PanelEx
        Friend WithEvents lblQueueRemaining As DevComponents.DotNetBar.LabelX
        Friend WithEvents lblQueueEnds As DevComponents.DotNetBar.LabelX
        Friend WithEvents tmrUpdate As System.Windows.Forms.Timer

    End Class
End NameSpace