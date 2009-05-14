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
        Me.lvwStatus = New System.Windows.Forms.ListView
        Me.colAccount = New System.Windows.Forms.ColumnHeader
        Me.colPilot = New System.Windows.Forms.ColumnHeader
        Me.colAccountXML = New System.Windows.Forms.ColumnHeader
        Me.colCharacterXML = New System.Windows.Forms.ColumnHeader
        Me.colTrainingXML = New System.Windows.Forms.ColumnHeader
        Me.btnClose = New System.Windows.Forms.Button
        Me.tmrClose = New System.Windows.Forms.Timer(Me.components)
        Me.lblErrorDetailsLbl = New System.Windows.Forms.Label
        Me.lblErrorDetails = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'lvwStatus
        '
        Me.lvwStatus.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colAccount, Me.colPilot, Me.colAccountXML, Me.colCharacterXML, Me.colTrainingXML})
        Me.lvwStatus.FullRowSelect = True
        Me.lvwStatus.GridLines = True
        Me.lvwStatus.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.lvwStatus.Location = New System.Drawing.Point(13, 13)
        Me.lvwStatus.Name = "lvwStatus"
        Me.lvwStatus.Size = New System.Drawing.Size(731, 277)
        Me.lvwStatus.TabIndex = 0
        Me.lvwStatus.UseCompatibleStateImageBehavior = False
        Me.lvwStatus.View = System.Windows.Forms.View.Details
        '
        'colAccount
        '
        Me.colAccount.Text = "Account"
        Me.colAccount.Width = 200
        '
        'colPilot
        '
        Me.colPilot.Text = "Character"
        Me.colPilot.Width = 200
        '
        'colAccountXML
        '
        Me.colAccountXML.Text = "Account XML"
        Me.colAccountXML.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colAccountXML.Width = 100
        '
        'colCharacterXML
        '
        Me.colCharacterXML.Text = "Character XML"
        Me.colCharacterXML.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colCharacterXML.Width = 100
        '
        'colTrainingXML
        '
        Me.colTrainingXML.Text = "Training XML"
        Me.colTrainingXML.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.colTrainingXML.Width = 100
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(643, 448)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(100, 23)
        Me.btnClose.TabIndex = 2
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'tmrClose
        '
        Me.tmrClose.Interval = 1000
        '
        'lblErrorDetailsLbl
        '
        Me.lblErrorDetailsLbl.AutoSize = True
        Me.lblErrorDetailsLbl.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblErrorDetailsLbl.Location = New System.Drawing.Point(10, 293)
        Me.lblErrorDetailsLbl.Name = "lblErrorDetailsLbl"
        Me.lblErrorDetailsLbl.Size = New System.Drawing.Size(134, 13)
        Me.lblErrorDetailsLbl.TabIndex = 4
        Me.lblErrorDetailsLbl.Text = "API Error Code Details:"
        '
        'lblErrorDetails
        '
        Me.lblErrorDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblErrorDetails.Location = New System.Drawing.Point(13, 306)
        Me.lblErrorDetails.Name = "lblErrorDetails"
        Me.lblErrorDetails.Size = New System.Drawing.Size(731, 136)
        Me.lblErrorDetails.TabIndex = 5
        '
        'EveAPIStatusForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(756, 476)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblErrorDetails)
        Me.Controls.Add(Me.lblErrorDetailsLbl)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lvwStatus)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Name = "EveAPIStatusForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Eve API Status"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lvwStatus As System.Windows.Forms.ListView
    Friend WithEvents colAccount As System.Windows.Forms.ColumnHeader
    Friend WithEvents colAccountXML As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents tmrClose As System.Windows.Forms.Timer
    Friend WithEvents colCharacterXML As System.Windows.Forms.ColumnHeader
    Friend WithEvents colTrainingXML As System.Windows.Forms.ColumnHeader
    Friend WithEvents colPilot As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblErrorDetailsLbl As System.Windows.Forms.Label
    Friend WithEvents lblErrorDetails As System.Windows.Forms.Label
End Class
