﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.lblCharacter = New System.Windows.Forms.Label()
        Me.cboCharacter = New System.Windows.Forms.ComboBox()
        Me.lblAPIMethod = New System.Windows.Forms.Label()
        Me.cboAPIMethod = New System.Windows.Forms.ComboBox()
        Me.cboAccount = New System.Windows.Forms.ComboBox()
        Me.lblAccount = New System.Windows.Forms.Label()
        Me.lblOtherInfo = New System.Windows.Forms.Label()
        Me.txtOtherInfo = New System.Windows.Forms.TextBox()
        Me.wbAPI = New System.Windows.Forms.WebBrowser()
        Me.lblCurrentlyViewing = New System.Windows.Forms.Label()
        Me.lblFileLocation = New System.Windows.Forms.Label()
        Me.btnFetchAPI = New DevComponents.DotNetBar.ButtonX()
        Me.chkReturnCachedXML = New DevComponents.DotNetBar.Controls.CheckBoxX()
        Me.chkReturnActualXML = New DevComponents.DotNetBar.Controls.CheckBoxX()
        Me.pnlAPIChecker = New DevComponents.DotNetBar.PanelEx()
        Me.pnlAPIChecker.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblCharacter
        '
        Me.lblCharacter.AutoSize = True
        Me.lblCharacter.Enabled = False
        Me.lblCharacter.Location = New System.Drawing.Point(308, 12)
        Me.lblCharacter.Name = "lblCharacter"
        Me.lblCharacter.Size = New System.Drawing.Size(59, 13)
        Me.lblCharacter.TabIndex = 0
        Me.lblCharacter.Text = "Character:"
        '
        'cboCharacter
        '
        Me.cboCharacter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboCharacter.Enabled = False
        Me.cboCharacter.FormattingEnabled = True
        Me.cboCharacter.Location = New System.Drawing.Point(395, 9)
        Me.cboCharacter.Name = "cboCharacter"
        Me.cboCharacter.Size = New System.Drawing.Size(166, 21)
        Me.cboCharacter.Sorted = True
        Me.cboCharacter.TabIndex = 1
        '
        'lblAPIMethod
        '
        Me.lblAPIMethod.AutoSize = True
        Me.lblAPIMethod.Location = New System.Drawing.Point(12, 12)
        Me.lblAPIMethod.Name = "lblAPIMethod"
        Me.lblAPIMethod.Size = New System.Drawing.Size(67, 13)
        Me.lblAPIMethod.TabIndex = 2
        Me.lblAPIMethod.Text = "API Method:"
        '
        'cboAPIMethod
        '
        Me.cboAPIMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAPIMethod.FormattingEnabled = True
        Me.cboAPIMethod.Location = New System.Drawing.Point(84, 9)
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
        Me.cboAccount.Location = New System.Drawing.Point(395, 36)
        Me.cboAccount.Name = "cboAccount"
        Me.cboAccount.Size = New System.Drawing.Size(166, 21)
        Me.cboAccount.Sorted = True
        Me.cboAccount.TabIndex = 5
        '
        'lblAccount
        '
        Me.lblAccount.AutoSize = True
        Me.lblAccount.Enabled = False
        Me.lblAccount.Location = New System.Drawing.Point(308, 39)
        Me.lblAccount.Name = "lblAccount"
        Me.lblAccount.Size = New System.Drawing.Size(50, 13)
        Me.lblAccount.TabIndex = 4
        Me.lblAccount.Text = "Account:"
        '
        'lblOtherInfo
        '
        Me.lblOtherInfo.AutoSize = True
        Me.lblOtherInfo.Enabled = False
        Me.lblOtherInfo.Location = New System.Drawing.Point(308, 66)
        Me.lblOtherInfo.Name = "lblOtherInfo"
        Me.lblOtherInfo.Size = New System.Drawing.Size(74, 13)
        Me.lblOtherInfo.TabIndex = 6
        Me.lblOtherInfo.Text = "Before RefID:"
        '
        'txtOtherInfo
        '
        Me.txtOtherInfo.Enabled = False
        Me.txtOtherInfo.Location = New System.Drawing.Point(395, 63)
        Me.txtOtherInfo.Name = "txtOtherInfo"
        Me.txtOtherInfo.Size = New System.Drawing.Size(166, 21)
        Me.txtOtherInfo.TabIndex = 7
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
        Me.btnFetchAPI.Location = New System.Drawing.Point(15, 66)
        Me.btnFetchAPI.Name = "btnFetchAPI"
        Me.btnFetchAPI.Size = New System.Drawing.Size(75, 23)
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
        Me.chkReturnCachedXML.BackgroundStyle.Class = ""
        Me.chkReturnCachedXML.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.chkReturnCachedXML.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton
        Me.chkReturnCachedXML.Location = New System.Drawing.Point(15, 36)
        Me.chkReturnCachedXML.Name = "chkReturnCachedXML"
        Me.chkReturnCachedXML.Size = New System.Drawing.Size(143, 16)
        Me.chkReturnCachedXML.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.chkReturnCachedXML.TabIndex = 14
        Me.chkReturnCachedXML.Text = "Return Cached XML Only"
        '
        'chkReturnActualXML
        '
        Me.chkReturnActualXML.AutoSize = True
        '
        '
        '
        Me.chkReturnActualXML.BackgroundStyle.Class = ""
        Me.chkReturnActualXML.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.chkReturnActualXML.CheckBoxStyle = DevComponents.DotNetBar.eCheckBoxStyle.RadioButton
        Me.chkReturnActualXML.Location = New System.Drawing.Point(164, 36)
        Me.chkReturnActualXML.Name = "chkReturnActualXML"
        Me.chkReturnActualXML.Size = New System.Drawing.Size(112, 16)
        Me.chkReturnActualXML.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.chkReturnActualXML.TabIndex = 15
        Me.chkReturnActualXML.Text = "Return Actual XML"
        '
        'pnlAPIChecker
        '
        Me.pnlAPIChecker.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlAPIChecker.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlAPIChecker.Controls.Add(Me.lblAPIMethod)
        Me.pnlAPIChecker.Controls.Add(Me.chkReturnActualXML)
        Me.pnlAPIChecker.Controls.Add(Me.lblCharacter)
        Me.pnlAPIChecker.Controls.Add(Me.chkReturnCachedXML)
        Me.pnlAPIChecker.Controls.Add(Me.cboCharacter)
        Me.pnlAPIChecker.Controls.Add(Me.btnFetchAPI)
        Me.pnlAPIChecker.Controls.Add(Me.cboAPIMethod)
        Me.pnlAPIChecker.Controls.Add(Me.lblFileLocation)
        Me.pnlAPIChecker.Controls.Add(Me.lblAccount)
        Me.pnlAPIChecker.Controls.Add(Me.lblCurrentlyViewing)
        Me.pnlAPIChecker.Controls.Add(Me.cboAccount)
        Me.pnlAPIChecker.Controls.Add(Me.wbAPI)
        Me.pnlAPIChecker.Controls.Add(Me.lblOtherInfo)
        Me.pnlAPIChecker.Controls.Add(Me.txtOtherInfo)
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
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.pnlAPIChecker.ResumeLayout(False)
        Me.pnlAPIChecker.PerformLayout()
        Me.ResumeLayout(False)

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
    Friend WithEvents lblCurrentlyViewing As System.Windows.Forms.Label
    Friend WithEvents lblFileLocation As System.Windows.Forms.Label
    Friend WithEvents btnFetchAPI As DevComponents.DotNetBar.ButtonX
    Friend WithEvents chkReturnCachedXML As DevComponents.DotNetBar.Controls.CheckBoxX
    Friend WithEvents chkReturnActualXML As DevComponents.DotNetBar.Controls.CheckBoxX
    Friend WithEvents pnlAPIChecker As DevComponents.DotNetBar.PanelEx
End Class
