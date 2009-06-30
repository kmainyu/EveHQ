<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEditBPDetails
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
        Me.lblBPName = New System.Windows.Forms.Label
        Me.lblAssetID = New System.Windows.Forms.Label
        Me.lblMELevel = New System.Windows.Forms.Label
        Me.lblStatus = New System.Windows.Forms.Label
        Me.lblRuns = New System.Windows.Forms.Label
        Me.lblPELevel = New System.Windows.Forms.Label
        Me.lblCurrent = New System.Windows.Forms.Label
        Me.lblNew = New System.Windows.Forms.Label
        Me.nudMELevel = New System.Windows.Forms.NumericUpDown
        Me.nudPELevel = New System.Windows.Forms.NumericUpDown
        Me.nudRuns = New System.Windows.Forms.NumericUpDown
        Me.cboStatus = New System.Windows.Forms.ComboBox
        Me.lblCurrentME = New System.Windows.Forms.Label
        Me.lblCurrentPE = New System.Windows.Forms.Label
        Me.lblCurrentRuns = New System.Windows.Forms.Label
        Me.lblCurrentStatus = New System.Windows.Forms.Label
        Me.btnAccept = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.pbBP = New System.Windows.Forms.PictureBox
        CType(Me.nudMELevel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPELevel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudRuns, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbBP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblBPName
        '
        Me.lblBPName.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBPName.Location = New System.Drawing.Point(82, 12)
        Me.lblBPName.Name = "lblBPName"
        Me.lblBPName.Size = New System.Drawing.Size(306, 43)
        Me.lblBPName.TabIndex = 12
        Me.lblBPName.Text = "Blueprint Name"
        '
        'lblAssetID
        '
        Me.lblAssetID.AutoSize = True
        Me.lblAssetID.Location = New System.Drawing.Point(83, 63)
        Me.lblAssetID.Name = "lblAssetID"
        Me.lblAssetID.Size = New System.Drawing.Size(52, 13)
        Me.lblAssetID.TabIndex = 13
        Me.lblAssetID.Text = "AssetID: "
        '
        'lblMELevel
        '
        Me.lblMELevel.AutoSize = True
        Me.lblMELevel.Location = New System.Drawing.Point(36, 119)
        Me.lblMELevel.Name = "lblMELevel"
        Me.lblMELevel.Size = New System.Drawing.Size(53, 13)
        Me.lblMELevel.TabIndex = 14
        Me.lblMELevel.Text = "ME Level:"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(36, 209)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(42, 13)
        Me.lblStatus.TabIndex = 15
        Me.lblStatus.Text = "Status:"
        '
        'lblRuns
        '
        Me.lblRuns.AutoSize = True
        Me.lblRuns.Location = New System.Drawing.Point(36, 173)
        Me.lblRuns.Name = "lblRuns"
        Me.lblRuns.Size = New System.Drawing.Size(35, 13)
        Me.lblRuns.TabIndex = 16
        Me.lblRuns.Text = "Runs:"
        '
        'lblPELevel
        '
        Me.lblPELevel.AutoSize = True
        Me.lblPELevel.Location = New System.Drawing.Point(36, 146)
        Me.lblPELevel.Name = "lblPELevel"
        Me.lblPELevel.Size = New System.Drawing.Size(51, 13)
        Me.lblPELevel.TabIndex = 17
        Me.lblPELevel.Text = "PE Level:"
        '
        'lblCurrent
        '
        Me.lblCurrent.AutoSize = True
        Me.lblCurrent.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrent.Location = New System.Drawing.Point(165, 95)
        Me.lblCurrent.Name = "lblCurrent"
        Me.lblCurrent.Size = New System.Drawing.Size(50, 13)
        Me.lblCurrent.TabIndex = 18
        Me.lblCurrent.Text = "Current"
        '
        'lblNew
        '
        Me.lblNew.AutoSize = True
        Me.lblNew.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblNew.Location = New System.Drawing.Point(309, 95)
        Me.lblNew.Name = "lblNew"
        Me.lblNew.Size = New System.Drawing.Size(30, 13)
        Me.lblNew.TabIndex = 19
        Me.lblNew.Text = "New"
        '
        'nudMELevel
        '
        Me.nudMELevel.Location = New System.Drawing.Point(281, 117)
        Me.nudMELevel.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudMELevel.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.nudMELevel.Name = "nudMELevel"
        Me.nudMELevel.Size = New System.Drawing.Size(87, 21)
        Me.nudMELevel.TabIndex = 20
        Me.nudMELevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudPELevel
        '
        Me.nudPELevel.Location = New System.Drawing.Point(281, 144)
        Me.nudPELevel.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudPELevel.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.nudPELevel.Name = "nudPELevel"
        Me.nudPELevel.Size = New System.Drawing.Size(87, 21)
        Me.nudPELevel.TabIndex = 21
        Me.nudPELevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudRuns
        '
        Me.nudRuns.Location = New System.Drawing.Point(281, 171)
        Me.nudRuns.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudRuns.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
        Me.nudRuns.Name = "nudRuns"
        Me.nudRuns.Size = New System.Drawing.Size(87, 21)
        Me.nudRuns.TabIndex = 22
        Me.nudRuns.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'cboStatus
        '
        Me.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStatus.FormattingEnabled = True
        Me.cboStatus.Location = New System.Drawing.Point(281, 206)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.Size = New System.Drawing.Size(87, 21)
        Me.cboStatus.TabIndex = 23
        '
        'lblCurrentME
        '
        Me.lblCurrentME.Location = New System.Drawing.Point(136, 119)
        Me.lblCurrentME.Name = "lblCurrentME"
        Me.lblCurrentME.Size = New System.Drawing.Size(73, 13)
        Me.lblCurrentME.TabIndex = 24
        Me.lblCurrentME.Text = "0"
        Me.lblCurrentME.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCurrentPE
        '
        Me.lblCurrentPE.Location = New System.Drawing.Point(133, 146)
        Me.lblCurrentPE.Name = "lblCurrentPE"
        Me.lblCurrentPE.Size = New System.Drawing.Size(76, 13)
        Me.lblCurrentPE.TabIndex = 25
        Me.lblCurrentPE.Text = "0"
        Me.lblCurrentPE.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCurrentRuns
        '
        Me.lblCurrentRuns.Location = New System.Drawing.Point(130, 173)
        Me.lblCurrentRuns.Name = "lblCurrentRuns"
        Me.lblCurrentRuns.Size = New System.Drawing.Size(79, 13)
        Me.lblCurrentRuns.TabIndex = 26
        Me.lblCurrentRuns.Text = "0"
        Me.lblCurrentRuns.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblCurrentStatus
        '
        Me.lblCurrentStatus.Location = New System.Drawing.Point(127, 209)
        Me.lblCurrentStatus.Name = "lblCurrentStatus"
        Me.lblCurrentStatus.Size = New System.Drawing.Size(82, 13)
        Me.lblCurrentStatus.TabIndex = 27
        Me.lblCurrentStatus.Text = "0"
        Me.lblCurrentStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'btnAccept
        '
        Me.btnAccept.Location = New System.Drawing.Point(232, 274)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.TabIndex = 28
        Me.btnAccept.Text = "Accept"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(313, 274)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 29
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'pbBP
        '
        Me.pbBP.BackColor = System.Drawing.SystemColors.ButtonShadow
        Me.pbBP.Location = New System.Drawing.Point(12, 12)
        Me.pbBP.Name = "pbBP"
        Me.pbBP.Size = New System.Drawing.Size(64, 64)
        Me.pbBP.TabIndex = 11
        Me.pbBP.TabStop = False
        '
        'frmEditBPDetails
        '
        Me.AcceptButton = Me.btnAccept
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(400, 308)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.lblCurrentStatus)
        Me.Controls.Add(Me.lblCurrentRuns)
        Me.Controls.Add(Me.lblCurrentPE)
        Me.Controls.Add(Me.lblCurrentME)
        Me.Controls.Add(Me.cboStatus)
        Me.Controls.Add(Me.nudRuns)
        Me.Controls.Add(Me.nudPELevel)
        Me.Controls.Add(Me.nudMELevel)
        Me.Controls.Add(Me.lblNew)
        Me.Controls.Add(Me.lblCurrent)
        Me.Controls.Add(Me.lblPELevel)
        Me.Controls.Add(Me.lblRuns)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.lblMELevel)
        Me.Controls.Add(Me.lblAssetID)
        Me.Controls.Add(Me.lblBPName)
        Me.Controls.Add(Me.pbBP)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmEditBPDetails"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Edit Blueprint Details"
        CType(Me.nudMELevel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPELevel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudRuns, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbBP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pbBP As System.Windows.Forms.PictureBox
    Friend WithEvents lblBPName As System.Windows.Forms.Label
    Friend WithEvents lblAssetID As System.Windows.Forms.Label
    Friend WithEvents lblMELevel As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents lblRuns As System.Windows.Forms.Label
    Friend WithEvents lblPELevel As System.Windows.Forms.Label
    Friend WithEvents lblCurrent As System.Windows.Forms.Label
    Friend WithEvents lblNew As System.Windows.Forms.Label
    Friend WithEvents nudMELevel As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudPELevel As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudRuns As System.Windows.Forms.NumericUpDown
    Friend WithEvents cboStatus As System.Windows.Forms.ComboBox
    Friend WithEvents lblCurrentME As System.Windows.Forms.Label
    Friend WithEvents lblCurrentPE As System.Windows.Forms.Label
    Friend WithEvents lblCurrentRuns As System.Windows.Forms.Label
    Friend WithEvents lblCurrentStatus As System.Windows.Forms.Label
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
End Class
