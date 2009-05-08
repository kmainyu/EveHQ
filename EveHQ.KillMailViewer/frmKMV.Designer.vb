<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmKMV
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
        Me.gbAPIInfo = New System.Windows.Forms.GroupBox
        Me.lblAPIStatus = New System.Windows.Forms.Label
        Me.btnGetCharacters = New System.Windows.Forms.Button
        Me.txtAPIKey = New System.Windows.Forms.TextBox
        Me.lblAPIKey = New System.Windows.Forms.Label
        Me.txtUserID = New System.Windows.Forms.TextBox
        Me.lblUserID = New System.Windows.Forms.Label
        Me.gbCharacters = New System.Windows.Forms.GroupBox
        Me.chkUseCorp = New System.Windows.Forms.CheckBox
        Me.lvwCharacters = New System.Windows.Forms.ListView
        Me.colCharacterName = New System.Windows.Forms.ColumnHeader
        Me.btnFetchKillMails = New System.Windows.Forms.Button
        Me.lblKMSummary = New System.Windows.Forms.Label
        Me.lvwKillMails = New System.Windows.Forms.ListView
        Me.colVictim = New System.Windows.Forms.ColumnHeader
        Me.colShip = New System.Windows.Forms.ColumnHeader
        Me.colKillTime = New System.Windows.Forms.ColumnHeader
        Me.lblKillmailDetails = New System.Windows.Forms.Label
        Me.txtKillMailDetails = New System.Windows.Forms.TextBox
        Me.radUseAccount = New System.Windows.Forms.RadioButton
        Me.radUseAPI = New System.Windows.Forms.RadioButton
        Me.cboAccount = New System.Windows.Forms.ComboBox
        Me.btnUploadToBC = New System.Windows.Forms.Button
        Me.gbAPIInfo.SuspendLayout()
        Me.gbCharacters.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbAPIInfo
        '
        Me.gbAPIInfo.Controls.Add(Me.cboAccount)
        Me.gbAPIInfo.Controls.Add(Me.radUseAPI)
        Me.gbAPIInfo.Controls.Add(Me.radUseAccount)
        Me.gbAPIInfo.Controls.Add(Me.lblAPIStatus)
        Me.gbAPIInfo.Controls.Add(Me.btnGetCharacters)
        Me.gbAPIInfo.Controls.Add(Me.txtAPIKey)
        Me.gbAPIInfo.Controls.Add(Me.lblAPIKey)
        Me.gbAPIInfo.Controls.Add(Me.txtUserID)
        Me.gbAPIInfo.Controls.Add(Me.lblUserID)
        Me.gbAPIInfo.Location = New System.Drawing.Point(12, 12)
        Me.gbAPIInfo.Name = "gbAPIInfo"
        Me.gbAPIInfo.Size = New System.Drawing.Size(532, 147)
        Me.gbAPIInfo.TabIndex = 0
        Me.gbAPIInfo.TabStop = False
        Me.gbAPIInfo.Text = "Character API Information"
        '
        'lblAPIStatus
        '
        Me.lblAPIStatus.AutoSize = True
        Me.lblAPIStatus.Enabled = False
        Me.lblAPIStatus.Location = New System.Drawing.Point(102, 121)
        Me.lblAPIStatus.Name = "lblAPIStatus"
        Me.lblAPIStatus.Size = New System.Drawing.Size(154, 13)
        Me.lblAPIStatus.TabIndex = 5
        Me.lblAPIStatus.Text = "API Status: Not yet connected"
        '
        'btnGetCharacters
        '
        Me.btnGetCharacters.Enabled = False
        Me.btnGetCharacters.Location = New System.Drawing.Point(422, 116)
        Me.btnGetCharacters.Name = "btnGetCharacters"
        Me.btnGetCharacters.Size = New System.Drawing.Size(100, 23)
        Me.btnGetCharacters.TabIndex = 4
        Me.btnGetCharacters.Text = "Get Characters"
        Me.btnGetCharacters.UseVisualStyleBackColor = True
        '
        'txtAPIKey
        '
        Me.txtAPIKey.Enabled = False
        Me.txtAPIKey.Location = New System.Drawing.Point(102, 92)
        Me.txtAPIKey.Name = "txtAPIKey"
        Me.txtAPIKey.Size = New System.Drawing.Size(420, 21)
        Me.txtAPIKey.TabIndex = 3
        '
        'lblAPIKey
        '
        Me.lblAPIKey.AutoSize = True
        Me.lblAPIKey.Enabled = False
        Me.lblAPIKey.Location = New System.Drawing.Point(39, 95)
        Me.lblAPIKey.Name = "lblAPIKey"
        Me.lblAPIKey.Size = New System.Drawing.Size(49, 13)
        Me.lblAPIKey.TabIndex = 2
        Me.lblAPIKey.Text = "API Key:"
        '
        'txtUserID
        '
        Me.txtUserID.Enabled = False
        Me.txtUserID.Location = New System.Drawing.Point(102, 66)
        Me.txtUserID.Name = "txtUserID"
        Me.txtUserID.Size = New System.Drawing.Size(100, 21)
        Me.txtUserID.TabIndex = 1
        '
        'lblUserID
        '
        Me.lblUserID.AutoSize = True
        Me.lblUserID.Enabled = False
        Me.lblUserID.Location = New System.Drawing.Point(39, 69)
        Me.lblUserID.Name = "lblUserID"
        Me.lblUserID.Size = New System.Drawing.Size(44, 13)
        Me.lblUserID.TabIndex = 0
        Me.lblUserID.Text = "UserID:"
        '
        'gbCharacters
        '
        Me.gbCharacters.Controls.Add(Me.chkUseCorp)
        Me.gbCharacters.Controls.Add(Me.lvwCharacters)
        Me.gbCharacters.Controls.Add(Me.btnFetchKillMails)
        Me.gbCharacters.Location = New System.Drawing.Point(550, 12)
        Me.gbCharacters.Name = "gbCharacters"
        Me.gbCharacters.Size = New System.Drawing.Size(279, 147)
        Me.gbCharacters.TabIndex = 1
        Me.gbCharacters.TabStop = False
        Me.gbCharacters.Text = "Available Characters"
        '
        'chkUseCorp
        '
        Me.chkUseCorp.AutoSize = True
        Me.chkUseCorp.Location = New System.Drawing.Point(6, 94)
        Me.chkUseCorp.Name = "chkUseCorp"
        Me.chkUseCorp.Size = New System.Drawing.Size(107, 17)
        Me.chkUseCorp.TabIndex = 7
        Me.chkUseCorp.Text = "Get Corp Killmails"
        Me.chkUseCorp.UseVisualStyleBackColor = True
        '
        'lvwCharacters
        '
        Me.lvwCharacters.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colCharacterName})
        Me.lvwCharacters.FullRowSelect = True
        Me.lvwCharacters.GridLines = True
        Me.lvwCharacters.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.lvwCharacters.HideSelection = False
        Me.lvwCharacters.Location = New System.Drawing.Point(6, 19)
        Me.lvwCharacters.Name = "lvwCharacters"
        Me.lvwCharacters.Size = New System.Drawing.Size(267, 69)
        Me.lvwCharacters.TabIndex = 6
        Me.lvwCharacters.UseCompatibleStateImageBehavior = False
        Me.lvwCharacters.View = System.Windows.Forms.View.Details
        '
        'colCharacterName
        '
        Me.colCharacterName.Width = 240
        '
        'btnFetchKillMails
        '
        Me.btnFetchKillMails.Enabled = False
        Me.btnFetchKillMails.Location = New System.Drawing.Point(173, 116)
        Me.btnFetchKillMails.Name = "btnFetchKillMails"
        Me.btnFetchKillMails.Size = New System.Drawing.Size(100, 23)
        Me.btnFetchKillMails.TabIndex = 5
        Me.btnFetchKillMails.Text = "Fetch Killmails"
        Me.btnFetchKillMails.UseVisualStyleBackColor = True
        '
        'lblKMSummary
        '
        Me.lblKMSummary.AutoSize = True
        Me.lblKMSummary.Location = New System.Drawing.Point(12, 172)
        Me.lblKMSummary.Name = "lblKMSummary"
        Me.lblKMSummary.Size = New System.Drawing.Size(84, 13)
        Me.lblKMSummary.TabIndex = 2
        Me.lblKMSummary.Text = "Killmail Summary"
        '
        'lvwKillMails
        '
        Me.lvwKillMails.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lvwKillMails.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colVictim, Me.colShip, Me.colKillTime})
        Me.lvwKillMails.FullRowSelect = True
        Me.lvwKillMails.GridLines = True
        Me.lvwKillMails.HideSelection = False
        Me.lvwKillMails.Location = New System.Drawing.Point(12, 188)
        Me.lvwKillMails.Name = "lvwKillMails"
        Me.lvwKillMails.ShowItemToolTips = True
        Me.lvwKillMails.Size = New System.Drawing.Size(405, 411)
        Me.lvwKillMails.TabIndex = 3
        Me.lvwKillMails.UseCompatibleStateImageBehavior = False
        Me.lvwKillMails.View = System.Windows.Forms.View.Details
        '
        'colVictim
        '
        Me.colVictim.Text = "Victim"
        Me.colVictim.Width = 125
        '
        'colShip
        '
        Me.colShip.Text = "Ship Lost"
        Me.colShip.Width = 125
        '
        'colKillTime
        '
        Me.colKillTime.Text = "Kill Time"
        Me.colKillTime.Width = 125
        '
        'lblKillmailDetails
        '
        Me.lblKillmailDetails.AutoSize = True
        Me.lblKillmailDetails.Location = New System.Drawing.Point(420, 172)
        Me.lblKillmailDetails.Name = "lblKillmailDetails"
        Me.lblKillmailDetails.Size = New System.Drawing.Size(72, 13)
        Me.lblKillmailDetails.TabIndex = 5
        Me.lblKillmailDetails.Text = "Killmail Details"
        '
        'txtKillMailDetails
        '
        Me.txtKillMailDetails.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtKillMailDetails.Location = New System.Drawing.Point(423, 188)
        Me.txtKillMailDetails.Multiline = True
        Me.txtKillMailDetails.Name = "txtKillMailDetails"
        Me.txtKillMailDetails.ReadOnly = True
        Me.txtKillMailDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.txtKillMailDetails.Size = New System.Drawing.Size(445, 440)
        Me.txtKillMailDetails.TabIndex = 6
        '
        'radUseAccount
        '
        Me.radUseAccount.AutoSize = True
        Me.radUseAccount.Checked = True
        Me.radUseAccount.Location = New System.Drawing.Point(17, 20)
        Me.radUseAccount.Name = "radUseAccount"
        Me.radUseAccount.Size = New System.Drawing.Size(129, 17)
        Me.radUseAccount.TabIndex = 6
        Me.radUseAccount.Text = "Use Existing Account:"
        Me.radUseAccount.UseVisualStyleBackColor = True
        '
        'radUseAPI
        '
        Me.radUseAPI.AutoSize = True
        Me.radUseAPI.Location = New System.Drawing.Point(17, 43)
        Me.radUseAPI.Name = "radUseAPI"
        Me.radUseAPI.Size = New System.Drawing.Size(106, 17)
        Me.radUseAPI.TabIndex = 7
        Me.radUseAPI.Text = "Use Specific API:"
        Me.radUseAPI.UseVisualStyleBackColor = True
        '
        'cboAccount
        '
        Me.cboAccount.FormattingEnabled = True
        Me.cboAccount.Location = New System.Drawing.Point(152, 19)
        Me.cboAccount.Name = "cboAccount"
        Me.cboAccount.Size = New System.Drawing.Size(146, 21)
        Me.cboAccount.Sorted = True
        Me.cboAccount.TabIndex = 8
        '
        'btnUploadToBC
        '
        Me.btnUploadToBC.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnUploadToBC.Location = New System.Drawing.Point(12, 605)
        Me.btnUploadToBC.Name = "btnUploadToBC"
        Me.btnUploadToBC.Size = New System.Drawing.Size(123, 23)
        Me.btnUploadToBC.TabIndex = 7
        Me.btnUploadToBC.Text = "Upload to BattleClinic"
        Me.btnUploadToBC.UseVisualStyleBackColor = True
        '
        'frmKMV
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(880, 640)
        Me.Controls.Add(Me.btnUploadToBC)
        Me.Controls.Add(Me.txtKillMailDetails)
        Me.Controls.Add(Me.lblKillmailDetails)
        Me.Controls.Add(Me.lvwKillMails)
        Me.Controls.Add(Me.lblKMSummary)
        Me.Controls.Add(Me.gbCharacters)
        Me.Controls.Add(Me.gbAPIInfo)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmKMV"
        Me.Text = "EveHQ Killmail Viewer"
        Me.gbAPIInfo.ResumeLayout(False)
        Me.gbAPIInfo.PerformLayout()
        Me.gbCharacters.ResumeLayout(False)
        Me.gbCharacters.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gbAPIInfo As System.Windows.Forms.GroupBox
    Friend WithEvents btnGetCharacters As System.Windows.Forms.Button
    Friend WithEvents txtAPIKey As System.Windows.Forms.TextBox
    Friend WithEvents lblAPIKey As System.Windows.Forms.Label
    Friend WithEvents txtUserID As System.Windows.Forms.TextBox
    Friend WithEvents lblUserID As System.Windows.Forms.Label
    Friend WithEvents gbCharacters As System.Windows.Forms.GroupBox
    Friend WithEvents lblAPIStatus As System.Windows.Forms.Label
    Friend WithEvents lvwCharacters As System.Windows.Forms.ListView
    Friend WithEvents btnFetchKillMails As System.Windows.Forms.Button
    Friend WithEvents colCharacterName As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblKMSummary As System.Windows.Forms.Label
    Friend WithEvents lvwKillMails As System.Windows.Forms.ListView
    Friend WithEvents colVictim As System.Windows.Forms.ColumnHeader
    Friend WithEvents colShip As System.Windows.Forms.ColumnHeader
    Friend WithEvents colKillTime As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblKillmailDetails As System.Windows.Forms.Label
    Friend WithEvents txtKillMailDetails As System.Windows.Forms.TextBox
    Friend WithEvents chkUseCorp As System.Windows.Forms.CheckBox
    Friend WithEvents radUseAPI As System.Windows.Forms.RadioButton
    Friend WithEvents radUseAccount As System.Windows.Forms.RadioButton
    Friend WithEvents cboAccount As System.Windows.Forms.ComboBox
    Friend WithEvents btnUploadToBC As System.Windows.Forms.Button
End Class
