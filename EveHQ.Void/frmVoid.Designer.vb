<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmVoid
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmVoid))
        Me.gbWHInfo = New System.Windows.Forms.GroupBox
        Me.lblMaxJumpableMass = New System.Windows.Forms.Label
        Me.lblMaxJumpableMassLbl = New System.Windows.Forms.Label
        Me.lblMaxTotalMass = New System.Windows.Forms.Label
        Me.lblMaxTotalMassLbl = New System.Windows.Forms.Label
        Me.lblStabilityWindow = New System.Windows.Forms.Label
        Me.lblStabilityWindowLbl = New System.Windows.Forms.Label
        Me.lblTargetSystemClass = New System.Windows.Forms.Label
        Me.lblTargetSystemClassLbl = New System.Windows.Forms.Label
        Me.cboWHType = New System.Windows.Forms.ComboBox
        Me.lblWHType = New System.Windows.Forms.Label
        Me.gbWHSystemInfo = New System.Windows.Forms.GroupBox
        Me.lblAnomalyName = New System.Windows.Forms.Label
        Me.lblAnomalyNameLbl = New System.Windows.Forms.Label
        Me.lblSystemClass = New System.Windows.Forms.Label
        Me.lblSystemClassLbl = New System.Windows.Forms.Label
        Me.cboWHSystem = New System.Windows.Forms.ComboBox
        Me.lblLocus = New System.Windows.Forms.Label
        Me.lvwEffects = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.gbWHInfo.SuspendLayout()
        Me.gbWHSystemInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbWHInfo
        '
        Me.gbWHInfo.Controls.Add(Me.lblMaxJumpableMass)
        Me.gbWHInfo.Controls.Add(Me.lblMaxJumpableMassLbl)
        Me.gbWHInfo.Controls.Add(Me.lblMaxTotalMass)
        Me.gbWHInfo.Controls.Add(Me.lblMaxTotalMassLbl)
        Me.gbWHInfo.Controls.Add(Me.lblStabilityWindow)
        Me.gbWHInfo.Controls.Add(Me.lblStabilityWindowLbl)
        Me.gbWHInfo.Controls.Add(Me.lblTargetSystemClass)
        Me.gbWHInfo.Controls.Add(Me.lblTargetSystemClassLbl)
        Me.gbWHInfo.Controls.Add(Me.cboWHType)
        Me.gbWHInfo.Controls.Add(Me.lblWHType)
        Me.gbWHInfo.Location = New System.Drawing.Point(12, 12)
        Me.gbWHInfo.Name = "gbWHInfo"
        Me.gbWHInfo.Size = New System.Drawing.Size(354, 185)
        Me.gbWHInfo.TabIndex = 0
        Me.gbWHInfo.TabStop = False
        Me.gbWHInfo.Text = "Wormhole Information"
        '
        'lblMaxJumpableMass
        '
        Me.lblMaxJumpableMass.AutoSize = True
        Me.lblMaxJumpableMass.Location = New System.Drawing.Point(153, 151)
        Me.lblMaxJumpableMass.Name = "lblMaxJumpableMass"
        Me.lblMaxJumpableMass.Size = New System.Drawing.Size(23, 13)
        Me.lblMaxJumpableMass.TabIndex = 9
        Me.lblMaxJumpableMass.Text = "n/a"
        '
        'lblMaxJumpableMassLbl
        '
        Me.lblMaxJumpableMassLbl.AutoSize = True
        Me.lblMaxJumpableMassLbl.Location = New System.Drawing.Point(17, 151)
        Me.lblMaxJumpableMassLbl.Name = "lblMaxJumpableMassLbl"
        Me.lblMaxJumpableMassLbl.Size = New System.Drawing.Size(130, 13)
        Me.lblMaxJumpableMassLbl.TabIndex = 8
        Me.lblMaxJumpableMassLbl.Text = "Maximum Jumpable Mass:"
        '
        'lblMaxTotalMass
        '
        Me.lblMaxTotalMass.AutoSize = True
        Me.lblMaxTotalMass.Location = New System.Drawing.Point(153, 122)
        Me.lblMaxTotalMass.Name = "lblMaxTotalMass"
        Me.lblMaxTotalMass.Size = New System.Drawing.Size(23, 13)
        Me.lblMaxTotalMass.TabIndex = 7
        Me.lblMaxTotalMass.Text = "n/a"
        '
        'lblMaxTotalMassLbl
        '
        Me.lblMaxTotalMassLbl.AutoSize = True
        Me.lblMaxTotalMassLbl.Location = New System.Drawing.Point(17, 122)
        Me.lblMaxTotalMassLbl.Name = "lblMaxTotalMassLbl"
        Me.lblMaxTotalMassLbl.Size = New System.Drawing.Size(109, 13)
        Me.lblMaxTotalMassLbl.TabIndex = 6
        Me.lblMaxTotalMassLbl.Text = "Maximum Total Mass:"
        '
        'lblStabilityWindow
        '
        Me.lblStabilityWindow.AutoSize = True
        Me.lblStabilityWindow.Location = New System.Drawing.Point(153, 93)
        Me.lblStabilityWindow.Name = "lblStabilityWindow"
        Me.lblStabilityWindow.Size = New System.Drawing.Size(23, 13)
        Me.lblStabilityWindow.TabIndex = 5
        Me.lblStabilityWindow.Text = "n/a"
        '
        'lblStabilityWindowLbl
        '
        Me.lblStabilityWindowLbl.AutoSize = True
        Me.lblStabilityWindowLbl.Location = New System.Drawing.Point(17, 93)
        Me.lblStabilityWindowLbl.Name = "lblStabilityWindowLbl"
        Me.lblStabilityWindowLbl.Size = New System.Drawing.Size(87, 13)
        Me.lblStabilityWindowLbl.TabIndex = 4
        Me.lblStabilityWindowLbl.Text = "Life Expectancy:"
        '
        'lblTargetSystemClass
        '
        Me.lblTargetSystemClass.AutoSize = True
        Me.lblTargetSystemClass.Location = New System.Drawing.Point(153, 64)
        Me.lblTargetSystemClass.Name = "lblTargetSystemClass"
        Me.lblTargetSystemClass.Size = New System.Drawing.Size(23, 13)
        Me.lblTargetSystemClass.TabIndex = 3
        Me.lblTargetSystemClass.Text = "n/a"
        '
        'lblTargetSystemClassLbl
        '
        Me.lblTargetSystemClassLbl.AutoSize = True
        Me.lblTargetSystemClassLbl.Location = New System.Drawing.Point(17, 64)
        Me.lblTargetSystemClassLbl.Name = "lblTargetSystemClassLbl"
        Me.lblTargetSystemClassLbl.Size = New System.Drawing.Size(109, 13)
        Me.lblTargetSystemClassLbl.TabIndex = 2
        Me.lblTargetSystemClassLbl.Text = "Target System Class:"
        '
        'cboWHType
        '
        Me.cboWHType.FormattingEnabled = True
        Me.cboWHType.Location = New System.Drawing.Point(106, 30)
        Me.cboWHType.Name = "cboWHType"
        Me.cboWHType.Size = New System.Drawing.Size(151, 21)
        Me.cboWHType.Sorted = True
        Me.cboWHType.TabIndex = 1
        '
        'lblWHType
        '
        Me.lblWHType.AutoSize = True
        Me.lblWHType.Location = New System.Drawing.Point(17, 33)
        Me.lblWHType.Name = "lblWHType"
        Me.lblWHType.Size = New System.Drawing.Size(82, 13)
        Me.lblWHType.TabIndex = 0
        Me.lblWHType.Text = "Wormhole Type"
        '
        'gbWHSystemInfo
        '
        Me.gbWHSystemInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.gbWHSystemInfo.Controls.Add(Me.lvwEffects)
        Me.gbWHSystemInfo.Controls.Add(Me.lblAnomalyName)
        Me.gbWHSystemInfo.Controls.Add(Me.lblAnomalyNameLbl)
        Me.gbWHSystemInfo.Controls.Add(Me.lblSystemClass)
        Me.gbWHSystemInfo.Controls.Add(Me.lblSystemClassLbl)
        Me.gbWHSystemInfo.Controls.Add(Me.cboWHSystem)
        Me.gbWHSystemInfo.Controls.Add(Me.lblLocus)
        Me.gbWHSystemInfo.Location = New System.Drawing.Point(12, 224)
        Me.gbWHSystemInfo.Name = "gbWHSystemInfo"
        Me.gbWHSystemInfo.Size = New System.Drawing.Size(354, 381)
        Me.gbWHSystemInfo.TabIndex = 1
        Me.gbWHSystemInfo.TabStop = False
        Me.gbWHSystemInfo.Text = "Wormhole System Information"
        '
        'lblAnomalyName
        '
        Me.lblAnomalyName.AutoSize = True
        Me.lblAnomalyName.Location = New System.Drawing.Point(153, 93)
        Me.lblAnomalyName.Name = "lblAnomalyName"
        Me.lblAnomalyName.Size = New System.Drawing.Size(23, 13)
        Me.lblAnomalyName.TabIndex = 5
        Me.lblAnomalyName.Text = "n/a"
        '
        'lblAnomalyNameLbl
        '
        Me.lblAnomalyNameLbl.AutoSize = True
        Me.lblAnomalyNameLbl.Location = New System.Drawing.Point(17, 93)
        Me.lblAnomalyNameLbl.Name = "lblAnomalyNameLbl"
        Me.lblAnomalyNameLbl.Size = New System.Drawing.Size(82, 13)
        Me.lblAnomalyNameLbl.TabIndex = 4
        Me.lblAnomalyNameLbl.Text = "Anomaly Name:"
        '
        'lblSystemClass
        '
        Me.lblSystemClass.AutoSize = True
        Me.lblSystemClass.Location = New System.Drawing.Point(153, 64)
        Me.lblSystemClass.Name = "lblSystemClass"
        Me.lblSystemClass.Size = New System.Drawing.Size(23, 13)
        Me.lblSystemClass.TabIndex = 3
        Me.lblSystemClass.Text = "n/a"
        '
        'lblSystemClassLbl
        '
        Me.lblSystemClassLbl.AutoSize = True
        Me.lblSystemClassLbl.Location = New System.Drawing.Point(17, 64)
        Me.lblSystemClassLbl.Name = "lblSystemClassLbl"
        Me.lblSystemClassLbl.Size = New System.Drawing.Size(109, 13)
        Me.lblSystemClassLbl.TabIndex = 2
        Me.lblSystemClassLbl.Text = "Target System Class:"
        '
        'cboWHSystem
        '
        Me.cboWHSystem.FormattingEnabled = True
        Me.cboWHSystem.Location = New System.Drawing.Point(106, 30)
        Me.cboWHSystem.Name = "cboWHSystem"
        Me.cboWHSystem.Size = New System.Drawing.Size(151, 21)
        Me.cboWHSystem.Sorted = True
        Me.cboWHSystem.TabIndex = 1
        '
        'lblLocus
        '
        Me.lblLocus.AutoSize = True
        Me.lblLocus.Location = New System.Drawing.Point(17, 33)
        Me.lblLocus.Name = "lblLocus"
        Me.lblLocus.Size = New System.Drawing.Size(83, 13)
        Me.lblLocus.TabIndex = 0
        Me.lblLocus.Text = "Locus Signature"
        '
        'lvwEffects
        '
        Me.lvwEffects.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lvwEffects.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.lvwEffects.FullRowSelect = True
        Me.lvwEffects.GridLines = True
        Me.lvwEffects.Location = New System.Drawing.Point(6, 118)
        Me.lvwEffects.Name = "lvwEffects"
        Me.lvwEffects.Size = New System.Drawing.Size(342, 257)
        Me.lvwEffects.TabIndex = 6
        Me.lvwEffects.UseCompatibleStateImageBehavior = False
        Me.lvwEffects.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Effect"
        Me.ColumnHeader1.Width = 225
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Value"
        Me.ColumnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ColumnHeader2.Width = 80
        '
        'frmVoid
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(748, 617)
        Me.Controls.Add(Me.gbWHSystemInfo)
        Me.Controls.Add(Me.gbWHInfo)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmVoid"
        Me.Text = "EveHQ Void"
        Me.gbWHInfo.ResumeLayout(False)
        Me.gbWHInfo.PerformLayout()
        Me.gbWHSystemInfo.ResumeLayout(False)
        Me.gbWHSystemInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbWHInfo As System.Windows.Forms.GroupBox
    Friend WithEvents lblTargetSystemClassLbl As System.Windows.Forms.Label
    Friend WithEvents cboWHType As System.Windows.Forms.ComboBox
    Friend WithEvents lblWHType As System.Windows.Forms.Label
    Friend WithEvents lblMaxJumpableMass As System.Windows.Forms.Label
    Friend WithEvents lblMaxJumpableMassLbl As System.Windows.Forms.Label
    Friend WithEvents lblMaxTotalMass As System.Windows.Forms.Label
    Friend WithEvents lblMaxTotalMassLbl As System.Windows.Forms.Label
    Friend WithEvents lblStabilityWindow As System.Windows.Forms.Label
    Friend WithEvents lblStabilityWindowLbl As System.Windows.Forms.Label
    Friend WithEvents lblTargetSystemClass As System.Windows.Forms.Label
    Friend WithEvents gbWHSystemInfo As System.Windows.Forms.GroupBox
    Friend WithEvents lblAnomalyName As System.Windows.Forms.Label
    Friend WithEvents lblAnomalyNameLbl As System.Windows.Forms.Label
    Friend WithEvents lblSystemClass As System.Windows.Forms.Label
    Friend WithEvents lblSystemClassLbl As System.Windows.Forms.Label
    Friend WithEvents cboWHSystem As System.Windows.Forms.ComboBox
    Friend WithEvents lblLocus As System.Windows.Forms.Label
    Friend WithEvents lvwEffects As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
End Class
