<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DBCPilotInfoConfig
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DBCPilotInfoConfig))
        Me.lblDefaultPilot = New System.Windows.Forms.Label
        Me.cboPilots = New System.Windows.Forms.ComboBox
        Me.lblWidth = New System.Windows.Forms.Label
        Me.lblHeight = New System.Windows.Forms.Label
        Me.nudWidth = New System.Windows.Forms.NumericUpDown
        Me.nudHeight = New System.Windows.Forms.NumericUpDown
        Me.btnAccept = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        CType(Me.nudWidth, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudHeight, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblDefaultPilot
        '
        Me.lblDefaultPilot.AutoSize = True
        Me.lblDefaultPilot.Location = New System.Drawing.Point(12, 63)
        Me.lblDefaultPilot.Name = "lblDefaultPilot"
        Me.lblDefaultPilot.Size = New System.Drawing.Size(69, 13)
        Me.lblDefaultPilot.TabIndex = 0
        Me.lblDefaultPilot.Text = "Default Pilot:"
        '
        'cboPilots
        '
        Me.cboPilots.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.Location = New System.Drawing.Point(87, 60)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(175, 21)
        Me.cboPilots.TabIndex = 1
        '
        'lblWidth
        '
        Me.lblWidth.AutoSize = True
        Me.lblWidth.Location = New System.Drawing.Point(12, 19)
        Me.lblWidth.Name = "lblWidth"
        Me.lblWidth.Size = New System.Drawing.Size(39, 13)
        Me.lblWidth.TabIndex = 2
        Me.lblWidth.Text = "Width:"
        '
        'lblHeight
        '
        Me.lblHeight.AutoSize = True
        Me.lblHeight.Location = New System.Drawing.Point(141, 19)
        Me.lblHeight.Name = "lblHeight"
        Me.lblHeight.Size = New System.Drawing.Size(42, 13)
        Me.lblHeight.TabIndex = 3
        Me.lblHeight.Text = "Height:"
        '
        'nudWidth
        '
        Me.nudWidth.Location = New System.Drawing.Point(57, 17)
        Me.nudWidth.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudWidth.Minimum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.nudWidth.Name = "nudWidth"
        Me.nudWidth.Size = New System.Drawing.Size(60, 21)
        Me.nudWidth.TabIndex = 4
        Me.nudWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudWidth.Value = New Decimal(New Integer() {300, 0, 0, 0})
        '
        'nudHeight
        '
        Me.nudHeight.Location = New System.Drawing.Point(189, 17)
        Me.nudHeight.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudHeight.Minimum = New Decimal(New Integer() {220, 0, 0, 0})
        Me.nudHeight.Name = "nudHeight"
        Me.nudHeight.Size = New System.Drawing.Size(60, 21)
        Me.nudHeight.TabIndex = 5
        Me.nudHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudHeight.Value = New Decimal(New Integer() {220, 0, 0, 0})
        '
        'btnAccept
        '
        Me.btnAccept.Location = New System.Drawing.Point(106, 121)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.TabIndex = 6
        Me.btnAccept.Text = "Accept"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(187, 121)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'DBCPilotInfoConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(274, 156)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.nudHeight)
        Me.Controls.Add(Me.nudWidth)
        Me.Controls.Add(Me.lblHeight)
        Me.Controls.Add(Me.lblWidth)
        Me.Controls.Add(Me.cboPilots)
        Me.Controls.Add(Me.lblDefaultPilot)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "DBCPilotInfoConfig"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pilot Info Configuration"
        CType(Me.nudWidth, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudHeight, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblDefaultPilot As System.Windows.Forms.Label
    Friend WithEvents cboPilots As System.Windows.Forms.ComboBox
    Friend WithEvents lblWidth As System.Windows.Forms.Label
    Friend WithEvents lblHeight As System.Windows.Forms.Label
    Friend WithEvents nudWidth As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudHeight As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
