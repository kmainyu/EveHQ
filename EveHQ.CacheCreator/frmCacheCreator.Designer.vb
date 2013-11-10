<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmCacheCreator
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FrmCacheCreator))
        Me.txtServerName = New System.Windows.Forms.TextBox()
        Me.btnGenerateCache = New System.Windows.Forms.Button()
        Me.btnCheckDB = New System.Windows.Forms.Button()
        Me.lblDB = New System.Windows.Forms.Label()
        Me.btnCheckMarketGroup = New System.Windows.Forms.Button()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.gbCheckingTools = New System.Windows.Forms.GroupBox()
        Me.gbEveHQCacheGeneration = New System.Windows.Forms.GroupBox()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.gbInstructions = New System.Windows.Forms.GroupBox()
        Me.lblInstructions = New System.Windows.Forms.Label()
        Me.gbCheckingTools.SuspendLayout()
        Me.gbEveHQCacheGeneration.SuspendLayout()
        Me.gbInstructions.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtServerName
        '
        Me.txtServerName.Location = New System.Drawing.Point(106, 12)
        Me.txtServerName.Name = "txtServerName"
        Me.txtServerName.Size = New System.Drawing.Size(221, 20)
        Me.txtServerName.TabIndex = 2
        Me.txtServerName.Text = "localhost\SQL2008E"
        '
        'btnGenerateCache
        '
        Me.btnGenerateCache.Location = New System.Drawing.Point(9, 118)
        Me.btnGenerateCache.Name = "btnGenerateCache"
        Me.btnGenerateCache.Size = New System.Drawing.Size(299, 23)
        Me.btnGenerateCache.TabIndex = 5
        Me.btnGenerateCache.Text = "Generate All Cache Files"
        Me.btnGenerateCache.UseVisualStyleBackColor = True
        '
        'btnCheckDB
        '
        Me.btnCheckDB.Location = New System.Drawing.Point(9, 89)
        Me.btnCheckDB.Name = "btnCheckDB"
        Me.btnCheckDB.Size = New System.Drawing.Size(299, 23)
        Me.btnCheckDB.TabIndex = 6
        Me.btnCheckDB.Text = "Check SQL Database"
        Me.btnCheckDB.UseVisualStyleBackColor = True
        '
        'lblDB
        '
        Me.lblDB.AutoSize = True
        Me.lblDB.Location = New System.Drawing.Point(6, 70)
        Me.lblDB.Name = "lblDB"
        Me.lblDB.Size = New System.Drawing.Size(56, 13)
        Me.lblDB.TabIndex = 7
        Me.lblDB.Text = "Database:"
        '
        'btnCheckMarketGroup
        '
        Me.btnCheckMarketGroup.Location = New System.Drawing.Point(6, 19)
        Me.btnCheckMarketGroup.Name = "btnCheckMarketGroup"
        Me.btnCheckMarketGroup.Size = New System.Drawing.Size(287, 23)
        Me.btnCheckMarketGroup.TabIndex = 9
        Me.btnCheckMarketGroup.Text = "Check Market Groups"
        Me.btnCheckMarketGroup.UseVisualStyleBackColor = True
        '
        'lblInfo
        '
        Me.lblInfo.Location = New System.Drawing.Point(6, 25)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(302, 35)
        Me.lblInfo.TabIndex = 10
        Me.lblInfo.Text = "Before starting this, ensure the typeID and iconID YAML files are in the resource" & _
    "s folder so the database can be updated."
        '
        'gbCheckingTools
        '
        Me.gbCheckingTools.Controls.Add(Me.btnCheckMarketGroup)
        Me.gbCheckingTools.Location = New System.Drawing.Point(9, 147)
        Me.gbCheckingTools.Name = "gbCheckingTools"
        Me.gbCheckingTools.Size = New System.Drawing.Size(299, 58)
        Me.gbCheckingTools.TabIndex = 11
        Me.gbCheckingTools.TabStop = False
        Me.gbCheckingTools.Text = "Checking Tools"
        '
        'gbEveHQCacheGeneration
        '
        Me.gbEveHQCacheGeneration.Controls.Add(Me.lblInfo)
        Me.gbEveHQCacheGeneration.Controls.Add(Me.gbCheckingTools)
        Me.gbEveHQCacheGeneration.Controls.Add(Me.btnGenerateCache)
        Me.gbEveHQCacheGeneration.Controls.Add(Me.btnCheckDB)
        Me.gbEveHQCacheGeneration.Controls.Add(Me.lblDB)
        Me.gbEveHQCacheGeneration.Location = New System.Drawing.Point(12, 12)
        Me.gbEveHQCacheGeneration.Name = "gbEveHQCacheGeneration"
        Me.gbEveHQCacheGeneration.Size = New System.Drawing.Size(318, 213)
        Me.gbEveHQCacheGeneration.TabIndex = 12
        Me.gbEveHQCacheGeneration.TabStop = False
        Me.gbEveHQCacheGeneration.Text = "EveHQ Cache Generation"
        '
        'gbInstructions
        '
        Me.gbInstructions.Controls.Add(Me.lblInstructions)
        Me.gbInstructions.Location = New System.Drawing.Point(336, 12)
        Me.gbInstructions.Name = "gbInstructions"
        Me.gbInstructions.Size = New System.Drawing.Size(257, 213)
        Me.gbInstructions.TabIndex = 14
        Me.gbInstructions.TabStop = False
        Me.gbInstructions.Text = "Instructions!"
        '
        'lblInstructions
        '
        Me.lblInstructions.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInstructions.Location = New System.Drawing.Point(7, 20)
        Me.lblInstructions.Name = "lblInstructions"
        Me.lblInstructions.Size = New System.Drawing.Size(244, 185)
        Me.lblInstructions.TabIndex = 0
        Me.lblInstructions.Text = resources.GetString("lblInstructions.Text")
        '
        'FrmCacheCreator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(600, 232)
        Me.Controls.Add(Me.gbInstructions)
        Me.Controls.Add(Me.gbEveHQCacheGeneration)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmCacheCreator"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "EveHQ Cache Creator"
        Me.gbCheckingTools.ResumeLayout(False)
        Me.gbEveHQCacheGeneration.ResumeLayout(False)
        Me.gbEveHQCacheGeneration.PerformLayout()
        Me.gbInstructions.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtServerName As System.Windows.Forms.TextBox
    Friend WithEvents btnGenerateCache As System.Windows.Forms.Button
    Friend WithEvents btnCheckDB As System.Windows.Forms.Button
    Friend WithEvents lblDB As System.Windows.Forms.Label
    Friend WithEvents btnCheckMarketGroup As System.Windows.Forms.Button
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents gbCheckingTools As System.Windows.Forms.GroupBox
    Friend WithEvents gbEveHQCacheGeneration As System.Windows.Forms.GroupBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents gbInstructions As System.Windows.Forms.GroupBox
    Friend WithEvents lblInstructions As System.Windows.Forms.Label

End Class
