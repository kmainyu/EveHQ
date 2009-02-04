<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmModifyPrice
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmModifyPrice))
        Me.lblCurrentBasePrice = New System.Windows.Forms.Label
        Me.lblCurrentMarketPrice = New System.Windows.Forms.Label
        Me.btnAccept = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.txtNewPrice = New System.Windows.Forms.TextBox
        Me.lblNewPrice = New System.Windows.Forms.Label
        Me.lblCurrentCustomPrice = New System.Windows.Forms.Label
        Me.lblCustomPrice = New System.Windows.Forms.Label
        Me.lblMarketPrice = New System.Windows.Forms.Label
        Me.lblBasePrice = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lblCurrentBasePrice
        '
        Me.lblCurrentBasePrice.AutoSize = True
        Me.lblCurrentBasePrice.Location = New System.Drawing.Point(13, 13)
        Me.lblCurrentBasePrice.Name = "lblCurrentBasePrice"
        Me.lblCurrentBasePrice.Size = New System.Drawing.Size(98, 13)
        Me.lblCurrentBasePrice.TabIndex = 0
        Me.lblCurrentBasePrice.Text = "Current Base Price:"
        '
        'lblCurrentMarketPrice
        '
        Me.lblCurrentMarketPrice.AutoSize = True
        Me.lblCurrentMarketPrice.Location = New System.Drawing.Point(13, 39)
        Me.lblCurrentMarketPrice.Name = "lblCurrentMarketPrice"
        Me.lblCurrentMarketPrice.Size = New System.Drawing.Size(107, 13)
        Me.lblCurrentMarketPrice.TabIndex = 4
        Me.lblCurrentMarketPrice.Text = "Current Market Price:"
        '
        'btnAccept
        '
        Me.btnAccept.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAccept.Location = New System.Drawing.Point(272, 138)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.TabIndex = 4
        Me.btnAccept.Text = "Accept"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(353, 138)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'txtNewPrice
        '
        Me.txtNewPrice.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtNewPrice.Location = New System.Drawing.Point(141, 103)
        Me.txtNewPrice.Name = "txtNewPrice"
        Me.txtNewPrice.Size = New System.Drawing.Size(287, 20)
        Me.txtNewPrice.TabIndex = 2
        '
        'lblNewPrice
        '
        Me.lblNewPrice.AutoSize = True
        Me.lblNewPrice.Location = New System.Drawing.Point(13, 106)
        Me.lblNewPrice.Name = "lblNewPrice"
        Me.lblNewPrice.Size = New System.Drawing.Size(97, 13)
        Me.lblNewPrice.TabIndex = 16
        Me.lblNewPrice.Text = "New Custom Price:"
        '
        'lblCurrentCustomPrice
        '
        Me.lblCurrentCustomPrice.AutoSize = True
        Me.lblCurrentCustomPrice.Location = New System.Drawing.Point(13, 65)
        Me.lblCurrentCustomPrice.Name = "lblCurrentCustomPrice"
        Me.lblCurrentCustomPrice.Size = New System.Drawing.Size(109, 13)
        Me.lblCurrentCustomPrice.TabIndex = 19
        Me.lblCurrentCustomPrice.Text = "Current Custom Price:"
        '
        'lblCustomPrice
        '
        Me.lblCustomPrice.AutoSize = True
        Me.lblCustomPrice.Location = New System.Drawing.Point(138, 65)
        Me.lblCustomPrice.Name = "lblCustomPrice"
        Me.lblCustomPrice.Size = New System.Drawing.Size(28, 13)
        Me.lblCustomPrice.TabIndex = 22
        Me.lblCustomPrice.Text = "0.00"
        '
        'lblMarketPrice
        '
        Me.lblMarketPrice.AutoSize = True
        Me.lblMarketPrice.Location = New System.Drawing.Point(138, 39)
        Me.lblMarketPrice.Name = "lblMarketPrice"
        Me.lblMarketPrice.Size = New System.Drawing.Size(28, 13)
        Me.lblMarketPrice.TabIndex = 21
        Me.lblMarketPrice.Text = "0.00"
        '
        'lblBasePrice
        '
        Me.lblBasePrice.AutoSize = True
        Me.lblBasePrice.Location = New System.Drawing.Point(138, 13)
        Me.lblBasePrice.Name = "lblBasePrice"
        Me.lblBasePrice.Size = New System.Drawing.Size(28, 13)
        Me.lblBasePrice.TabIndex = 20
        Me.lblBasePrice.Text = "0.00"
        '
        'frmModifyPrice
        '
        Me.AcceptButton = Me.btnAccept
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(440, 172)
        Me.Controls.Add(Me.lblCustomPrice)
        Me.Controls.Add(Me.lblMarketPrice)
        Me.Controls.Add(Me.lblBasePrice)
        Me.Controls.Add(Me.lblCurrentCustomPrice)
        Me.Controls.Add(Me.txtNewPrice)
        Me.Controls.Add(Me.lblNewPrice)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.lblCurrentMarketPrice)
        Me.Controls.Add(Me.lblCurrentBasePrice)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmModifyPrice"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Modify Price"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblCurrentBasePrice As System.Windows.Forms.Label
    Friend WithEvents lblCurrentMarketPrice As System.Windows.Forms.Label
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents txtNewPrice As System.Windows.Forms.TextBox
    Friend WithEvents lblNewPrice As System.Windows.Forms.Label
    Friend WithEvents lblCurrentCustomPrice As System.Windows.Forms.Label
    Friend WithEvents lblCustomPrice As System.Windows.Forms.Label
    Friend WithEvents lblMarketPrice As System.Windows.Forms.Label
    Friend WithEvents lblBasePrice As System.Windows.Forms.Label
End Class
