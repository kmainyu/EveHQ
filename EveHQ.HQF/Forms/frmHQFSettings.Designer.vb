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
        Me.components = New System.ComponentModel.Container
        Dim TreeNode1 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("General")
        Dim TreeNode2 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Calculation Constants")
        Dim TreeNode3 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Data and Cache")
        Dim TreeNode4 As System.Windows.Forms.TreeNode = New System.Windows.Forms.TreeNode("Slot Layout")
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmHQFSettings))
        Me.gbGeneral = New System.Windows.Forms.GroupBox
        Me.chkCloseInfoPanel = New System.Windows.Forms.CheckBox
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
        Me.gbSlotFormat = New System.Windows.Forms.GroupBox
        Me.lblSubSlotColour = New System.Windows.Forms.Label
        Me.pbSubSlotColour = New System.Windows.Forms.PictureBox
        Me.btnMoveDown = New System.Windows.Forms.Button
        Me.btnMoveUp = New System.Windows.Forms.Button
        Me.lvwColumns = New System.Windows.Forms.ListView
        Me.colSlotColumns = New System.Windows.Forms.ColumnHeader
        Me.lblSlotColumns = New System.Windows.Forms.Label
        Me.gbCache = New System.Windows.Forms.GroupBox
        Me.btnCheckMarket = New System.Windows.Forms.Button
        Me.btnExportShipBonuses = New System.Windows.Forms.Button
        Me.btnExportImplantEffects = New System.Windows.Forms.Button
        Me.btnExportEffects = New System.Windows.Forms.Button
        Me.btnCheckAttributeIntFloat = New System.Windows.Forms.Button
        Me.btnCheckModuleMetaData = New System.Windows.Forms.Button
        Me.btnDeleteAllFittings = New System.Windows.Forms.Button
        Me.btnCheckData = New System.Windows.Forms.Button
        Me.btnDeleteCache = New System.Windows.Forms.Button
        Me.gbConstants = New System.Windows.Forms.GroupBox
        Me.lblCapRechargeUnit = New System.Windows.Forms.Label
        Me.lblShieldRechargeUnit = New System.Windows.Forms.Label
        Me.lblMissileRangeUnit = New System.Windows.Forms.Label
        Me.nudMissileRange = New System.Windows.Forms.NumericUpDown
        Me.lblMissileRange = New System.Windows.Forms.Label
        Me.nudShieldRecharge = New System.Windows.Forms.NumericUpDown
        Me.lblShieldRecharge = New System.Windows.Forms.Label
        Me.nudCapRecharge = New System.Windows.Forms.NumericUpDown
        Me.lblCapRecharge = New System.Windows.Forms.Label
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.gbGeneral.SuspendLayout()
        CType(Me.pbHiSlotColour, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbMidSlotColour, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbLowSlotColour, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbRigSlotColour, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbSlotFormat.SuspendLayout()
        CType(Me.pbSubSlotColour, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbCache.SuspendLayout()
        Me.gbConstants.SuspendLayout()
        CType(Me.nudMissileRange, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudShieldRecharge, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudCapRecharge, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.gbGeneral.Location = New System.Drawing.Point(444, 373)
        Me.gbGeneral.Name = "gbGeneral"
        Me.gbGeneral.Size = New System.Drawing.Size(128, 50)
        Me.gbGeneral.TabIndex = 1
        Me.gbGeneral.TabStop = False
        Me.gbGeneral.Text = "General Settings"
        Me.gbGeneral.Visible = False
        '
        'chkCloseInfoPanel
        '
        Me.chkCloseInfoPanel.AutoSize = True
        Me.chkCloseInfoPanel.Location = New System.Drawing.Point(25, 142)
        Me.chkCloseInfoPanel.Name = "chkCloseInfoPanel"
        Me.chkCloseInfoPanel.Size = New System.Drawing.Size(242, 17)
        Me.chkCloseInfoPanel.TabIndex = 11
        Me.chkCloseInfoPanel.Text = "Close EveHQ ""Info Panel"" when opening HQF"
        Me.chkCloseInfoPanel.UseVisualStyleBackColor = True
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
        Me.chkAutoUpdateHQFSkills.Size = New System.Drawing.Size(239, 17)
        Me.chkAutoUpdateHQFSkills.TabIndex = 9
        Me.chkAutoUpdateHQFSkills.Text = "Update HQF Skills to Actual Skills on Start-up"
        Me.chkAutoUpdateHQFSkills.UseVisualStyleBackColor = True
        '
        'chkRestoreLastSession
        '
        Me.chkRestoreLastSession.AutoSize = True
        Me.chkRestoreLastSession.Location = New System.Drawing.Point(25, 96)
        Me.chkRestoreLastSession.Name = "chkRestoreLastSession"
        Me.chkRestoreLastSession.Size = New System.Drawing.Size(245, 17)
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
        Me.lblDefaultPilot.Size = New System.Drawing.Size(124, 13)
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
        TreeNode2.Name = "nodeConstants"
        TreeNode2.Text = "Calculation Constants"
        TreeNode3.Name = "nodeCache"
        TreeNode3.Text = "Data and Cache"
        TreeNode4.Name = "nodeSlotFormat"
        TreeNode4.Text = "Slot Layout"
        Me.tvwSettings.Nodes.AddRange(New System.Windows.Forms.TreeNode() {TreeNode1, TreeNode2, TreeNode3, TreeNode4})
        Me.tvwSettings.Size = New System.Drawing.Size(176, 473)
        Me.tvwSettings.TabIndex = 27
        '
        'lblHiSlotColour
        '
        Me.lblHiSlotColour.AutoSize = True
        Me.lblHiSlotColour.Location = New System.Drawing.Point(284, 101)
        Me.lblHiSlotColour.Name = "lblHiSlotColour"
        Me.lblHiSlotColour.Size = New System.Drawing.Size(71, 13)
        Me.lblHiSlotColour.TabIndex = 20
        Me.lblHiSlotColour.Text = "Hi Slot Colour"
        '
        'lblMidSlotColour
        '
        Me.lblMidSlotColour.AutoSize = True
        Me.lblMidSlotColour.Location = New System.Drawing.Point(284, 131)
        Me.lblMidSlotColour.Name = "lblMidSlotColour"
        Me.lblMidSlotColour.Size = New System.Drawing.Size(78, 13)
        Me.lblMidSlotColour.TabIndex = 21
        Me.lblMidSlotColour.Text = "Mid Slot Colour"
        '
        'pbHiSlotColour
        '
        Me.pbHiSlotColour.BackColor = System.Drawing.Color.LightSteelBlue
        Me.pbHiSlotColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbHiSlotColour.Location = New System.Drawing.Point(411, 94)
        Me.pbHiSlotColour.Name = "pbHiSlotColour"
        Me.pbHiSlotColour.Size = New System.Drawing.Size(24, 24)
        Me.pbHiSlotColour.TabIndex = 22
        Me.pbHiSlotColour.TabStop = False
        '
        'pbMidSlotColour
        '
        Me.pbMidSlotColour.BackColor = System.Drawing.Color.Plum
        Me.pbMidSlotColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbMidSlotColour.Location = New System.Drawing.Point(411, 124)
        Me.pbMidSlotColour.Name = "pbMidSlotColour"
        Me.pbMidSlotColour.Size = New System.Drawing.Size(24, 24)
        Me.pbMidSlotColour.TabIndex = 23
        Me.pbMidSlotColour.TabStop = False
        '
        'lblLowSlotColour
        '
        Me.lblLowSlotColour.AutoSize = True
        Me.lblLowSlotColour.Location = New System.Drawing.Point(284, 161)
        Me.lblLowSlotColour.Name = "lblLowSlotColour"
        Me.lblLowSlotColour.Size = New System.Drawing.Size(81, 13)
        Me.lblLowSlotColour.TabIndex = 24
        Me.lblLowSlotColour.Text = "Low Slot Colour"
        '
        'pbLowSlotColour
        '
        Me.pbLowSlotColour.BackColor = System.Drawing.Color.Gold
        Me.pbLowSlotColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbLowSlotColour.Location = New System.Drawing.Point(411, 154)
        Me.pbLowSlotColour.Name = "pbLowSlotColour"
        Me.pbLowSlotColour.Size = New System.Drawing.Size(24, 24)
        Me.pbLowSlotColour.TabIndex = 25
        Me.pbLowSlotColour.TabStop = False
        '
        'lblRigSlotColour
        '
        Me.lblRigSlotColour.AutoSize = True
        Me.lblRigSlotColour.Location = New System.Drawing.Point(284, 191)
        Me.lblRigSlotColour.Name = "lblRigSlotColour"
        Me.lblRigSlotColour.Size = New System.Drawing.Size(77, 13)
        Me.lblRigSlotColour.TabIndex = 26
        Me.lblRigSlotColour.Text = "Rig Slot Colour"
        '
        'pbRigSlotColour
        '
        Me.pbRigSlotColour.BackColor = System.Drawing.Color.Red
        Me.pbRigSlotColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbRigSlotColour.Location = New System.Drawing.Point(411, 184)
        Me.pbRigSlotColour.Name = "pbRigSlotColour"
        Me.pbRigSlotColour.Size = New System.Drawing.Size(24, 24)
        Me.pbRigSlotColour.TabIndex = 27
        Me.pbRigSlotColour.TabStop = False
        '
        'gbSlotFormat
        '
        Me.gbSlotFormat.Controls.Add(Me.lblSubSlotColour)
        Me.gbSlotFormat.Controls.Add(Me.pbSubSlotColour)
        Me.gbSlotFormat.Controls.Add(Me.btnMoveDown)
        Me.gbSlotFormat.Controls.Add(Me.btnMoveUp)
        Me.gbSlotFormat.Controls.Add(Me.lvwColumns)
        Me.gbSlotFormat.Controls.Add(Me.lblSlotColumns)
        Me.gbSlotFormat.Controls.Add(Me.pbRigSlotColour)
        Me.gbSlotFormat.Controls.Add(Me.lblRigSlotColour)
        Me.gbSlotFormat.Controls.Add(Me.pbLowSlotColour)
        Me.gbSlotFormat.Controls.Add(Me.lblLowSlotColour)
        Me.gbSlotFormat.Controls.Add(Me.pbMidSlotColour)
        Me.gbSlotFormat.Controls.Add(Me.pbHiSlotColour)
        Me.gbSlotFormat.Controls.Add(Me.lblMidSlotColour)
        Me.gbSlotFormat.Controls.Add(Me.lblHiSlotColour)
        Me.gbSlotFormat.Location = New System.Drawing.Point(465, 211)
        Me.gbSlotFormat.Name = "gbSlotFormat"
        Me.gbSlotFormat.Size = New System.Drawing.Size(107, 35)
        Me.gbSlotFormat.TabIndex = 3
        Me.gbSlotFormat.TabStop = False
        Me.gbSlotFormat.Text = "Slot Layout"
        Me.gbSlotFormat.Visible = False
        '
        'lblSubSlotColour
        '
        Me.lblSubSlotColour.AutoSize = True
        Me.lblSubSlotColour.Location = New System.Drawing.Point(284, 221)
        Me.lblSubSlotColour.Name = "lblSubSlotColour"
        Me.lblSubSlotColour.Size = New System.Drawing.Size(114, 13)
        Me.lblSubSlotColour.TabIndex = 33
        Me.lblSubSlotColour.Text = "Subsystem Slot Colour"
        '
        'pbSubSlotColour
        '
        Me.pbSubSlotColour.BackColor = System.Drawing.Color.DarkSeaGreen
        Me.pbSubSlotColour.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.pbSubSlotColour.Location = New System.Drawing.Point(411, 214)
        Me.pbSubSlotColour.Name = "pbSubSlotColour"
        Me.pbSubSlotColour.Size = New System.Drawing.Size(24, 24)
        Me.pbSubSlotColour.TabIndex = 32
        Me.pbSubSlotColour.TabStop = False
        '
        'btnMoveDown
        '
        Me.btnMoveDown.Location = New System.Drawing.Point(104, 384)
        Me.btnMoveDown.Name = "btnMoveDown"
        Me.btnMoveDown.Size = New System.Drawing.Size(80, 23)
        Me.btnMoveDown.TabIndex = 31
        Me.btnMoveDown.Text = "Move Down"
        Me.btnMoveDown.UseVisualStyleBackColor = True
        '
        'btnMoveUp
        '
        Me.btnMoveUp.Location = New System.Drawing.Point(18, 384)
        Me.btnMoveUp.Name = "btnMoveUp"
        Me.btnMoveUp.Size = New System.Drawing.Size(80, 23)
        Me.btnMoveUp.TabIndex = 30
        Me.btnMoveUp.Text = "Move Up"
        Me.btnMoveUp.UseVisualStyleBackColor = True
        '
        'lvwColumns
        '
        Me.lvwColumns.CheckBoxes = True
        Me.lvwColumns.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colSlotColumns})
        Me.lvwColumns.FullRowSelect = True
        Me.lvwColumns.HideSelection = False
        Me.lvwColumns.Location = New System.Drawing.Point(18, 54)
        Me.lvwColumns.Name = "lvwColumns"
        Me.lvwColumns.Size = New System.Drawing.Size(222, 324)
        Me.lvwColumns.TabIndex = 29
        Me.lvwColumns.UseCompatibleStateImageBehavior = False
        Me.lvwColumns.View = System.Windows.Forms.View.Details
        '
        'colSlotColumns
        '
        Me.colSlotColumns.Text = "Slot Columns"
        Me.colSlotColumns.Width = 200
        '
        'lblSlotColumns
        '
        Me.lblSlotColumns.AutoSize = True
        Me.lblSlotColumns.Location = New System.Drawing.Point(15, 38)
        Me.lblSlotColumns.Name = "lblSlotColumns"
        Me.lblSlotColumns.Size = New System.Drawing.Size(113, 13)
        Me.lblSlotColumns.TabIndex = 28
        Me.lblSlotColumns.Text = "Slot Column Selection:"
        '
        'gbCache
        '
        Me.gbCache.Controls.Add(Me.btnCheckMarket)
        Me.gbCache.Controls.Add(Me.btnExportShipBonuses)
        Me.gbCache.Controls.Add(Me.btnExportImplantEffects)
        Me.gbCache.Controls.Add(Me.btnExportEffects)
        Me.gbCache.Controls.Add(Me.btnCheckAttributeIntFloat)
        Me.gbCache.Controls.Add(Me.btnCheckModuleMetaData)
        Me.gbCache.Controls.Add(Me.btnDeleteAllFittings)
        Me.gbCache.Controls.Add(Me.btnCheckData)
        Me.gbCache.Controls.Add(Me.btnDeleteCache)
        Me.gbCache.Location = New System.Drawing.Point(194, 12)
        Me.gbCache.Name = "gbCache"
        Me.gbCache.Size = New System.Drawing.Size(498, 500)
        Me.gbCache.TabIndex = 29
        Me.gbCache.TabStop = False
        Me.gbCache.Text = "Data and Cache Settings"
        Me.gbCache.Visible = False
        '
        'btnCheckMarket
        '
        Me.btnCheckMarket.Location = New System.Drawing.Point(330, 458)
        Me.btnCheckMarket.Name = "btnCheckMarket"
        Me.btnCheckMarket.Size = New System.Drawing.Size(105, 33)
        Me.btnCheckMarket.TabIndex = 9
        Me.btnCheckMarket.Text = "Check Market"
        Me.btnCheckMarket.UseVisualStyleBackColor = True
        Me.btnCheckMarket.Visible = False
        '
        'btnExportShipBonuses
        '
        Me.btnExportShipBonuses.Location = New System.Drawing.Point(36, 170)
        Me.btnExportShipBonuses.Name = "btnExportShipBonuses"
        Me.btnExportShipBonuses.Size = New System.Drawing.Size(137, 23)
        Me.btnExportShipBonuses.TabIndex = 8
        Me.btnExportShipBonuses.Text = "Export Ship Bonuses"
        Me.btnExportShipBonuses.UseVisualStyleBackColor = True
        Me.btnExportShipBonuses.Visible = False
        '
        'btnExportImplantEffects
        '
        Me.btnExportImplantEffects.Location = New System.Drawing.Point(36, 141)
        Me.btnExportImplantEffects.Name = "btnExportImplantEffects"
        Me.btnExportImplantEffects.Size = New System.Drawing.Size(137, 23)
        Me.btnExportImplantEffects.TabIndex = 7
        Me.btnExportImplantEffects.Text = "Export Implant Effects"
        Me.btnExportImplantEffects.UseVisualStyleBackColor = True
        Me.btnExportImplantEffects.Visible = False
        '
        'btnExportEffects
        '
        Me.btnExportEffects.Location = New System.Drawing.Point(36, 112)
        Me.btnExportEffects.Name = "btnExportEffects"
        Me.btnExportEffects.Size = New System.Drawing.Size(137, 23)
        Me.btnExportEffects.TabIndex = 6
        Me.btnExportEffects.Text = "Export Effects"
        Me.btnExportEffects.UseVisualStyleBackColor = True
        Me.btnExportEffects.Visible = False
        '
        'btnCheckAttributeIntFloat
        '
        Me.btnCheckAttributeIntFloat.Location = New System.Drawing.Point(6, 433)
        Me.btnCheckAttributeIntFloat.Name = "btnCheckAttributeIntFloat"
        Me.btnCheckAttributeIntFloat.Size = New System.Drawing.Size(102, 58)
        Me.btnCheckAttributeIntFloat.TabIndex = 5
        Me.btnCheckAttributeIntFloat.Text = "Check Module Attribute Int/Float"
        Me.btnCheckAttributeIntFloat.UseVisualStyleBackColor = True
        Me.btnCheckAttributeIntFloat.Visible = False
        '
        'btnCheckModuleMetaData
        '
        Me.btnCheckModuleMetaData.Location = New System.Drawing.Point(114, 445)
        Me.btnCheckModuleMetaData.Name = "btnCheckModuleMetaData"
        Me.btnCheckModuleMetaData.Size = New System.Drawing.Size(102, 46)
        Me.btnCheckModuleMetaData.TabIndex = 4
        Me.btnCheckModuleMetaData.Text = "Check Module Meta Data"
        Me.btnCheckModuleMetaData.UseVisualStyleBackColor = True
        Me.btnCheckModuleMetaData.Visible = False
        '
        'btnDeleteAllFittings
        '
        Me.btnDeleteAllFittings.Location = New System.Drawing.Point(267, 47)
        Me.btnDeleteAllFittings.Name = "btnDeleteAllFittings"
        Me.btnDeleteAllFittings.Size = New System.Drawing.Size(102, 23)
        Me.btnDeleteAllFittings.TabIndex = 3
        Me.btnDeleteAllFittings.Text = "Delete All Fittings"
        Me.btnDeleteAllFittings.UseVisualStyleBackColor = True
        '
        'btnCheckData
        '
        Me.btnCheckData.Location = New System.Drawing.Point(222, 468)
        Me.btnCheckData.Name = "btnCheckData"
        Me.btnCheckData.Size = New System.Drawing.Size(102, 23)
        Me.btnCheckData.TabIndex = 2
        Me.btnCheckData.Text = "Check Data"
        Me.btnCheckData.UseVisualStyleBackColor = True
        Me.btnCheckData.Visible = False
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
        'gbConstants
        '
        Me.gbConstants.Controls.Add(Me.lblCapRechargeUnit)
        Me.gbConstants.Controls.Add(Me.lblShieldRechargeUnit)
        Me.gbConstants.Controls.Add(Me.lblMissileRangeUnit)
        Me.gbConstants.Controls.Add(Me.nudMissileRange)
        Me.gbConstants.Controls.Add(Me.lblMissileRange)
        Me.gbConstants.Controls.Add(Me.nudShieldRecharge)
        Me.gbConstants.Controls.Add(Me.lblShieldRecharge)
        Me.gbConstants.Controls.Add(Me.nudCapRecharge)
        Me.gbConstants.Controls.Add(Me.lblCapRecharge)
        Me.gbConstants.Location = New System.Drawing.Point(219, 316)
        Me.gbConstants.Name = "gbConstants"
        Me.gbConstants.Size = New System.Drawing.Size(159, 43)
        Me.gbConstants.TabIndex = 30
        Me.gbConstants.TabStop = False
        Me.gbConstants.Text = "Calculation Constants"
        Me.gbConstants.Visible = False
        '
        'lblCapRechargeUnit
        '
        Me.lblCapRechargeUnit.AutoSize = True
        Me.lblCapRechargeUnit.Location = New System.Drawing.Point(258, 47)
        Me.lblCapRechargeUnit.Name = "lblCapRechargeUnit"
        Me.lblCapRechargeUnit.Size = New System.Drawing.Size(13, 13)
        Me.lblCapRechargeUnit.TabIndex = 8
        Me.lblCapRechargeUnit.Text = "x"
        '
        'lblShieldRechargeUnit
        '
        Me.lblShieldRechargeUnit.AutoSize = True
        Me.lblShieldRechargeUnit.Location = New System.Drawing.Point(258, 75)
        Me.lblShieldRechargeUnit.Name = "lblShieldRechargeUnit"
        Me.lblShieldRechargeUnit.Size = New System.Drawing.Size(13, 13)
        Me.lblShieldRechargeUnit.TabIndex = 7
        Me.lblShieldRechargeUnit.Text = "x"
        '
        'lblMissileRangeUnit
        '
        Me.lblMissileRangeUnit.AutoSize = True
        Me.lblMissileRangeUnit.Location = New System.Drawing.Point(258, 102)
        Me.lblMissileRangeUnit.Name = "lblMissileRangeUnit"
        Me.lblMissileRangeUnit.Size = New System.Drawing.Size(13, 13)
        Me.lblMissileRangeUnit.TabIndex = 6
        Me.lblMissileRangeUnit.Text = "x"
        '
        'nudMissileRange
        '
        Me.nudMissileRange.DecimalPlaces = 2
        Me.nudMissileRange.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudMissileRange.Location = New System.Drawing.Point(180, 100)
        Me.nudMissileRange.Maximum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudMissileRange.Minimum = New Decimal(New Integer() {50, 0, 0, 131072})
        Me.nudMissileRange.Name = "nudMissileRange"
        Me.nudMissileRange.Size = New System.Drawing.Size(72, 21)
        Me.nudMissileRange.TabIndex = 5
        Me.nudMissileRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip1.SetToolTip(Me.nudMissileRange, "Defines the proportion of the theoretical maximum range of missiles (velocity x f" & _
                "light time) to be used for optimal missile optimal range calculations")
        Me.nudMissileRange.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'lblMissileRange
        '
        Me.lblMissileRange.AutoSize = True
        Me.lblMissileRange.Location = New System.Drawing.Point(15, 102)
        Me.lblMissileRange.Name = "lblMissileRange"
        Me.lblMissileRange.Size = New System.Drawing.Size(122, 13)
        Me.lblMissileRange.TabIndex = 4
        Me.lblMissileRange.Text = "Missile Range Constant:"
        Me.ToolTip1.SetToolTip(Me.lblMissileRange, "Defines the proportion of the theoretical maximum range of missiles (velocity x f" & _
                "light time) to be used for optimal missile optimal range calculations")
        '
        'nudShieldRecharge
        '
        Me.nudShieldRecharge.DecimalPlaces = 2
        Me.nudShieldRecharge.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudShieldRecharge.Location = New System.Drawing.Point(180, 73)
        Me.nudShieldRecharge.Maximum = New Decimal(New Integer() {25, 0, 0, 65536})
        Me.nudShieldRecharge.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.nudShieldRecharge.Name = "nudShieldRecharge"
        Me.nudShieldRecharge.Size = New System.Drawing.Size(72, 21)
        Me.nudShieldRecharge.TabIndex = 3
        Me.nudShieldRecharge.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip1.SetToolTip(Me.nudShieldRecharge, "Defines the peak recharge rate of the shields (max = 2.50 x average)")
        Me.nudShieldRecharge.Value = New Decimal(New Integer() {25, 0, 0, 65536})
        '
        'lblShieldRecharge
        '
        Me.lblShieldRecharge.AutoSize = True
        Me.lblShieldRecharge.Location = New System.Drawing.Point(15, 75)
        Me.lblShieldRecharge.Name = "lblShieldRecharge"
        Me.lblShieldRecharge.Size = New System.Drawing.Size(135, 13)
        Me.lblShieldRecharge.TabIndex = 2
        Me.lblShieldRecharge.Text = "Shield Recharge Constant:"
        Me.ToolTip1.SetToolTip(Me.lblShieldRecharge, "Defines the peak recharge rate of the shields (max = 2.50 x average)")
        '
        'nudCapRecharge
        '
        Me.nudCapRecharge.DecimalPlaces = 2
        Me.nudCapRecharge.Increment = New Decimal(New Integer() {1, 0, 0, 131072})
        Me.nudCapRecharge.Location = New System.Drawing.Point(180, 45)
        Me.nudCapRecharge.Maximum = New Decimal(New Integer() {25, 0, 0, 65536})
        Me.nudCapRecharge.Minimum = New Decimal(New Integer() {2, 0, 0, 0})
        Me.nudCapRecharge.Name = "nudCapRecharge"
        Me.nudCapRecharge.Size = New System.Drawing.Size(72, 21)
        Me.nudCapRecharge.TabIndex = 1
        Me.nudCapRecharge.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip1.SetToolTip(Me.nudCapRecharge, "Defines the peak recharge rate of the capacitor (max = 2.50 x average)")
        Me.nudCapRecharge.Value = New Decimal(New Integer() {25, 0, 0, 65536})
        '
        'lblCapRecharge
        '
        Me.lblCapRecharge.AutoSize = True
        Me.lblCapRecharge.Location = New System.Drawing.Point(15, 47)
        Me.lblCapRecharge.Name = "lblCapRecharge"
        Me.lblCapRecharge.Size = New System.Drawing.Size(153, 13)
        Me.lblCapRecharge.TabIndex = 0
        Me.lblCapRecharge.Text = "Capacitor Recharge Constant:"
        Me.ToolTip1.SetToolTip(Me.lblCapRecharge, "Defines the peak recharge rate of the capacitor (max = 2.50 x average)")
        '
        'frmHQFSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(704, 524)
        Me.Controls.Add(Me.gbCache)
        Me.Controls.Add(Me.gbSlotFormat)
        Me.Controls.Add(Me.gbConstants)
        Me.Controls.Add(Me.gbGeneral)
        Me.Controls.Add(Me.tvwSettings)
        Me.Controls.Add(Me.btnClose)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
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
        Me.gbSlotFormat.ResumeLayout(False)
        Me.gbSlotFormat.PerformLayout()
        CType(Me.pbSubSlotColour, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbCache.ResumeLayout(False)
        Me.gbConstants.ResumeLayout(False)
        Me.gbConstants.PerformLayout()
        CType(Me.nudMissileRange, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudShieldRecharge, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudCapRecharge, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents gbSlotFormat As System.Windows.Forms.GroupBox
    Friend WithEvents chkRestoreLastSession As System.Windows.Forms.CheckBox
    Friend WithEvents gbCache As System.Windows.Forms.GroupBox
    Friend WithEvents btnDeleteCache As System.Windows.Forms.Button
    Friend WithEvents chkAutoUpdateHQFSkills As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowPerformance As System.Windows.Forms.CheckBox
    Friend WithEvents chkCloseInfoPanel As System.Windows.Forms.CheckBox
    Friend WithEvents gbConstants As System.Windows.Forms.GroupBox
    Friend WithEvents nudShieldRecharge As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblShieldRecharge As System.Windows.Forms.Label
    Friend WithEvents nudCapRecharge As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblCapRecharge As System.Windows.Forms.Label
    Friend WithEvents lblSlotColumns As System.Windows.Forms.Label
    Friend WithEvents lvwColumns As System.Windows.Forms.ListView
    Friend WithEvents colSlotColumns As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnMoveDown As System.Windows.Forms.Button
    Friend WithEvents btnMoveUp As System.Windows.Forms.Button
    Friend WithEvents btnCheckData As System.Windows.Forms.Button
    Friend WithEvents btnDeleteAllFittings As System.Windows.Forms.Button
    Friend WithEvents btnCheckModuleMetaData As System.Windows.Forms.Button
    Friend WithEvents btnCheckAttributeIntFloat As System.Windows.Forms.Button
    Friend WithEvents btnExportShipBonuses As System.Windows.Forms.Button
    Friend WithEvents btnExportImplantEffects As System.Windows.Forms.Button
    Friend WithEvents btnExportEffects As System.Windows.Forms.Button
    Friend WithEvents nudMissileRange As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblMissileRange As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents lblMissileRangeUnit As System.Windows.Forms.Label
    Friend WithEvents lblCapRechargeUnit As System.Windows.Forms.Label
    Friend WithEvents lblShieldRechargeUnit As System.Windows.Forms.Label
    Friend WithEvents lblSubSlotColour As System.Windows.Forms.Label
    Friend WithEvents pbSubSlotColour As System.Windows.Forms.PictureBox
    Friend WithEvents btnCheckMarket As System.Windows.Forms.Button
End Class
