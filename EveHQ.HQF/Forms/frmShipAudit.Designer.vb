<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmShipAudit
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
        Me.lvwAudit = New DevComponents.DotNetBar.Controls.ListViewEx
        Me.colAttribute = New System.Windows.Forms.ColumnHeader
        Me.colEffect = New System.Windows.Forms.ColumnHeader
        Me.colOldValue = New System.Windows.Forms.ColumnHeader
        Me.colNewValue = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'lvwAudit
        '
        '
        '
        '
        Me.lvwAudit.Border.Class = "ListViewBorder"
        Me.lvwAudit.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lvwAudit.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colAttribute, Me.colEffect, Me.colOldValue, Me.colNewValue})
        Me.lvwAudit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwAudit.FullRowSelect = True
        Me.lvwAudit.GridLines = True
        Me.lvwAudit.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvwAudit.Location = New System.Drawing.Point(0, 0)
        Me.lvwAudit.Name = "lvwAudit"
        Me.lvwAudit.Size = New System.Drawing.Size(792, 566)
        Me.lvwAudit.TabIndex = 0
        Me.lvwAudit.UseCompatibleStateImageBehavior = False
        Me.lvwAudit.View = System.Windows.Forms.View.Details
        '
        'colAttribute
        '
        Me.colAttribute.Text = "Attribute"
        Me.colAttribute.Width = 280
        '
        'colEffect
        '
        Me.colEffect.Text = "Effect"
        Me.colEffect.Width = 280
        '
        'colOldValue
        '
        Me.colOldValue.Text = "Old Value"
        Me.colOldValue.Width = 100
        '
        'colNewValue
        '
        Me.colNewValue.Text = "New Value"
        Me.colNewValue.Width = 100
        '
        'frmShipAudit
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(792, 566)
        Me.Controls.Add(Me.lvwAudit)
        Me.DoubleBuffered = True
        Me.EnableGlass = False
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmShipAudit"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Ship Attribute Audit Log"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents colAttribute As System.Windows.Forms.ColumnHeader
    Friend WithEvents colEffect As System.Windows.Forms.ColumnHeader
    Friend WithEvents colOldValue As System.Windows.Forms.ColumnHeader
    Friend WithEvents colNewValue As System.Windows.Forms.ColumnHeader
    Friend WithEvents lvwAudit As DevComponents.DotNetBar.Controls.ListViewEx
End Class
