<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CharacterBlock
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
        Me.pbPilot = New System.Windows.Forms.PictureBox
        Me.lblPilotName = New System.Windows.Forms.Label
        Me.lblSkill = New System.Windows.Forms.Label
        Me.lblTime = New System.Windows.Forms.Label
        Me.lblIsk = New System.Windows.Forms.Label
        CType(Me.pbPilot, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pbPilot
        '
        Me.pbPilot.Location = New System.Drawing.Point(3, 3)
        Me.pbPilot.Name = "pbPilot"
        Me.pbPilot.Size = New System.Drawing.Size(64, 64)
        Me.pbPilot.TabIndex = 0
        Me.pbPilot.TabStop = False
        '
        'lblPilotName
        '
        Me.lblPilotName.AutoSize = True
        Me.lblPilotName.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPilotName.Location = New System.Drawing.Point(73, 3)
        Me.lblPilotName.Name = "lblPilotName"
        Me.lblPilotName.Size = New System.Drawing.Size(75, 16)
        Me.lblPilotName.TabIndex = 1
        Me.lblPilotName.Text = "Pilot Name"
        '
        'lblSkill
        '
        Me.lblSkill.AutoSize = True
        Me.lblSkill.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSkill.Location = New System.Drawing.Point(73, 22)
        Me.lblSkill.Name = "lblSkill"
        Me.lblSkill.Size = New System.Drawing.Size(78, 13)
        Me.lblSkill.TabIndex = 2
        Me.lblSkill.Text = "Skill In Training"
        '
        'lblTime
        '
        Me.lblTime.AutoSize = True
        Me.lblTime.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTime.Location = New System.Drawing.Point(73, 37)
        Me.lblTime.Name = "lblTime"
        Me.lblTime.Size = New System.Drawing.Size(50, 13)
        Me.lblTime.TabIndex = 3
        Me.lblTime.Text = "End Time"
        '
        'lblIsk
        '
        Me.lblIsk.AutoSize = True
        Me.lblIsk.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblIsk.Location = New System.Drawing.Point(73, 52)
        Me.lblIsk.Name = "lblIsk"
        Me.lblIsk.Size = New System.Drawing.Size(41, 13)
        Me.lblIsk.TabIndex = 4
        Me.lblIsk.Text = "Wealth"
        '
        'CharacterBlock
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.BackColor = System.Drawing.Color.Transparent
        Me.Controls.Add(Me.lblIsk)
        Me.Controls.Add(Me.lblTime)
        Me.Controls.Add(Me.lblSkill)
        Me.Controls.Add(Me.lblPilotName)
        Me.Controls.Add(Me.pbPilot)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "CharacterBlock"
        Me.Size = New System.Drawing.Size(300, 70)
        CType(Me.pbPilot, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pbPilot As System.Windows.Forms.PictureBox
    Friend WithEvents lblPilotName As System.Windows.Forms.Label
    Friend WithEvents lblSkill As System.Windows.Forms.Label
    Friend WithEvents lblTime As System.Windows.Forms.Label
    Friend WithEvents lblIsk As System.Windows.Forms.Label

End Class
