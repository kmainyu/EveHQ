Namespace Controls.DBControls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class DBCRSSFeed
        Inherits Widget

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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DBCRSSFeed))
            Me.pnlFeedItems = New DevComponents.DotNetBar.PanelEx()
            Me.cpFeed = New DevComponents.DotNetBar.Controls.CircularProgress()
            Me.AGPContent.SuspendLayout()
            CType(Me.pbConfig, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlFeedItems.SuspendLayout()
            Me.SuspendLayout()
            '
            'lblHeader
            '
            '
            '
            '
            Me.lblHeader.BackgroundStyle.Class = ""
            Me.lblHeader.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblHeader.Image = CType(resources.GetObject("lblHeader.Image"), System.Drawing.Image)
            Me.lblHeader.Text = "RSS Feed"
            '
            'AGPContent
            '
            Me.AGPContent.Controls.Add(Me.pnlFeedItems)
            Me.AGPContent.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.AGPContent.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.AGPContent.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.AGPContent.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.AGPContent.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.AGPContent.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.AGPContent.Style.GradientAngle = 90
            Me.AGPContent.Controls.SetChildIndex(Me.pnlFeedItems, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lblHeader, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.pbConfig, 0)
            '
            'pnlFeedItems
            '
            Me.pnlFeedItems.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                                             Or System.Windows.Forms.AnchorStyles.Left) _
                                            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.pnlFeedItems.AutoScroll = True
            Me.pnlFeedItems.CanvasColor = System.Drawing.SystemColors.Control
            Me.pnlFeedItems.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.pnlFeedItems.Controls.Add(Me.cpFeed)
            Me.pnlFeedItems.Location = New System.Drawing.Point(6, 35)
            Me.pnlFeedItems.Name = "pnlFeedItems"
            Me.pnlFeedItems.Size = New System.Drawing.Size(288, 172)
            Me.pnlFeedItems.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.pnlFeedItems.Style.BackColor1.Alpha = CType(0, Byte)
            Me.pnlFeedItems.Style.BackColor1.Color = System.Drawing.Color.Transparent
            Me.pnlFeedItems.Style.BackColor2.Alpha = CType(0, Byte)
            Me.pnlFeedItems.Style.BackColor2.Color = System.Drawing.Color.Transparent
            Me.pnlFeedItems.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.pnlFeedItems.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
            Me.pnlFeedItems.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.pnlFeedItems.Style.GradientAngle = 90
            Me.pnlFeedItems.TabIndex = 15
            '
            'cpFeed
            '
            '
            '
            '
            Me.cpFeed.BackgroundStyle.Class = ""
            Me.cpFeed.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.cpFeed.Location = New System.Drawing.Point(3, 3)
            Me.cpFeed.Name = "cpFeed"
            Me.cpFeed.Size = New System.Drawing.Size(48, 48)
            Me.cpFeed.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.cpFeed.TabIndex = 0
            '
            'DBCRSSFeed
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Name = "DBCRSSFeed"
            Me.AGPContent.ResumeLayout(False)
            CType(Me.pbConfig, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlFeedItems.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents pnlFeedItems As DevComponents.DotNetBar.PanelEx
        Friend WithEvents cpFeed As DevComponents.DotNetBar.Controls.CircularProgress

    End Class
End NameSpace