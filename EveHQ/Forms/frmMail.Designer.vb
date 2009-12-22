<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMail
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
        Me.btnCreateDBTables = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnCreateDBTables
        '
        Me.btnCreateDBTables.Location = New System.Drawing.Point(12, 12)
        Me.btnCreateDBTables.Name = "btnCreateDBTables"
        Me.btnCreateDBTables.Size = New System.Drawing.Size(143, 23)
        Me.btnCreateDBTables.TabIndex = 0
        Me.btnCreateDBTables.Text = "Create Tables"
        Me.btnCreateDBTables.UseVisualStyleBackColor = True
        '
        'frmMail
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(440, 394)
        Me.Controls.Add(Me.btnCreateDBTables)
        Me.Name = "frmMail"
        Me.Text = "EveHQ Mail Viewer"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCreateDBTables As System.Windows.Forms.Button
End Class
