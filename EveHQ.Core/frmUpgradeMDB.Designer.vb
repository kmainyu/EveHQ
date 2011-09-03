<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUpgradeMDB
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUpgradeMDB))
		Me.PictureBox1 = New System.Windows.Forms.PictureBox()
		Me.lblGenerating = New System.Windows.Forms.Label()
		Me.PanelEx1 = New DevComponents.DotNetBar.PanelEx()
		Me.lblStatus = New System.Windows.Forms.Label()
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
		Me.PanelEx1.SuspendLayout()
		Me.SuspendLayout()
		'
		'PictureBox1
		'
		Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
		Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
		Me.PictureBox1.Name = "PictureBox1"
		Me.PictureBox1.Size = New System.Drawing.Size(48, 48)
		Me.PictureBox1.TabIndex = 0
		Me.PictureBox1.TabStop = False
		'
		'lblGenerating
		'
		Me.lblGenerating.Location = New System.Drawing.Point(66, 9)
		Me.lblGenerating.Name = "lblGenerating"
		Me.lblGenerating.Size = New System.Drawing.Size(285, 31)
		Me.lblGenerating.TabIndex = 1
		Me.lblGenerating.Text = "Please wait while EveHQ upgrades the old Access (.mdb) database to the new SQLCE " & _
			"(.sdf) format..." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
		'
		'PanelEx1
		'
		Me.PanelEx1.CanvasColor = System.Drawing.SystemColors.Control
		Me.PanelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
		Me.PanelEx1.Controls.Add(Me.lblStatus)
		Me.PanelEx1.Controls.Add(Me.PictureBox1)
		Me.PanelEx1.Controls.Add(Me.lblGenerating)
		Me.PanelEx1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.PanelEx1.Location = New System.Drawing.Point(0, 0)
		Me.PanelEx1.Name = "PanelEx1"
		Me.PanelEx1.Size = New System.Drawing.Size(354, 72)
		Me.PanelEx1.Style.Alignment = System.Drawing.StringAlignment.Center
		Me.PanelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
		Me.PanelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
		Me.PanelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
		Me.PanelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
		Me.PanelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
		Me.PanelEx1.Style.GradientAngle = 90
		Me.PanelEx1.TabIndex = 2
		'
		'lblStatus
		'
		Me.lblStatus.Location = New System.Drawing.Point(66, 41)
		Me.lblStatus.Name = "lblStatus"
		Me.lblStatus.Size = New System.Drawing.Size(276, 19)
		Me.lblStatus.TabIndex = 2
		'
		'frmUpgradeMDB
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(354, 72)
		Me.ControlBox = False
		Me.Controls.Add(Me.PanelEx1)
		Me.EnableGlass = False
		Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
		Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
		Me.Name = "frmUpgradeMDB"
		Me.ShowInTaskbar = False
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
		Me.Text = "Upgrading Access Database..."
		CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
		Me.PanelEx1.ResumeLayout(False)
		Me.ResumeLayout(False)

	End Sub
	Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
	Friend WithEvents lblGenerating As System.Windows.Forms.Label
	Friend WithEvents PanelEx1 As DevComponents.DotNetBar.PanelEx
	Friend WithEvents lblStatus As System.Windows.Forms.Label
End Class
