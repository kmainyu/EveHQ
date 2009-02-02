<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmAPIChecker
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAPIChecker))
        Me.lblCharacter = New System.Windows.Forms.Label
        Me.cboCharacter = New System.Windows.Forms.ComboBox
        Me.lblAPIMethod = New System.Windows.Forms.Label
        Me.cboAPIMethod = New System.Windows.Forms.ComboBox
        Me.cboAccount = New System.Windows.Forms.ComboBox
        Me.lblAccount = New System.Windows.Forms.Label
        Me.lblOtherInfo = New System.Windows.Forms.Label
        Me.txtOtherInfo = New System.Windows.Forms.TextBox
        Me.wbAPI = New System.Windows.Forms.WebBrowser
        Me.btnGetAPI = New System.Windows.Forms.Button
        Me.lblCurrentlyViewing = New System.Windows.Forms.Label
        Me.lblFileLocation = New System.Windows.Forms.Label
        Me.chkReturnCached = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'lblCharacter
        '
        Me.lblCharacter.AutoSize = True
        Me.lblCharacter.Enabled = False
        Me.lblCharacter.Location = New System.Drawing.Point(309, 13)
        Me.lblCharacter.Name = "lblCharacter"
        Me.lblCharacter.Size = New System.Drawing.Size(56, 13)
        Me.lblCharacter.TabIndex = 0
        Me.lblCharacter.Text = "Character:"
        '
        'cboCharacter
        '
        Me.cboCharacter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCharacter.Enabled = False
        Me.cboCharacter.FormattingEnabled = True
        Me.cboCharacter.Location = New System.Drawing.Point(396, 10)
        Me.cboCharacter.Name = "cboCharacter"
        Me.cboCharacter.Size = New System.Drawing.Size(166, 21)
        Me.cboCharacter.Sorted = True
        Me.cboCharacter.TabIndex = 1
        '
        'lblAPIMethod
        '
        Me.lblAPIMethod.AutoSize = True
        Me.lblAPIMethod.Location = New System.Drawing.Point(13, 13)
        Me.lblAPIMethod.Name = "lblAPIMethod"
        Me.lblAPIMethod.Size = New System.Drawing.Size(66, 13)
        Me.lblAPIMethod.TabIndex = 2
        Me.lblAPIMethod.Text = "API Method:"
        '
        'cboAPIMethod
        '
        Me.cboAPIMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAPIMethod.FormattingEnabled = True
        Me.cboAPIMethod.Location = New System.Drawing.Point(85, 10)
        Me.cboAPIMethod.Name = "cboAPIMethod"
        Me.cboAPIMethod.Size = New System.Drawing.Size(166, 21)
        Me.cboAPIMethod.Sorted = True
        Me.cboAPIMethod.TabIndex = 3
        '
        'cboAccount
        '
        Me.cboAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAccount.Enabled = False
        Me.cboAccount.FormattingEnabled = True
        Me.cboAccount.Location = New System.Drawing.Point(396, 37)
        Me.cboAccount.Name = "cboAccount"
        Me.cboAccount.Size = New System.Drawing.Size(166, 21)
        Me.cboAccount.Sorted = True
        Me.cboAccount.TabIndex = 5
        '
        'lblAccount
        '
        Me.lblAccount.AutoSize = True
        Me.lblAccount.Enabled = False
        Me.lblAccount.Location = New System.Drawing.Point(309, 40)
        Me.lblAccount.Name = "lblAccount"
        Me.lblAccount.Size = New System.Drawing.Size(50, 13)
        Me.lblAccount.TabIndex = 4
        Me.lblAccount.Text = "Account:"
        '
        'lblOtherInfo
        '
        Me.lblOtherInfo.AutoSize = True
        Me.lblOtherInfo.Enabled = False
        Me.lblOtherInfo.Location = New System.Drawing.Point(309, 67)
        Me.lblOtherInfo.Name = "lblOtherInfo"
        Me.lblOtherInfo.Size = New System.Drawing.Size(72, 13)
        Me.lblOtherInfo.TabIndex = 6
        Me.lblOtherInfo.Text = "Before RefID:"
        '
        'txtOtherInfo
        '
        Me.txtOtherInfo.Enabled = False
        Me.txtOtherInfo.Location = New System.Drawing.Point(396, 64)
        Me.txtOtherInfo.Name = "txtOtherInfo"
        Me.txtOtherInfo.Size = New System.Drawing.Size(166, 20)
        Me.txtOtherInfo.TabIndex = 7
        '
        'wbAPI
        '
        Me.wbAPI.AllowWebBrowserDrop = False
        Me.wbAPI.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.wbAPI.IsWebBrowserContextMenuEnabled = False
        Me.wbAPI.Location = New System.Drawing.Point(12, 133)
        Me.wbAPI.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wbAPI.Name = "wbAPI"
        Me.wbAPI.Size = New System.Drawing.Size(865, 541)
        Me.wbAPI.TabIndex = 8
        Me.wbAPI.WebBrowserShortcutsEnabled = False
        '
        'btnGetAPI
        '
        Me.btnGetAPI.Location = New System.Drawing.Point(16, 62)
        Me.btnGetAPI.Name = "btnGetAPI"
        Me.btnGetAPI.Size = New System.Drawing.Size(75, 23)
        Me.btnGetAPI.TabIndex = 9
        Me.btnGetAPI.Text = "Get API"
        Me.btnGetAPI.UseVisualStyleBackColor = True
        '
        'lblCurrentlyViewing
        '
        Me.lblCurrentlyViewing.AutoSize = True
        Me.lblCurrentlyViewing.Location = New System.Drawing.Point(9, 104)
        Me.lblCurrentlyViewing.Name = "lblCurrentlyViewing"
        Me.lblCurrentlyViewing.Size = New System.Drawing.Size(111, 13)
        Me.lblCurrentlyViewing.TabIndex = 10
        Me.lblCurrentlyViewing.Text = "Currently Viewing: n/a"
        '
        'lblFileLocation
        '
        Me.lblFileLocation.AutoSize = True
        Me.lblFileLocation.Location = New System.Drawing.Point(9, 117)
        Me.lblFileLocation.Name = "lblFileLocation"
        Me.lblFileLocation.Size = New System.Drawing.Size(124, 13)
        Me.lblFileLocation.TabIndex = 11
        Me.lblFileLocation.Text = "Cache File Location: n/a"
        '
        'chkReturnCached
        '
        Me.chkReturnCached.AutoSize = True
        Me.chkReturnCached.Location = New System.Drawing.Point(85, 35)
        Me.chkReturnCached.Name = "chkReturnCached"
        Me.chkReturnCached.Size = New System.Drawing.Size(147, 17)
        Me.chkReturnCached.TabIndex = 12
        Me.chkReturnCached.Text = "Return Cached XML Only"
        Me.chkReturnCached.UseVisualStyleBackColor = True
        '
        'frmAPIChecker
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(889, 686)
        Me.Controls.Add(Me.chkReturnCached)
        Me.Controls.Add(Me.lblFileLocation)
        Me.Controls.Add(Me.lblCurrentlyViewing)
        Me.Controls.Add(Me.btnGetAPI)
        Me.Controls.Add(Me.wbAPI)
        Me.Controls.Add(Me.txtOtherInfo)
        Me.Controls.Add(Me.lblOtherInfo)
        Me.Controls.Add(Me.cboAccount)
        Me.Controls.Add(Me.lblAccount)
        Me.Controls.Add(Me.cboAPIMethod)
        Me.Controls.Add(Me.lblAPIMethod)
        Me.Controls.Add(Me.cboCharacter)
        Me.Controls.Add(Me.lblCharacter)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmAPIChecker"
        Me.Text = "API Checker"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblCharacter As System.Windows.Forms.Label
    Friend WithEvents cboCharacter As System.Windows.Forms.ComboBox
    Friend WithEvents lblAPIMethod As System.Windows.Forms.Label
    Friend WithEvents cboAPIMethod As System.Windows.Forms.ComboBox
    Friend WithEvents cboAccount As System.Windows.Forms.ComboBox
    Friend WithEvents lblAccount As System.Windows.Forms.Label
    Friend WithEvents lblOtherInfo As System.Windows.Forms.Label
    Friend WithEvents txtOtherInfo As System.Windows.Forms.TextBox
    Friend WithEvents wbAPI As System.Windows.Forms.WebBrowser
    Friend WithEvents btnGetAPI As System.Windows.Forms.Button
    Friend WithEvents lblCurrentlyViewing As System.Windows.Forms.Label
    Friend WithEvents lblFileLocation As System.Windows.Forms.Label
    Friend WithEvents chkReturnCached As System.Windows.Forms.CheckBox
End Class
