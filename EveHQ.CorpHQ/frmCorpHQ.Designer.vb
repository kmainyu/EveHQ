<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCorpHQ
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
        Me.btnGetStandings = New System.Windows.Forms.Button
        Me.cboOwner = New System.Windows.Forms.ComboBox
        Me.lblSelectOwner = New System.Windows.Forms.Label
        Me.lvwStandings = New System.Windows.Forms.ListView
        Me.colName = New System.Windows.Forms.ColumnHeader
        Me.colID = New System.Windows.Forms.ColumnHeader
        Me.colType = New System.Windows.Forms.ColumnHeader
        Me.colRawValue = New System.Windows.Forms.ColumnHeader
        Me.colActualValue = New System.Windows.Forms.ColumnHeader
        Me.btExportStandings = New System.Windows.Forms.Button
        Me.btnClearCache = New System.Windows.Forms.Button
        Me.lblTypeFilter = New System.Windows.Forms.Label
        Me.cboFilter = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'btnGetStandings
        '
        Me.btnGetStandings.Location = New System.Drawing.Point(13, 13)
        Me.btnGetStandings.Name = "btnGetStandings"
        Me.btnGetStandings.Size = New System.Drawing.Size(119, 23)
        Me.btnGetStandings.TabIndex = 0
        Me.btnGetStandings.Text = "Get Standings"
        Me.btnGetStandings.UseVisualStyleBackColor = True
        '
        'cboOwner
        '
        Me.cboOwner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOwner.Enabled = False
        Me.cboOwner.FormattingEnabled = True
        Me.cboOwner.Location = New System.Drawing.Point(247, 15)
        Me.cboOwner.Name = "cboOwner"
        Me.cboOwner.Size = New System.Drawing.Size(209, 21)
        Me.cboOwner.Sorted = True
        Me.cboOwner.TabIndex = 1
        '
        'lblSelectOwner
        '
        Me.lblSelectOwner.AutoSize = True
        Me.lblSelectOwner.Enabled = False
        Me.lblSelectOwner.Location = New System.Drawing.Point(167, 18)
        Me.lblSelectOwner.Name = "lblSelectOwner"
        Me.lblSelectOwner.Size = New System.Drawing.Size(74, 13)
        Me.lblSelectOwner.TabIndex = 2
        Me.lblSelectOwner.Text = "Select Owner:"
        '
        'lvwStandings
        '
        Me.lvwStandings.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwStandings.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colName, Me.colID, Me.colType, Me.colRawValue, Me.colActualValue})
        Me.lvwStandings.Enabled = False
        Me.lvwStandings.FullRowSelect = True
        Me.lvwStandings.GridLines = True
        Me.lvwStandings.Location = New System.Drawing.Point(11, 69)
        Me.lvwStandings.Name = "lvwStandings"
        Me.lvwStandings.Size = New System.Drawing.Size(731, 381)
        Me.lvwStandings.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwStandings.TabIndex = 3
        Me.lvwStandings.UseCompatibleStateImageBehavior = False
        Me.lvwStandings.View = System.Windows.Forms.View.Details
        '
        'colName
        '
        Me.colName.Text = "Entity"
        Me.colName.Width = 200
        '
        'colID
        '
        Me.colID.Text = "Entity ID"
        Me.colID.Width = 100
        '
        'colType
        '
        Me.colType.Text = "Entity Type"
        Me.colType.Width = 100
        '
        'colRawValue
        '
        Me.colRawValue.Text = "Standing Value (Raw)"
        Me.colRawValue.Width = 150
        '
        'colActualValue
        '
        Me.colActualValue.Text = "Standing Value (Actual)"
        Me.colActualValue.Width = 150
        '
        'btExportStandings
        '
        Me.btExportStandings.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btExportStandings.Location = New System.Drawing.Point(627, 456)
        Me.btExportStandings.Name = "btExportStandings"
        Me.btExportStandings.Size = New System.Drawing.Size(115, 23)
        Me.btExportStandings.TabIndex = 4
        Me.btExportStandings.Text = "Export Standings"
        Me.btExportStandings.UseVisualStyleBackColor = True
        '
        'btnClearCache
        '
        Me.btnClearCache.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClearCache.Location = New System.Drawing.Point(623, 13)
        Me.btnClearCache.Name = "btnClearCache"
        Me.btnClearCache.Size = New System.Drawing.Size(119, 23)
        Me.btnClearCache.TabIndex = 5
        Me.btnClearCache.Text = "Clear Cache"
        Me.btnClearCache.UseVisualStyleBackColor = True
        '
        'lblTypeFilter
        '
        Me.lblTypeFilter.AutoSize = True
        Me.lblTypeFilter.Enabled = False
        Me.lblTypeFilter.Location = New System.Drawing.Point(167, 45)
        Me.lblTypeFilter.Name = "lblTypeFilter"
        Me.lblTypeFilter.Size = New System.Drawing.Size(65, 13)
        Me.lblTypeFilter.TabIndex = 7
        Me.lblTypeFilter.Text = "Select Filter:"
        '
        'cboFilter
        '
        Me.cboFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFilter.Enabled = False
        Me.cboFilter.FormattingEnabled = True
        Me.cboFilter.Items.AddRange(New Object() {"<All>", "Agent", "Corporation", "Faction", "Player/Corp"})
        Me.cboFilter.Location = New System.Drawing.Point(247, 42)
        Me.cboFilter.Name = "cboFilter"
        Me.cboFilter.Size = New System.Drawing.Size(103, 21)
        Me.cboFilter.Sorted = True
        Me.cboFilter.TabIndex = 6
        Me.cboFilter.Tag = "0"
        '
        'frmCorpHQ
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(754, 491)
        Me.Controls.Add(Me.lblTypeFilter)
        Me.Controls.Add(Me.cboFilter)
        Me.Controls.Add(Me.btnClearCache)
        Me.Controls.Add(Me.btExportStandings)
        Me.Controls.Add(Me.lvwStandings)
        Me.Controls.Add(Me.lblSelectOwner)
        Me.Controls.Add(Me.cboOwner)
        Me.Controls.Add(Me.btnGetStandings)
        Me.Name = "frmCorpHQ"
        Me.Text = "CorpHQ"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnGetStandings As System.Windows.Forms.Button
    Friend WithEvents cboOwner As System.Windows.Forms.ComboBox
    Friend WithEvents lblSelectOwner As System.Windows.Forms.Label
    Friend WithEvents lvwStandings As System.Windows.Forms.ListView
    Friend WithEvents colName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colID As System.Windows.Forms.ColumnHeader
    Friend WithEvents colRawValue As System.Windows.Forms.ColumnHeader
    Friend WithEvents btExportStandings As System.Windows.Forms.Button
    Friend WithEvents btnClearCache As System.Windows.Forms.Button
    Friend WithEvents colActualValue As System.Windows.Forms.ColumnHeader
    Friend WithEvents colType As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblTypeFilter As System.Windows.Forms.Label
    Friend WithEvents cboFilter As System.Windows.Forms.ComboBox
End Class
