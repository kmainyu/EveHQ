<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMetaVariations
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMetaVariations))
        Me.chkShowAllColumns = New System.Windows.Forms.CheckBox
        Me.lvwComparisons = New System.Windows.Forms.ListView
        Me.ColumnHeader39 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader40 = New System.Windows.Forms.ColumnHeader
        Me.chkApplySkills = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'chkShowAllColumns
        '
        Me.chkShowAllColumns.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkShowAllColumns.AutoSize = True
        Me.chkShowAllColumns.Location = New System.Drawing.Point(4, 441)
        Me.chkShowAllColumns.Name = "chkShowAllColumns"
        Me.chkShowAllColumns.Size = New System.Drawing.Size(109, 17)
        Me.chkShowAllColumns.TabIndex = 4
        Me.chkShowAllColumns.Text = "Show All Columns"
        Me.chkShowAllColumns.UseVisualStyleBackColor = True
        '
        'lvwComparisons
        '
        Me.lvwComparisons.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwComparisons.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader39, Me.ColumnHeader40})
        Me.lvwComparisons.FullRowSelect = True
        Me.lvwComparisons.GridLines = True
        Me.lvwComparisons.Location = New System.Drawing.Point(1, 1)
        Me.lvwComparisons.Name = "lvwComparisons"
        Me.lvwComparisons.Size = New System.Drawing.Size(882, 434)
        Me.lvwComparisons.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwComparisons.TabIndex = 3
        Me.lvwComparisons.UseCompatibleStateImageBehavior = False
        Me.lvwComparisons.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader39
        '
        Me.ColumnHeader39.Text = "Item"
        Me.ColumnHeader39.Width = 270
        '
        'ColumnHeader40
        '
        Me.ColumnHeader40.Text = "Meta Type"
        Me.ColumnHeader40.Width = 150
        '
        'chkApplySkills
        '
        Me.chkApplySkills.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkApplySkills.AutoSize = True
        Me.chkApplySkills.Location = New System.Drawing.Point(119, 441)
        Me.chkApplySkills.Name = "chkApplySkills"
        Me.chkApplySkills.Size = New System.Drawing.Size(174, 17)
        Me.chkApplySkills.TabIndex = 5
        Me.chkApplySkills.Text = "Apply Current Ship/Skill Effects"
        Me.chkApplySkills.UseVisualStyleBackColor = True
        '
        'frmMetaVariations
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 464)
        Me.Controls.Add(Me.chkApplySkills)
        Me.Controls.Add(Me.chkShowAllColumns)
        Me.Controls.Add(Me.lvwComparisons)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMetaVariations"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "HQF Meta Variations"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents chkShowAllColumns As System.Windows.Forms.CheckBox
    Friend WithEvents lvwComparisons As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader39 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader40 As System.Windows.Forms.ColumnHeader
    Friend WithEvents chkApplySkills As System.Windows.Forms.CheckBox
End Class
