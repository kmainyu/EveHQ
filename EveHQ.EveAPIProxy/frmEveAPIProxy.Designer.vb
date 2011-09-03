<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEveAPIProxy
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
        Me.lvwEvents = New System.Windows.Forms.ListView
        Me.colDate = New System.Windows.Forms.ColumnHeader
        Me.colType = New System.Windows.Forms.ColumnHeader
        Me.colDescription = New System.Windows.Forms.ColumnHeader
        Me.tmrMessage = New System.Windows.Forms.Timer(Me.components)
        Me.colRef = New System.Windows.Forms.ColumnHeader
        Me.SuspendLayout()
        '
        'lvwEvents
        '
        Me.lvwEvents.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwEvents.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colDate, Me.colType, Me.colRef, Me.colDescription})
        Me.lvwEvents.FullRowSelect = True
        Me.lvwEvents.GridLines = True
        Me.lvwEvents.Location = New System.Drawing.Point(12, 12)
        Me.lvwEvents.Name = "lvwEvents"
        Me.lvwEvents.Size = New System.Drawing.Size(857, 465)
        Me.lvwEvents.TabIndex = 0
        Me.lvwEvents.UseCompatibleStateImageBehavior = False
        Me.lvwEvents.View = System.Windows.Forms.View.Details
        '
        'colDate
        '
        Me.colDate.Text = "Date"
        Me.colDate.Width = 120
        '
        'colType
        '
        Me.colType.Text = "Event Type"
        Me.colType.Width = 150
        '
        'colDescription
        '
        Me.colDescription.Text = "Description"
        Me.colDescription.Width = 500
        '
        'tmrMessage
        '
        Me.tmrMessage.Enabled = True
        Me.tmrMessage.Interval = 250
        '
        'colRef
        '
        Me.colRef.Text = "Ref"
        Me.colRef.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colRef.Width = 50
        '
        'frmEveAPIProxy
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(881, 489)
        Me.Controls.Add(Me.lvwEvents)
        Me.Name = "frmEveAPIProxy"
        Me.Text = "EveHQ - Eve API Proxy"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lvwEvents As System.Windows.Forms.ListView
    Friend WithEvents colDate As System.Windows.Forms.ColumnHeader
    Friend WithEvents colType As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDescription As System.Windows.Forms.ColumnHeader
    Friend WithEvents tmrMessage As System.Windows.Forms.Timer
    Friend WithEvents colRef As System.Windows.Forms.ColumnHeader

End Class
