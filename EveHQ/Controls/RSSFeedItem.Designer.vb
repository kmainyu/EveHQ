Namespace Controls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class RSSFeedItem
        Inherits System.Windows.Forms.UserControl

        'UserControl overrides dispose to clean up the component list.
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
            Me.lblFeeItemDate = New DevComponents.DotNetBar.LabelX
            Me.lblFeedItemTitle = New System.Windows.Forms.LinkLabel
            Me.pnlRSS = New DevComponents.DotNetBar.PanelEx
            Me.pnlRSS.SuspendLayout()
            Me.SuspendLayout()
            '
            'lblFeeItemDate
            '
            Me.lblFeeItemDate.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                                              Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            '
            '
            '
            Me.lblFeeItemDate.BackgroundStyle.Class = ""
            Me.lblFeeItemDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblFeeItemDate.Location = New System.Drawing.Point(9, 21)
            Me.lblFeeItemDate.Name = "lblFeeItemDate"
            Me.lblFeeItemDate.Size = New System.Drawing.Size(167, 16)
            Me.lblFeeItemDate.TabIndex = 1
            Me.lblFeeItemDate.Text = "Feed Item Date"
            '
            'lblFeedItemTitle
            '
            Me.lblFeedItemTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                                                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblFeedItemTitle.AutoEllipsis = True
            Me.lblFeedItemTitle.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblFeedItemTitle.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
            Me.lblFeedItemTitle.LinkColor = System.Drawing.Color.Black
            Me.lblFeedItemTitle.Location = New System.Drawing.Point(3, 3)
            Me.lblFeedItemTitle.Name = "lblFeedItemTitle"
            Me.lblFeedItemTitle.Size = New System.Drawing.Size(173, 22)
            Me.lblFeedItemTitle.TabIndex = 2
            Me.lblFeedItemTitle.TabStop = True
            Me.lblFeedItemTitle.Text = "Feed Item Title"
            '
            'pnlRSS
            '
            Me.pnlRSS.CanvasColor = System.Drawing.SystemColors.Control
            Me.pnlRSS.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.pnlRSS.Controls.Add(Me.lblFeeItemDate)
            Me.pnlRSS.Controls.Add(Me.lblFeedItemTitle)
            Me.pnlRSS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlRSS.Location = New System.Drawing.Point(0, 0)
            Me.pnlRSS.Name = "pnlRSS"
            Me.pnlRSS.Size = New System.Drawing.Size(176, 40)
            Me.pnlRSS.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.pnlRSS.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.pnlRSS.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.pnlRSS.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.pnlRSS.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.pnlRSS.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.pnlRSS.Style.GradientAngle = 180
            Me.pnlRSS.TabIndex = 3
            '
            'RSSFeedItem
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.pnlRSS)
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Name = "RSSFeedItem"
            Me.Size = New System.Drawing.Size(176, 40)
            Me.pnlRSS.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents lblFeeItemDate As DevComponents.DotNetBar.LabelX
        Friend WithEvents lblFeedItemTitle As System.Windows.Forms.LinkLabel
        Friend WithEvents pnlRSS As DevComponents.DotNetBar.PanelEx

    End Class
End NameSpace