<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAddTransaction
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
        Me.lblTransactionID = New System.Windows.Forms.Label
        Me.lblDate = New System.Windows.Forms.Label
        Me.txtTransactionID = New System.Windows.Forms.TextBox
        Me.lblInvestmentName = New System.Windows.Forms.Label
        Me.txtInvestmentName = New System.Windows.Forms.TextBox
        Me.btnAccept = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.dtpDate = New System.Windows.Forms.DateTimePicker
        Me.txtInvestmentID = New System.Windows.Forms.TextBox
        Me.lblInvestmentID = New System.Windows.Forms.Label
        Me.lblTransactionType = New System.Windows.Forms.Label
        Me.cboType = New System.Windows.Forms.ComboBox
        Me.lblQuantity = New System.Windows.Forms.Label
        Me.txtQuantity = New System.Windows.Forms.TextBox
        Me.txtUnitValue = New System.Windows.Forms.TextBox
        Me.lblValue = New System.Windows.Forms.Label
        Me.txtCurrentQuantity = New System.Windows.Forms.TextBox
        Me.lblCurrentQuantity = New System.Windows.Forms.Label
        Me.txtNotes = New System.Windows.Forms.TextBox
        Me.lblNotes = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lblTransactionID
        '
        Me.lblTransactionID.AutoSize = True
        Me.lblTransactionID.Location = New System.Drawing.Point(13, 13)
        Me.lblTransactionID.Name = "lblTransactionID"
        Me.lblTransactionID.Size = New System.Drawing.Size(81, 13)
        Me.lblTransactionID.TabIndex = 0
        Me.lblTransactionID.Text = "Transaction ID:"
        '
        'lblDate
        '
        Me.lblDate.AutoSize = True
        Me.lblDate.Location = New System.Drawing.Point(295, 111)
        Me.lblDate.Name = "lblDate"
        Me.lblDate.Size = New System.Drawing.Size(34, 13)
        Me.lblDate.TabIndex = 1
        Me.lblDate.Text = "Date:"
        '
        'txtTransactionID
        '
        Me.txtTransactionID.Enabled = False
        Me.txtTransactionID.Location = New System.Drawing.Point(115, 10)
        Me.txtTransactionID.Name = "txtTransactionID"
        Me.txtTransactionID.Size = New System.Drawing.Size(121, 21)
        Me.txtTransactionID.TabIndex = 5
        Me.txtTransactionID.TabStop = False
        '
        'lblInvestmentName
        '
        Me.lblInvestmentName.AutoSize = True
        Me.lblInvestmentName.Location = New System.Drawing.Point(13, 39)
        Me.lblInvestmentName.Name = "lblInvestmentName"
        Me.lblInvestmentName.Size = New System.Drawing.Size(96, 13)
        Me.lblInvestmentName.TabIndex = 4
        Me.lblInvestmentName.Text = "Investment Name:"
        '
        'txtInvestmentName
        '
        Me.txtInvestmentName.Enabled = False
        Me.txtInvestmentName.Location = New System.Drawing.Point(115, 36)
        Me.txtInvestmentName.Name = "txtInvestmentName"
        Me.txtInvestmentName.Size = New System.Drawing.Size(404, 21)
        Me.txtInvestmentName.TabIndex = 0
        '
        'btnAccept
        '
        Me.btnAccept.Location = New System.Drawing.Point(363, 334)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.TabIndex = 4
        Me.btnAccept.Text = "Accept"
        Me.btnAccept.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(444, 334)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'dtpDate
        '
        Me.dtpDate.CustomFormat = "dd/MM/yyyy HH:mm:dd"
        Me.dtpDate.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right
        Me.dtpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpDate.Location = New System.Drawing.Point(334, 108)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Size = New System.Drawing.Size(185, 21)
        Me.dtpDate.TabIndex = 1
        '
        'txtInvestmentID
        '
        Me.txtInvestmentID.Enabled = False
        Me.txtInvestmentID.Location = New System.Drawing.Point(334, 10)
        Me.txtInvestmentID.Name = "txtInvestmentID"
        Me.txtInvestmentID.Size = New System.Drawing.Size(121, 21)
        Me.txtInvestmentID.TabIndex = 11
        Me.txtInvestmentID.TabStop = False
        '
        'lblInvestmentID
        '
        Me.lblInvestmentID.AutoSize = True
        Me.lblInvestmentID.Location = New System.Drawing.Point(252, 13)
        Me.lblInvestmentID.Name = "lblInvestmentID"
        Me.lblInvestmentID.Size = New System.Drawing.Size(80, 13)
        Me.lblInvestmentID.TabIndex = 10
        Me.lblInvestmentID.Text = "Investment ID:"
        '
        'lblTransactionType
        '
        Me.lblTransactionType.AutoSize = True
        Me.lblTransactionType.Location = New System.Drawing.Point(13, 111)
        Me.lblTransactionType.Name = "lblTransactionType"
        Me.lblTransactionType.Size = New System.Drawing.Size(94, 13)
        Me.lblTransactionType.TabIndex = 12
        Me.lblTransactionType.Text = "Transaction Type:"
        '
        'cboType
        '
        Me.cboType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboType.FormattingEnabled = True
        Me.cboType.Items.AddRange(New Object() {"Purchase", "Sale", "Valuation", "Income", "Cost", "Income (Retained)", "Cost (Retained)", "Transfer To Investment", "Transfer From Investment"})
        Me.cboType.Location = New System.Drawing.Point(115, 108)
        Me.cboType.Name = "cboType"
        Me.cboType.Size = New System.Drawing.Size(174, 21)
        Me.cboType.TabIndex = 0
        '
        'lblQuantity
        '
        Me.lblQuantity.AutoSize = True
        Me.lblQuantity.Location = New System.Drawing.Point(279, 160)
        Me.lblQuantity.Name = "lblQuantity"
        Me.lblQuantity.Size = New System.Drawing.Size(53, 13)
        Me.lblQuantity.TabIndex = 14
        Me.lblQuantity.Text = "Quantity:"
        '
        'txtQuantity
        '
        Me.txtQuantity.Location = New System.Drawing.Point(334, 157)
        Me.txtQuantity.Name = "txtQuantity"
        Me.txtQuantity.Size = New System.Drawing.Size(121, 21)
        Me.txtQuantity.TabIndex = 3
        '
        'txtUnitValue
        '
        Me.txtUnitValue.Location = New System.Drawing.Point(115, 157)
        Me.txtUnitValue.Name = "txtUnitValue"
        Me.txtUnitValue.Size = New System.Drawing.Size(121, 21)
        Me.txtUnitValue.TabIndex = 2
        '
        'lblValue
        '
        Me.lblValue.AutoSize = True
        Me.lblValue.Location = New System.Drawing.Point(16, 160)
        Me.lblValue.Name = "lblValue"
        Me.lblValue.Size = New System.Drawing.Size(59, 13)
        Me.lblValue.TabIndex = 16
        Me.lblValue.Text = "Unit Value:"
        '
        'txtCurrentQuantity
        '
        Me.txtCurrentQuantity.Enabled = False
        Me.txtCurrentQuantity.Location = New System.Drawing.Point(115, 62)
        Me.txtCurrentQuantity.Name = "txtCurrentQuantity"
        Me.txtCurrentQuantity.Size = New System.Drawing.Size(121, 21)
        Me.txtCurrentQuantity.TabIndex = 18
        '
        'lblCurrentQuantity
        '
        Me.lblCurrentQuantity.AutoSize = True
        Me.lblCurrentQuantity.Location = New System.Drawing.Point(13, 65)
        Me.lblCurrentQuantity.Name = "lblCurrentQuantity"
        Me.lblCurrentQuantity.Size = New System.Drawing.Size(93, 13)
        Me.lblCurrentQuantity.TabIndex = 19
        Me.lblCurrentQuantity.Text = "Current Quantity:"
        '
        'txtNotes
        '
        Me.txtNotes.Location = New System.Drawing.Point(115, 198)
        Me.txtNotes.MaxLength = 512
        Me.txtNotes.Multiline = True
        Me.txtNotes.Name = "txtNotes"
        Me.txtNotes.Size = New System.Drawing.Size(404, 130)
        Me.txtNotes.TabIndex = 20
        '
        'lblNotes
        '
        Me.lblNotes.AutoSize = True
        Me.lblNotes.Location = New System.Drawing.Point(16, 201)
        Me.lblNotes.Name = "lblNotes"
        Me.lblNotes.Size = New System.Drawing.Size(39, 13)
        Me.lblNotes.TabIndex = 21
        Me.lblNotes.Text = "Notes:"
        '
        'frmAddTransaction
        '
        Me.AcceptButton = Me.btnAccept
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(531, 363)
        Me.Controls.Add(Me.lblNotes)
        Me.Controls.Add(Me.txtNotes)
        Me.Controls.Add(Me.txtCurrentQuantity)
        Me.Controls.Add(Me.lblCurrentQuantity)
        Me.Controls.Add(Me.txtUnitValue)
        Me.Controls.Add(Me.lblValue)
        Me.Controls.Add(Me.txtQuantity)
        Me.Controls.Add(Me.lblQuantity)
        Me.Controls.Add(Me.cboType)
        Me.Controls.Add(Me.lblTransactionType)
        Me.Controls.Add(Me.txtInvestmentID)
        Me.Controls.Add(Me.lblInvestmentID)
        Me.Controls.Add(Me.dtpDate)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.txtInvestmentName)
        Me.Controls.Add(Me.lblInvestmentName)
        Me.Controls.Add(Me.txtTransactionID)
        Me.Controls.Add(Me.lblDate)
        Me.Controls.Add(Me.lblTransactionID)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmAddTransaction"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add New Investment Transaction"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblTransactionID As System.Windows.Forms.Label
    Friend WithEvents lblDate As System.Windows.Forms.Label
    Friend WithEvents txtTransactionID As System.Windows.Forms.TextBox
    Friend WithEvents lblInvestmentName As System.Windows.Forms.Label
    Friend WithEvents txtInvestmentName As System.Windows.Forms.TextBox
    Friend WithEvents btnAccept As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents dtpDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtInvestmentID As System.Windows.Forms.TextBox
    Friend WithEvents lblInvestmentID As System.Windows.Forms.Label
    Friend WithEvents lblTransactionType As System.Windows.Forms.Label
    Friend WithEvents cboType As System.Windows.Forms.ComboBox
    Friend WithEvents lblQuantity As System.Windows.Forms.Label
    Friend WithEvents txtQuantity As System.Windows.Forms.TextBox
    Friend WithEvents txtUnitValue As System.Windows.Forms.TextBox
    Friend WithEvents lblValue As System.Windows.Forms.Label
    Friend WithEvents txtCurrentQuantity As System.Windows.Forms.TextBox
    Friend WithEvents lblCurrentQuantity As System.Windows.Forms.Label
    Friend WithEvents txtNotes As System.Windows.Forms.TextBox
    Friend WithEvents lblNotes As System.Windows.Forms.Label
End Class
