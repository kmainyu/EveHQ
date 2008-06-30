<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class frmHQFSettings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("General")
        Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Data Cache")
        Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Slot Colours")
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmHQFSettings))
        Me.gbGeneral = New System.Windows.Forms.GroupBox
        Me.chkShowPerformance = New System.Windows.Forms.CheckBox
        Me.chkAutoUpdateHQFSkills = New System.Windows.Forms.CheckBox
        Me.chkRestoreLastSession = New System.Windows.Forms.CheckBox
        Me.cboStartupPilot = New System.Windows.Forms.ComboBox
        Me.lblDefaultPilot = New System.Windows.Forms.Label
        Me.btnClose = New System.Windows.Forms.Button
        Me.fbd1 = New System.Windows.Forms.FolderBrowserDialog
        Me.ofd1 = New System.Windows.Forms.OpenFileDialog
        Me.tvwSettings = New System.Windows.Forms.TreeView
        Me.cd1 = New System.Windows.Forms.ColorDialog
        Me.lblHiSlotColour = New System.Windows.Forms.Label
        Me.lblMidSlotColour = New System.Windows.Forms.Label
        Me.pbHiSlotColour = New System.Windows.Forms.PictureBox
        Me.pbMidSlotColour = New System.Windows.Forms.PictureBox
        Me.lblLowSlotColour = New System.Windows.Forms.Label
        Me.pbLowSlotColour = New System.Windows.Forms.PictureBox
        Me.lblRigSlotColour = New System.Windows.Forms.Label
        Me.pbRigSlotColour = New System.Windows.Forms.PictureBox
        Me.gbSlotColours = New System.Windows.Forms.GroupBox
        Me.gbCache = New System.Windows.Forms.GroupBox
        Me.btnDeleteCache = New System.Windows.Forms.Button
        Me.chkCloseInfoPanel = New System.Windows.Forms.CheckBox
        Me.gbGeneral.SuspendLayout()
        CType(Me.pbHiSlotColour, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbMidSlotColour, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbLowSlotColour, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbRigSlotColour, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbSlotColours.SuspendLayout()
        Me.gbCache.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbGeneral
        '
        Me.gbGeneral.Controls.Add(Me.chkCloseInfoPanel)
        Me.gbGeneral.Controls.Add(Me.chkShowPerformance)
        Me.gbGeneral.Controls.Add(Me.chkAutoUpdateHQFSkills)
        Me.gbGeneral.Controls.Add(Me.chkRestoreLastSession)
        Me.gbGeneral.Controls.Add(Me.cboStartupPilot)
        Me.gbGeneral.Controls.Add(Me.lblDefaultPilot)
        Me.gbGeneral.Location = New System.Drawing.Point(194, 12)
        Me.gbGeneral.Name = "gbGeneral"
        Me.gbGeneral.Size = New System.Drawing.Size(498, 473)
        Me.gbGeneral.TabIndex = 1
        Me.gbGeneral.TabStop = False
        Me.gbGeneral.Text = "General Settings"
        Me.gbGeneral.Visible = False
        '
        'chkShowPerformance
        '
        Me.chkShowPerformance.AutoSize = True
        Me.chkShowPerformance.Location = New System.Drawing.Point(25, 187)
        Me.chkShowPerformance.Name = "chkShowPerformance"
        Me.chkShowPerformance.Size = New System.Drawing.Size(142, 17)
        Me.chkShowPerformance.TabIndex = 10
        Me.chkShowPerformance.Text = "Show Performance Data"
        Me.chkShowPerformance.UseVisualStyleBackColor = True
        '
        'chkAutoUpdateHQFSkills
        '
        Me.chkAutoUpdateHQFSkills.AutoSize = True
        Me.chkAutoUpdateHQFSkills.Location = New System.Drawing.Point(25, 119)
        Me.chkAutoUpdateHQFSkills.Name = "chkAutoUpdateHQFSkills"
        Me.chkAutoUpdateHQFSkills.Size = New System.Drawing.Size(240, 17)
        Me.chkAutoUpdateHQFSkills.TabIndex = 9
        Me.chkAutoUpdateHQFSkills.Text = "Update HQF Skills to Actual Skills on Start-up"
        Me.chkAutoUpdateHQFSkills.UseVisualStyleBackColor = True
        '
        'chkRestoreLastSession
        '
        Me.chkRestoreLastSession.AutoSize = True
        Me.chkRestoreLastSession.Location = New System.Drawing.Point(25, 96)
        Me.chkRestoreLastSession.Name = "chkRestoreLastSession"
        Me.chkRestoreLastSession.Size = New System.Drawing.Size(247, 17)
        Me.chkRestoreLastSession.TabIndex = 8
        Me.chkRestoreLastSession.Text = "Restore Previous Session When Opening HQF"
        Me.chkRestoreLastSession.UseVisualStyleBackColor = True
        '
        'cboStartupPilot
        '
        Me.cboStartupPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboStartupPilot.FormattingEnabled = True
        Me.cboStartupPilot.Location = New System.Drawing.Point(146, 49)
        Me.cboStartupPilot.Name = "cboStartupPilot"
        Me.cboStartupPilot.Size = New System.Drawing.Size(168, 21)
        Me.cboStartupPilot.Sorted = True
        Me.cboStartupPilot.TabIndex = 7
        '
        'lblDefaultPilot
        '
        Me.lblDefaultPilot.AutoSize = True
        Me.lblDefaultPilot.Location = New System.Drawing.Point(22, 52)
        Me.lblDefaultPilot.Name = "lblDefaultPilot"
        Me.lblDefaultPilot.Size = New System.Drawing.Size(118, 13)
        Me.lblDefaultPilot.TabIndex = 6
        Me.lblDefaultPilot.Text = "Default Pilot for Fittings:"
        '
        'btnClose
        '
        Me.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnClose.Location = New System.Drawing.Point(12, 492)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 25)
        Me.btnClose.TabIndex = 26
        Me.btnClose.Text = "&OK"
        '
        'ofd1
        '
        Me.ofd1.FileName = "OpenFileDialog1"
        '
        'tvwSettings
        '
        Me.tvwSettings.Location = New System.Drawing.Point(12, 12)
        Me.tvwSettings.Name = "tvwSettings"
        TreeNode1.Name = "nodeGeneral"
        TreeNode1.Text = "General"
        TreeNode2.Name = "nodeCache"
        TreeNode2.Text = "Data Cache"
        TreeNode3.Name = "nodeSlotColours"
        TreeNode3.Text = "Slot Colours"
        Me.tvwSettings.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode1, TreeNode2, TreeNode3})
        Me.tvwSettings.Size = New System.Drawing.Size(176, 473)
        Me.tvwSettings.TabIndex = 27
        '
        'lblHiSlotColour
        '
        Me.lblHiSlotColour.AutoSize = True
        Me.lblHiSlotColour.Location = New System.Drawing.Point(32, 40)
        Me.lblHiSlotColour.Name = "lblHiSlotColour"
        Me.lblHiSlotColour.Size = New System.Drawing.Size(71, 13)
        Me.lblHiSlotColour.TabIndex = 20
        Me.lblHiSlotColour.Text = "Hi Slot Colour"
        '
        'lblMidSlotColour
        '
        Me.lblMidSlotColour.AutoSize = True
        Me.lblMidSlotColour.Location = New System.Drawing.Point(32, 70)
        Me.lblMidSlotColour.Name = "lblMidSlotColour"
        Me.lblMidSlotColour.Size = New System.Drawing.Size(78, 13)
        Me.lblMidSlotColour.TabIndex = 21
        Me.lblMidSlotColour.Text = "Mid Slot Colour"
        '
        'pbHiSlotColour
        '
        Me.pbHiSlotColour.BackColor = System.Drawing.Color.LightSteelBlue
        Me.pbHiSlotColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbHiSlotColour.Location = New System.Drawing.Point(159, 33)
        Me.pbHiSlotColour.Name = "pbHiSlotColour"
        Me.pbHiSlotColour.Size = New System.Drawing.Size(24, 24)
        Me.pbHiSlotColour.TabIndex = 22
        Me.pbHiSlotColour.TabStop = False
        '
        'pbMidSlotColour
        '
        Me.pbMidSlotColour.BackColor = System.Drawing.Color.Plum
        Me.pbMidSlotColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbMidSlotColour.Location = New System.Drawing.Point(159, 63)
        Me.pbMidSlotColour.Name = "pbMidSlotColour"
        Me.pbMidSlotColour.Size = New System.Drawing.Size(24, 24)
        Me.pbMidSlotColour.TabIndex = 23
        Me.pbMidSlotColour.TabStop = False
        '
        'lblLowSlotColour
        '
        Me.lblLowSlotColour.AutoSize = True
        Me.lblLowSlotColour.Location = New System.Drawing.Point(32, 100)
        Me.lblLowSlotColour.Name = "lblLowSlotColour"
        Me.lblLowSlotColour.Size = New System.Drawing.Size(81, 13)
        Me.lblLowSlotColour.TabIndex = 24
        Me.lblLowSlotColour.Text = "Low Slot Colour"
        '
        'pbLowSlotColour
        '
        Me.pbLowSlotColour.BackColor = System.Drawing.Color.Gold
        Me.pbLowSlotColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbLowSlotColour.Location = New System.Drawing.Point(159, 93)
        Me.pbLowSlotColour.Name = "pbLowSlotColour"
        Me.pbLowSlotColour.Size = New System.Drawing.Size(24, 24)
        Me.pbLowSlotColour.TabIndex = 25
        Me.pbLowSlotColour.TabStop = False
        '
        'lblRigSlotColour
        '
        Me.lblRigSlotColour.AutoSize = True
        Me.lblRigSlotColour.Location = New System.Drawing.Point(32, 130)
        Me.lblRigSlotColour.Name = "lblRigSlotColour"
        Me.lblRigSlotColour.Size = New System.Drawing.Size(77, 13)
        Me.lblRigSlotColour.TabIndex = 26
        Me.lblRigSlotColour.Text = "Rig Slot Colour"
        '
        'pbRigSlotColour
        '
        Me.pbRigSlotColour.BackColor = System.Drawing.Color.Red
        Me.pbRigSlotColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbRigSlotColour.Location = New System.Drawing.Point(159, 123)
        Me.pbRigSlotColour.Name = "pbRigSlotColour"
        Me.pbRigSlotColour.Size = New System.Drawing.Size(24, 24)
        Me.pbRigSlotColour.TabIndex = 27
        Me.pbRigSlotColour.TabStop = False
        '
        'gbSlotColours
        '
        Me.gbSlotColours.Controls.Add(Me.pbRigSlotColour)
        Me.gbSlotColours.Controls.Add(Me.lblRigSlotColour)
        Me.gbSlotColours.Controls.Add(Me.pbLowSlotColour)
        Me.gbSlotColours.Controls.Add(Me.lblLowSlotColour)
        Me.gbSlotColours.Controls.Add(Me.pbMidSlotColour)
        Me.gbSlotColours.Controls.Add(Me.pbHiSlotColour)
        Me.gbSlotColours.Controls.Add(Me.lblMidSlotColour)
        Me.gbSlotColours.Controls.Add(Me.lblHiSlotColour)
        Me.gbSlotColours.Location = New System.Drawing.Point(479, 146)
        Me.gbSlotColours.Name = "gbSlotColours"
        Me.gbSlotColours.Size = New System.Drawing.Size(135, 96)
        Me.gbSlotColours.TabIndex = 3
        Me.gbSlotColours.TabStop = False
        Me.gbSlotColours.Text = "Slot Colours"
        Me.gbSlotColours.Visible = False
        '
        'gbCache
        '
        Me.gbCache.Controls.Add(Me.btnDeleteCache)
        Me.gbCache.Location = New System.Drawing.Point(285, 254)
        Me.gbCache.Name = "gbCache"
        Me.gbCache.Size = New System.Drawing.Size(138, 45)
        Me.gbCache.TabIndex = 29
        Me.gbCache.TabStop = False
        Me.gbCache.Text = "Data Cache Settings"
        Me.gbCache.Visible = False
        '
        'btnDeleteCache
        '
        Me.btnDeleteCache.Location = New System.Drawing.Point(36, 47)
        Me.btnDeleteCache.Name = "btnDeleteCache"
        Me.btnDeleteCache.Size = New System.Drawing.Size(102, 23)
        Me.btnDeleteCache.TabIndex = 1
        Me.btnDeleteCache.Text = "Delete Cache"
        Me.btnDeleteCache.UseVisualStyleBackColor = True
        '
        'chkCloseInfoPanel
        '
        Me.chkCloseInfoPanel.AutoSize = True
        Me.chkCloseInfoPanel.Location = New System.Drawing.Point(25, 142)
        Me.chkCloseInfoPanel.Name = "chkCloseInfoPanel"
        Me.chkCloseInfoPanel.Size = New System.Drawing.Size(246, 17)
        Me.chkCloseInfoPanel.TabIndex = 11
        Me.chkCloseInfoPanel.Text = "Close EveHQ ""Info Panel"" when opening HQF"
        Me.chkCloseInfoPanel.UseVisualStyleBackColor = True
        '
        'frmHQFSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(704, 524)
        Me.Controls.Add(Me.gbGeneral)
        Me.Controls.Add(Me.gbCache)
        Me.Controls.Add(Me.gbSlotColours)
        Me.Controls.Add(Me.tvwSettings)
        Me.Controls.Add(Me.btnClose)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmHQFSettings"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "HQF Settings"
        Me.gbGeneral.ResumeLayout(False)
        Me.gbGeneral.PerformLayout()
        CType(Me.pbHiSlotColour, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbMidSlotColour, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbLowSlotColour, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbRigSlotColour, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbSlotColours.ResumeLayout(False)
        Me.gbSlotColours.PerformLayout()
        Me.gbCache.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents gbGeneral As System.Windows.Forms.GroupBox
    Friend WithEvents fbd1 As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents cboStartupPilot As System.Windows.Forms.ComboBox
    Friend WithEvents lblDefaultPilot As System.Windows.Forms.Label
    Friend WithEvents ofd1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents tvwSettings As System.Windows.Forms.TreeView
    Friend WithEvents cd1 As System.Windows.Forms.ColorDialog
    Friend WithEvents lblHiSlotColour As System.Windows.Forms.Label
    Friend WithEvents lblMidSlotColour As System.Windows.Forms.Label
    Friend WithEvents pbHiSlotColour As System.Windows.Forms.PictureBox
    Friend WithEvents pbMidSlotColour As System.Windows.Forms.PictureBox
    Friend WithEvents lblLowSlotColour As System.Windows.Forms.Label
    Friend WithEvents pbLowSlotColour As System.Windows.Forms.PictureBox
    Friend WithEvents lblRigSlotColour As System.Windows.Forms.Label
    Friend WithEvents pbRigSlotColour As System.Windows.Forms.PictureBox
    Friend WithEvents gbSlotColours As System.Windows.Forms.GroupBox
    Friend WithEvents chkRestoreLastSession As System.Windows.Forms.CheckBox
    Friend WithEvents gbCache As System.Windows.Forms.GroupBox
    Friend WithEvents btnDeleteCache As System.Windows.Forms.Button
    Friend WithEvents chkAutoUpdateHQFSkills As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowPerformance As System.Windows.Forms.CheckBox
    Friend WithEvents chkCloseInfoPanel As System.Windows.Forms.CheckBox
End Class
