Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmAPIChecker
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmAPIChecker))
            Me.lblAPIType = New System.Windows.Forms.Label()
            Me.lblWalletAccount = New System.Windows.Forms.Label()
            Me.lblOtherInfo = New System.Windows.Forms.Label()
            Me.wbAPI = New System.Windows.Forms.WebBrowser()
            Me.lblCurrentlyViewing = New System.Windows.Forms.Label()
            Me.lblFileLocation = New System.Windows.Forms.Label()
            Me.btnFetchAPI = New DevComponents.DotNetBar.ButtonX()
            Me.chkReturnCachedXML = New DevComponents.DotNetBar.Controls.CheckBoxX()
            Me.chkReturnActualXML = New DevComponents.DotNetBar.Controls.CheckBoxX()
            Me.pnlAPIChecker = New DevComponents.DotNetBar.PanelEx()
            Me.chkReturnStandardXML = New DevComponents.DotNetBar.Controls.CheckBoxX()
            Me.txtOtherInfo = New DevComponents.DotNetBar.Controls.TextBoxX()
            Me.cboWalletAccount = New DevComponents.DotNetBar.Controls.ComboBoxEx()
            Me.cboAPIOwner = New DevComponents.DotNetBar.Controls.ComboBoxEx()
            Me.lblAPIOwner = New System.Windows.Forms.Label()
            Me.cboAPIType = New DevComponents.DotNetBar.Controls.ComboBoxEx()
            Me.cboAPICategory = New DevComponents.DotNetBar.Controls.ComboBoxEx()
            Me.ciCharacter = New DevComponents.Editors.ComboItem()
            Me.ciCorporation = New DevComponents.Editors.ComboItem()
            Me.ciAccount = New DevComponents.Editors.ComboItem()
            Me.ciStatic = New DevComponents.Editors.ComboItem()
            Me.lblAPICategory = New System.Windows.Forms.Label()
            Me.pnlAPIChecker.SuspendLayout()
            Me.SuspendLayout()
            '
            'lblAPIType
            '
            Me.lblAPIType.AutoSize = True
            Me.lblAPIType.Location = New System.Drawing.Point(8, 42)
            Me.lblAPIType.Name = "lblAPIType"
            Me.lblAPIType.Size = New System.Drawing.Size(55, 13)
            Me.lblAPIType.TabIndex = 2
            Me.lblAPIType.Text = "API Type:"
            '
            'lblWalletAccount
            '
            Me.lblWalletAccount.AutoSize = True
            Me.lblWalletAccount.Enabled = False
            Me.lblWalletAccount.Location = New System.Drawing.Point(319, 15)
            Me.lblWalletAccount.Name = "lblWalletAccount"
            Me.lblWalletAccount.Size = New System.Drawing.Size(83, 13)
            Me.lblWalletAccount.TabIndex = 4
            Me.lblWalletAccount.Text = "Wallet Account:"
            '
            'lblOtherInfo
            '
            Me.lblOtherInfo.AutoSize = True
            Me.lblOtherInfo.Enabled = False
            Me.lblOtherInfo.Location = New System.Drawing.Point(319, 44)
            Me.lblOtherInfo.Name = "lblOtherInfo"
            Me.lblOtherInfo.Size = New System.Drawing.Size(62, 13)
            Me.lblOtherInfo.TabIndex = 6
            Me.lblOtherInfo.Text = "Other Info:"
            '
            'wbAPI
            '
            Me.wbAPI.AllowWebBrowserDrop = False
            Me.wbAPI.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                                      Or System.Windows.Forms.AnchorStyles.Left) _
                                     Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.wbAPI.IsWebBrowserContextMenuEnabled = False
            Me.wbAPI.Location = New System.Drawing.Point(11, 132)
            Me.wbAPI.MinimumSize = New System.Drawing.Size(20, 20)
            Me.wbAPI.Name = "wbAPI"
            Me.wbAPI.Size = New System.Drawing.Size(1150, 699)
            Me.wbAPI.TabIndex = 8
            Me.wbAPI.WebBrowserShortcutsEnabled = False
            '
            'lblCurrentlyViewing
            '
            Me.lblCurrentlyViewing.AutoSize = True
            Me.lblCurrentlyViewing.Location = New System.Drawing.Point(8, 103)
            Me.lblCurrentlyViewing.Name = "lblCurrentlyViewing"
            Me.lblCurrentlyViewing.Size = New System.Drawing.Size(114, 13)
            Me.lblCurrentlyViewing.TabIndex = 10
            Me.lblCurrentlyViewing.Text = "Currently Viewing: n/a"
            '
            'lblFileLocation
            '
            Me.lblFileLocation.AutoSize = True
            Me.lblFileLocation.Location = New System.Drawing.Point(8, 116)
            Me.lblFileLocation.Name = "lblFileLocation"
            Me.lblFileLocation.Size = New System.Drawing.Size(122, 13)
            Me.lblFileLocation.TabIndex = 11
            Me.lblFileLocation.Text = "Cache File Location: n/a"
            '
            'btnFetchAPI
            '
            Me.btnFetchAPI.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnFetchAPI.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnFetchAPI.Location = New System.Drawing.Point(408, 66)
            Me.btnFetchAPI.Name = "btnFetchAPI"
            Me.btnFetchAPI.Size = New System.Drawing.Size(75, 21)
            Me.btnFetchAPI.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnFetchAPI.TabIndex = 13
            Me.btnFetchAPI.Text = "Check API"
            '
            'chkReturnCachedXML
            '
            Me.chkReturnCachedXML.AutoSize = True
            '
            '
            '
            Me.chkReturnCachedXML.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.chkReturnCachedXML.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton
            Me.chkReturnCachedXML.Location = New System.Drawing.Point(710, 15)
            Me.chkReturnCachedXML.Name = "chkReturnCachedXML"
            Me.chkReturnCachedXML.Size = New System.Drawing.Size(82, 16)
            Me.chkReturnCachedXML.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.chkReturnCachedXML.TabIndex = 14
            Me.chkReturnCachedXML.TabStop = False
            Me.chkReturnCachedXML.Text = "Cached XML"
            '
            'chkReturnActualXML
            '
            Me.chkReturnActualXML.AutoSize = True
            '
            '
            '
            Me.chkReturnActualXML.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.chkReturnActualXML.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton
            Me.chkReturnActualXML.Location = New System.Drawing.Point(798, 15)
            Me.chkReturnActualXML.Name = "chkReturnActualXML"
            Me.chkReturnActualXML.Size = New System.Drawing.Size(76, 16)
            Me.chkReturnActualXML.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.chkReturnActualXML.TabIndex = 15
            Me.chkReturnActualXML.Text = "Actual XML"
            '
            'pnlAPIChecker
            '
            Me.pnlAPIChecker.CanvasColor = System.Drawing.SystemColors.Control
            Me.pnlAPIChecker.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.pnlAPIChecker.Controls.Add(Me.chkReturnStandardXML)
            Me.pnlAPIChecker.Controls.Add(Me.txtOtherInfo)
            Me.pnlAPIChecker.Controls.Add(Me.cboWalletAccount)
            Me.pnlAPIChecker.Controls.Add(Me.cboAPIOwner)
            Me.pnlAPIChecker.Controls.Add(Me.lblAPIOwner)
            Me.pnlAPIChecker.Controls.Add(Me.cboAPIType)
            Me.pnlAPIChecker.Controls.Add(Me.cboAPICategory)
            Me.pnlAPIChecker.Controls.Add(Me.lblAPICategory)
            Me.pnlAPIChecker.Controls.Add(Me.lblAPIType)
            Me.pnlAPIChecker.Controls.Add(Me.chkReturnActualXML)
            Me.pnlAPIChecker.Controls.Add(Me.chkReturnCachedXML)
            Me.pnlAPIChecker.Controls.Add(Me.btnFetchAPI)
            Me.pnlAPIChecker.Controls.Add(Me.lblFileLocation)
            Me.pnlAPIChecker.Controls.Add(Me.lblWalletAccount)
            Me.pnlAPIChecker.Controls.Add(Me.lblCurrentlyViewing)
            Me.pnlAPIChecker.Controls.Add(Me.wbAPI)
            Me.pnlAPIChecker.Controls.Add(Me.lblOtherInfo)
            Me.pnlAPIChecker.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlAPIChecker.Location = New System.Drawing.Point(0, 0)
            Me.pnlAPIChecker.Name = "pnlAPIChecker"
            Me.pnlAPIChecker.Size = New System.Drawing.Size(1173, 845)
            Me.pnlAPIChecker.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.pnlAPIChecker.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.pnlAPIChecker.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.pnlAPIChecker.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.pnlAPIChecker.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.pnlAPIChecker.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.pnlAPIChecker.Style.GradientAngle = 90
            Me.pnlAPIChecker.TabIndex = 16
            '
            'chkReturnStandardXML
            '
            Me.chkReturnStandardXML.AutoSize = True
            '
            '
            '
            Me.chkReturnStandardXML.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.chkReturnStandardXML.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton
            Me.chkReturnStandardXML.Checked = True
            Me.chkReturnStandardXML.CheckState = System.Windows.Forms.CheckState.Checked
            Me.chkReturnStandardXML.CheckValue = "Y"
            Me.chkReturnStandardXML.Location = New System.Drawing.Point(596, 15)
            Me.chkReturnStandardXML.Name = "chkReturnStandardXML"
            Me.chkReturnStandardXML.Size = New System.Drawing.Size(108, 16)
            Me.chkReturnStandardXML.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.chkReturnStandardXML.TabIndex = 23
            Me.chkReturnStandardXML.Text = "Standard Caching"
            '
            'txtOtherInfo
            '
            '
            '
            '
            Me.txtOtherInfo.Border.Class = "TextBoxBorder"
            Me.txtOtherInfo.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.txtOtherInfo.Enabled = False
            Me.txtOtherInfo.Location = New System.Drawing.Point(408, 39)
            Me.txtOtherInfo.Name = "txtOtherInfo"
            Me.txtOtherInfo.Size = New System.Drawing.Size(466, 21)
            Me.txtOtherInfo.TabIndex = 22
            '
            'cboWalletAccount
            '
            Me.cboWalletAccount.DisplayMember = "Text"
            Me.cboWalletAccount.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.cboWalletAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboWalletAccount.Enabled = False
            Me.cboWalletAccount.FormattingEnabled = True
            Me.cboWalletAccount.ItemHeight = 15
            Me.cboWalletAccount.Location = New System.Drawing.Point(408, 12)
            Me.cboWalletAccount.Name = "cboWalletAccount"
            Me.cboWalletAccount.Size = New System.Drawing.Size(170, 21)
            Me.cboWalletAccount.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.cboWalletAccount.TabIndex = 21
            '
            'cboAPIOwner
            '
            Me.cboAPIOwner.DisplayMember = "Text"
            Me.cboAPIOwner.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.cboAPIOwner.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboAPIOwner.FormattingEnabled = True
            Me.cboAPIOwner.ItemHeight = 15
            Me.cboAPIOwner.Location = New System.Drawing.Point(90, 66)
            Me.cboAPIOwner.Name = "cboAPIOwner"
            Me.cboAPIOwner.Size = New System.Drawing.Size(192, 21)
            Me.cboAPIOwner.Sorted = True
            Me.cboAPIOwner.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.cboAPIOwner.TabIndex = 20
            '
            'lblAPIOwner
            '
            Me.lblAPIOwner.AutoSize = True
            Me.lblAPIOwner.Location = New System.Drawing.Point(8, 69)
            Me.lblAPIOwner.Name = "lblAPIOwner"
            Me.lblAPIOwner.Size = New System.Drawing.Size(63, 13)
            Me.lblAPIOwner.TabIndex = 19
            Me.lblAPIOwner.Text = "API Owner:"
            '
            'cboAPIType
            '
            Me.cboAPIType.DisplayMember = "Text"
            Me.cboAPIType.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.cboAPIType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboAPIType.FormattingEnabled = True
            Me.cboAPIType.ItemHeight = 15
            Me.cboAPIType.Location = New System.Drawing.Point(90, 39)
            Me.cboAPIType.Name = "cboAPIType"
            Me.cboAPIType.Size = New System.Drawing.Size(192, 21)
            Me.cboAPIType.Sorted = True
            Me.cboAPIType.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.cboAPIType.TabIndex = 18
            '
            'cboAPICategory
            '
            Me.cboAPICategory.DisplayMember = "Text"
            Me.cboAPICategory.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.cboAPICategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboAPICategory.FormattingEnabled = True
            Me.cboAPICategory.ItemHeight = 15
            Me.cboAPICategory.Items.AddRange(New Object() {Me.ciCharacter, Me.ciCorporation, Me.ciAccount, Me.ciStatic})
            Me.cboAPICategory.Location = New System.Drawing.Point(90, 12)
            Me.cboAPICategory.Name = "cboAPICategory"
            Me.cboAPICategory.Size = New System.Drawing.Size(192, 21)
            Me.cboAPICategory.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.cboAPICategory.TabIndex = 17
            '
            'ciCharacter
            '
            Me.ciCharacter.Text = "Character"
            '
            'ciCorporation
            '
            Me.ciCorporation.Text = "Corporation"
            '
            'ciAccount
            '
            Me.ciAccount.Text = "Account"
            '
            'ciStatic
            '
            Me.ciStatic.Text = "Static"
            '
            'lblAPICategory
            '
            Me.lblAPICategory.AutoSize = True
            Me.lblAPICategory.Location = New System.Drawing.Point(8, 15)
            Me.lblAPICategory.Name = "lblAPICategory"
            Me.lblAPICategory.Size = New System.Drawing.Size(76, 13)
            Me.lblAPICategory.TabIndex = 16
            Me.lblAPICategory.Text = "API Category:"
            '
            'frmAPIChecker
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1173, 845)
            Me.Controls.Add(Me.pnlAPIChecker)
            Me.DoubleBuffered = True
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.Name = "frmAPIChecker"
            Me.Text = "API Checker"
            Me.pnlAPIChecker.ResumeLayout(False)
            Me.pnlAPIChecker.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents lblAPIType As System.Windows.Forms.Label
        Friend WithEvents lblWalletAccount As System.Windows.Forms.Label
        Friend WithEvents lblOtherInfo As System.Windows.Forms.Label
        Friend WithEvents wbAPI As System.Windows.Forms.WebBrowser
        Friend WithEvents lblCurrentlyViewing As System.Windows.Forms.Label
        Friend WithEvents lblFileLocation As System.Windows.Forms.Label
        Friend WithEvents btnFetchAPI As DevComponents.DotNetBar.ButtonX
        Friend WithEvents chkReturnCachedXML As DevComponents.DotNetBar.Controls.CheckBoxX
        Friend WithEvents chkReturnActualXML As DevComponents.DotNetBar.Controls.CheckBoxX
        Friend WithEvents pnlAPIChecker As DevComponents.DotNetBar.PanelEx
        Friend WithEvents cboAPIType As DevComponents.DotNetBar.Controls.ComboBoxEx
        Friend WithEvents cboAPICategory As DevComponents.DotNetBar.Controls.ComboBoxEx
        Friend WithEvents ciCharacter As DevComponents.Editors.ComboItem
        Friend WithEvents ciCorporation As DevComponents.Editors.ComboItem
        Friend WithEvents ciAccount As DevComponents.Editors.ComboItem
        Friend WithEvents ciStatic As DevComponents.Editors.ComboItem
        Friend WithEvents lblAPICategory As System.Windows.Forms.Label
        Friend WithEvents cboAPIOwner As DevComponents.DotNetBar.Controls.ComboBoxEx
        Friend WithEvents lblAPIOwner As System.Windows.Forms.Label
        Friend WithEvents cboWalletAccount As DevComponents.DotNetBar.Controls.ComboBoxEx
        Friend WithEvents txtOtherInfo As DevComponents.DotNetBar.Controls.TextBoxX
        Friend WithEvents chkReturnStandardXML As DevComponents.DotNetBar.Controls.CheckBoxX
    End Class
End NameSpace