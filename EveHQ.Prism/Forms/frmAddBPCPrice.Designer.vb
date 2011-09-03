<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddBPCPrice
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
        Me.pnlCustomBP = New DevComponents.DotNetBar.PanelEx
        Me.lblMaxRunCost = New System.Windows.Forms.Label
        Me.nudMaxRunCost = New DevComponents.Editors.DoubleInput
        Me.lblMinRunCost = New System.Windows.Forms.Label
        Me.nudMinRunCost = New DevComponents.Editors.DoubleInput
        Me.btnCancel = New DevComponents.DotNetBar.ButtonX
        Me.btnAccept = New DevComponents.DotNetBar.ButtonX
        Me.pbBP = New System.Windows.Forms.PictureBox
        Me.lblBPName = New System.Windows.Forms.Label
        Me.pnlCustomBP.SuspendLayout()
        CType(Me.nudMaxRunCost, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudMinRunCost, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbBP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlCustomBP
        '
        Me.pnlCustomBP.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlCustomBP.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlCustomBP.Controls.Add(Me.lblBPName)
        Me.pnlCustomBP.Controls.Add(Me.lblMaxRunCost)
        Me.pnlCustomBP.Controls.Add(Me.nudMaxRunCost)
        Me.pnlCustomBP.Controls.Add(Me.lblMinRunCost)
        Me.pnlCustomBP.Controls.Add(Me.nudMinRunCost)
        Me.pnlCustomBP.Controls.Add(Me.btnCancel)
        Me.pnlCustomBP.Controls.Add(Me.btnAccept)
        Me.pnlCustomBP.Controls.Add(Me.pbBP)
        Me.pnlCustomBP.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlCustomBP.Location = New System.Drawing.Point(0, 0)
        Me.pnlCustomBP.Name = "pnlCustomBP"
        Me.pnlCustomBP.Size = New System.Drawing.Size(329, 122)
        Me.pnlCustomBP.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlCustomBP.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlCustomBP.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlCustomBP.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlCustomBP.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlCustomBP.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlCustomBP.Style.GradientAngle = 90
        Me.pnlCustomBP.TabIndex = 50
        '
        'lblMaxRunCost
        '
        Me.lblMaxRunCost.AutoSize = True
        Me.lblMaxRunCost.Location = New System.Drawing.Point(92, 57)
        Me.lblMaxRunCost.Name = "lblMaxRunCost"
        Me.lblMaxRunCost.Size = New System.Drawing.Size(78, 13)
        Me.lblMaxRunCost.TabIndex = 55
        Me.lblMaxRunCost.Text = "Max Run Cost:"
        '
        'nudMaxRunCost
        '
        '
        '
        '
        Me.nudMaxRunCost.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.nudMaxRunCost.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.nudMaxRunCost.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
        Me.nudMaxRunCost.Increment = 1
        Me.nudMaxRunCost.Location = New System.Drawing.Point(176, 52)
        Me.nudMaxRunCost.MaxValue = 1000000000
        Me.nudMaxRunCost.MinValue = 0
        Me.nudMaxRunCost.Name = "nudMaxRunCost"
        Me.nudMaxRunCost.ShowUpDown = True
        Me.nudMaxRunCost.Size = New System.Drawing.Size(135, 21)
        Me.nudMaxRunCost.TabIndex = 54
        '
        'lblMinRunCost
        '
        Me.lblMinRunCost.AutoSize = True
        Me.lblMinRunCost.Location = New System.Drawing.Point(92, 30)
        Me.lblMinRunCost.Name = "lblMinRunCost"
        Me.lblMinRunCost.Size = New System.Drawing.Size(74, 13)
        Me.lblMinRunCost.TabIndex = 53
        Me.lblMinRunCost.Text = "Min Run Cost:"
        '
        'nudMinRunCost
        '
        '
        '
        '
        Me.nudMinRunCost.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.nudMinRunCost.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.nudMinRunCost.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
        Me.nudMinRunCost.Increment = 1
        Me.nudMinRunCost.Location = New System.Drawing.Point(176, 25)
        Me.nudMinRunCost.MaxValue = 100000000000
        Me.nudMinRunCost.MinValue = 0
        Me.nudMinRunCost.Name = "nudMinRunCost"
        Me.nudMinRunCost.ShowUpDown = True
        Me.nudMinRunCost.Size = New System.Drawing.Size(135, 21)
        Me.nudMinRunCost.TabIndex = 52
        '
        'btnCancel
        '
        Me.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnCancel.CallBasePaintBackground = True
        Me.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(245, 90)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnCancel.TabIndex = 51
        Me.btnCancel.Text = "Cancel"
        '
        'btnAccept
        '
        Me.btnAccept.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAccept.CallBasePaintBackground = True
        Me.btnAccept.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAccept.Location = New System.Drawing.Point(164, 90)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnAccept.TabIndex = 50
        Me.btnAccept.Text = "Accept"
        '
        'pbBP
        '
        Me.pbBP.BackColor = System.Drawing.SystemColors.ButtonShadow
        Me.pbBP.Location = New System.Drawing.Point(12, 25)
        Me.pbBP.Name = "pbBP"
        Me.pbBP.Size = New System.Drawing.Size(64, 64)
        Me.pbBP.TabIndex = 30
        Me.pbBP.TabStop = False
        '
        'lblBPName
        '
        Me.lblBPName.AutoSize = True
        Me.lblBPName.Location = New System.Drawing.Point(12, 9)
        Me.lblBPName.Name = "lblBPName"
        Me.lblBPName.Size = New System.Drawing.Size(79, 13)
        Me.lblBPName.TabIndex = 56
        Me.lblBPName.Text = "Blueprint Name"
        '
        'frmAddBPCPrice
        '
        Me.AcceptButton = Me.btnAccept
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(329, 122)
        Me.Controls.Add(Me.pnlCustomBP)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAddBPCPrice"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add BPC Price"
        Me.pnlCustomBP.ResumeLayout(False)
        Me.pnlCustomBP.PerformLayout()
        CType(Me.nudMaxRunCost, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudMinRunCost, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbBP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pbBP As System.Windows.Forms.PictureBox
    Friend WithEvents pnlCustomBP As DevComponents.DotNetBar.PanelEx
    Friend WithEvents btnCancel As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnAccept As DevComponents.DotNetBar.ButtonX
    Friend WithEvents nudMinRunCost As DevComponents.Editors.DoubleInput
    Friend WithEvents lblMaxRunCost As System.Windows.Forms.Label
    Friend WithEvents nudMaxRunCost As DevComponents.Editors.DoubleInput
    Friend WithEvents lblMinRunCost As System.Windows.Forms.Label
    Friend WithEvents lblBPName As System.Windows.Forms.Label
End Class
