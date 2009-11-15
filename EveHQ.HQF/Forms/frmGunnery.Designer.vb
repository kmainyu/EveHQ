<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmGunnery
    Inherits System.Windows.Forms.Form

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
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmGunnery))
        Me.lvGuns = New System.Windows.Forms.ListView
        Me.colName = New System.Windows.Forms.ColumnHeader
        Me.colCap = New System.Windows.Forms.ColumnHeader
        Me.colOptimal = New System.Windows.Forms.ColumnHeader
        Me.colFalloff = New System.Windows.Forms.ColumnHeader
        Me.colTracking = New System.Windows.Forms.ColumnHeader
        Me.colEMDamage = New System.Windows.Forms.ColumnHeader
        Me.colExDamage = New System.Windows.Forms.ColumnHeader
        Me.colKiDamage = New System.Windows.Forms.ColumnHeader
        Me.colThDamage = New System.Windows.Forms.ColumnHeader
        Me.colDamage = New System.Windows.Forms.ColumnHeader
        Me.colDPS = New System.Windows.Forms.ColumnHeader
        Me.ctxResults = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CopyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.lblCPU = New System.Windows.Forms.Label
        Me.lblPG = New System.Windows.Forms.Label
        Me.lblDmgMod = New System.Windows.Forms.Label
        Me.lblROF = New System.Windows.Forms.Label
        Me.gbStandardInfo = New System.Windows.Forms.GroupBox
        Me.ctxResults.SuspendLayout()
        Me.gbStandardInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'lvGuns
        '
        Me.lvGuns.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvGuns.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colName, Me.colCap, Me.colOptimal, Me.colFalloff, Me.colTracking, Me.colEMDamage, Me.colExDamage, Me.colKiDamage, Me.colThDamage, Me.colDamage, Me.colDPS})
        Me.lvGuns.ContextMenuStrip = Me.ctxResults
        Me.lvGuns.FullRowSelect = True
        Me.lvGuns.Location = New System.Drawing.Point(12, 76)
        Me.lvGuns.Name = "lvGuns"
        Me.lvGuns.Size = New System.Drawing.Size(875, 448)
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
        Me.lblCPU.Location = New System.Drawing.Point(20, 27)
        Me.lblCPU.Name = "lblCPU"
        Me.lblCPU.Size = New System.Drawing.Size(31, 13)
        Me.lblCPU.TabIndex = 23
        Me.lblCPU.Text = "CPU:"
        '
        'lblPG
        '
        Me.lblPG.AutoSize = True
        Me.lblPG.Location = New System.Drawing.Point(183, 27)
        Me.lblPG.Name = "lblPG"
        Me.lblPG.Size = New System.Drawing.Size(24, 13)
        Me.lblPG.TabIndex = 24
        Me.lblPG.Text = "PG:"
        '
        'lblDmgMod
        '
        Me.lblDmgMod.AutoSize = True
        Me.lblDmgMod.Location = New System.Drawing.Point(319, 27)
        Me.lblDmgMod.Name = "lblDmgMod"
        Me.lblDmgMod.Size = New System.Drawing.Size(73, 13)
        Me.lblDmgMod.TabIndex = 26
        Me.lblDmgMod.Text = "Damage Mod:"
        '
        'lblROF
        '
        Me.lblROF.AutoSize = True
        Me.lblROF.Location = New System.Drawing.Point(514, 27)
        Me.lblROF.Name = "lblROF"
        Me.lblROF.Size = New System.Drawing.Size(32, 13)
        Me.lblROF.TabIndex = 27
        Me.lblROF.Text = "ROF:"
        '
        'gbStandardInfo
        '
        Me.gbStandardInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.gbStandardInfo.Controls.Add(Me.lblPG)
        Me.gbStandardInfo.Controls.Add(Me.lblROF)
        Me.gbStandardInfo.Controls.Add(Me.lblCPU)
        Me.gbStandardInfo.Controls.Add(Me.lblDmgMod)
        Me.gbStandardInfo.Location = New System.Drawing.Point(12, 12)
        Me.gbStandardInfo.Name = "gbStandardInfo"
        Me.gbStandardInfo.Size = New System.Drawing.Size(875, 58)
        Me.gbStandardInfo.TabIndex = 28
        Me.gbStandardInfo.TabStop = False
        Me.gbStandardInfo.Text = "Standard Weapon Attributes"
        '
        'frmGunnery
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(899, 536)
        Me.Controls.Add(Me.gbStandardInfo)
        Me.Controls.Add(Me.lvGuns)
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
        Me.gbStandardInfo.ResumeLayout(False)
        Me.gbStandardInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lvGuns As System.Windows.Forms.ListView
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
    Friend WithEvents gbStandardInfo As System.Windows.Forms.GroupBox
    Friend WithEvents colFalloff As System.Windows.Forms.ColumnHeader
    Friend WithEvents ctxResults As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents CopyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
