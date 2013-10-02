Namespace Requisitions
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class FrmMergeRequisitions
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
            Me.PanelEx1 = New DevComponents.DotNetBar.PanelEx()
            Me.cboReqs = New DevComponents.DotNetBar.Controls.ComboBoxEx()
            Me.btnCancel = New DevComponents.DotNetBar.ButtonX()
            Me.btnAccept = New DevComponents.DotNetBar.ButtonX()
            Me.lblDescription = New System.Windows.Forms.Label()
            Me.lblReqs = New System.Windows.Forms.Label()
            Me.chkRetainOldReqs = New DevComponents.DotNetBar.Controls.CheckBoxX()
            Me.PanelEx1.SuspendLayout()
            Me.SuspendLayout()
            '
            'PanelEx1
            '
            Me.PanelEx1.CanvasColor = System.Drawing.SystemColors.Control
            Me.PanelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.PanelEx1.Controls.Add(Me.chkRetainOldReqs)
            Me.PanelEx1.Controls.Add(Me.cboReqs)
            Me.PanelEx1.Controls.Add(Me.btnCancel)
            Me.PanelEx1.Controls.Add(Me.btnAccept)
            Me.PanelEx1.Controls.Add(Me.lblDescription)
            Me.PanelEx1.Controls.Add(Me.lblReqs)
            Me.PanelEx1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.PanelEx1.Location = New System.Drawing.Point(0, 0)
            Me.PanelEx1.Name = "PanelEx1"
            Me.PanelEx1.Size = New System.Drawing.Size(373, 150)
            Me.PanelEx1.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.PanelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.PanelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.PanelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.PanelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.PanelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.PanelEx1.Style.GradientAngle = 90
            Me.PanelEx1.TabIndex = 17
            '
            'cboReqs
            '
            Me.cboReqs.DisplayMember = "Text"
            Me.cboReqs.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.cboReqs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboReqs.FormattingEnabled = True
            Me.cboReqs.ItemHeight = 15
            Me.cboReqs.Location = New System.Drawing.Point(102, 48)
            Me.cboReqs.Name = "cboReqs"
            Me.cboReqs.Size = New System.Drawing.Size(257, 21)
            Me.cboReqs.Sorted = True
            Me.cboReqs.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.cboReqs.TabIndex = 18
            '
            'btnCancel
            '
            Me.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(286, 117)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnCancel.TabIndex = 17
            Me.btnCancel.Text = "Cancel"
            '
            'btnAccept
            '
            Me.btnAccept.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnAccept.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnAccept.Location = New System.Drawing.Point(205, 117)
            Me.btnAccept.Name = "btnAccept"
            Me.btnAccept.Size = New System.Drawing.Size(75, 23)
            Me.btnAccept.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnAccept.TabIndex = 16
            Me.btnAccept.Text = "Merge"
            '
            'lblDescription
            '
            Me.lblDescription.Location = New System.Drawing.Point(12, 9)
            Me.lblDescription.Name = "lblDescription"
            Me.lblDescription.Size = New System.Drawing.Size(349, 27)
            Me.lblDescription.TabIndex = 13
            Me.lblDescription.Text = "Select the name of the Requisition that will host the new items..."
            Me.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblReqs
            '
            Me.lblReqs.AutoSize = True
            Me.lblReqs.Location = New System.Drawing.Point(3, 51)
            Me.lblReqs.Name = "lblReqs"
            Me.lblReqs.Size = New System.Drawing.Size(93, 13)
            Me.lblReqs.TabIndex = 9
            Me.lblReqs.Text = "Requisition Name:"
            '
            'chkRetainOldReqs
            '
            Me.chkRetainOldReqs.AutoSize = True
            '
            '
            '
            Me.chkRetainOldReqs.BackgroundStyle.Class = ""
            Me.chkRetainOldReqs.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.chkRetainOldReqs.Location = New System.Drawing.Point(102, 76)
            Me.chkRetainOldReqs.Name = "chkRetainOldReqs"
            Me.chkRetainOldReqs.Size = New System.Drawing.Size(148, 16)
            Me.chkRetainOldReqs.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.chkRetainOldReqs.TabIndex = 19
            Me.chkRetainOldReqs.Text = "Keep Merged Requisitions"
            '
            'frmMergeRequisitions
            '
            Me.AcceptButton = Me.btnAccept
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.btnCancel
            Me.ClientSize = New System.Drawing.Size(373, 150)
            Me.Controls.Add(Me.PanelEx1)
            Me.DoubleBuffered = True
            Me.EnableGlass = False
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmMergeRequisitions"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Merge Requisitions"
            Me.PanelEx1.ResumeLayout(False)
            Me.PanelEx1.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents PanelEx1 As DevComponents.DotNetBar.PanelEx
        Friend WithEvents cboReqs As DevComponents.DotNetBar.Controls.ComboBoxEx
        Friend WithEvents btnCancel As DevComponents.DotNetBar.ButtonX
        Friend WithEvents btnAccept As DevComponents.DotNetBar.ButtonX
        Friend WithEvents lblDescription As System.Windows.Forms.Label
        Friend WithEvents lblReqs As System.Windows.Forms.Label
        Friend WithEvents chkRetainOldReqs As DevComponents.DotNetBar.Controls.CheckBoxX
    End Class
End NameSpace