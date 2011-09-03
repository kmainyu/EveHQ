<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DBCWHole
    Inherits Widget

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
        Me.cboWHType = New DevComponents.DotNetBar.Controls.ComboBoxEx
        Me.lblTargetSystemClass = New DevComponents.DotNetBar.LabelX
        Me.lblStabilityWindow = New DevComponents.DotNetBar.LabelX
        Me.lblMaxTotalMass = New DevComponents.DotNetBar.LabelX
        Me.lblMaxJumpableMass = New DevComponents.DotNetBar.LabelX
        Me.lblTSC = New DevComponents.DotNetBar.LabelX
        Me.lblLE = New DevComponents.DotNetBar.LabelX
        Me.lblMTM = New DevComponents.DotNetBar.LabelX
        Me.lblMJM = New DevComponents.DotNetBar.LabelX
        Me.lblWHType = New DevComponents.DotNetBar.LabelX
        Me.AGPContent.SuspendLayout()
        CType(Me.pbConfig, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblHeader
        '
        '
        '
        '
        Me.lblHeader.BackgroundStyle.Class = ""
        Me.lblHeader.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblHeader.Text = "Wormhole Data"
        '
        'AGPContent
        '
        Me.AGPContent.Controls.Add(Me.lblWHType)
        Me.AGPContent.Controls.Add(Me.lblMJM)
        Me.AGPContent.Controls.Add(Me.lblMTM)
        Me.AGPContent.Controls.Add(Me.lblLE)
        Me.AGPContent.Controls.Add(Me.lblTSC)
        Me.AGPContent.Controls.Add(Me.lblMaxJumpableMass)
        Me.AGPContent.Controls.Add(Me.lblMaxTotalMass)
        Me.AGPContent.Controls.Add(Me.lblStabilityWindow)
        Me.AGPContent.Controls.Add(Me.lblTargetSystemClass)
        Me.AGPContent.Controls.Add(Me.cboWHType)
        Me.AGPContent.MinimumSize = New System.Drawing.Size(300, 170)
        Me.AGPContent.Size = New System.Drawing.Size(300, 170)
        Me.AGPContent.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.AGPContent.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.AGPContent.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.AGPContent.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.AGPContent.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.AGPContent.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.AGPContent.Style.GradientAngle = 90
        Me.AGPContent.Controls.SetChildIndex(Me.lblHeader, 0)
        Me.AGPContent.Controls.SetChildIndex(Me.pbConfig, 0)
        Me.AGPContent.Controls.SetChildIndex(Me.cboWHType, 0)
        Me.AGPContent.Controls.SetChildIndex(Me.lblTargetSystemClass, 0)
        Me.AGPContent.Controls.SetChildIndex(Me.lblStabilityWindow, 0)
        Me.AGPContent.Controls.SetChildIndex(Me.lblMaxTotalMass, 0)
        Me.AGPContent.Controls.SetChildIndex(Me.lblMaxJumpableMass, 0)
        Me.AGPContent.Controls.SetChildIndex(Me.lblTSC, 0)
        Me.AGPContent.Controls.SetChildIndex(Me.lblLE, 0)
        Me.AGPContent.Controls.SetChildIndex(Me.lblMTM, 0)
        Me.AGPContent.Controls.SetChildIndex(Me.lblMJM, 0)
        Me.AGPContent.Controls.SetChildIndex(Me.lblWHType, 0)
        '
        'cboWHType
        '
        Me.cboWHType.DisplayMember = "Text"
        Me.cboWHType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboWHType.FormattingEnabled = True
        Me.cboWHType.ItemHeight = 15
        Me.cboWHType.Location = New System.Drawing.Point(153, 45)
        Me.cboWHType.Name = "cboWHType"
        Me.cboWHType.Size = New System.Drawing.Size(117, 21)
        Me.cboWHType.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboWHType.TabIndex = 25
        '
        'lblTargetSystemClass
        '
        Me.lblTargetSystemClass.AutoSize = True
        '
        '
        '
        Me.lblTargetSystemClass.BackgroundStyle.Class = ""
        Me.lblTargetSystemClass.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblTargetSystemClass.Location = New System.Drawing.Point(153, 76)
        Me.lblTargetSystemClass.Name = "lblTargetSystemClass"
        Me.lblTargetSystemClass.Size = New System.Drawing.Size(19, 16)
        Me.lblTargetSystemClass.TabIndex = 26
        Me.lblTargetSystemClass.Text = "n/a"
        '
        'lblStabilityWindow
        '
        Me.lblStabilityWindow.AutoSize = True
        '
        '
        '
        Me.lblStabilityWindow.BackgroundStyle.Class = ""
        Me.lblStabilityWindow.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblStabilityWindow.Location = New System.Drawing.Point(153, 98)
        Me.lblStabilityWindow.Name = "lblStabilityWindow"
        Me.lblStabilityWindow.Size = New System.Drawing.Size(19, 16)
        Me.lblStabilityWindow.TabIndex = 27
        Me.lblStabilityWindow.Text = "n/a"
        '
        'lblMaxTotalMass
        '
        Me.lblMaxTotalMass.AutoSize = True
        '
        '
        '
        Me.lblMaxTotalMass.BackgroundStyle.Class = ""
        Me.lblMaxTotalMass.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblMaxTotalMass.Location = New System.Drawing.Point(153, 120)
        Me.lblMaxTotalMass.Name = "lblMaxTotalMass"
        Me.lblMaxTotalMass.Size = New System.Drawing.Size(19, 16)
        Me.lblMaxTotalMass.TabIndex = 28
        Me.lblMaxTotalMass.Text = "n/a"
        '
        'lblMaxJumpableMass
        '
        Me.lblMaxJumpableMass.AutoSize = True
        '
        '
        '
        Me.lblMaxJumpableMass.BackgroundStyle.Class = ""
        Me.lblMaxJumpableMass.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblMaxJumpableMass.Location = New System.Drawing.Point(153, 142)
        Me.lblMaxJumpableMass.Name = "lblMaxJumpableMass"
        Me.lblMaxJumpableMass.Size = New System.Drawing.Size(19, 16)
        Me.lblMaxJumpableMass.TabIndex = 29
        Me.lblMaxJumpableMass.Text = "n/a"
        '
        'lblTSC
        '
        Me.lblTSC.AutoSize = True
        '
        '
        '
        Me.lblTSC.BackgroundStyle.Class = ""
        Me.lblTSC.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblTSC.Location = New System.Drawing.Point(17, 76)
        Me.lblTSC.Name = "lblTSC"
        Me.lblTSC.Size = New System.Drawing.Size(105, 16)
        Me.lblTSC.TabIndex = 30
        Me.lblTSC.Text = "Target System Class:"
        '
        'lblLE
        '
        Me.lblLE.AutoSize = True
        '
        '
        '
        Me.lblLE.BackgroundStyle.Class = ""
        Me.lblLE.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblLE.Location = New System.Drawing.Point(17, 98)
        Me.lblLE.Name = "lblLE"
        Me.lblLE.Size = New System.Drawing.Size(82, 16)
        Me.lblLE.TabIndex = 31
        Me.lblLE.Text = "Life Expectancy:"
        '
        'lblMTM
        '
        Me.lblMTM.AutoSize = True
        '
        '
        '
        Me.lblMTM.BackgroundStyle.Class = ""
        Me.lblMTM.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblMTM.Location = New System.Drawing.Point(18, 120)
        Me.lblMTM.Name = "lblMTM"
        Me.lblMTM.Size = New System.Drawing.Size(109, 16)
        Me.lblMTM.TabIndex = 32
        Me.lblMTM.Text = "Maximum Total Mass:"
        '
        'lblMJM
        '
        Me.lblMJM.AutoSize = True
        '
        '
        '
        Me.lblMJM.BackgroundStyle.Class = ""
        Me.lblMJM.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblMJM.Location = New System.Drawing.Point(17, 142)
        Me.lblMJM.Name = "lblMJM"
        Me.lblMJM.Size = New System.Drawing.Size(130, 16)
        Me.lblMJM.TabIndex = 33
        Me.lblMJM.Text = "Maximum Jumpable Mass:"
        '
        'lblWHType
        '
        Me.lblWHType.AutoSize = True
        '
        '
        '
        Me.lblWHType.BackgroundStyle.Class = ""
        Me.lblWHType.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblWHType.Location = New System.Drawing.Point(17, 45)
        Me.lblWHType.Name = "lblWHType"
        Me.lblWHType.Size = New System.Drawing.Size(83, 16)
        Me.lblWHType.TabIndex = 34
        Me.lblWHType.Text = "Wormhole Type:"
        '
        'DBCWHole
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ControlSize = New System.Drawing.Size(300, 170)
        Me.Name = "DBCWHole"
        Me.Size = New System.Drawing.Size(300, 170)
        Me.AGPContent.ResumeLayout(False)
        Me.AGPContent.PerformLayout()
        CType(Me.pbConfig, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cboWHType As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents lblTargetSystemClass As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblMaxJumpableMass As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblMaxTotalMass As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblStabilityWindow As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblWHType As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblMJM As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblMTM As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblLE As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblTSC As DevComponents.DotNetBar.LabelX

End Class
