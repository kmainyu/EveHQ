<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGunnery
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGunnery))
        Me.lvGuns = New DevComponents.DotNetBar.Controls.ListViewEx()
        Me.colName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colCap = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colOptimal = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colFalloff = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colTracking = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colEMDamage = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colExDamage = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colKiDamage = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colThDamage = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colDamage = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colDPS = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ctxResults = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblCPU = New System.Windows.Forms.Label()
        Me.lblPG = New System.Windows.Forms.Label()
        Me.lblDmgMod = New System.Windows.Forms.Label()
        Me.lblROF = New System.Windows.Forms.Label()
        Me.gpStandardInfo = New DevComponents.DotNetBar.Controls.GroupPanel()
        Me.ctxResults.SuspendLayout()
        Me.gpStandardInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'lvGuns
        '
        Me.lvGuns.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        '
        '
        '
        Me.lvGuns.Border.Class = "ListViewBorder"
        Me.lvGuns.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lvGuns.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colName, Me.colCap, Me.colOptimal, Me.colFalloff, Me.colTracking, Me.colEMDamage, Me.colExDamage, Me.colKiDamage, Me.colThDamage, Me.colDamage, Me.colDPS})
        Me.lvGuns.ContextMenuStrip = Me.ctxResults
        Me.lvGuns.FullRowSelect = True
        Me.lvGuns.Location = New System.Drawing.Point(12, 74)
        Me.lvGuns.Name = "lvGuns"
        Me.lvGuns.Size = New System.Drawing.Size(875, 450)
        Me.lvGuns.TabIndex = 1
        Me.lvGuns.UseCompatibleStateImageBehavior = False
        Me.lvGuns.View = System.Windows.Forms.View.Details
        '
        'colName
        '
        Me.colName.Text = "Ammo"
        Me.colName.Width = 250
        '
        'colCap
        '
        Me.colCap.Text = "Cap Use"
        '
        'colOptimal
        '
        Me.colOptimal.Text = "Optimal"
        '
        'colFalloff
        '
        Me.colFalloff.Text = "Falloff"
        '
        'colTracking
        '
        Me.colTracking.Text = "Tracking"
        '
        'colEMDamage
        '
        Me.colEMDamage.Text = "EM"
        '
        'colExDamage
        '
        Me.colExDamage.Text = "Explosive"
        '
        'colKiDamage
        '
        Me.colKiDamage.Text = "Kinetic"
        '
        'colThDamage
        '
        Me.colThDamage.Text = "Thermal"
        '
        'colDamage
        '
        Me.colDamage.Text = "Damage"
        '
        'colDPS
        '
        Me.colDPS.Text = "DPS"
        '
        'ctxResults
        '
        Me.ctxResults.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyToolStripMenuItem})
        Me.ctxResults.Name = "ctxResults"
        Me.ctxResults.Size = New System.Drawing.Size(175, 26)
        '
        'CopyToolStripMenuItem
        '
        Me.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem"
        Me.CopyToolStripMenuItem.Size = New System.Drawing.Size(174, 22)
        Me.CopyToolStripMenuItem.Text = "Copy To Clipboard"
        '
        'lblCPU
        '
        Me.lblCPU.AutoSize = True
        Me.lblCPU.BackColor = System.Drawing.Color.Transparent
        Me.lblCPU.Location = New System.Drawing.Point(12, 9)
        Me.lblCPU.Name = "lblCPU"
        Me.lblCPU.Size = New System.Drawing.Size(31, 13)
        Me.lblCPU.TabIndex = 23
        Me.lblCPU.Text = "CPU:"
        '
        'lblPG
        '
        Me.lblPG.AutoSize = True
        Me.lblPG.BackColor = System.Drawing.Color.Transparent
        Me.lblPG.Location = New System.Drawing.Point(175, 9)
        Me.lblPG.Name = "lblPG"
        Me.lblPG.Size = New System.Drawing.Size(24, 13)
        Me.lblPG.TabIndex = 24
        Me.lblPG.Text = "PG:"
        '
        'lblDmgMod
        '
        Me.lblDmgMod.AutoSize = True
        Me.lblDmgMod.BackColor = System.Drawing.Color.Transparent
        Me.lblDmgMod.Location = New System.Drawing.Point(311, 9)
        Me.lblDmgMod.Name = "lblDmgMod"
        Me.lblDmgMod.Size = New System.Drawing.Size(73, 13)
        Me.lblDmgMod.TabIndex = 26
        Me.lblDmgMod.Text = "Damage Mod:"
        '
        'lblROF
        '
        Me.lblROF.AutoSize = True
        Me.lblROF.BackColor = System.Drawing.Color.Transparent
        Me.lblROF.Location = New System.Drawing.Point(506, 9)
        Me.lblROF.Name = "lblROF"
        Me.lblROF.Size = New System.Drawing.Size(32, 13)
        Me.lblROF.TabIndex = 27
        Me.lblROF.Text = "ROF:"
        '
        'gpStandardInfo
        '
        Me.gpStandardInfo.CanvasColor = System.Drawing.SystemColors.Control
        Me.gpStandardInfo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.gpStandardInfo.Controls.Add(Me.lblPG)
        Me.gpStandardInfo.Controls.Add(Me.lblCPU)
        Me.gpStandardInfo.Controls.Add(Me.lblROF)
        Me.gpStandardInfo.Controls.Add(Me.lblDmgMod)
        Me.gpStandardInfo.IsShadowEnabled = True
        Me.gpStandardInfo.Location = New System.Drawing.Point(12, 12)
        Me.gpStandardInfo.Name = "gpStandardInfo"
        Me.gpStandardInfo.Size = New System.Drawing.Size(875, 56)
        '
        '
        '
        Me.gpStandardInfo.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.gpStandardInfo.Style.BackColorGradientAngle = 90
        Me.gpStandardInfo.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.gpStandardInfo.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpStandardInfo.Style.BorderBottomWidth = 1
        Me.gpStandardInfo.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.gpStandardInfo.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpStandardInfo.Style.BorderLeftWidth = 1
        Me.gpStandardInfo.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpStandardInfo.Style.BorderRightWidth = 1
        Me.gpStandardInfo.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpStandardInfo.Style.BorderTopWidth = 1
        Me.gpStandardInfo.Style.Class = ""
        Me.gpStandardInfo.Style.CornerDiameter = 4
        Me.gpStandardInfo.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpStandardInfo.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpStandardInfo.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpStandardInfo.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpStandardInfo.StyleMouseDown.Class = ""
        Me.gpStandardInfo.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.gpStandardInfo.StyleMouseOver.Class = ""
        Me.gpStandardInfo.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.gpStandardInfo.TabIndex = 29
        Me.gpStandardInfo.Text = "Standard Weapon Attributes"
        '
        'frmGunnery
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(899, 536)
        Me.Controls.Add(Me.gpStandardInfo)
        Me.Controls.Add(Me.lvGuns)
        Me.DoubleBuffered = True
        Me.EnableGlass = False
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmGunnery"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "HQF Ammo Analysis"
        Me.ctxResults.ResumeLayout(False)
        Me.gpStandardInfo.ResumeLayout(False)
        Me.gpStandardInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lvGuns As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents colName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCap As System.Windows.Forms.ColumnHeader
    Friend WithEvents colOptimal As System.Windows.Forms.ColumnHeader
    Friend WithEvents colTracking As System.Windows.Forms.ColumnHeader
    Friend WithEvents colEMDamage As System.Windows.Forms.ColumnHeader
    Friend WithEvents colExDamage As System.Windows.Forms.ColumnHeader
    Friend WithEvents colKiDamage As System.Windows.Forms.ColumnHeader
    Friend WithEvents colThDamage As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDamage As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDPS As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblCPU As System.Windows.Forms.Label
    Friend WithEvents lblPG As System.Windows.Forms.Label
    Friend WithEvents lblDmgMod As System.Windows.Forms.Label
    Friend WithEvents lblROF As System.Windows.Forms.Label
    Friend WithEvents colFalloff As System.Windows.Forms.ColumnHeader
    Friend WithEvents ctxResults As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents gpStandardInfo As DevComponents.DotNetBar.Controls.GroupPanel
End Class
