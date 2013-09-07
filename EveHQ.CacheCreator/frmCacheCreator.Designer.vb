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
        Me.btnGenerateHQFIcons = New System.Windows.Forms.Button()
        Me.btnCheckMarketGroup = New System.Windows.Forms.Button()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.gbCheckingTools = New System.Windows.Forms.GroupBox()
        Me.gbEITTCacheGeneration = New System.Windows.Forms.GroupBox()
        Me.gbEveCacheExtraction = New System.Windows.Forms.GroupBox()
        Me.gbEveServerType = New System.Windows.Forms.GroupBox()
        Me.rbBuckingham = New System.Windows.Forms.RadioButton()
        Me.rbDuality = New System.Windows.Forms.RadioButton()
        Me.rbSingularity = New System.Windows.Forms.RadioButton()
        Me.rbTranquility = New System.Windows.Forms.RadioButton()
        Me.btnSQL = New System.Windows.Forms.Button()
        Me.lvwLog = New System.Windows.Forms.ListView()
        Me.colTime = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.colDetails = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.btnXML = New System.Windows.Forms.Button()
        Me.btnEveLocation = New System.Windows.Forms.Button()
        Me.lblEveProtocol = New System.Windows.Forms.Label()
        Me.lblEveProtocolLabel = New System.Windows.Forms.Label()
        Me.lblEveLocationLabel = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.gbInstructions = New System.Windows.Forms.GroupBox()
        Me.lblInstructions = New System.Windows.Forms.Label()
        Me.gbCheckingTools.SuspendLayout()
        Me.gbEITTCacheGeneration.SuspendLayout()
        Me.gbEveCacheExtraction.SuspendLayout()
        Me.gbEveServerType.SuspendLayout()
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
        'btnGenerateHQFIcons
        '
        Me.btnGenerateHQFIcons.Location = New System.Drawing.Point(9, 147)
        Me.btnGenerateHQFIcons.Name = "btnGenerateHQFIcons"
        Me.btnGenerateHQFIcons.Size = New System.Drawing.Size(299, 23)
        Me.btnGenerateHQFIcons.TabIndex = 8
        Me.btnGenerateHQFIcons.Text = "Generate HQF Icons"
        Me.btnGenerateHQFIcons.UseVisualStyleBackColor = True
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
        Me.gbCheckingTools.Location = New System.Drawing.Point(9, 176)
        Me.gbCheckingTools.Name = "gbCheckingTools"
        Me.gbCheckingTools.Size = New System.Drawing.Size(299, 113)
        Me.gbCheckingTools.TabIndex = 11
        Me.gbCheckingTools.TabStop = False
        Me.gbCheckingTools.Text = "Checking Tools"
        '
        'gbEITTCacheGeneration
        '
        Me.gbEITTCacheGeneration.Controls.Add(Me.lblInfo)
        Me.gbEITTCacheGeneration.Controls.Add(Me.gbCheckingTools)
        Me.gbEITTCacheGeneration.Controls.Add(Me.btnGenerateCache)
        Me.gbEITTCacheGeneration.Controls.Add(Me.btnCheckDB)
        Me.gbEITTCacheGeneration.Controls.Add(Me.btnGenerateHQFIcons)
        Me.gbEITTCacheGeneration.Controls.Add(Me.lblDB)
        Me.gbEITTCacheGeneration.Location = New System.Drawing.Point(12, 12)
        Me.gbEITTCacheGeneration.Name = "gbEITTCacheGeneration"
        Me.gbEITTCacheGeneration.Size = New System.Drawing.Size(318, 302)
        Me.gbEITTCacheGeneration.TabIndex = 12
        Me.gbEITTCacheGeneration.TabStop = False
        Me.gbEITTCacheGeneration.Text = "EveHQ Cache Generation"
        '
        'gbEveCacheExtraction
        '
        Me.gbEveCacheExtraction.Controls.Add(Me.gbEveServerType)
        Me.gbEveCacheExtraction.Controls.Add(Me.btnSQL)
        Me.gbEveCacheExtraction.Controls.Add(Me.lvwLog)
        Me.gbEveCacheExtraction.Controls.Add(Me.btnXML)
        Me.gbEveCacheExtraction.Controls.Add(Me.btnEveLocation)
        Me.gbEveCacheExtraction.Controls.Add(Me.lblEveProtocol)
        Me.gbEveCacheExtraction.Controls.Add(Me.lblEveProtocolLabel)
        Me.gbEveCacheExtraction.Controls.Add(Me.lblEveLocationLabel)
        Me.gbEveCacheExtraction.Location = New System.Drawing.Point(12, 320)
        Me.gbEveCacheExtraction.Name = "gbEveCacheExtraction"
        Me.gbEveCacheExtraction.Size = New System.Drawing.Size(787, 530)
        Me.gbEveCacheExtraction.TabIndex = 13
        Me.gbEveCacheExtraction.TabStop = False
        Me.gbEveCacheExtraction.Text = "Eve Cache Extraction"
        '
        'gbEveServerType
        '
        Me.gbEveServerType.Controls.Add(Me.rbBuckingham)
        Me.gbEveServerType.Controls.Add(Me.rbDuality)
        Me.gbEveServerType.Controls.Add(Me.rbSingularity)
        Me.gbEveServerType.Controls.Add(Me.rbTranquility)
        Me.gbEveServerType.Location = New System.Drawing.Point(519, 21)
        Me.gbEveServerType.Name = "gbEveServerType"
        Me.gbEveServerType.Size = New System.Drawing.Size(262, 75)
        Me.gbEveServerType.TabIndex = 9
        Me.gbEveServerType.TabStop = False
        Me.gbEveServerType.Text = "Eve Server Name"
        '
        'rbBuckingham
        '
        Me.rbBuckingham.AutoSize = True
        Me.rbBuckingham.Location = New System.Drawing.Point(111, 44)
        Me.rbBuckingham.Name = "rbBuckingham"
        Me.rbBuckingham.Size = New System.Drawing.Size(84, 17)
        Me.rbBuckingham.TabIndex = 3
        Me.rbBuckingham.Text = "Buckingham"
        Me.rbBuckingham.UseVisualStyleBackColor = True
        '
        'rbDuality
        '
        Me.rbDuality.AutoSize = True
        Me.rbDuality.Location = New System.Drawing.Point(111, 21)
        Me.rbDuality.Name = "rbDuality"
        Me.rbDuality.Size = New System.Drawing.Size(57, 17)
        Me.rbDuality.TabIndex = 2
        Me.rbDuality.Text = "Duality"
        Me.rbDuality.UseVisualStyleBackColor = True
        '
        'rbSingularity
        '
        Me.rbSingularity.AutoSize = True
        Me.rbSingularity.Location = New System.Drawing.Point(7, 44)
        Me.rbSingularity.Name = "rbSingularity"
        Me.rbSingularity.Size = New System.Drawing.Size(73, 17)
        Me.rbSingularity.TabIndex = 1
        Me.rbSingularity.Text = "Singularity"
        Me.rbSingularity.UseVisualStyleBackColor = True
        '
        'rbTranquility
        '
        Me.rbTranquility.AutoSize = True
        Me.rbTranquility.Checked = True
        Me.rbTranquility.Location = New System.Drawing.Point(7, 21)
        Me.rbTranquility.Name = "rbTranquility"
        Me.rbTranquility.Size = New System.Drawing.Size(73, 17)
        Me.rbTranquility.TabIndex = 0
        Me.rbTranquility.TabStop = True
        Me.rbTranquility.Text = "Tranquility"
        Me.rbTranquility.UseVisualStyleBackColor = True
        '
        'btnSQL
        '
        Me.btnSQL.Location = New System.Drawing.Point(112, 73)
        Me.btnSQL.Name = "btnSQL"
        Me.btnSQL.Size = New System.Drawing.Size(100, 23)
        Me.btnSQL.TabIndex = 8
        Me.btnSQL.Text = "Update SQL"
        Me.btnSQL.UseVisualStyleBackColor = True
        '
        'lvwLog
        '
        Me.lvwLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwLog.AutoArrange = False
        Me.lvwLog.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colTime, Me.colDetails})
        Me.lvwLog.Location = New System.Drawing.Point(6, 102)
        Me.lvwLog.Name = "lvwLog"
        Me.lvwLog.Size = New System.Drawing.Size(775, 422)
        Me.lvwLog.TabIndex = 7
        Me.lvwLog.UseCompatibleStateImageBehavior = False
        Me.lvwLog.View = System.Windows.Forms.View.Details
        '
        'colTime
        '
        Me.colTime.Text = "Time"
        Me.colTime.Width = 150
        '
        'colDetails
        '
        Me.colDetails.Text = "Details"
        Me.colDetails.Width = 600
        '
        'btnXML
        '
        Me.btnXML.Location = New System.Drawing.Point(6, 73)
        Me.btnXML.Name = "btnXML"
        Me.btnXML.Size = New System.Drawing.Size(100, 23)
        Me.btnXML.TabIndex = 6
        Me.btnXML.Text = "Dump As XML"
        Me.btnXML.UseVisualStyleBackColor = True
        '
        'btnEveLocation
        '
        Me.btnEveLocation.Location = New System.Drawing.Point(88, 21)
        Me.btnEveLocation.Name = "btnEveLocation"
        Me.btnEveLocation.Size = New System.Drawing.Size(368, 20)
        Me.btnEveLocation.TabIndex = 5
        Me.btnEveLocation.Text = "<unknown>"
        Me.btnEveLocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ToolTip1.SetToolTip(Me.btnEveLocation, "Click to change the location of the Eve executable")
        Me.btnEveLocation.UseVisualStyleBackColor = True
        '
        'lblEveProtocol
        '
        Me.lblEveProtocol.AutoSize = True
        Me.lblEveProtocol.Location = New System.Drawing.Point(85, 47)
        Me.lblEveProtocol.Name = "lblEveProtocol"
        Me.lblEveProtocol.Size = New System.Drawing.Size(63, 13)
        Me.lblEveProtocol.TabIndex = 3
        Me.lblEveProtocol.Text = "<unknown>"
        '
        'lblEveProtocolLabel
        '
        Me.lblEveProtocolLabel.AutoSize = True
        Me.lblEveProtocolLabel.Location = New System.Drawing.Point(6, 47)
        Me.lblEveProtocolLabel.Name = "lblEveProtocolLabel"
        Me.lblEveProtocolLabel.Size = New System.Drawing.Size(71, 13)
        Me.lblEveProtocolLabel.TabIndex = 2
        Me.lblEveProtocolLabel.Text = "Eve Protocol:"
        '
        'lblEveLocationLabel
        '
        Me.lblEveLocationLabel.AutoSize = True
        Me.lblEveLocationLabel.Location = New System.Drawing.Point(6, 25)
        Me.lblEveLocationLabel.Name = "lblEveLocationLabel"
        Me.lblEveLocationLabel.Size = New System.Drawing.Size(73, 13)
        Me.lblEveLocationLabel.TabIndex = 0
        Me.lblEveLocationLabel.Text = "Eve Location:"
        '
        'gbInstructions
        '
        Me.gbInstructions.Controls.Add(Me.lblInstructions)
        Me.gbInstructions.Location = New System.Drawing.Point(336, 12)
        Me.gbInstructions.Name = "gbInstructions"
        Me.gbInstructions.Size = New System.Drawing.Size(463, 302)
        Me.gbInstructions.TabIndex = 14
        Me.gbInstructions.TabStop = False
        Me.gbInstructions.Text = "Instructions!"
        '
        'lblInstructions
        '
        Me.lblInstructions.Location = New System.Drawing.Point(7, 20)
        Me.lblInstructions.Name = "lblInstructions"
        Me.lblInstructions.Size = New System.Drawing.Size(450, 279)
        Me.lblInstructions.TabIndex = 0
        Me.lblInstructions.Text = resources.GetString("lblInstructions.Text")
        '
        'FrmCacheCreator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(811, 862)
        Me.Controls.Add(Me.gbInstructions)
        Me.Controls.Add(Me.gbEveCacheExtraction)
        Me.Controls.Add(Me.gbEITTCacheGeneration)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "FrmCacheCreator"
        Me.Text = "EveHQ Cache Creator"
        Me.gbCheckingTools.ResumeLayout(False)
        Me.gbEITTCacheGeneration.ResumeLayout(False)
        Me.gbEITTCacheGeneration.PerformLayout()
        Me.gbEveCacheExtraction.ResumeLayout(False)
        Me.gbEveCacheExtraction.PerformLayout()
        Me.gbEveServerType.ResumeLayout(False)
        Me.gbEveServerType.PerformLayout()
        Me.gbInstructions.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents txtServerName As System.Windows.Forms.TextBox
    Friend WithEvents btnGenerateCache As System.Windows.Forms.Button
    Friend WithEvents btnCheckDB As System.Windows.Forms.Button
    Friend WithEvents lblDB As System.Windows.Forms.Label
    Friend WithEvents btnGenerateHQFIcons As System.Windows.Forms.Button
    Friend WithEvents btnCheckMarketGroup As System.Windows.Forms.Button
    Friend WithEvents lblInfo As System.Windows.Forms.Label
    Friend WithEvents gbCheckingTools As System.Windows.Forms.GroupBox
    Friend WithEvents gbEITTCacheGeneration As System.Windows.Forms.GroupBox
    Friend WithEvents gbEveCacheExtraction As System.Windows.Forms.GroupBox
    Friend WithEvents lblEveLocationLabel As System.Windows.Forms.Label
    Friend WithEvents lblEveProtocol As System.Windows.Forms.Label
    Friend WithEvents lblEveProtocolLabel As System.Windows.Forms.Label
    Friend WithEvents btnEveLocation As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents btnXML As System.Windows.Forms.Button
    Friend WithEvents lvwLog As System.Windows.Forms.ListView
    Friend WithEvents colTime As System.Windows.Forms.ColumnHeader
    Friend WithEvents colDetails As System.Windows.Forms.ColumnHeader
    Friend WithEvents gbInstructions As System.Windows.Forms.GroupBox
    Friend WithEvents lblInstructions As System.Windows.Forms.Label
    Friend WithEvents btnSQL As System.Windows.Forms.Button
    Friend WithEvents gbEveServerType As System.Windows.Forms.GroupBox
    Friend WithEvents rbDuality As System.Windows.Forms.RadioButton
    Friend WithEvents rbSingularity As System.Windows.Forms.RadioButton
    Friend WithEvents rbTranquility As System.Windows.Forms.RadioButton
    Friend WithEvents rbBuckingham As System.Windows.Forms.RadioButton

End Class
