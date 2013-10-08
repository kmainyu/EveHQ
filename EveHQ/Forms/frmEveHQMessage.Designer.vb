Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmEveHQMessage
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
            Me.btnClose = New DevComponents.DotNetBar.ButtonX()
            Me.chkIgnore = New DevComponents.DotNetBar.Controls.CheckBoxX()
            Me.lblMessage = New DevComponents.DotNetBar.LabelX()
            Me.PanelEx1 = New DevComponents.DotNetBar.PanelEx()
            Me.lblTitle = New DevComponents.DotNetBar.LabelX()
            Me.ReflectionImage1 = New DevComponents.DotNetBar.Controls.ReflectionImage()
            Me.PanelEx1.SuspendLayout()
            Me.SuspendLayout()
            '
            'btnClose
            '
            Me.btnClose.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
            Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnClose.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
            Me.btnClose.Location = New System.Drawing.Point(307, 350)
            Me.btnClose.Name = "btnClose"
            Me.btnClose.Size = New System.Drawing.Size(75, 23)
            Me.btnClose.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.btnClose.TabIndex = 1
            Me.btnClose.Text = "Close"
            '
            'chkIgnore
            '
            Me.chkIgnore.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            '
            '
            '
            Me.chkIgnore.BackgroundStyle.Class = ""
            Me.chkIgnore.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.chkIgnore.Location = New System.Drawing.Point(12, 350)
            Me.chkIgnore.Name = "chkIgnore"
            Me.chkIgnore.Size = New System.Drawing.Size(221, 16)
            Me.chkIgnore.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.chkIgnore.TabIndex = 2
            Me.chkIgnore.Text = "Do Not Show This Message Again"
            '
            'lblMessage
            '
            Me.lblMessage.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                                           Or System.Windows.Forms.AnchorStyles.Left) _
                                          Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            '
            '
            '
            Me.lblMessage.BackgroundStyle.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.lblMessage.BackgroundStyle.BorderBottomWidth = 1
            Me.lblMessage.BackgroundStyle.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2
            Me.lblMessage.BackgroundStyle.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.lblMessage.BackgroundStyle.BorderLeftWidth = 1
            Me.lblMessage.BackgroundStyle.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.lblMessage.BackgroundStyle.BorderRightWidth = 1
            Me.lblMessage.BackgroundStyle.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.lblMessage.BackgroundStyle.BorderTopWidth = 1
            Me.lblMessage.BackgroundStyle.Class = ""
            Me.lblMessage.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblMessage.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
            Me.lblMessage.Location = New System.Drawing.Point(12, 72)
            Me.lblMessage.Name = "lblMessage"
            Me.lblMessage.PaddingBottom = 3
            Me.lblMessage.PaddingLeft = 3
            Me.lblMessage.PaddingRight = 3
            Me.lblMessage.PaddingTop = 3
            Me.lblMessage.SingleLineColor = System.Drawing.Color.Transparent
            Me.lblMessage.Size = New System.Drawing.Size(370, 272)
            Me.lblMessage.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.lblMessage.TabIndex = 3
            Me.lblMessage.TextLineAlignment = System.Drawing.StringAlignment.Near
            Me.lblMessage.WordWrap = True
            '
            'PanelEx1
            '
            Me.PanelEx1.CanvasColor = System.Drawing.SystemColors.Control
            Me.PanelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.PanelEx1.Controls.Add(Me.lblTitle)
            Me.PanelEx1.Controls.Add(Me.ReflectionImage1)
            Me.PanelEx1.Controls.Add(Me.lblMessage)
            Me.PanelEx1.Controls.Add(Me.btnClose)
            Me.PanelEx1.Controls.Add(Me.chkIgnore)
            Me.PanelEx1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.PanelEx1.Location = New System.Drawing.Point(0, 0)
            Me.PanelEx1.Name = "PanelEx1"
            Me.PanelEx1.Size = New System.Drawing.Size(394, 376)
            Me.PanelEx1.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.PanelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.PanelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.PanelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.PanelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.PanelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.PanelEx1.Style.GradientAngle = 90
            Me.PanelEx1.TabIndex = 4
            '
            'lblTitle
            '
            '
            '
            '
            Me.lblTitle.BackgroundStyle.BorderBottomWidth = 1
            Me.lblTitle.BackgroundStyle.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2
            Me.lblTitle.BackgroundStyle.BorderLeftWidth = 1
            Me.lblTitle.BackgroundStyle.BorderRightWidth = 1
            Me.lblTitle.BackgroundStyle.BorderTopWidth = 1
            Me.lblTitle.BackgroundStyle.Class = ""
            Me.lblTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblTitle.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTitle.ImagePosition = DevComponents.DotNetBar.eImagePosition.Top
            Me.lblTitle.Location = New System.Drawing.Point(53, 12)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.PaddingBottom = 3
            Me.lblTitle.PaddingLeft = 3
            Me.lblTitle.PaddingRight = 3
            Me.lblTitle.PaddingTop = 3
            Me.lblTitle.SingleLineColor = System.Drawing.Color.Transparent
            Me.lblTitle.Size = New System.Drawing.Size(329, 54)
            Me.lblTitle.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.lblTitle.TabIndex = 5
            Me.lblTitle.TextAlignment = System.Drawing.StringAlignment.Center
            Me.lblTitle.TextLineAlignment = System.Drawing.StringAlignment.Near
            Me.lblTitle.WordWrap = True
            '
            'ReflectionImage1
            '
            '
            '
            '
            Me.ReflectionImage1.BackgroundStyle.Class = ""
            Me.ReflectionImage1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.ReflectionImage1.BackgroundStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
            Me.ReflectionImage1.Image = Global.EveHQ.My.Resources.Resources.Info32
            Me.ReflectionImage1.Location = New System.Drawing.Point(12, 3)
            Me.ReflectionImage1.Name = "ReflectionImage1"
            Me.ReflectionImage1.Size = New System.Drawing.Size(35, 63)
            Me.ReflectionImage1.TabIndex = 4
            '
            'frmEveHQMessage
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(394, 376)
            Me.ControlBox = False
            Me.Controls.Add(Me.PanelEx1)
            Me.DoubleBuffered = True
            Me.EnableGlass = False
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
            Me.Name = "frmEveHQMessage"
            Me.ShowIcon = False
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "EveHQ Message"
            Me.PanelEx1.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents btnClose As DevComponents.DotNetBar.ButtonX
        Friend WithEvents chkIgnore As DevComponents.DotNetBar.Controls.CheckBoxX
        Friend WithEvents lblMessage As DevComponents.DotNetBar.LabelX
        Friend WithEvents PanelEx1 As DevComponents.DotNetBar.PanelEx
        Friend WithEvents lblTitle As DevComponents.DotNetBar.LabelX
        Friend WithEvents ReflectionImage1 As DevComponents.DotNetBar.Controls.ReflectionImage
    End Class
End NameSpace