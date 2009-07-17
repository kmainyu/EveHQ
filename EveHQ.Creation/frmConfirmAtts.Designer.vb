<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConfirmAtts
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfirmAtts))
        Me.lblInfo = New System.Windows.Forms.Label
        Me.lblC = New System.Windows.Forms.Label
        Me.lblI = New System.Windows.Forms.Label
        Me.lblM = New System.Windows.Forms.Label
        Me.lblP = New System.Windows.Forms.Label
        Me.lblW = New System.Windows.Forms.Label
        Me.nudC = New System.Windows.Forms.NumericUpDown
        Me.nudI = New System.Windows.Forms.NumericUpDown
        Me.nudM = New System.Windows.Forms.NumericUpDown
        Me.nudP = New System.Windows.Forms.NumericUpDown
        Me.nudW = New System.Windows.Forms.NumericUpDown
        Me.btnAccept = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblAttributesTotalLabel = New System.Windows.Forms.Label
        Me.lblAttTotal = New System.Windows.Forms.Label
        Me.lblCharacterIDLabel = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblCharID = New System.Windows.Forms.Label
        Me.txtCharName = New System.Windows.Forms.TextBox
        CType(Me.nudC, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudI, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudP, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudW, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblInfo
        '
        Me.lblInfo.Location = New System.Drawing.Point(12, 9)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(334, 85)
        Me.lblInfo.TabIndex = 0
        Me.lblInfo.Text = resources.GetString("lblInfo.Text")
        Me.lblInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblC
        '
        Me.lblC.AutoSize = True
        Me.lblC.Location = New System.Drawing.Point(59, 108)
        Me.lblC.Name = "lblC"
        Me.lblC.Size = New System.Drawing.Size(90, 13)
        Me.lblC.TabIndex = 1
        Me.lblC.Text = "Charisma (0 to 0)"
        '
        'lblI
        '
        Me.lblI.AutoSize = True
        Me.lblI.Location = New System.Drawing.Point(59, 134)
        Me.lblI.Name = "lblI"
        Me.lblI.Size = New System.Drawing.Size(101, 13)
        Me.lblI.TabIndex = 2
        Me.lblI.Text = "Intelligence (0 to 0)"
        '
        'lblM
        '
        Me.lblM.AutoSize = True
        Me.lblM.Location = New System.Drawing.Point(59, 160)
        Me.lblM.Name = "lblM"
        Me.lblM.Size = New System.Drawing.Size(84, 13)
        Me.lblM.TabIndex = 3
        Me.lblM.Text = "Memory (0 to 0)"
        '
        'lblP
        '
        Me.lblP.AutoSize = True
        Me.lblP.Location = New System.Drawing.Point(59, 186)
        Me.lblP.Name = "lblP"
        Me.lblP.Size = New System.Drawing.Size(97, 13)
        Me.lblP.TabIndex = 4
        Me.lblP.Text = "Perception (0 to 0)"
        '
        'lblW
        '
        Me.lblW.AutoSize = True
        Me.lblW.Location = New System.Drawing.Point(59, 212)
        Me.lblW.Name = "lblW"
        Me.lblW.Size = New System.Drawing.Size(92, 13)
        Me.lblW.TabIndex = 5
        Me.lblW.Text = "Willpower (0 to 0)"
        '
        'nudC
        '
        Me.nudC.Location = New System.Drawing.Point(216, 106)
        Me.nudC.Name = "nudC"
        Me.nudC.Size = New System.Drawing.Size(70, 21)
        Me.nudC.TabIndex = 6
        '
        'nudI
        '
        Me.nudI.Location = New System.Drawing.Point(216, 132)
        Me.nudI.Name = "nudI"
        Me.nudI.Size = New System.Drawing.Size(70, 21)
        Me.nudI.TabIndex = 7
        '
        'nudM
        '
        Me.nudM.Location = New System.Drawing.Point(216, 158)
        Me.nudM.Name = "nudM"
        Me.nudM.Size = New System.Drawing.Size(70, 21)
        Me.nudM.TabIndex = 8
        '
        'nudP
        '
        Me.nudP.Location = New System.Drawing.Point(216, 184)
        Me.nudP.Name = "nudP"
        Me.nudP.Size = New System.Drawing.Size(70, 21)
        Me.nudP.TabIndex = 9
        '
        'nudW
        '
        Me.nudW.Location = New System.Drawing.Point(216, 210)
        Me.nudW.Name = "nudW"
        Me.nudW.Size = New System.Drawing.Size(70, 21)
        Me.nudW.TabIndex = 10
        '
        'btnAccept
        '
        Me.btnAccept.Location = New System.Drawing.Point(191, 357)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.TabIndex = 11
        Me.btnAccept.Text = "&Accept"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(272, 357)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 12
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblAttributesTotalLabel
        '
        Me.lblAttributesTotalLabel.AutoSize = True
        Me.lblAttributesTotalLabel.Location = New System.Drawing.Point(59, 238)
        Me.lblAttributesTotalLabel.Name = "lblAttributesTotalLabel"
        Me.lblAttributesTotalLabel.Size = New System.Drawing.Size(86, 13)
        Me.lblAttributesTotalLabel.TabIndex = 13
        Me.lblAttributesTotalLabel.Text = "Attributes Total:"
        '
        'lblAttTotal
        '
        Me.lblAttTotal.AutoSize = True
        Me.lblAttTotal.Location = New System.Drawing.Point(231, 238)
        Me.lblAttTotal.Name = "lblAttTotal"
        Me.lblAttTotal.Size = New System.Drawing.Size(19, 13)
        Me.lblAttTotal.TabIndex = 14
        Me.lblAttTotal.Text = "39"
        '
        'lblCharacterIDLabel
        '
        Me.lblCharacterIDLabel.AutoSize = True
        Me.lblCharacterIDLabel.Location = New System.Drawing.Point(62, 285)
        Me.lblCharacterIDLabel.Name = "lblCharacterIDLabel"
        Me.lblCharacterIDLabel.Size = New System.Drawing.Size(73, 13)
        Me.lblCharacterIDLabel.TabIndex = 15
        Me.lblCharacterIDLabel.Text = "Character ID:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(62, 309)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(89, 13)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Character Name:"
        '
        'lblCharID
        '
        Me.lblCharID.AutoSize = True
        Me.lblCharID.Location = New System.Drawing.Point(213, 285)
        Me.lblCharID.Name = "lblCharID"
        Me.lblCharID.Size = New System.Drawing.Size(0, 13)
        Me.lblCharID.TabIndex = 17
        '
        'txtCharName
        '
        Me.txtCharName.Location = New System.Drawing.Point(166, 306)
        Me.txtCharName.Name = "txtCharName"
        Me.txtCharName.Size = New System.Drawing.Size(153, 21)
        Me.txtCharName.TabIndex = 18
        '
        'frmConfirmAtts
        '
        Me.AcceptButton = Me.btnAccept
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(359, 392)
        Me.Controls.Add(Me.txtCharName)
        Me.Controls.Add(Me.lblCharID)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblCharacterIDLabel)
        Me.Controls.Add(Me.lblAttTotal)
        Me.Controls.Add(Me.lblAttributesTotalLabel)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.nudW)
        Me.Controls.Add(Me.nudP)
        Me.Controls.Add(Me.nudM)
        Me.Controls.Add(Me.nudI)
        Me.Controls.Add(Me.nudC)
        Me.Controls.Add(Me.lblW)
        Me.Controls.Add(Me.lblP)
        Me.Controls.Add(Me.lblM)
        Me.Controls.Add(Me.lblI)
        Me.Controls.Add(Me.lblC)
        Me.Controls.Add(Me.lblInfo)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmConfirmAtts"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Confirm Character Details"
        CType(Me.nudC, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudI, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudP, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudW, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents lblC As System.Windows.Forms.Label
    Friend WithEvents lblI As System.Windows.Forms.Label
    Friend WithEvents lblM As System.Windows.Forms.Label
    Friend WithEvents lblP As System.Windows.Forms.Label
    Friend WithEvents lblW As System.Windows.Forms.Label
    Friend WithEvents nudC As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudI As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudM As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudP As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudW As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblAttributesTotalLabel As System.Windows.Forms.Label
    Friend WithEvents lblAttTotal As System.Windows.Forms.Label
    Friend WithEvents lblCharacterIDLabel As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblCharID As System.Windows.Forms.Label
    Friend WithEvents txtCharName As System.Windows.Forms.TextBox
End Class
