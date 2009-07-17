<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddInvestment
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
        Me.lblInvestmentID = New System.Windows.Forms.Label
        Me.lblDateCreated = New System.Windows.Forms.Label
        Me.txtInvestmentID = New System.Windows.Forms.TextBox
        Me.txtDateCreated = New System.Windows.Forms.TextBox
        Me.lblInvestmentName = New System.Windows.Forms.Label
        Me.txtInvestmentName = New System.Windows.Forms.TextBox
        Me.lblInvestmentOwner = New System.Windows.Forms.Label
        Me.cboOwner = New System.Windows.Forms.ComboBox
        Me.lblInvestmentDescription = New System.Windows.Forms.Label
        Me.txtDescription = New System.Windows.Forms.TextBox
        Me.btnAccept = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.cboType = New System.Windows.Forms.ComboBox
        Me.lblInvestmentType = New System.Windows.Forms.Label
        Me.chkValueIsCost = New System.Windows.Forms.CheckBox
        Me.txtDateClosed = New System.Windows.Forms.TextBox
        Me.lblDateClosed = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lblInvestmentID
        '
        Me.lblInvestmentID.AutoSize = True
        Me.lblInvestmentID.Location = New System.Drawing.Point(13, 13)
        Me.lblInvestmentID.Name = "lblInvestmentID"
        Me.lblInvestmentID.Size = New System.Drawing.Size(80, 13)
        Me.lblInvestmentID.TabIndex = 0
        Me.lblInvestmentID.Text = "Investment ID:"
        '
        'lblDateCreated
        '
        Me.lblDateCreated.AutoSize = True
        Me.lblDateCreated.Location = New System.Drawing.Point(228, 13)
        Me.lblDateCreated.Name = "lblDateCreated"
        Me.lblDateCreated.Size = New System.Drawing.Size(76, 13)
        Me.lblDateCreated.TabIndex = 1
        Me.lblDateCreated.Text = "Date Created:"
        '
        'txtInvestmentID
        '
        Me.txtInvestmentID.Enabled = False
        Me.txtInvestmentID.Location = New System.Drawing.Point(115, 10)
        Me.txtInvestmentID.Name = "txtInvestmentID"
        Me.txtInvestmentID.Size = New System.Drawing.Size(84, 21)
        Me.txtInvestmentID.TabIndex = 5
        Me.txtInvestmentID.TabStop = False
        '
        'txtDateCreated
        '
        Me.txtDateCreated.Enabled = False
        Me.txtDateCreated.Location = New System.Drawing.Point(307, 10)
        Me.txtDateCreated.Name = "txtDateCreated"
        Me.txtDateCreated.Size = New System.Drawing.Size(160, 21)
        Me.txtDateCreated.TabIndex = 6
        Me.txtDateCreated.TabStop = False
        '
        'lblInvestmentName
        '
        Me.lblInvestmentName.AutoSize = True
        Me.lblInvestmentName.Location = New System.Drawing.Point(13, 57)
        Me.lblInvestmentName.Name = "lblInvestmentName"
        Me.lblInvestmentName.Size = New System.Drawing.Size(96, 13)
        Me.lblInvestmentName.TabIndex = 4
        Me.lblInvestmentName.Text = "Investment Name:"
        '
        'txtInvestmentName
        '
        Me.txtInvestmentName.Location = New System.Drawing.Point(115, 54)
        Me.txtInvestmentName.Name = "txtInvestmentName"
        Me.txtInvestmentName.Size = New System.Drawing.Size(404, 21)
        Me.txtInvestmentName.TabIndex = 0
        '
        'lblInvestmentOwner
        '
        Me.lblInvestmentOwner.AutoSize = True
        Me.lblInvestmentOwner.Location = New System.Drawing.Point(13, 110)
        Me.lblInvestmentOwner.Name = "lblInvestmentOwner"
        Me.lblInvestmentOwner.Size = New System.Drawing.Size(101, 13)
        Me.lblInvestmentOwner.TabIndex = 6
        Me.lblInvestmentOwner.Text = "Investment Owner:"
        '
        'cboOwner
        '
        Me.cboOwner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOwner.FormattingEnabled = True
        Me.cboOwner.Location = New System.Drawing.Point(115, 107)
        Me.cboOwner.Name = "cboOwner"
        Me.cboOwner.Size = New System.Drawing.Size(185, 21)
        Me.cboOwner.Sorted = True
        Me.cboOwner.TabIndex = 2
        '
        'lblInvestmentDescription
        '
        Me.lblInvestmentDescription.AutoSize = True
        Me.lblInvestmentDescription.Location = New System.Drawing.Point(13, 142)
        Me.lblInvestmentDescription.Name = "lblInvestmentDescription"
        Me.lblInvestmentDescription.Size = New System.Drawing.Size(64, 13)
        Me.lblInvestmentDescription.TabIndex = 8
        Me.lblInvestmentDescription.Text = "Description:"
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(115, 139)
        Me.txtDescription.MaxLength = 512
        Me.txtDescription.Multiline = True
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(404, 168)
        Me.txtDescription.TabIndex = 3
        '
        'btnAccept
        '
        Me.btnAccept.Location = New System.Drawing.Point(363, 349)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.TabIndex = 5
        Me.btnAccept.Text = "Accept"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(444, 349)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'cboType
        '
        Me.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboType.FormattingEnabled = True
        Me.cboType.Items.AddRange(New Object() {"Cash", "Shares"})
        Me.cboType.Location = New System.Drawing.Point(115, 80)
        Me.cboType.Name = "cboType"
        Me.cboType.Size = New System.Drawing.Size(185, 21)
        Me.cboType.Sorted = True
        Me.cboType.TabIndex = 1
        '
        'lblInvestmentType
        '
        Me.lblInvestmentType.AutoSize = True
        Me.lblInvestmentType.Location = New System.Drawing.Point(13, 83)
        Me.lblInvestmentType.Name = "lblInvestmentType"
        Me.lblInvestmentType.Size = New System.Drawing.Size(93, 13)
        Me.lblInvestmentType.TabIndex = 10
        Me.lblInvestmentType.Text = "Investment Type:"
        '
        'chkValueIsCost
        '
        Me.chkValueIsCost.AutoSize = True
        Me.chkValueIsCost.Location = New System.Drawing.Point(115, 314)
        Me.chkValueIsCost.Name = "chkValueIsCost"
        Me.chkValueIsCost.Size = New System.Drawing.Size(145, 17)
        Me.chkValueIsCost.TabIndex = 4
        Me.chkValueIsCost.Text = "State Valuation at ""Cost"""
        Me.chkValueIsCost.UseVisualStyleBackColor = True
        '
        'txtDateClosed
        '
        Me.txtDateClosed.Enabled = False
        Me.txtDateClosed.Location = New System.Drawing.Point(307, 30)
        Me.txtDateClosed.Name = "txtDateClosed"
        Me.txtDateClosed.Size = New System.Drawing.Size(160, 21)
        Me.txtDateClosed.TabIndex = 12
        Me.txtDateClosed.TabStop = False
        '
        'lblDateClosed
        '
        Me.lblDateClosed.AutoSize = True
        Me.lblDateClosed.Location = New System.Drawing.Point(228, 31)
        Me.lblDateClosed.Name = "lblDateClosed"
        Me.lblDateClosed.Size = New System.Drawing.Size(69, 13)
        Me.lblDateClosed.TabIndex = 11
        Me.lblDateClosed.Text = "Date Closed:"
        '
        'frmAddInvestment
        '
        Me.AcceptButton = Me.btnAccept
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(531, 378)
        Me.Controls.Add(Me.txtDateClosed)
        Me.Controls.Add(Me.lblDateClosed)
        Me.Controls.Add(Me.chkValueIsCost)
        Me.Controls.Add(Me.cboType)
        Me.Controls.Add(Me.lblInvestmentType)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.txtDescription)
        Me.Controls.Add(Me.lblInvestmentDescription)
        Me.Controls.Add(Me.cboOwner)
        Me.Controls.Add(Me.lblInvestmentOwner)
        Me.Controls.Add(Me.txtInvestmentName)
        Me.Controls.Add(Me.lblInvestmentName)
        Me.Controls.Add(Me.txtDateCreated)
        Me.Controls.Add(Me.txtInvestmentID)
        Me.Controls.Add(Me.lblDateCreated)
        Me.Controls.Add(Me.lblInvestmentID)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAddInvestment"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add New Investment"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblInvestmentID As System.Windows.Forms.Label
    Friend WithEvents lblDateCreated As System.Windows.Forms.Label
    Friend WithEvents txtInvestmentID As System.Windows.Forms.TextBox
    Friend WithEvents txtDateCreated As System.Windows.Forms.TextBox
    Friend WithEvents lblInvestmentName As System.Windows.Forms.Label
    Friend WithEvents txtInvestmentName As System.Windows.Forms.TextBox
    Friend WithEvents lblInvestmentOwner As System.Windows.Forms.Label
    Friend WithEvents cboOwner As System.Windows.Forms.ComboBox
    Friend WithEvents lblInvestmentDescription As System.Windows.Forms.Label
    Friend WithEvents txtDescription As System.Windows.Forms.TextBox
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents cboType As System.Windows.Forms.ComboBox
    Friend WithEvents lblInvestmentType As System.Windows.Forms.Label
    Friend WithEvents chkValueIsCost As System.Windows.Forms.CheckBox
    Friend WithEvents txtDateClosed As System.Windows.Forms.TextBox
    Friend WithEvents lblDateClosed As System.Windows.Forms.Label
End Class
