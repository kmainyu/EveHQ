Namespace Forms
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class FrmHelp
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
            Me.components = New System.ComponentModel.Container()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmHelp))
            Me.pnlHelp = New DevComponents.DotNetBar.PanelEx()
            Me.wbHelp = New System.Windows.Forms.WebBrowser()
            Me.pnlFeedItems = New DevComponents.DotNetBar.PanelEx()
            Me.pnlTwitterFeed = New DevComponents.DotNetBar.Controls.GroupPanel()
            Me.pbTwitterUpdate = New System.Windows.Forms.PictureBox()
            Me.pnlTwitterFeedItems = New DevComponents.DotNetBar.PanelEx()
            Me.lblTwitterFeed = New DevComponents.DotNetBar.LabelX()
            Me.pnlBlogFeed = New DevComponents.DotNetBar.Controls.GroupPanel()
            Me.pbBlogUpdate = New System.Windows.Forms.PictureBox()
            Me.pnlBlogFeedItems = New DevComponents.DotNetBar.PanelEx()
            Me.lblBlogFeed = New DevComponents.DotNetBar.LabelX()
            Me.tmrUpdate = New System.Windows.Forms.Timer(Me.components)
            Me.SuperTooltip1 = New DevComponents.DotNetBar.SuperTooltip()
            Me.pnlHelp.SuspendLayout()
            Me.pnlFeedItems.SuspendLayout()
            Me.pnlTwitterFeed.SuspendLayout()
            CType(Me.pbTwitterUpdate, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlBlogFeed.SuspendLayout()
            CType(Me.pbBlogUpdate, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlHelp
            '
            Me.pnlHelp.CanvasColor = System.Drawing.SystemColors.Control
            Me.pnlHelp.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.pnlHelp.Controls.Add(Me.wbHelp)
            Me.pnlHelp.Controls.Add(Me.pnlFeedItems)
            Me.pnlHelp.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlHelp.Location = New System.Drawing.Point(0, 0)
            Me.pnlHelp.Name = "pnlHelp"
            Me.pnlHelp.Size = New System.Drawing.Size(1073, 760)
            Me.pnlHelp.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.pnlHelp.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.pnlHelp.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.pnlHelp.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.pnlHelp.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.pnlHelp.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.pnlHelp.Style.GradientAngle = 90
            Me.pnlHelp.TabIndex = 0
            '
            'wbHelp
            '
            Me.wbHelp.Dock = System.Windows.Forms.DockStyle.Fill
            Me.wbHelp.Location = New System.Drawing.Point(350, 0)
            Me.wbHelp.MinimumSize = New System.Drawing.Size(20, 20)
            Me.wbHelp.Name = "wbHelp"
            Me.wbHelp.Size = New System.Drawing.Size(723, 760)
            Me.wbHelp.TabIndex = 1
            '
            'pnlFeedItems
            '
            Me.pnlFeedItems.CanvasColor = System.Drawing.SystemColors.Control
            Me.pnlFeedItems.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.pnlFeedItems.Controls.Add(Me.pnlTwitterFeed)
            Me.pnlFeedItems.Controls.Add(Me.pnlBlogFeed)
            Me.pnlFeedItems.Dock = System.Windows.Forms.DockStyle.Left
            Me.pnlFeedItems.Location = New System.Drawing.Point(0, 0)
            Me.pnlFeedItems.Name = "pnlFeedItems"
            Me.pnlFeedItems.Size = New System.Drawing.Size(350, 760)
            Me.pnlFeedItems.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.pnlFeedItems.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.pnlFeedItems.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.pnlFeedItems.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.pnlFeedItems.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.pnlFeedItems.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.pnlFeedItems.Style.GradientAngle = 90
            Me.pnlFeedItems.TabIndex = 0
            '
            'pnlTwitterFeed
            '
            Me.pnlTwitterFeed.CanvasColor = System.Drawing.SystemColors.Control
            Me.pnlTwitterFeed.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.pnlTwitterFeed.Controls.Add(Me.pbTwitterUpdate)
            Me.pnlTwitterFeed.Controls.Add(Me.pnlTwitterFeedItems)
            Me.pnlTwitterFeed.Controls.Add(Me.lblTwitterFeed)
            Me.pnlTwitterFeed.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlTwitterFeed.IsShadowEnabled = True
            Me.pnlTwitterFeed.Location = New System.Drawing.Point(0, 400)
            Me.pnlTwitterFeed.Name = "pnlTwitterFeed"
            Me.pnlTwitterFeed.Size = New System.Drawing.Size(350, 360)
            '
            '
            '
            Me.pnlTwitterFeed.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.pnlTwitterFeed.Style.BackColorGradientAngle = 90
            Me.pnlTwitterFeed.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.pnlTwitterFeed.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.pnlTwitterFeed.Style.BorderBottomWidth = 1
            Me.pnlTwitterFeed.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.pnlTwitterFeed.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.pnlTwitterFeed.Style.BorderLeftWidth = 1
            Me.pnlTwitterFeed.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.pnlTwitterFeed.Style.BorderRightWidth = 1
            Me.pnlTwitterFeed.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.pnlTwitterFeed.Style.BorderTopWidth = 1
            Me.pnlTwitterFeed.Style.CornerDiameter = 4
            Me.pnlTwitterFeed.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.pnlTwitterFeed.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
            Me.pnlTwitterFeed.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.pnlTwitterFeed.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
            '
            '
            '
            Me.pnlTwitterFeed.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
            '
            '
            '
            Me.pnlTwitterFeed.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.pnlTwitterFeed.TabIndex = 1
            '
            'pbTwitterUpdate
            '
            Me.pbTwitterUpdate.BackColor = System.Drawing.Color.LightSteelBlue
            Me.pbTwitterUpdate.Image = CType(resources.GetObject("pbTwitterUpdate.Image"), System.Drawing.Image)
            Me.pbTwitterUpdate.Location = New System.Drawing.Point(315, 6)
            Me.pbTwitterUpdate.Name = "pbTwitterUpdate"
            Me.pbTwitterUpdate.Size = New System.Drawing.Size(16, 16)
            Me.pbTwitterUpdate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
            Me.pbTwitterUpdate.TabIndex = 2
            Me.pbTwitterUpdate.TabStop = False
            Me.pbTwitterUpdate.Visible = False
            '
            'pnlTwitterFeedItems
            '
            Me.pnlTwitterFeedItems.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                                                    Or System.Windows.Forms.AnchorStyles.Left) _
                                                   Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.pnlTwitterFeedItems.AutoScroll = True
            Me.pnlTwitterFeedItems.CanvasColor = System.Drawing.SystemColors.Control
            Me.pnlTwitterFeedItems.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.pnlTwitterFeedItems.Location = New System.Drawing.Point(3, 32)
            Me.pnlTwitterFeedItems.Name = "pnlTwitterFeedItems"
            Me.pnlTwitterFeedItems.Size = New System.Drawing.Size(342, 323)
            Me.pnlTwitterFeedItems.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.pnlTwitterFeedItems.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.pnlTwitterFeedItems.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.pnlTwitterFeedItems.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.pnlTwitterFeedItems.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.pnlTwitterFeedItems.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.pnlTwitterFeedItems.Style.GradientAngle = 90
            Me.pnlTwitterFeedItems.TabIndex = 2
            '
            'lblTwitterFeed
            '
            Me.lblTwitterFeed.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                                              Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTwitterFeed.BackColor = System.Drawing.Color.LightSteelBlue
            '
            '
            '
            Me.lblTwitterFeed.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblTwitterFeed.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTwitterFeed.Image = CType(resources.GetObject("lblTwitterFeed.Image"), System.Drawing.Image)
            Me.lblTwitterFeed.Location = New System.Drawing.Point(3, 3)
            Me.lblTwitterFeed.Name = "lblTwitterFeed"
            Me.lblTwitterFeed.Size = New System.Drawing.Size(342, 23)
            Me.lblTwitterFeed.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.SuperTooltip1.SetSuperTooltip(Me.lblTwitterFeed, New DevComponents.DotNetBar.SuperTooltipInfo("", "", "Double-click to refresh the EveHQ Twitter Feed", Nothing, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
            Me.lblTwitterFeed.TabIndex = 1
            Me.lblTwitterFeed.Text = "EveHQ Twitter Feed"
            '
            'pnlBlogFeed
            '
            Me.pnlBlogFeed.CanvasColor = System.Drawing.SystemColors.Control
            Me.pnlBlogFeed.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.pnlBlogFeed.Controls.Add(Me.pbBlogUpdate)
            Me.pnlBlogFeed.Controls.Add(Me.pnlBlogFeedItems)
            Me.pnlBlogFeed.Controls.Add(Me.lblBlogFeed)
            Me.pnlBlogFeed.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlBlogFeed.IsShadowEnabled = True
            Me.pnlBlogFeed.Location = New System.Drawing.Point(0, 0)
            Me.pnlBlogFeed.Name = "pnlBlogFeed"
            Me.pnlBlogFeed.Size = New System.Drawing.Size(350, 400)
            '
            '
            '
            Me.pnlBlogFeed.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.pnlBlogFeed.Style.BackColorGradientAngle = 90
            Me.pnlBlogFeed.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.pnlBlogFeed.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.pnlBlogFeed.Style.BorderBottomWidth = 1
            Me.pnlBlogFeed.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.pnlBlogFeed.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.pnlBlogFeed.Style.BorderLeftWidth = 1
            Me.pnlBlogFeed.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.pnlBlogFeed.Style.BorderRightWidth = 1
            Me.pnlBlogFeed.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
            Me.pnlBlogFeed.Style.BorderTopWidth = 1
            Me.pnlBlogFeed.Style.CornerDiameter = 4
            Me.pnlBlogFeed.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.pnlBlogFeed.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
            Me.pnlBlogFeed.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.pnlBlogFeed.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
            '
            '
            '
            Me.pnlBlogFeed.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
            '
            '
            '
            Me.pnlBlogFeed.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.pnlBlogFeed.TabIndex = 0
            '
            'pbBlogUpdate
            '
            Me.pbBlogUpdate.BackColor = System.Drawing.Color.LightSteelBlue
            Me.pbBlogUpdate.Image = CType(resources.GetObject("pbBlogUpdate.Image"), System.Drawing.Image)
            Me.pbBlogUpdate.Location = New System.Drawing.Point(315, 6)
            Me.pbBlogUpdate.Name = "pbBlogUpdate"
            Me.pbBlogUpdate.Size = New System.Drawing.Size(16, 16)
            Me.pbBlogUpdate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
            Me.pbBlogUpdate.TabIndex = 1
            Me.pbBlogUpdate.TabStop = False
            Me.pbBlogUpdate.Visible = False
            '
            'pnlBlogFeedItems
            '
            Me.pnlBlogFeedItems.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                                                 Or System.Windows.Forms.AnchorStyles.Left) _
                                                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.pnlBlogFeedItems.AutoScroll = True
            Me.pnlBlogFeedItems.CanvasColor = System.Drawing.SystemColors.Control
            Me.pnlBlogFeedItems.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.pnlBlogFeedItems.Location = New System.Drawing.Point(3, 32)
            Me.pnlBlogFeedItems.Name = "pnlBlogFeedItems"
            Me.pnlBlogFeedItems.Size = New System.Drawing.Size(342, 363)
            Me.pnlBlogFeedItems.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.pnlBlogFeedItems.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.pnlBlogFeedItems.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.pnlBlogFeedItems.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.pnlBlogFeedItems.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.pnlBlogFeedItems.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.pnlBlogFeedItems.Style.GradientAngle = 90
            Me.pnlBlogFeedItems.TabIndex = 1
            '
            'lblBlogFeed
            '
            Me.lblBlogFeed.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                                           Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblBlogFeed.BackColor = System.Drawing.Color.LightSteelBlue
            '
            '
            '
            Me.lblBlogFeed.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblBlogFeed.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblBlogFeed.Image = CType(resources.GetObject("lblBlogFeed.Image"), System.Drawing.Image)
            Me.lblBlogFeed.Location = New System.Drawing.Point(3, 3)
            Me.lblBlogFeed.Name = "lblBlogFeed"
            Me.lblBlogFeed.Size = New System.Drawing.Size(342, 23)
            Me.lblBlogFeed.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.SuperTooltip1.SetSuperTooltip(Me.lblBlogFeed, New DevComponents.DotNetBar.SuperTooltipInfo("", "", "Double-click to refresh the EveHQ Blog Feed", Nothing, Nothing, DevComponents.DotNetBar.eTooltipColor.Yellow))
            Me.lblBlogFeed.TabIndex = 0
            Me.lblBlogFeed.Text = "EveHQ Blog Feed"
            '
            'tmrUpdate
            '
            Me.tmrUpdate.Enabled = True
            '
            'SuperTooltip1
            '
            Me.SuperTooltip1.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
            '
            'frmHelp
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1073, 760)
            Me.Controls.Add(Me.pnlHelp)
            Me.DoubleBuffered = True
            Me.EnableGlass = False
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Name = "frmHelp"
            Me.Text = "EveHQ Help and Infomation"
            Me.pnlHelp.ResumeLayout(False)
            Me.pnlFeedItems.ResumeLayout(False)
            Me.pnlTwitterFeed.ResumeLayout(False)
            Me.pnlTwitterFeed.PerformLayout()
            CType(Me.pbTwitterUpdate, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlBlogFeed.ResumeLayout(False)
            Me.pnlBlogFeed.PerformLayout()
            CType(Me.pbBlogUpdate, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents pnlHelp As DevComponents.DotNetBar.PanelEx
        Friend WithEvents pnlFeedItems As DevComponents.DotNetBar.PanelEx
        Friend WithEvents lblBlogFeed As DevComponents.DotNetBar.LabelX
        Friend WithEvents pnlTwitterFeed As DevComponents.DotNetBar.Controls.GroupPanel
        Friend WithEvents pnlTwitterFeedItems As DevComponents.DotNetBar.PanelEx
        Friend WithEvents lblTwitterFeed As DevComponents.DotNetBar.LabelX
        Friend WithEvents pnlBlogFeed As DevComponents.DotNetBar.Controls.GroupPanel
        Friend WithEvents pnlBlogFeedItems As DevComponents.DotNetBar.PanelEx
        Friend WithEvents tmrUpdate As System.Windows.Forms.Timer
        Friend WithEvents pbBlogUpdate As System.Windows.Forms.PictureBox
        Friend WithEvents pbTwitterUpdate As System.Windows.Forms.PictureBox
        Friend WithEvents SuperTooltip1 As DevComponents.DotNetBar.SuperTooltip
        Friend WithEvents wbHelp As System.Windows.Forms.WebBrowser
    End Class
End NameSpace