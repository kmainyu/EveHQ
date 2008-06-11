<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDoomsday
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
        Me.btnClose = New System.Windows.Forms.Button
        Me.lvwDoomsday = New System.Windows.Forms.ListView
        Me.colDDRace = New System.Windows.Forms.ColumnHeader
        Me.colDDDamageType = New System.Windows.Forms.ColumnHeader
        Me.colDDDamage = New System.Windows.Forms.ColumnHeader
        Me.colDDEffShield = New System.Windows.Forms.ColumnHeader
        Me.colDDEffArmor = New System.Windows.Forms.ColumnHeader
        Me.colDDEffStructure = New System.Windows.Forms.ColumnHeader
        Me.colDDEffTotal = New System.Windows.Forms.ColumnHeader
        Me.colDDDifference = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'btnClose
        '
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(541, 131)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 12
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'lvwDoomsday
        '
        Me.lvwDoomsday.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colDDRace, Me.colDDDamageType, Me.colDDDamage, Me.colDDEffShield, Me.colDDEffArmor, Me.colDDEffStructure, Me.colDDEffTotal, Me.colDDDifference})
        Me.lvwDoomsday.FullRowSelect = True
        Me.lvwDoomsday.GridLines = True
        Me.lvwDoomsday.Location = New System.Drawing.Point(12, 12)
        Me.lvwDoomsday.Name = "lvwDoomsday"
        Me.lvwDoomsday.Size = New System.Drawing.Size(604, 113)
        Me.lvwDoomsday.TabIndex = 13
        Me.lvwDoomsday.UseCompatibleStateImageBehavior = False
        Me.lvwDoomsday.View = System.Windows.Forms.View.Details
        '
        'colDDRace
        '
        Me.colDDRace.Text = "Race"
        Me.colDDRace.Width = 75
        '
        'colDDDamageType
        '
        Me.colDDDamageType.Text = "Dmg Type"
        Me.colDDDamageType.Width = 75
        '
        'colDDDamage
        '
        Me.colDDDamage.Text = "Damage"
        Me.colDDDamage.Width = 75
        '
        'colDDEffShield
        '
        Me.colDDEffShield.Text = "Eff Shield"
        Me.colDDEffShield.Width = 75
        '
        'colDDEffArmor
        '
        Me.colDDEffArmor.Text = "Eff Armor"
        Me.colDDEffArmor.Width = 75
        '
        'colDDEffStructure
        '
        Me.colDDEffStructure.Text = "Eff Structure"
        Me.colDDEffStructure.Width = 75
        '
        'colDDEffTotal
        '
        Me.colDDEffTotal.Text = "Eff Total"
        Me.colDDEffTotal.Width = 75
        '
        'colDDDifference
        '
        Me.colDDDifference.Text = "Residual"
        Me.colDDDifference.Width = 75
        '
        'frmDoomsday
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(630, 161)
        Me.Controls.Add(Me.lvwDoomsday)
        Me.Controls.Add(Me.btnClose)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDoomsday"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Doomsday Device Resistance Check"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents lvwDoomsday As System.Windows.Forms.ListView
    Friend WithEvents colDDRace As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDDDamageType As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDDDamage As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDDEffShield As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDDEffArmor As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDDEffStructure As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDDEffTotal As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDDDifference As System.Windows.Forms.ColumnHeader
End Class
