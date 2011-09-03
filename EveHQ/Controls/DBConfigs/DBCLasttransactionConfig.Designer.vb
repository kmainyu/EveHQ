<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DBCLasttransactionConfig
    Inherits DevComponents.DotNetBar.Office2007Form

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
        Me.spinDefaultTransactions = New DevComponents.Editors.IntegerInput
        Me.lblNumTransactions = New DevComponents.DotNetBar.LabelX
        Me.btnCancel = New DevComponents.DotNetBar.ButtonX
        Me.btnAccept = New DevComponents.DotNetBar.ButtonX
        Me.cboPilots = New DevComponents.DotNetBar.Controls.ComboBoxEx
        Me.lblDefaultPilot = New DevComponents.DotNetBar.LabelX
        CType(Me.spinDefaultTransactions, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'spinDefaultTransactions
        '
        '
        '
        '
        Me.spinDefaultTransactions.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.spinDefaultTransactions.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.spinDefaultTransactions.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
        Me.spinDefaultTransactions.Location = New System.Drawing.Point(149, 46)
        Me.spinDefaultTransactions.Name = "spinDefaultTransactions"
        Me.spinDefaultTransactions.ShowUpDown = True
        Me.spinDefaultTransactions.Size = New System.Drawing.Size(103, 21)
        Me.spinDefaultTransactions.TabIndex = 22
        Me.spinDefaultTransactions.Value = 1
        '
        'lblNumTransactions
        '
        Me.lblNumTransactions.AutoSize = True
        '
        '
        '
        Me.lblNumTransactions.BackgroundStyle.Class = ""
        Me.lblNumTransactions.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblNumTransactions.Location = New System.Drawing.Point(14, 51)
        Me.lblNumTransactions.Name = "lblNumTransactions"
        Me.lblNumTransactions.Size = New System.Drawing.Size(123, 16)
        Me.lblNumTransactions.TabIndex = 21
        Me.lblNumTransactions.Text = "Number of Transactions:"
        '
        'btnCancel
        '
        Me.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnCancel.Location = New System.Drawing.Point(177, 81)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnCancel.TabIndex = 17
        Me.btnCancel.Text = "Cancel"
        '
        'btnAccept
        '
        Me.btnAccept.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAccept.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAccept.Location = New System.Drawing.Point(96, 82)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnAccept.TabIndex = 20
        Me.btnAccept.Text = "Accept"
        '
        'cboPilots
        '
        Me.cboPilots.DisplayMember = "Text"
        Me.cboPilots.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.ItemHeight = 15
        Me.cboPilots.Location = New System.Drawing.Point(85, 16)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(167, 21)
        Me.cboPilots.Sorted = True
        Me.cboPilots.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboPilots.TabIndex = 19
        '
        'lblDefaultPilot
        '
        Me.lblDefaultPilot.AutoSize = True
        '
        '
        '
        Me.lblDefaultPilot.BackgroundStyle.Class = ""
        Me.lblDefaultPilot.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblDefaultPilot.Location = New System.Drawing.Point(13, 21)
        Me.lblDefaultPilot.Name = "lblDefaultPilot"
        Me.lblDefaultPilot.Size = New System.Drawing.Size(65, 16)
        Me.lblDefaultPilot.TabIndex = 18
        Me.lblDefaultPilot.Text = "Default Pilot:"
        '
        'DBCLasttransactionConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(264, 116)
        Me.Controls.Add(Me.spinDefaultTransactions)
        Me.Controls.Add(Me.lblNumTransactions)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.cboPilots)
        Me.Controls.Add(Me.lblDefaultPilot)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DBCLasttransactionConfig"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Last Transactions Configuration"
        CType(Me.spinDefaultTransactions, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents spinDefaultTransactions As DevComponents.Editors.IntegerInput
    Friend WithEvents lblNumTransactions As DevComponents.DotNetBar.LabelX
    Friend WithEvents btnCancel As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnAccept As DevComponents.DotNetBar.ButtonX
    Friend WithEvents cboPilots As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents lblDefaultPilot As DevComponents.DotNetBar.LabelX
End Class
