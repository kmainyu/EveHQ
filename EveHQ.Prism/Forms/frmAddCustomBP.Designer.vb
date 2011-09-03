<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddCustomBP
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
        Me.lblPELevel = New System.Windows.Forms.Label
        Me.lblMELevel = New System.Windows.Forms.Label
        Me.pbBP = New System.Windows.Forms.PictureBox
        Me.pnlCustomBP = New DevComponents.DotNetBar.PanelEx
        Me.btnAccept = New DevComponents.DotNetBar.ButtonX
        Me.btnCancel = New DevComponents.DotNetBar.ButtonX
        Me.cboBPs = New DevComponents.DotNetBar.Controls.ComboBoxEx
        Me.nudMELevel = New DevComponents.Editors.IntegerInput
        Me.nudPELevel = New DevComponents.Editors.IntegerInput
        CType(Me.pbBP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlCustomBP.SuspendLayout()
        CType(Me.nudMELevel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPELevel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblPELevel
        '
        Me.lblPELevel.AutoSize = True
        Me.lblPELevel.Location = New System.Drawing.Point(243, 57)
        Me.lblPELevel.Name = "lblPELevel"
        Me.lblPELevel.Size = New System.Drawing.Size(51, 13)
        Me.lblPELevel.TabIndex = 36
        Me.lblPELevel.Text = "PE Level:"
        '
        'lblMELevel
        '
        Me.lblMELevel.AutoSize = True
        Me.lblMELevel.Location = New System.Drawing.Point(82, 57)
        Me.lblMELevel.Name = "lblMELevel"
        Me.lblMELevel.Size = New System.Drawing.Size(53, 13)
        Me.lblMELevel.TabIndex = 33
        Me.lblMELevel.Text = "ME Level:"
        '
        'pbBP
        '
        Me.pbBP.BackColor = System.Drawing.SystemColors.ButtonShadow
        Me.pbBP.Location = New System.Drawing.Point(12, 12)
        Me.pbBP.Name = "pbBP"
        Me.pbBP.Size = New System.Drawing.Size(64, 64)
        Me.pbBP.TabIndex = 30
        Me.pbBP.TabStop = False
        '
        'pnlCustomBP
        '
        Me.pnlCustomBP.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlCustomBP.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlCustomBP.Controls.Add(Me.nudPELevel)
        Me.pnlCustomBP.Controls.Add(Me.nudMELevel)
        Me.pnlCustomBP.Controls.Add(Me.cboBPs)
        Me.pnlCustomBP.Controls.Add(Me.btnCancel)
        Me.pnlCustomBP.Controls.Add(Me.btnAccept)
        Me.pnlCustomBP.Controls.Add(Me.pbBP)
        Me.pnlCustomBP.Controls.Add(Me.lblMELevel)
        Me.pnlCustomBP.Controls.Add(Me.lblPELevel)
        Me.pnlCustomBP.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlCustomBP.Location = New System.Drawing.Point(0, 0)
        Me.pnlCustomBP.Name = "pnlCustomBP"
        Me.pnlCustomBP.Size = New System.Drawing.Size(397, 140)
        Me.pnlCustomBP.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlCustomBP.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlCustomBP.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlCustomBP.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlCustomBP.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlCustomBP.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlCustomBP.Style.GradientAngle = 90
        Me.pnlCustomBP.TabIndex = 50
        '
        'btnAccept
        '
        Me.btnAccept.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAccept.CallBasePaintBackground = True
        Me.btnAccept.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAccept.Location = New System.Drawing.Point(231, 101)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnAccept.TabIndex = 50
        Me.btnAccept.Text = "Accept"
        '
        'btnCancel
        '
        Me.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnCancel.CallBasePaintBackground = True
        Me.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(312, 101)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnCancel.TabIndex = 51
        Me.btnCancel.Text = "Cancel"
        '
        'cboBPs
        '
        Me.cboBPs.DisplayMember = "Text"
        Me.cboBPs.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboBPs.FormattingEnabled = True
        Me.cboBPs.ItemHeight = 15
        Me.cboBPs.Location = New System.Drawing.Point(82, 22)
        Me.cboBPs.Name = "cboBPs"
        Me.cboBPs.Size = New System.Drawing.Size(305, 21)
        Me.cboBPs.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboBPs.TabIndex = 52
        '
        'nudMELevel
        '
        '
        '
        '
        Me.nudMELevel.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.nudMELevel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.nudMELevel.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
        Me.nudMELevel.Location = New System.Drawing.Point(141, 55)
        Me.nudMELevel.MaxValue = 100000
        Me.nudMELevel.MinValue = -10
        Me.nudMELevel.Name = "nudMELevel"
        Me.nudMELevel.ShowUpDown = True
        Me.nudMELevel.Size = New System.Drawing.Size(87, 21)
        Me.nudMELevel.TabIndex = 53
        '
        'nudPELevel
        '
        '
        '
        '
        Me.nudPELevel.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.nudPELevel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.nudPELevel.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
        Me.nudPELevel.Location = New System.Drawing.Point(300, 55)
        Me.nudPELevel.MaxValue = 100000
        Me.nudPELevel.MinValue = -10
        Me.nudPELevel.Name = "nudPELevel"
        Me.nudPELevel.ShowUpDown = True
        Me.nudPELevel.Size = New System.Drawing.Size(87, 21)
        Me.nudPELevel.TabIndex = 54
        '
        'frmAddCustomBP
        '
        Me.AcceptButton = Me.btnAccept
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(397, 140)
        Me.Controls.Add(Me.pnlCustomBP)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAddCustomBP"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add Custom Blueprint"
        CType(Me.pbBP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlCustomBP.ResumeLayout(False)
        Me.pnlCustomBP.PerformLayout()
        CType(Me.nudMELevel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPELevel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblPELevel As System.Windows.Forms.Label
    Friend WithEvents lblMELevel As System.Windows.Forms.Label
    Friend WithEvents pbBP As System.Windows.Forms.PictureBox
    Friend WithEvents pnlCustomBP As DevComponents.DotNetBar.PanelEx
    Friend WithEvents nudMELevel As DevComponents.Editors.IntegerInput
    Friend WithEvents cboBPs As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents btnCancel As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnAccept As DevComponents.DotNetBar.ButtonX
    Friend WithEvents nudPELevel As DevComponents.Editors.IntegerInput
End Class
