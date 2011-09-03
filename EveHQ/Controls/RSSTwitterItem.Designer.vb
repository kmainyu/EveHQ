<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RSSTwitterItem
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RSSTwitterItem))
        Me.lblFeeItemDate = New DevComponents.DotNetBar.LabelX()
        Me.pnlRSS = New DevComponents.DotNetBar.PanelEx()
        Me.lblFeedItemTitle = New DevComponents.DotNetBar.LabelX()
        Me.picLogo = New DevComponents.DotNetBar.Controls.ReflectionImage()
        Me.pnlRSS.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblFeeItemDate
        '
        '
        '
        '
        Me.lblFeeItemDate.BackgroundStyle.Class = ""
        Me.lblFeeItemDate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblFeeItemDate.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFeeItemDate.Location = New System.Drawing.Point(4, 36)
        Me.lblFeeItemDate.Name = "lblFeeItemDate"
        Me.lblFeeItemDate.Size = New System.Drawing.Size(32, 13)
        Me.lblFeeItemDate.TabIndex = 1
        Me.lblFeeItemDate.Text = "Date"
        Me.lblFeeItemDate.TextAlignment = System.Drawing.StringAlignment.Center
        '
        'pnlRSS
        '
        Me.pnlRSS.CanvasColor = System.Drawing.SystemColors.Control
        Me.pnlRSS.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.pnlRSS.Controls.Add(Me.lblFeedItemTitle)
        Me.pnlRSS.Controls.Add(Me.lblFeeItemDate)
        Me.pnlRSS.Controls.Add(Me.picLogo)
        Me.pnlRSS.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlRSS.Location = New System.Drawing.Point(0, 0)
        Me.pnlRSS.Name = "pnlRSS"
        Me.pnlRSS.Size = New System.Drawing.Size(347, 64)
        Me.pnlRSS.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.pnlRSS.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.pnlRSS.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.pnlRSS.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.pnlRSS.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.pnlRSS.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.pnlRSS.Style.GradientAngle = 180
        Me.pnlRSS.TabIndex = 3
        '
        'lblFeedItemTitle
        '
        Me.lblFeedItemTitle.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        '
        '
        '
        Me.lblFeedItemTitle.BackgroundStyle.Class = ""
        Me.lblFeedItemTitle.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblFeedItemTitle.Location = New System.Drawing.Point(42, 4)
        Me.lblFeedItemTitle.Name = "lblFeedItemTitle"
        Me.lblFeedItemTitle.Size = New System.Drawing.Size(302, 57)
        Me.lblFeedItemTitle.TabIndex = 4
        Me.lblFeedItemTitle.Text = "Feed Information"
        Me.lblFeedItemTitle.TextLineAlignment = System.Drawing.StringAlignment.Near
        '
        'picLogo
        '
        '
        '
        '
        Me.picLogo.BackgroundStyle.Class = ""
        Me.picLogo.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.picLogo.BackgroundStyle.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.picLogo.Image = CType(resources.GetObject("picLogo.Image"), System.Drawing.Image)
        Me.picLogo.Location = New System.Drawing.Point(4, 4)
        Me.picLogo.Name = "picLogo"
        Me.picLogo.ReflectionEnabled = False
        Me.picLogo.Size = New System.Drawing.Size(32, 35)
        Me.picLogo.TabIndex = 3
        '
        'RSSTwitterItem
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pnlRSS)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "RSSTwitterItem"
        Me.Size = New System.Drawing.Size(347, 64)
        Me.pnlRSS.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblFeeItemDate As DevComponents.DotNetBar.LabelX
    Friend WithEvents pnlRSS As DevComponents.DotNetBar.PanelEx
    Friend WithEvents picLogo As DevComponents.DotNetBar.Controls.ReflectionImage
    Friend WithEvents lblFeedItemTitle As DevComponents.DotNetBar.LabelX

End Class
