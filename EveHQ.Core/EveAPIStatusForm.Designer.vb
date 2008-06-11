<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EveAPIStatusForm
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
        Me.components = New System.ComponentModel.Container
        Me.lstStatus = New System.Windows.Forms.ListView
        Me.Category = New System.Windows.Forms.ColumnHeader
        Me.Status = New System.Windows.Forms.ColumnHeader
        Me.btnClose = New System.Windows.Forms.Button
        Me.tmrClose = New System.Windows.Forms.Timer(Me.components)
        Me.lblErrorReason = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lstStatus
        '
        Me.lstStatus.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Category, Me.Status})
        Me.lstStatus.FullRowSelect = True
        Me.lstStatus.GridLines = True
        Me.lstStatus.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lstStatus.Location = New System.Drawing.Point(13, 13)
        Me.lstStatus.Name = "lstStatus"
        Me.lstStatus.Size = New System.Drawing.Size(569, 241)
        Me.lstStatus.TabIndex = 0
        Me.lstStatus.UseCompatibleStateImageBehavior = False
        Me.lstStatus.View = System.Windows.Forms.View.Details
        '
        'Category
        '
        Me.Category.Width = 350
        '
        'Status
        '
        Me.Status.Width = 190
        '
        'btnClose
        '
        Me.btnClose.Enabled = False
        Me.btnClose.Location = New System.Drawing.Point(365, 260)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(217, 23)
        Me.btnClose.TabIndex = 2
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'tmrClose
        '
        Me.tmrClose.Interval = 1000
        '
        'lblErrorReason
        '
        Me.lblErrorReason.Location = New System.Drawing.Point(12, 257)
        Me.lblErrorReason.Name = "lblErrorReason"
        Me.lblErrorReason.Size = New System.Drawing.Size(335, 27)
        Me.lblErrorReason.TabIndex = 3
        '
        'EveAPIStatusForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(594, 293)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblErrorReason)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lstStatus)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "EveAPIStatusForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Eve API Status"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lstStatus As System.Windows.Forms.ListView
    Friend WithEvents Category As System.Windows.Forms.ColumnHeader
    Friend WithEvents Status As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents tmrClose As System.Windows.Forms.Timer
    Friend WithEvents lblErrorReason As System.Windows.Forms.Label
End Class
