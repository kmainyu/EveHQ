Namespace Controls.DBControls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class DBCWSystem
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
            Me.lvwEffects = New System.Windows.Forms.ListView
            Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
            Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
            Me.lblSystemClass = New DevComponents.DotNetBar.LabelX
            Me.lblAnomalyName = New DevComponents.DotNetBar.LabelX
            Me.lblSystemClassLbl = New DevComponents.DotNetBar.LabelX
            Me.lblAnomalyNameLbl = New DevComponents.DotNetBar.LabelX
            Me.lblLocus = New DevComponents.DotNetBar.LabelX
            Me.cboWHSystem = New DevComponents.DotNetBar.Controls.ComboBoxEx
            Me.AGPContent.SuspendLayout()
            CType(Me.pbConfig, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'lblHeader
            '
            '
            '
            '
            Me.lblHeader.BackgroundStyle.Class = ""
            Me.lblHeader.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblHeader.Size = New System.Drawing.Size(348, 23)
            Me.lblHeader.Text = "WSpace Information Tool"
            '
            'AGPContent
            '
            Me.AGPContent.Controls.Add(Me.cboWHSystem)
            Me.AGPContent.Controls.Add(Me.lblLocus)
            Me.AGPContent.Controls.Add(Me.lblAnomalyNameLbl)
            Me.AGPContent.Controls.Add(Me.lblSystemClassLbl)
            Me.AGPContent.Controls.Add(Me.lblAnomalyName)
            Me.AGPContent.Controls.Add(Me.lblSystemClass)
            Me.AGPContent.Controls.Add(Me.lvwEffects)
            Me.AGPContent.Size = New System.Drawing.Size(360, 250)
            Me.AGPContent.Style.Alignment = System.Drawing.StringAlignment.Center
            Me.AGPContent.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
            Me.AGPContent.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
            Me.AGPContent.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
            Me.AGPContent.Style.BorderColor.Color = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.AGPContent.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
            Me.AGPContent.Style.GradientAngle = 90
            Me.AGPContent.Controls.SetChildIndex(Me.lblHeader, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.pbConfig, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lvwEffects, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lblSystemClass, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lblAnomalyName, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lblSystemClassLbl, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lblAnomalyNameLbl, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.lblLocus, 0)
            Me.AGPContent.Controls.SetChildIndex(Me.cboWHSystem, 0)
            '
            'pbConfig
            '
            Me.pbConfig.Location = New System.Drawing.Point(330, 8)
            '
            'lvwEffects
            '
            Me.lvwEffects.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                                           Or System.Windows.Forms.AnchorStyles.Left) _
                                          Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lvwEffects.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
            Me.lvwEffects.FullRowSelect = True
            Me.lvwEffects.GridLines = True
            Me.lvwEffects.Location = New System.Drawing.Point(6, 115)
            Me.lvwEffects.Name = "lvwEffects"
            Me.lvwEffects.Size = New System.Drawing.Size(348, 127)
            Me.lvwEffects.TabIndex = 2
            Me.lvwEffects.UseCompatibleStateImageBehavior = False
            Me.lvwEffects.View = System.Windows.Forms.View.Details
            '
            'ColumnHeader1
            '
            Me.ColumnHeader1.Text = "Effect"
            Me.ColumnHeader1.Width = 225
            '
            'ColumnHeader2
            '
            Me.ColumnHeader2.Text = "Value"
            Me.ColumnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.ColumnHeader2.Width = 80
            '
            'lblSystemClass
            '
            Me.lblSystemClass.AutoSize = True
            '
            '
            '
            Me.lblSystemClass.BackgroundStyle.Class = ""
            Me.lblSystemClass.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblSystemClass.Location = New System.Drawing.Point(106, 73)
            Me.lblSystemClass.Name = "lblSystemClass"
            Me.lblSystemClass.Size = New System.Drawing.Size(19, 16)
            Me.lblSystemClass.TabIndex = 22
            Me.lblSystemClass.Text = "n/a"
            '
            'lblAnomalyName
            '
            Me.lblAnomalyName.AutoSize = True
            '
            '
            '
            Me.lblAnomalyName.BackgroundStyle.Class = ""
            Me.lblAnomalyName.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblAnomalyName.Location = New System.Drawing.Point(106, 94)
            Me.lblAnomalyName.Name = "lblAnomalyName"
            Me.lblAnomalyName.Size = New System.Drawing.Size(19, 16)
            Me.lblAnomalyName.TabIndex = 23
            Me.lblAnomalyName.Text = "n/a"
            '
            'lblSystemClassLbl
            '
            Me.lblSystemClassLbl.AutoSize = True
            '
            '
            '
            Me.lblSystemClassLbl.BackgroundStyle.Class = ""
            Me.lblSystemClassLbl.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblSystemClassLbl.Location = New System.Drawing.Point(15, 73)
            Me.lblSystemClassLbl.Name = "lblSystemClassLbl"
            Me.lblSystemClassLbl.Size = New System.Drawing.Size(70, 16)
            Me.lblSystemClassLbl.TabIndex = 24
            Me.lblSystemClassLbl.Text = "System Class:"
            '
            'lblAnomalyNameLbl
            '
            Me.lblAnomalyNameLbl.AutoSize = True
            '
            '
            '
            Me.lblAnomalyNameLbl.BackgroundStyle.Class = ""
            Me.lblAnomalyNameLbl.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblAnomalyNameLbl.Location = New System.Drawing.Point(15, 94)
            Me.lblAnomalyNameLbl.Name = "lblAnomalyNameLbl"
            Me.lblAnomalyNameLbl.Size = New System.Drawing.Size(75, 16)
            Me.lblAnomalyNameLbl.TabIndex = 25
            Me.lblAnomalyNameLbl.Text = "Anomaly Type:"
            '
            'lblLocus
            '
            Me.lblLocus.AutoSize = True
            '
            '
            '
            Me.lblLocus.BackgroundStyle.Class = ""
            Me.lblLocus.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.lblLocus.Location = New System.Drawing.Point(15, 47)
            Me.lblLocus.Name = "lblLocus"
            Me.lblLocus.Size = New System.Drawing.Size(84, 16)
            Me.lblLocus.TabIndex = 26
            Me.lblLocus.Text = "Locus Signature:"
            '
            'cboWHSystem
            '
            Me.cboWHSystem.DisplayMember = "Text"
            Me.cboWHSystem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
            Me.cboWHSystem.FormattingEnabled = True
            Me.cboWHSystem.ItemHeight = 15
            Me.cboWHSystem.Location = New System.Drawing.Point(106, 47)
            Me.cboWHSystem.Name = "cboWHSystem"
            Me.cboWHSystem.Size = New System.Drawing.Size(136, 21)
            Me.cboWHSystem.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.cboWHSystem.TabIndex = 0
            '
            'DBCWSystem
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Name = "DBCWSystem"
            Me.Size = New System.Drawing.Size(360, 250)
            Me.AGPContent.ResumeLayout(False)
            Me.AGPContent.PerformLayout()
            CType(Me.pbConfig, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents lvwEffects As System.Windows.Forms.ListView
        Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
        Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
        Friend WithEvents lblLocus As DevComponents.DotNetBar.LabelX
        Friend WithEvents lblAnomalyNameLbl As DevComponents.DotNetBar.LabelX
        Friend WithEvents lblSystemClassLbl As DevComponents.DotNetBar.LabelX
        Friend WithEvents lblAnomalyName As DevComponents.DotNetBar.LabelX
        Friend WithEvents lblSystemClass As DevComponents.DotNetBar.LabelX
        Friend WithEvents cboWHSystem As DevComponents.DotNetBar.Controls.ComboBoxEx

    End Class
End NameSpace