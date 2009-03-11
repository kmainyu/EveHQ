<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEveExport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEveExport))
        Me.lblDescription = New System.Windows.Forms.Label
        Me.lblEveFolder = New System.Windows.Forms.Label
        Me.lvwFittings = New System.Windows.Forms.ListView
        Me.colFitting = New System.Windows.Forms.ColumnHeader
        Me.btnExport = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblFilename = New System.Windows.Forms.Label
        Me.txtFilename = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'lblDescription
        '
        Me.lblDescription.AutoSize = True
        Me.lblDescription.Location = New System.Drawing.Point(12, 9)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(312, 13)
        Me.lblDescription.TabIndex = 0
        Me.lblDescription.Text = "This feature will export the selected fittings to the following folder:"
        '
        'lblEveFolder
        '
        Me.lblEveFolder.AutoSize = True
        Me.lblEveFolder.Location = New System.Drawing.Point(12, 31)
        Me.lblEveFolder.Name = "lblEveFolder"
        Me.lblEveFolder.Size = New System.Drawing.Size(80, 13)
        Me.lblEveFolder.TabIndex = 1
        Me.lblEveFolder.Text = "Folder Location"
        '
        'lvwFittings
        '
        Me.lvwFittings.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colFitting})
        Me.lvwFittings.FullRowSelect = True
        Me.lvwFittings.GridLines = True
        Me.lvwFittings.Location = New System.Drawing.Point(15, 59)
        Me.lvwFittings.Name = "lvwFittings"
        Me.lvwFittings.Size = New System.Drawing.Size(516, 224)
        Me.lvwFittings.TabIndex = 2
        Me.lvwFittings.UseCompatibleStateImageBehavior = False
        Me.lvwFittings.View = System.Windows.Forms.View.Details
        '
        'colFitting
        '
        Me.colFitting.Text = "Fitting Name"
        Me.colFitting.Width = 490
        '
        'btnExport
        '
        Me.btnExport.Location = New System.Drawing.Point(375, 300)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(75, 23)
        Me.btnExport.TabIndex = 3
        Me.btnExport.Text = "Export"
        Me.btnExport.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(456, 300)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblFilename
        '
        Me.lblFilename.AutoSize = True
        Me.lblFilename.Location = New System.Drawing.Point(12, 294)
        Me.lblFilename.Name = "lblFilename"
        Me.lblFilename.Size = New System.Drawing.Size(85, 13)
        Me.lblFilename.TabIndex = 5
        Me.lblFilename.Text = "Export Filename:"
        '
        'txtFilename
        '
        Me.txtFilename.Location = New System.Drawing.Point(103, 291)
        Me.txtFilename.MaxLength = 20
        Me.txtFilename.Name = "txtFilename"
        Me.txtFilename.Size = New System.Drawing.Size(212, 20)
        Me.txtFilename.TabIndex = 6
        '
        'frmEveExport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(543, 335)
        Me.Controls.Add(Me.txtFilename)
        Me.Controls.Add(Me.lblFilename)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnExport)
        Me.Controls.Add(Me.lvwFittings)
        Me.Controls.Add(Me.lblEveFolder)
        Me.Controls.Add(Me.lblDescription)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmEveExport"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Export Fittings For Eve"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents lblEveFolder As System.Windows.Forms.Label
    Friend WithEvents lvwFittings As System.Windows.Forms.ListView
    Friend WithEvents btnExport As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents colFitting As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblFilename As System.Windows.Forms.Label
    Friend WithEvents txtFilename As System.Windows.Forms.TextBox
End Class
