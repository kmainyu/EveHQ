Namespace SkillQueueControl
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class SkillQueueBlock
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
            Me.panelSQB = New DevComponents.DotNetBar.PanelEx
            Me.lblSkillName = New DevComponents.DotNetBar.LabelX
            Me.lblTimeToTrain = New DevComponents.DotNetBar.LabelX
            Me.lblSkillLevel = New DevComponents.DotNetBar.LabelX
            Me.PictureBox1 = New System.Windows.Forms.PictureBox
            Me.tmrUpdate = New System.Windows.Forms.Timer(Me.components)
            Me.panelSQB.SuspendLayout()
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'panelSQB
            '
            Me.panelSQB.CanvasColor = System.Drawing.SystemColors.Control
            Me.panelSQB.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.VS2005
            Me.panelSQB.Controls.Add(Me.lblSkillName)
            Me.panelSQB.Controls.Add(Me.lblTimeToTrain)
            Me.panelSQB.Controls.Add(Me.lblSkillLevel)
            Me.panelSQB.Controls.Add(Me.PictureBox1)
            Me.panelSQB.Dock = System.Windows.Forms.DockStyle.Fill
            Me.panelSQB.Location = New System.Drawing.Point(0, 0)
            Me.panelSQB.Name = "panelSQB"
            Me.panelSQB.Size = New System.Drawing.Size(300, 48)
            Me.panelSQB.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.panelSQB.Style.BackColor1.Color = System.Drawing.Color.DimGray
            Me.panelSQB.Style.BackColor2.Color = System.Drawing.Color.Black
            Me.panelSQB.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.panelSQB.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.panelSQB.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.panelSQB.Style.GradientAngle = 90
            Me.panelSQB.TabIndex = 0
            '
            'lblSkillName
            '
            Me.lblSkillName.AutoSize = True
            '
            '
            '
            Me.lblSkillName.BackgroundStyle.Class = ""
            Me.lblSkillName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblSkillName.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblSkillName.ForeColor = System.Drawing.Color.White
            Me.lblSkillName.Location = New System.Drawing.Point(31, 3)
            Me.lblSkillName.Name = "lblSkillName"
            Me.lblSkillName.Size = New System.Drawing.Size(97, 17)
            Me.lblSkillName.TabIndex = 1
            Me.lblSkillName.Text = "Skill Name (Rank)"
            '
            'lblTimeToTrain
            '
            Me.lblTimeToTrain.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            '
            '
            '
            Me.lblTimeToTrain.BackgroundStyle.Class = ""
            Me.lblTimeToTrain.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblTimeToTrain.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTimeToTrain.ForeColor = System.Drawing.Color.White
            Me.lblTimeToTrain.Location = New System.Drawing.Point(80, 22)
            Me.lblTimeToTrain.Name = "lblTimeToTrain"
            Me.lblTimeToTrain.Size = New System.Drawing.Size(150, 13)
            Me.lblTimeToTrain.TabIndex = 3
            Me.lblTimeToTrain.Text = "10d 10h 10m 10s"
            Me.lblTimeToTrain.TextAlignment = System.Drawing.StringAlignment.Far
            '
            'lblSkillLevel
            '
            Me.lblSkillLevel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            '
            '
            '
            Me.lblSkillLevel.BackgroundStyle.Class = ""
            Me.lblSkillLevel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblSkillLevel.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblSkillLevel.ForeColor = System.Drawing.Color.White
            Me.lblSkillLevel.Location = New System.Drawing.Point(80, 5)
            Me.lblSkillLevel.Name = "lblSkillLevel"
            Me.lblSkillLevel.Size = New System.Drawing.Size(150, 13)
            Me.lblSkillLevel.TabIndex = 2
            Me.lblSkillLevel.Text = "Level X"
            Me.lblSkillLevel.TextAlignment = System.Drawing.StringAlignment.Far
            '
            'PictureBox1
            '
            Me.PictureBox1.Image = Global.EveHQ.Core.My.Resources.Resources.SkillBookClosed32
            Me.PictureBox1.Location = New System.Drawing.Point(3, 3)
            Me.PictureBox1.Name = "PictureBox1"
            Me.PictureBox1.Size = New System.Drawing.Size(32, 32)
            Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.PictureBox1.TabIndex = 0
            Me.PictureBox1.TabStop = False
            '
            'tmrUpdate
            '
            Me.tmrUpdate.Interval = 2000
            '
            'SkillQueueBlock
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.panelSQB)
            Me.DoubleBuffered = True
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Name = "SkillQueueBlock"
            Me.Size = New System.Drawing.Size(300, 48)
            Me.panelSQB.ResumeLayout(False)
            Me.panelSQB.PerformLayout()
            CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents panelSQB As DevComponents.DotNetBar.PanelEx
        Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
        Friend WithEvents lblSkillName As DevComponents.DotNetBar.LabelX
        Friend WithEvents lblTimeToTrain As DevComponents.DotNetBar.LabelX
        Friend WithEvents lblSkillLevel As DevComponents.DotNetBar.LabelX
        Friend WithEvents tmrUpdate As System.Windows.Forms.Timer

    End Class
End Namespace