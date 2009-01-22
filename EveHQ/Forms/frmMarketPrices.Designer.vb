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
        CType(Me.nudIgnoreBuyOrderLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudIgnoreSellOrderLimit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ofd1
        '
        Me.ofd1.FileName = "OpenFileDialog1"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(16, 96)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(75, 23)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "Button1"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'nudIgnoreBuyOrderLimit
        '
        Me.nudIgnoreBuyOrderLimit.DecimalPlaces = 2
        Me.nudIgnoreBuyOrderLimit.Location = New System.Drawing.Point(188, 12)
        Me.nudIgnoreBuyOrderLimit.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudIgnoreBuyOrderLimit.Name = "nudIgnoreBuyOrderLimit"
        Me.nudIgnoreBuyOrderLimit.Size = New System.Drawing.Size(74, 20)
        Me.nudIgnoreBuyOrderLimit.TabIndex = 2
        Me.nudIgnoreBuyOrderLimit.Value = New Decimal(New Integer() {1, 0, 0, 65536})
        '
        'nudIgnoreSellOrderLimit
        '
        Me.nudIgnoreSellOrderLimit.DecimalPlaces = 2
        Me.nudIgnoreSellOrderLimit.Location = New System.Drawing.Point(188, 38)
        Me.nudIgnoreSellOrderLimit.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.nudIgnoreSellOrderLimit.Name = "nudIgnoreSellOrderLimit"
        Me.nudIgnoreSellOrderLimit.Size = New System.Drawing.Size(74, 20)
        Me.nudIgnoreSellOrderLimit.TabIndex = 4
        Me.nudIgnoreSellOrderLimit.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'lblIgnoreBuyOrderUnit
        '
        Me.lblIgnoreBuyOrderUnit.AutoSize = True
        Me.lblIgnoreBuyOrderUnit.Location = New System.Drawing.Point(268, 14)
        Me.lblIgnoreBuyOrderUnit.Name = "lblIgnoreBuyOrderUnit"
        Me.lblIgnoreBuyOrderUnit.Size = New System.Drawing.Size(24, 13)
        Me.lblIgnoreBuyOrderUnit.TabIndex = 5
        Me.lblIgnoreBuyOrderUnit.Text = "ISK"
        '
        'lblIgnoreSellOrderUnit
        '
        Me.lblIgnoreSellOrderUnit.AutoSize = True
        Me.lblIgnoreSellOrderUnit.Location = New System.Drawing.Point(268, 40)
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
        Me.chkIgnoreBuyOrders.Location = New System.Drawing.Point(15, 13)
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
        Me.chkIgnoreSellOrders.Location = New System.Drawing.Point(15, 39)
        Me.chkIgnoreSellOrders.Name = "chkIgnoreSellOrders"
        Me.chkIgnoreSellOrders.Size = New System.Drawing.Size(168, 17)
        Me.chkIgnoreSellOrders.TabIndex = 8
        Me.chkIgnoreSellOrders.Text = "Ignore Sell Orders More Than:"
        Me.chkIgnoreSellOrders.UseVisualStyleBackColor = True
        '
        'frmMarketPrices
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(867, 567)
        Me.Controls.Add(Me.chkIgnoreSellOrders)
        Me.Controls.Add(Me.chkIgnoreBuyOrders)
        Me.Controls.Add(Me.lblIgnoreSellOrderUnit)
        Me.Controls.Add(Me.lblIgnoreBuyOrderUnit)
        Me.Controls.Add(Me.nudIgnoreSellOrderLimit)
        Me.Controls.Add(Me.nudIgnoreBuyOrderLimit)
        Me.Controls.Add(Me.Button1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMarketPrices"
        Me.Text = "Market Prices"
        CType(Me.nudIgnoreBuyOrderLimit, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudIgnoreSellOrderLimit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ofd1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents nudIgnoreBuyOrderLimit As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudIgnoreSellOrderLimit As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblIgnoreBuyOrderUnit As System.Windows.Forms.Label
    Friend WithEvents lblIgnoreSellOrderUnit As System.Windows.Forms.Label
    Friend WithEvents chkIgnoreBuyOrders As System.Windows.Forms.CheckBox
    Friend WithEvents chkIgnoreSellOrders As System.Windows.Forms.CheckBox
End Class
