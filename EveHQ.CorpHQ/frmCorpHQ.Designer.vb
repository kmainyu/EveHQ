' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
' 
' This file is part of EveHQ.
'
' EveHQ is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
'=========================================================================
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCorpHQ
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCorpHQ))
        Me.ctxStandings = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuExtrapolateStandings = New System.Windows.Forms.ToolStripMenuItem
        Me.lblInformation = New System.Windows.Forms.Label
        Me.ctxStandings.SuspendLayout()
        Me.SuspendLayout()
        '
        'ctxStandings
        '
        Me.ctxStandings.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuExtrapolateStandings})
        Me.ctxStandings.Name = "ctxStandings"
        Me.ctxStandings.Size = New System.Drawing.Size(192, 26)
        '
        'mnuExtrapolateStandings
        '
        Me.mnuExtrapolateStandings.Name = "mnuExtrapolateStandings"
        Me.mnuExtrapolateStandings.Size = New System.Drawing.Size(191, 22)
        Me.mnuExtrapolateStandings.Text = "Extrapolate Standings"
        '
        'lblInformation
        '
        Me.lblInformation.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInformation.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInformation.Location = New System.Drawing.Point(12, 9)
        Me.lblInformation.Name = "lblInformation"
        Me.lblInformation.Size = New System.Drawing.Size(730, 473)
        Me.lblInformation.TabIndex = 1
        Me.lblInformation.Text = resources.GetString("lblInformation.Text")
        Me.lblInformation.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmCorpHQ
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(754, 491)
        Me.Controls.Add(Me.lblInformation)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmCorpHQ"
        Me.Text = "CorpHQ"
        Me.ctxStandings.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ctxStandings As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuExtrapolateStandings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblInformation As System.Windows.Forms.Label
End Class
