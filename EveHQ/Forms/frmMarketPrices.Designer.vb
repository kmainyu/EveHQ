<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMarketPrices
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMarketPrices))
        Me.ofd1 = New System.Windows.Forms.OpenFileDialog
        Me.Button1 = New System.Windows.Forms.Button
        Me.nudIgnoreBuyOrderLimit = New System.Windows.Forms.NumericUpDown
        Me.nudIgnoreSellOrderLimit = New System.Windows.Forms.NumericUpDown
        Me.lblIgnoreBuyOrderUnit = New System.Windows.Forms.Label
        Me.lblIgnoreSellOrderUnit = New System.Windows.Forms.Label
        Me.chkIgnoreBuyOrders = New System.Windows.Forms.CheckBox
        Me.chkIgnoreSellOrders = New System.Windows.Forms.CheckBox
        Me.Button2 = New System.Windows.Forms.Button
        Me.lblProgress = New System.Windows.Forms.Label
        Me.lblDecompress = New System.Windows.Forms.Label
        Me.grpRegions = New System.Windows.Forms.GroupBox
        Me.btnAllRegions = New System.Windows.Forms.Button
        Me.btnEmpireRegions = New System.Windows.Forms.Button
        Me.btnNullRegions = New System.Windows.Forms.Button
        Me.btnNoRegions = New System.Windows.Forms.Button
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.tabDumps = New System.Windows.Forms.TabPage
        Me.tabPrices = New System.Windows.Forms.TabPage
        Me.panelECDumps = New System.Windows.Forms.Panel
        Me.panelPrices = New System.Windows.Forms.Panel
        Me.btnAmarr = New System.Windows.Forms.Button
        Me.btnCaldari = New System.Windows.Forms.Button
        Me.btnGallente = New System.Windows.Forms.Button
        Me.btnMinmatar = New System.Windows.Forms.Button
        CType(Me.nudIgnoreBuyOrderLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudIgnoreSellOrderLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabControl1.SuspendLayout()
        Me.tabDumps.SuspendLayout()
        Me.tabPrices.SuspendLayout()
        Me.panelECDumps.SuspendLayout()
        Me.panelPrices.SuspendLayout()
        Me.SuspendLayout()
        '
        'ofd1
        '
        Me.ofd1.FileName = "OpenFileDialog1"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(14, 69)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'nudIgnoreBuyOrderLimit
        '
        Me.nudIgnoreBuyOrderLimit.DecimalPlaces = 2
        Me.nudIgnoreBuyOrderLimit.Location = New System.Drawing.Point(187, 19)
        Me.nudIgnoreBuyOrderLimit.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudIgnoreBuyOrderLimit.Name = "nudIgnoreBuyOrderLimit"
        Me.nudIgnoreBuyOrderLimit.Size = New System.Drawing.Size(74, 20)
        Me.nudIgnoreBuyOrderLimit.TabIndex = 2
        Me.nudIgnoreBuyOrderLimit.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'nudIgnoreSellOrderLimit
        '
        Me.nudIgnoreSellOrderLimit.DecimalPlaces = 2
        Me.nudIgnoreSellOrderLimit.Location = New System.Drawing.Point(187, 45)
        Me.nudIgnoreSellOrderLimit.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudIgnoreSellOrderLimit.Name = "nudIgnoreSellOrderLimit"
        Me.nudIgnoreSellOrderLimit.Size = New System.Drawing.Size(74, 20)
        Me.nudIgnoreSellOrderLimit.TabIndex = 4
        Me.nudIgnoreSellOrderLimit.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'lblIgnoreBuyOrderUnit
        '
        Me.lblIgnoreBuyOrderUnit.AutoSize = True
        Me.lblIgnoreBuyOrderUnit.Location = New System.Drawing.Point(267, 21)
        Me.lblIgnoreBuyOrderUnit.Name = "lblIgnoreBuyOrderUnit"
        Me.lblIgnoreBuyOrderUnit.Size = New System.Drawing.Size(24, 13)
        Me.lblIgnoreBuyOrderUnit.TabIndex = 5
        Me.lblIgnoreBuyOrderUnit.Text = "ISK"
        '
        'lblIgnoreSellOrderUnit
        '
        Me.lblIgnoreSellOrderUnit.AutoSize = True
        Me.lblIgnoreSellOrderUnit.Location = New System.Drawing.Point(267, 47)
        Me.lblIgnoreSellOrderUnit.Name = "lblIgnoreSellOrderUnit"
        Me.lblIgnoreSellOrderUnit.Size = New System.Drawing.Size(39, 13)
        Me.lblIgnoreSellOrderUnit.TabIndex = 6
        Me.lblIgnoreSellOrderUnit.Text = "x Base"
        '
        'chkIgnoreBuyOrders
        '
        Me.chkIgnoreBuyOrders.AutoSize = True
        Me.chkIgnoreBuyOrders.Checked = True
        Me.chkIgnoreBuyOrders.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIgnoreBuyOrders.Location = New System.Drawing.Point(14, 20)
        Me.chkIgnoreBuyOrders.Name = "chkIgnoreBuyOrders"
        Me.chkIgnoreBuyOrders.Size = New System.Drawing.Size(167, 17)
        Me.chkIgnoreBuyOrders.TabIndex = 7
        Me.chkIgnoreBuyOrders.Text = "Ignore Buy Orders Less Than:"
        Me.chkIgnoreBuyOrders.UseVisualStyleBackColor = True
        '
        'chkIgnoreSellOrders
        '
        Me.chkIgnoreSellOrders.AutoSize = True
        Me.chkIgnoreSellOrders.Checked = True
        Me.chkIgnoreSellOrders.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIgnoreSellOrders.Location = New System.Drawing.Point(14, 46)
        Me.chkIgnoreSellOrders.Name = "chkIgnoreSellOrders"
        Me.chkIgnoreSellOrders.Size = New System.Drawing.Size(168, 17)
        Me.chkIgnoreSellOrders.TabIndex = 8
        Me.chkIgnoreSellOrders.Text = "Ignore Sell Orders More Than:"
        Me.chkIgnoreSellOrders.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(13, 106)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 9
        Me.Button2.Text = "Button2"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'lblProgress
        '
        Me.lblProgress.AutoSize = True
        Me.lblProgress.Location = New System.Drawing.Point(100, 88)
        Me.lblProgress.Name = "lblProgress"
        Me.lblProgress.Size = New System.Drawing.Size(81, 13)
        Me.lblProgress.TabIndex = 10
        Me.lblProgress.Text = "0 bytes (0 kb/s)"
        '
        'lblDecompress
        '
        Me.lblDecompress.AutoSize = True
        Me.lblDecompress.Location = New System.Drawing.Point(100, 116)
        Me.lblDecompress.Name = "lblDecompress"
        Me.lblDecompress.Size = New System.Drawing.Size(81, 13)
        Me.lblDecompress.TabIndex = 11
        Me.lblDecompress.Text = "0 bytes (0 kb/s)"
        '
        'grpRegions
        '
        Me.grpRegions.Location = New System.Drawing.Point(14, 13)
        Me.grpRegions.Name = "grpRegions"
        Me.grpRegions.Size = New System.Drawing.Size(610, 400)
        Me.grpRegions.TabIndex = 12
        Me.grpRegions.TabStop = False
        Me.grpRegions.Text = "Regions"
        '
        'btnAllRegions
        '
        Me.btnAllRegions.Location = New System.Drawing.Point(14, 419)
        Me.btnAllRegions.Name = "btnAllRegions"
        Me.btnAllRegions.Size = New System.Drawing.Size(70, 23)
        Me.btnAllRegions.TabIndex = 13
        Me.btnAllRegions.Text = "All Regions"
        Me.btnAllRegions.UseVisualStyleBackColor = True
        '
        'btnEmpireRegions
        '
        Me.btnEmpireRegions.Location = New System.Drawing.Point(90, 419)
        Me.btnEmpireRegions.Name = "btnEmpireRegions"
        Me.btnEmpireRegions.Size = New System.Drawing.Size(70, 23)
        Me.btnEmpireRegions.TabIndex = 14
        Me.btnEmpireRegions.Text = "Empire"
        Me.btnEmpireRegions.UseVisualStyleBackColor = True
        '
        'btnNullRegions
        '
        Me.btnNullRegions.Location = New System.Drawing.Point(166, 419)
        Me.btnNullRegions.Name = "btnNullRegions"
        Me.btnNullRegions.Size = New System.Drawing.Size(70, 23)
        Me.btnNullRegions.TabIndex = 15
        Me.btnNullRegions.Text = "0.0"
        Me.btnNullRegions.UseVisualStyleBackColor = True
        '
        'btnNoRegions
        '
        Me.btnNoRegions.Location = New System.Drawing.Point(242, 419)
        Me.btnNoRegions.Name = "btnNoRegions"
        Me.btnNoRegions.Size = New System.Drawing.Size(75, 23)
        Me.btnNoRegions.TabIndex = 16
        Me.btnNoRegions.Text = "No Regions"
        Me.btnNoRegions.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tabDumps)
        Me.TabControl1.Controls.Add(Me.tabPrices)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(789, 661)
        Me.TabControl1.TabIndex = 17
        '
        'tabDumps
        '
        Me.tabDumps.Controls.Add(Me.panelECDumps)
        Me.tabDumps.Location = New System.Drawing.Point(4, 22)
        Me.tabDumps.Name = "tabDumps"
        Me.tabDumps.Padding = New System.Windows.Forms.Padding(3)
        Me.tabDumps.Size = New System.Drawing.Size(781, 635)
        Me.tabDumps.TabIndex = 0
        Me.tabDumps.Text = "EC Market Dumps"
        Me.tabDumps.UseVisualStyleBackColor = True
        '
        'tabPrices
        '
        Me.tabPrices.Controls.Add(Me.panelPrices)
        Me.tabPrices.Location = New System.Drawing.Point(4, 22)
        Me.tabPrices.Name = "tabPrices"
        Me.tabPrices.Padding = New System.Windows.Forms.Padding(3)
        Me.tabPrices.Size = New System.Drawing.Size(781, 635)
        Me.tabPrices.TabIndex = 1
        Me.tabPrices.Text = "Price Selection"
        Me.tabPrices.UseVisualStyleBackColor = True
        '
        'panelECDumps
        '
        Me.panelECDumps.BackColor = System.Drawing.SystemColors.Control
        Me.panelECDumps.Controls.Add(Me.chkIgnoreBuyOrders)
        Me.panelECDumps.Controls.Add(Me.lblDecompress)
        Me.panelECDumps.Controls.Add(Me.Button1)
        Me.panelECDumps.Controls.Add(Me.lblProgress)
        Me.panelECDumps.Controls.Add(Me.nudIgnoreBuyOrderLimit)
        Me.panelECDumps.Controls.Add(Me.Button2)
        Me.panelECDumps.Controls.Add(Me.nudIgnoreSellOrderLimit)
        Me.panelECDumps.Controls.Add(Me.chkIgnoreSellOrders)
        Me.panelECDumps.Controls.Add(Me.lblIgnoreBuyOrderUnit)
        Me.panelECDumps.Controls.Add(Me.lblIgnoreSellOrderUnit)
        Me.panelECDumps.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelECDumps.Location = New System.Drawing.Point(3, 3)
        Me.panelECDumps.Name = "panelECDumps"
        Me.panelECDumps.Size = New System.Drawing.Size(775, 629)
        Me.panelECDumps.TabIndex = 0
        '
        'panelPrices
        '
        Me.panelPrices.BackColor = System.Drawing.SystemColors.Control
        Me.panelPrices.Controls.Add(Me.btnMinmatar)
        Me.panelPrices.Controls.Add(Me.btnGallente)
        Me.panelPrices.Controls.Add(Me.btnCaldari)
        Me.panelPrices.Controls.Add(Me.btnAmarr)
        Me.panelPrices.Controls.Add(Me.grpRegions)
        Me.panelPrices.Controls.Add(Me.btnNoRegions)
        Me.panelPrices.Controls.Add(Me.btnAllRegions)
        Me.panelPrices.Controls.Add(Me.btnNullRegions)
        Me.panelPrices.Controls.Add(Me.btnEmpireRegions)
        Me.panelPrices.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelPrices.Location = New System.Drawing.Point(3, 3)
        Me.panelPrices.Name = "panelPrices"
        Me.panelPrices.Size = New System.Drawing.Size(775, 629)
        Me.panelPrices.TabIndex = 0
        '
        'btnAmarr
        '
        Me.btnAmarr.Location = New System.Drawing.Point(323, 419)
        Me.btnAmarr.Name = "btnAmarr"
        Me.btnAmarr.Size = New System.Drawing.Size(70, 23)
        Me.btnAmarr.TabIndex = 17
        Me.btnAmarr.Text = "Amarr"
        Me.btnAmarr.UseVisualStyleBackColor = True
        '
        'btnCaldari
        '
        Me.btnCaldari.Location = New System.Drawing.Point(399, 419)
        Me.btnCaldari.Name = "btnCaldari"
        Me.btnCaldari.Size = New System.Drawing.Size(70, 23)
        Me.btnCaldari.TabIndex = 18
        Me.btnCaldari.Text = "Caldari"
        Me.btnCaldari.UseVisualStyleBackColor = True
        '
        'btnGallente
        '
        Me.btnGallente.Location = New System.Drawing.Point(475, 419)
        Me.btnGallente.Name = "btnGallente"
        Me.btnGallente.Size = New System.Drawing.Size(70, 23)
        Me.btnGallente.TabIndex = 19
        Me.btnGallente.Text = "Gallente"
        Me.btnGallente.UseVisualStyleBackColor = True
        '
        'btnMinmatar
        '
        Me.btnMinmatar.Location = New System.Drawing.Point(551, 419)
        Me.btnMinmatar.Name = "btnMinmatar"
        Me.btnMinmatar.Size = New System.Drawing.Size(70, 23)
        Me.btnMinmatar.TabIndex = 20
        Me.btnMinmatar.Text = "Minmatar"
        Me.btnMinmatar.UseVisualStyleBackColor = True
        '
        'frmMarketPrices
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(789, 661)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMarketPrices"
        Me.Text = "Market Prices"
        CType(Me.nudIgnoreBuyOrderLimit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudIgnoreSellOrderLimit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabControl1.ResumeLayout(False)
        Me.tabDumps.ResumeLayout(False)
        Me.tabPrices.ResumeLayout(False)
        Me.panelECDumps.ResumeLayout(False)
        Me.panelECDumps.PerformLayout()
        Me.panelPrices.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ofd1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents nudIgnoreBuyOrderLimit As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudIgnoreSellOrderLimit As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblIgnoreBuyOrderUnit As System.Windows.Forms.Label
    Friend WithEvents lblIgnoreSellOrderUnit As System.Windows.Forms.Label
    Friend WithEvents chkIgnoreBuyOrders As System.Windows.Forms.CheckBox
    Friend WithEvents chkIgnoreSellOrders As System.Windows.Forms.CheckBox
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents lblProgress As System.Windows.Forms.Label
    Friend WithEvents lblDecompress As System.Windows.Forms.Label
    Friend WithEvents grpRegions As System.Windows.Forms.GroupBox
    Friend WithEvents btnAllRegions As System.Windows.Forms.Button
    Friend WithEvents btnEmpireRegions As System.Windows.Forms.Button
    Friend WithEvents btnNullRegions As System.Windows.Forms.Button
    Friend WithEvents btnNoRegions As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tabDumps As System.Windows.Forms.TabPage
    Friend WithEvents panelECDumps As System.Windows.Forms.Panel
    Friend WithEvents tabPrices As System.Windows.Forms.TabPage
    Friend WithEvents panelPrices As System.Windows.Forms.Panel
    Friend WithEvents btnMinmatar As System.Windows.Forms.Button
    Friend WithEvents btnGallente As System.Windows.Forms.Button
    Friend WithEvents btnCaldari As System.Windows.Forms.Button
    Friend WithEvents btnAmarr As System.Windows.Forms.Button
End Class
