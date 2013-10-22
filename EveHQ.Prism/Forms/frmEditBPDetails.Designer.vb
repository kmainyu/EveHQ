Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class FrmEditBPDetails
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
            Me.lblBPName = New System.Windows.Forms.Label
            Me.lblAssetID = New System.Windows.Forms.Label
            Me.lblMELevel = New System.Windows.Forms.Label
            Me.lblStatus = New System.Windows.Forms.Label
            Me.lblRuns = New System.Windows.Forms.Label
            Me.lblPELevel = New System.Windows.Forms.Label
            Me.lblCurrent = New System.Windows.Forms.Label
            Me.lblNew = New System.Windows.Forms.Label
            Me.lblCurrentME = New System.Windows.Forms.Label
            Me.lblCurrentPE = New System.Windows.Forms.Label
            Me.lblCurrentRuns = New System.Windows.Forms.Label
            Me.lblCurrentStatus = New System.Windows.Forms.Label
            Me.pbBP = New System.Windows.Forms.PictureBox
            Me.pnlBPDetails = New DevComponents.DotNetBar.PanelEx
            Me.btnAccept = New DevComponents.DotNetBar.ButtonX
            Me.btnCancel = New DevComponents.DotNetBar.ButtonX
            Me.cboStatus = New DevComponents.DotNetBar.Controls.ComboBoxEx
            Me.nudMELevel = New DevComponents.Editors.IntegerInput
            Me.nudPELevel = New DevComponents.Editors.IntegerInput
            Me.nudRuns = New DevComponents.Editors.IntegerInput
            CType(Me.pbBP, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlBPDetails.SuspendLayout()
            CType(Me.nudMELevel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudPELevel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.nudRuns, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'lblBPName
            '
            Me.lblBPName.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblBPName.Location = New System.Drawing.Point(83, 13)
            Me.lblBPName.Name = "lblBPName"
            Me.lblBPName.Size = New System.Drawing.Size(306, 43)
            Me.lblBPName.TabIndex = 12
            Me.lblBPName.Text = "Blueprint Name"
            '
            'lblAssetID
            '
            Me.lblAssetID.AutoSize = True
            Me.lblAssetID.Location = New System.Drawing.Point(84, 64)
            Me.lblAssetID.Name = "lblAssetID"
            Me.lblAssetID.Size = New System.Drawing.Size(52, 13)
            Me.lblAssetID.TabIndex = 13
            Me.lblAssetID.Text = "AssetID: "
            '
            'lblMELevel
            '
            Me.lblMELevel.AutoSize = True
            Me.lblMELevel.Location = New System.Drawing.Point(37, 122)
            Me.lblMELevel.Name = "lblMELevel"
            Me.lblMELevel.Size = New System.Drawing.Size(53, 13)
            Me.lblMELevel.TabIndex = 14
            Me.lblMELevel.Text = "ME Level:"
            '
            'lblStatus
            '
            Me.lblStatus.AutoSize = True
            Me.lblStatus.Location = New System.Drawing.Point(37, 205)
            Me.lblStatus.Name = "lblStatus"
            Me.lblStatus.Size = New System.Drawing.Size(42, 13)
            Me.lblStatus.TabIndex = 15
            Me.lblStatus.Text = "Status:"
            '
            'lblRuns
            '
            Me.lblRuns.AutoSize = True
            Me.lblRuns.Location = New System.Drawing.Point(37, 178)
            Me.lblRuns.Name = "lblRuns"
            Me.lblRuns.Size = New System.Drawing.Size(35, 13)
            Me.lblRuns.TabIndex = 16
            Me.lblRuns.Text = "Runs:"
            '
            'lblPELevel
            '
            Me.lblPELevel.AutoSize = True
            Me.lblPELevel.Location = New System.Drawing.Point(37, 151)
            Me.lblPELevel.Name = "lblPELevel"
            Me.lblPELevel.Size = New System.Drawing.Size(51, 13)
            Me.lblPELevel.TabIndex = 17
            Me.lblPELevel.Text = "PE Level:"
            '
            'lblCurrent
            '
            Me.lblCurrent.AutoSize = True
            Me.lblCurrent.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblCurrent.Location = New System.Drawing.Point(166, 96)
            Me.lblCurrent.Name = "lblCurrent"
            Me.lblCurrent.Size = New System.Drawing.Size(50, 13)
            Me.lblCurrent.TabIndex = 18
            Me.lblCurrent.Text = "Current"
            '
            'lblNew
            '
            Me.lblNew.AutoSize = True
            Me.lblNew.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblNew.Location = New System.Drawing.Point(278, 96)
            Me.lblNew.Name = "lblNew"
            Me.lblNew.Size = New System.Drawing.Size(30, 13)
            Me.lblNew.TabIndex = 19
            Me.lblNew.Text = "New"
            '
            'lblCurrentME
            '
            Me.lblCurrentME.Location = New System.Drawing.Point(137, 122)
            Me.lblCurrentME.Name = "lblCurrentME"
            Me.lblCurrentME.Size = New System.Drawing.Size(73, 13)
            Me.lblCurrentME.TabIndex = 24
            Me.lblCurrentME.Text = "0"
            Me.lblCurrentME.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblCurrentPE
            '
            Me.lblCurrentPE.Location = New System.Drawing.Point(134, 151)
            Me.lblCurrentPE.Name = "lblCurrentPE"
            Me.lblCurrentPE.Size = New System.Drawing.Size(76, 13)
            Me.lblCurrentPE.TabIndex = 25
            Me.lblCurrentPE.Text = "0"
            Me.lblCurrentPE.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblCurrentRuns
            '
            Me.lblCurrentRuns.Location = New System.Drawing.Point(131, 178)
            Me.lblCurrentRuns.Name = "lblCurrentRuns"
            Me.lblCurrentRuns.Size = New System.Drawing.Size(79, 13)
            Me.lblCurrentRuns.TabIndex = 26
            Me.lblCurrentRuns.Text = "0"
            Me.lblCurrentRuns.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblCurrentStatus
            '
            Me.lblCurrentStatus.Location = New System.Drawing.Point(128, 205)
            Me.lblCurrentStatus.Name = "lblCurrentStatus"
            Me.lblCurrentStatus.Size = New System.Drawing.Size(82, 13)
            Me.lblCurrentStatus.TabIndex = 27
            Me.lblCurrentStatus.Text = "0"
            Me.lblCurrentStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'pbBP
            '
            Me.pbBP.BackColor = System.Drawing.SystemColors.ButtonShadow
            Me.pbBP.Location = New System.Drawing.Point(13, 13)
            Me.pbBP.Name = "pbBP"
            Me.pbBP.Size = New System.Drawing.Size(64, 64)
            Me.pbBP.TabIndex = 11
            Me.pbBP.TabStop = False
            '
            'pnlBPDetails
            '
            Me.pnlBPDetails.CanvasColor = System.Drawing.SystemColors.Control
            Me.pnlBPDetails.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.pnlBPDetails.Controls.Add(Me.nudRuns)
            Me.pnlBPDetails.Controls.Add(Me.nudPELevel)
            Me.pnlBPDetails.Controls.Add(Me.nudMELevel)
            Me.pnlBPDetails.Controls.Add(Me.cboStatus)
            Me.pnlBPDetails.Controls.Add(Me.btnCancel)
            Me.pnlBPDetails.Controls.Add(Me.btnAccept)
            Me.pnlBPDetails.Controls.Add(Me.pbBP)
            Me.pnlBPDetails.Controls.Add(Me.lblBPName)
            Me.pnlBPDetails.Controls.Add(Me.lblAssetID)
            Me.pnlBPDetails.Controls.Add(Me.lblCurrentStatus)
            Me.pnlBPDetails.Controls.Add(Me.lblMELevel)
            Me.pnlBPDetails.Controls.Add(Me.lblCurrentRuns)
            Me.pnlBPDetails.Controls.Add(Me.lblStatus)
            Me.pnlBPDetails.Controls.Add(Me.lblCurrentPE)
            Me.pnlBPDetails.Controls.Add(Me.lblRuns)
            Me.pnlBPDetails.Controls.Add(Me.lblCurrentME)
            Me.pnlBPDetails.Controls.Add(Me.lblPELevel)
            Me.pnlBPDetails.Controls.Add(Me.lblCurrent)
            Me.pnlBPDetails.Controls.Add(Me.lblNew)
            Me.pnlBPDetails.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlBPDetails.Location = New System.Drawing.Point(0, 0)
            Me.pnlBPDetails.Name = "pnlBPDetails"
            Me.pnlBPDetails.Size = New System.Drawing.Size(366, 272)
            Me.pnlBPDetails.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.pnlBPDetails.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.pnlBPDetails.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.pnlBPDetails.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.pnlBPDetails.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.pnlBPDetails.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.pnlBPDetails.Style.GradientAngle = 90
            Me.pnlBPDetails.TabIndex = 30
            '
            'btnAccept
            '
            Me.btnAccept.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnAccept.CallBasePaintBackground = True
            Me.btnAccept.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnAccept.Location = New System.Drawing.Point(200, 237)
            Me.btnAccept.Name = "btnAccept"
            Me.btnAccept.Size = New System.Drawing.Size(75, 23)
            Me.btnAccept.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnAccept.TabIndex = 30
            Me.btnAccept.Text = "Accept"
            '
            'btnCancel
            '
            Me.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnCancel.CallBasePaintBackground = True
            Me.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(281, 237)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnCancel.TabIndex = 31
            Me.btnCancel.Text = "Cancel"
            '
            'cboStatus
            '
            Me.cboStatus.DisplayMember = "Text"
            Me.cboStatus.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.cboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboStatus.FormattingEnabled = True
            Me.cboStatus.ItemHeight = 15
            Me.cboStatus.Location = New System.Drawing.Point(250, 201)
            Me.cboStatus.Name = "cboStatus"
            Me.cboStatus.Size = New System.Drawing.Size(94, 21)
            Me.cboStatus.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.cboStatus.TabIndex = 32
            '
            'nudMELevel
            '
            '
            '
            '
            Me.nudMELevel.BackgroundStyle.Class = "DateTimeInputBackground"
            Me.nudMELevel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.nudMELevel.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
            Me.nudMELevel.Location = New System.Drawing.Point(250, 118)
            Me.nudMELevel.MaxValue = 100000
            Me.nudMELevel.MinValue = -10
            Me.nudMELevel.Name = "nudMELevel"
            Me.nudMELevel.ShowUpDown = True
            Me.nudMELevel.Size = New System.Drawing.Size(94, 21)
            Me.nudMELevel.TabIndex = 33
            '
            'nudPELevel
            '
            '
            '
            '
            Me.nudPELevel.BackgroundStyle.Class = "DateTimeInputBackground"
            Me.nudPELevel.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.nudPELevel.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
            Me.nudPELevel.Location = New System.Drawing.Point(250, 147)
            Me.nudPELevel.MaxValue = 100000
            Me.nudPELevel.MinValue = -10
            Me.nudPELevel.Name = "nudPELevel"
            Me.nudPELevel.ShowUpDown = True
            Me.nudPELevel.Size = New System.Drawing.Size(94, 21)
            Me.nudPELevel.TabIndex = 34
            '
            'nudRuns
            '
            '
            '
            '
            Me.nudRuns.BackgroundStyle.Class = "DateTimeInputBackground"
            Me.nudRuns.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.nudRuns.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
            Me.nudRuns.Location = New System.Drawing.Point(250, 174)
            Me.nudRuns.MaxValue = 100000
            Me.nudRuns.MinValue = -10
            Me.nudRuns.Name = "nudRuns"
            Me.nudRuns.ShowUpDown = True
            Me.nudRuns.Size = New System.Drawing.Size(94, 21)
            Me.nudRuns.TabIndex = 35
            '
            'frmEditBPDetails
            '
            Me.AcceptButton = Me.btnAccept
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(366, 272)
            Me.Controls.Add(Me.pnlBPDetails)
            Me.DoubleBuffered = True
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmEditBPDetails"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Edit Blueprint Details"
            CType(Me.pbBP, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlBPDetails.ResumeLayout(False)
            Me.pnlBPDetails.PerformLayout()
            CType(Me.nudMELevel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudPELevel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.nudRuns, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents pbBP As System.Windows.Forms.PictureBox
        Friend WithEvents lblBPName As System.Windows.Forms.Label
        Friend WithEvents lblAssetID As System.Windows.Forms.Label
        Friend WithEvents lblMELevel As System.Windows.Forms.Label
        Friend WithEvents lblStatus As System.Windows.Forms.Label
        Friend WithEvents lblRuns As System.Windows.Forms.Label
        Friend WithEvents lblPELevel As System.Windows.Forms.Label
        Friend WithEvents lblCurrent As System.Windows.Forms.Label
        Friend WithEvents lblNew As System.Windows.Forms.Label
        Friend WithEvents lblCurrentME As System.Windows.Forms.Label
        Friend WithEvents lblCurrentPE As System.Windows.Forms.Label
        Friend WithEvents lblCurrentRuns As System.Windows.Forms.Label
        Friend WithEvents lblCurrentStatus As System.Windows.Forms.Label
        Friend WithEvents pnlBPDetails As DevComponents.DotNetBar.PanelEx
        Friend WithEvents nudRuns As DevComponents.Editors.IntegerInput
        Friend WithEvents nudPELevel As DevComponents.Editors.IntegerInput
        Friend WithEvents nudMELevel As DevComponents.Editors.IntegerInput
        Friend WithEvents cboStatus As DevComponents.DotNetBar.Controls.ComboBoxEx
        Friend WithEvents btnCancel As DevComponents.DotNetBar.ButtonX
        Friend WithEvents btnAccept As DevComponents.DotNetBar.ButtonX
    End Class
End NameSpace