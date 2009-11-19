<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBPCalculator
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
        Me.cboBPs = New System.Windows.Forms.ComboBox
        Me.chkOwnedBPOs = New System.Windows.Forms.CheckBox
        Me.gbBPOSelection = New System.Windows.Forms.GroupBox
        Me.lblBPRuns = New System.Windows.Forms.Label
        Me.lblBPRunsLbl = New System.Windows.Forms.Label
        Me.lblBPME = New System.Windows.Forms.Label
        Me.lblBPPE = New System.Windows.Forms.Label
        Me.lblBPWF = New System.Windows.Forms.Label
        Me.lblBPOMarketValue = New System.Windows.Forms.Label
        Me.lblBPOMarketValueLbl = New System.Windows.Forms.Label
        Me.pbBP = New System.Windows.Forms.PictureBox
        Me.lblBPMELbl = New System.Windows.Forms.Label
        Me.lblBPWFLbl = New System.Windows.Forms.Label
        Me.lblBPPELbl = New System.Windows.Forms.Label
        Me.gbProdLocation = New System.Windows.Forms.GroupBox
        Me.txtBatchSize = New System.Windows.Forms.TextBox
        Me.nudRunningCost = New System.Windows.Forms.NumericUpDown
        Me.nudInstallCost = New System.Windows.Forms.NumericUpDown
        Me.lblRunningCost = New System.Windows.Forms.Label
        Me.lblInstallCost = New System.Windows.Forms.Label
        Me.cboPOSArrays = New System.Windows.Forms.ComboBox
        Me.chkPOSProduction = New System.Windows.Forms.CheckBox
        Me.lblRuns = New System.Windows.Forms.Label
        Me.nudRuns = New System.Windows.Forms.NumericUpDown
        Me.lblProdQuantity = New System.Windows.Forms.Label
        Me.txtProdQuantity = New System.Windows.Forms.TextBox
        Me.lblBatchSizeLbl = New System.Windows.Forms.Label
        Me.gbAddResearch = New System.Windows.Forms.GroupBox
        Me.nudCopyRuns = New System.Windows.Forms.NumericUpDown
        Me.lblRunsPerCopy = New System.Windows.Forms.Label
        Me.lblCopyTime = New System.Windows.Forms.Label
        Me.lblBPCopyTimeLbl = New System.Windows.Forms.Label
        Me.lblPETime = New System.Windows.Forms.Label
        Me.lblMETime = New System.Windows.Forms.Label
        Me.nudPELevel = New System.Windows.Forms.NumericUpDown
        Me.nudMELevel = New System.Windows.Forms.NumericUpDown
        Me.chkResearchAtPOS = New System.Windows.Forms.CheckBox
        Me.lblPETimeLbl = New System.Windows.Forms.Label
        Me.lblMETimeLbl = New System.Windows.Forms.Label
        Me.lblNewMELbl = New System.Windows.Forms.Label
        Me.LblNewWFLbl = New System.Windows.Forms.Label
        Me.lblNewPELbl = New System.Windows.Forms.Label
        Me.txtNewWasteFactor = New System.Windows.Forms.TextBox
        Me.gbProduction = New System.Windows.Forms.GroupBox
        Me.lblUnitProfit = New System.Windows.Forms.Label
        Me.lblUnitProfitlbl = New System.Windows.Forms.Label
        Me.lblUnitValue = New System.Windows.Forms.Label
        Me.lblUnitValuelbl = New System.Windows.Forms.Label
        Me.lblUnitCost = New System.Windows.Forms.Label
        Me.lblUnitCostLbl = New System.Windows.Forms.Label
        Me.lblTotalWasteCost = New System.Windows.Forms.Label
        Me.lblTotalWasteCostLbl = New System.Windows.Forms.Label
        Me.lblUnitWasteCost = New System.Windows.Forms.Label
        Me.lblUnitWasteCostLbl = New System.Windows.Forms.Label
        Me.lblTotalCosts = New System.Windows.Forms.Label
        Me.lblTotalCostsLbl = New System.Windows.Forms.Label
        Me.lblFactoryCosts = New System.Windows.Forms.Label
        Me.lblFactoryCostsLbl = New System.Windows.Forms.Label
        Me.lblTotalBuildCost = New System.Windows.Forms.Label
        Me.lblUnitBuildCost = New System.Windows.Forms.Label
        Me.lblTotalBuildCostsLbl = New System.Windows.Forms.Label
        Me.lblUnitBuildCostsLbl = New System.Windows.Forms.Label
        Me.lblTotalBuildTime = New System.Windows.Forms.Label
        Me.lblUnitBuildTime = New System.Windows.Forms.Label
        Me.lblTotalBuildTimeLbl = New System.Windows.Forms.Label
        Me.lblUnitBuildTimeLbl = New System.Windows.Forms.Label
        Me.lblBPEfficiency = New System.Windows.Forms.Label
        Me.lblBPEfficiencyLbl = New System.Windows.Forms.Label
        Me.btnCopyToClipboard = New System.Windows.Forms.Button
        Me.gbSkills = New System.Windows.Forms.GroupBox
        Me.chkOverrideSkills = New System.Windows.Forms.CheckBox
        Me.lblPilot = New System.Windows.Forms.Label
        Me.cboPilot = New System.Windows.Forms.ComboBox
        Me.gbProdSkills = New System.Windows.Forms.GroupBox
        Me.cboIndustyImplant = New System.Windows.Forms.ComboBox
        Me.cboProdEffSkill = New System.Windows.Forms.ComboBox
        Me.cboIndustrySkill = New System.Windows.Forms.ComboBox
        Me.lblPESkill = New System.Windows.Forms.Label
        Me.lblIndustrySkill = New System.Windows.Forms.Label
        Me.gbResearchSkills = New System.Windows.Forms.GroupBox
        Me.cboScienceImplant = New System.Windows.Forms.ComboBox
        Me.cboMetallurgyImplant = New System.Windows.Forms.ComboBox
        Me.cboResearchImplant = New System.Windows.Forms.ComboBox
        Me.lblScienceSkill = New System.Windows.Forms.Label
        Me.cboScienceSkill = New System.Windows.Forms.ComboBox
        Me.cboMetallurgySkill = New System.Windows.Forms.ComboBox
        Me.cboResearchSkill = New System.Windows.Forms.ComboBox
        Me.lblMetallurgySkill = New System.Windows.Forms.Label
        Me.lblResearchSkill = New System.Windows.Forms.Label
        Me.tabBPResources = New System.Windows.Forms.TabControl
        Me.tabBPResourcesRequired = New System.Windows.Forms.TabPage
        Me.chkUseStandardBPCosting = New System.Windows.Forms.CheckBox
        Me.chkShowSkills = New System.Windows.Forms.CheckBox
        Me.clvResources = New DotNetLib.Windows.Forms.ContainerListView
        Me.colBPResMaterials = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBPResPerfect = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBPResWaste = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBPResTotal = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBPResPrice = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBPResValue = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBPResIdealML = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.tabBPResourcesOwned = New System.Windows.Forms.TabPage
        Me.cboAssetSelection = New System.Windows.Forms.ComboBox
        Me.lblAssetSelection = New System.Windows.Forms.Label
        Me.clvOwnedResources = New DotNetLib.Windows.Forms.ContainerListView
        Me.colBPOResMaterial = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBPOResNeeded = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBPOResOwned = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colBPOResSurplus = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.lblMaxUnits = New System.Windows.Forms.Label
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.gbBPOSelection.SuspendLayout()
        CType(Me.pbBP, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbProdLocation.SuspendLayout()
        CType(Me.nudRunningCost, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudInstallCost, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudRuns, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbAddResearch.SuspendLayout()
        CType(Me.nudCopyRuns, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPELevel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudMELevel, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbProduction.SuspendLayout()
        Me.gbSkills.SuspendLayout()
        Me.gbProdSkills.SuspendLayout()
        Me.gbResearchSkills.SuspendLayout()
        Me.tabBPResources.SuspendLayout()
        Me.tabBPResourcesRequired.SuspendLayout()
        Me.tabBPResourcesOwned.SuspendLayout()
        Me.SuspendLayout()
        '
        'cboBPs
        '
        Me.cboBPs.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.cboBPs.FormattingEnabled = True
        Me.cboBPs.ItemHeight = 13
        Me.cboBPs.Location = New System.Drawing.Point(6, 38)
        Me.cboBPs.MaxDropDownItems = 20
        Me.cboBPs.Name = "cboBPs"
        Me.cboBPs.Size = New System.Drawing.Size(393, 21)
        Me.cboBPs.TabIndex = 1
        '
        'chkOwnedBPOs
        '
        Me.chkOwnedBPOs.AutoSize = True
        Me.chkOwnedBPOs.Location = New System.Drawing.Point(6, 19)
        Me.chkOwnedBPOs.Name = "chkOwnedBPOs"
        Me.chkOwnedBPOs.Size = New System.Drawing.Size(133, 17)
        Me.chkOwnedBPOs.TabIndex = 2
        Me.chkOwnedBPOs.Text = "Owned blueprints only"
        Me.chkOwnedBPOs.UseVisualStyleBackColor = True
        '
        'gbBPOSelection
        '
        Me.gbBPOSelection.Controls.Add(Me.lblBPRuns)
        Me.gbBPOSelection.Controls.Add(Me.lblBPRunsLbl)
        Me.gbBPOSelection.Controls.Add(Me.lblBPME)
        Me.gbBPOSelection.Controls.Add(Me.lblBPPE)
        Me.gbBPOSelection.Controls.Add(Me.lblBPWF)
        Me.gbBPOSelection.Controls.Add(Me.lblBPOMarketValue)
        Me.gbBPOSelection.Controls.Add(Me.lblBPOMarketValueLbl)
        Me.gbBPOSelection.Controls.Add(Me.pbBP)
        Me.gbBPOSelection.Controls.Add(Me.lblBPMELbl)
        Me.gbBPOSelection.Controls.Add(Me.cboBPs)
        Me.gbBPOSelection.Controls.Add(Me.lblBPWFLbl)
        Me.gbBPOSelection.Controls.Add(Me.chkOwnedBPOs)
        Me.gbBPOSelection.Controls.Add(Me.lblBPPELbl)
        Me.gbBPOSelection.Location = New System.Drawing.Point(12, 12)
        Me.gbBPOSelection.Name = "gbBPOSelection"
        Me.gbBPOSelection.Size = New System.Drawing.Size(420, 135)
        Me.gbBPOSelection.TabIndex = 4
        Me.gbBPOSelection.TabStop = False
        Me.gbBPOSelection.Text = "Blueprint Selection && Information"
        '
        'lblBPRuns
        '
        Me.lblBPRuns.AutoSize = True
        Me.lblBPRuns.Location = New System.Drawing.Point(157, 112)
        Me.lblBPRuns.Name = "lblBPRuns"
        Me.lblBPRuns.Size = New System.Drawing.Size(13, 13)
        Me.lblBPRuns.TabIndex = 19
        Me.lblBPRuns.Text = "0"
        '
        'lblBPRunsLbl
        '
        Me.lblBPRunsLbl.AutoSize = True
        Me.lblBPRunsLbl.Location = New System.Drawing.Point(77, 112)
        Me.lblBPRunsLbl.Name = "lblBPRunsLbl"
        Me.lblBPRunsLbl.Size = New System.Drawing.Size(35, 13)
        Me.lblBPRunsLbl.TabIndex = 18
        Me.lblBPRunsLbl.Text = "Runs:"
        '
        'lblBPME
        '
        Me.lblBPME.AutoSize = True
        Me.lblBPME.Location = New System.Drawing.Point(157, 67)
        Me.lblBPME.Name = "lblBPME"
        Me.lblBPME.Size = New System.Drawing.Size(13, 13)
        Me.lblBPME.TabIndex = 17
        Me.lblBPME.Text = "0"
        '
        'lblBPPE
        '
        Me.lblBPPE.AutoSize = True
        Me.lblBPPE.Location = New System.Drawing.Point(157, 82)
        Me.lblBPPE.Name = "lblBPPE"
        Me.lblBPPE.Size = New System.Drawing.Size(13, 13)
        Me.lblBPPE.TabIndex = 16
        Me.lblBPPE.Text = "0"
        '
        'lblBPWF
        '
        Me.lblBPWF.AutoSize = True
        Me.lblBPWF.Location = New System.Drawing.Point(157, 97)
        Me.lblBPWF.Name = "lblBPWF"
        Me.lblBPWF.Size = New System.Drawing.Size(13, 13)
        Me.lblBPWF.TabIndex = 15
        Me.lblBPWF.Text = "0"
        '
        'lblBPOMarketValue
        '
        Me.lblBPOMarketValue.AutoSize = True
        Me.lblBPOMarketValue.Location = New System.Drawing.Point(272, 67)
        Me.lblBPOMarketValue.Name = "lblBPOMarketValue"
        Me.lblBPOMarketValue.Size = New System.Drawing.Size(28, 13)
        Me.lblBPOMarketValue.TabIndex = 12
        Me.lblBPOMarketValue.Text = "0 isk"
        '
        'lblBPOMarketValueLbl
        '
        Me.lblBPOMarketValueLbl.AutoSize = True
        Me.lblBPOMarketValueLbl.Location = New System.Drawing.Point(209, 67)
        Me.lblBPOMarketValueLbl.Name = "lblBPOMarketValueLbl"
        Me.lblBPOMarketValueLbl.Size = New System.Drawing.Size(57, 13)
        Me.lblBPOMarketValueLbl.TabIndex = 11
        Me.lblBPOMarketValueLbl.Text = "BPO Price:"
        '
        'pbBP
        '
        Me.pbBP.BackColor = System.Drawing.SystemColors.ButtonShadow
        Me.pbBP.Location = New System.Drawing.Point(7, 65)
        Me.pbBP.Name = "pbBP"
        Me.pbBP.Size = New System.Drawing.Size(64, 64)
        Me.pbBP.TabIndex = 10
        Me.pbBP.TabStop = False
        '
        'lblBPMELbl
        '
        Me.lblBPMELbl.AutoSize = True
        Me.lblBPMELbl.Location = New System.Drawing.Point(77, 67)
        Me.lblBPMELbl.Name = "lblBPMELbl"
        Me.lblBPMELbl.Size = New System.Drawing.Size(53, 13)
        Me.lblBPMELbl.TabIndex = 4
        Me.lblBPMELbl.Text = "ME level :"
        '
        'lblBPWFLbl
        '
        Me.lblBPWFLbl.AutoSize = True
        Me.lblBPWFLbl.Location = New System.Drawing.Point(77, 97)
        Me.lblBPWFLbl.Name = "lblBPWFLbl"
        Me.lblBPWFLbl.Size = New System.Drawing.Size(76, 13)
        Me.lblBPWFLbl.TabIndex = 8
        Me.lblBPWFLbl.Text = "Waste Factor:"
        '
        'lblBPPELbl
        '
        Me.lblBPPELbl.AutoSize = True
        Me.lblBPPELbl.Location = New System.Drawing.Point(77, 82)
        Me.lblBPPELbl.Name = "lblBPPELbl"
        Me.lblBPPELbl.Size = New System.Drawing.Size(51, 13)
        Me.lblBPPELbl.TabIndex = 6
        Me.lblBPPELbl.Text = "PE Level:"
        '
        'gbProdLocation
        '
        Me.gbProdLocation.Controls.Add(Me.txtBatchSize)
        Me.gbProdLocation.Controls.Add(Me.nudRunningCost)
        Me.gbProdLocation.Controls.Add(Me.nudInstallCost)
        Me.gbProdLocation.Controls.Add(Me.lblRunningCost)
        Me.gbProdLocation.Controls.Add(Me.lblInstallCost)
        Me.gbProdLocation.Controls.Add(Me.cboPOSArrays)
        Me.gbProdLocation.Controls.Add(Me.chkPOSProduction)
        Me.gbProdLocation.Controls.Add(Me.lblRuns)
        Me.gbProdLocation.Controls.Add(Me.nudRuns)
        Me.gbProdLocation.Controls.Add(Me.lblProdQuantity)
        Me.gbProdLocation.Controls.Add(Me.txtProdQuantity)
        Me.gbProdLocation.Controls.Add(Me.lblBatchSizeLbl)
        Me.gbProdLocation.Location = New System.Drawing.Point(6, 19)
        Me.gbProdLocation.Name = "gbProdLocation"
        Me.gbProdLocation.Size = New System.Drawing.Size(239, 199)
        Me.gbProdLocation.TabIndex = 10
        Me.gbProdLocation.TabStop = False
        Me.gbProdLocation.Text = "Production Location && Information"
        '
        'txtBatchSize
        '
        Me.txtBatchSize.Location = New System.Drawing.Point(115, 145)
        Me.txtBatchSize.Name = "txtBatchSize"
        Me.txtBatchSize.ReadOnly = True
        Me.txtBatchSize.Size = New System.Drawing.Size(93, 21)
        Me.txtBatchSize.TabIndex = 154
        Me.txtBatchSize.TabStop = False
        Me.txtBatchSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudRunningCost
        '
        Me.nudRunningCost.DecimalPlaces = 2
        Me.nudRunningCost.Location = New System.Drawing.Point(115, 93)
        Me.nudRunningCost.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.nudRunningCost.Name = "nudRunningCost"
        Me.nudRunningCost.Size = New System.Drawing.Size(93, 21)
        Me.nudRunningCost.TabIndex = 17
        Me.nudRunningCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudRunningCost.Value = New Decimal(New Integer() {333, 0, 0, 0})
        '
        'nudInstallCost
        '
        Me.nudInstallCost.DecimalPlaces = 2
        Me.nudInstallCost.Location = New System.Drawing.Point(115, 67)
        Me.nudInstallCost.Maximum = New Decimal(New Integer() {10000000, 0, 0, 0})
        Me.nudInstallCost.Name = "nudInstallCost"
        Me.nudInstallCost.Size = New System.Drawing.Size(93, 21)
        Me.nudInstallCost.TabIndex = 16
        Me.nudInstallCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudInstallCost.Value = New Decimal(New Integer() {1000, 0, 0, 0})
        '
        'lblRunningCost
        '
        Me.lblRunningCost.AutoSize = True
        Me.lblRunningCost.Location = New System.Drawing.Point(6, 95)
        Me.lblRunningCost.Name = "lblRunningCost"
        Me.lblRunningCost.Size = New System.Drawing.Size(78, 13)
        Me.lblRunningCost.TabIndex = 15
        Me.lblRunningCost.Text = "Cost Per Hour:"
        '
        'lblInstallCost
        '
        Me.lblInstallCost.AutoSize = True
        Me.lblInstallCost.Location = New System.Drawing.Point(6, 69)
        Me.lblInstallCost.Name = "lblInstallCost"
        Me.lblInstallCost.Size = New System.Drawing.Size(65, 13)
        Me.lblInstallCost.TabIndex = 14
        Me.lblInstallCost.Text = "Install Cost:"
        '
        'cboPOSArrays
        '
        Me.cboPOSArrays.Enabled = False
        Me.cboPOSArrays.FormattingEnabled = True
        Me.cboPOSArrays.Items.AddRange(New Object() {"Select your POS array..."})
        Me.cboPOSArrays.Location = New System.Drawing.Point(6, 39)
        Me.cboPOSArrays.Name = "cboPOSArrays"
        Me.cboPOSArrays.Size = New System.Drawing.Size(224, 21)
        Me.cboPOSArrays.TabIndex = 13
        Me.cboPOSArrays.Text = "Select your POS array..."
        '
        'chkPOSProduction
        '
        Me.chkPOSProduction.AutoSize = True
        Me.chkPOSProduction.Enabled = False
        Me.chkPOSProduction.Location = New System.Drawing.Point(6, 19)
        Me.chkPOSProduction.Name = "chkPOSProduction"
        Me.chkPOSProduction.Size = New System.Drawing.Size(204, 17)
        Me.chkPOSProduction.TabIndex = 12
        Me.chkPOSProduction.Text = "I'm using a POS array for production."
        Me.chkPOSProduction.UseVisualStyleBackColor = True
        '
        'lblRuns
        '
        Me.lblRuns.AutoSize = True
        Me.lblRuns.Location = New System.Drawing.Point(6, 121)
        Me.lblRuns.Name = "lblRuns"
        Me.lblRuns.Size = New System.Drawing.Size(89, 13)
        Me.lblRuns.TabIndex = 16
        Me.lblRuns.Text = "Production Runs:"
        '
        'nudRuns
        '
        Me.nudRuns.Location = New System.Drawing.Point(115, 119)
        Me.nudRuns.Maximum = New Decimal(New Integer() {1500, 0, 0, 0})
        Me.nudRuns.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudRuns.Name = "nudRuns"
        Me.nudRuns.Size = New System.Drawing.Size(93, 21)
        Me.nudRuns.TabIndex = 153
        Me.nudRuns.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.nudRuns.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'lblProdQuantity
        '
        Me.lblProdQuantity.AutoSize = True
        Me.lblProdQuantity.Location = New System.Drawing.Point(6, 174)
        Me.lblProdQuantity.Name = "lblProdQuantity"
        Me.lblProdQuantity.Size = New System.Drawing.Size(107, 13)
        Me.lblProdQuantity.TabIndex = 20
        Me.lblProdQuantity.Text = "Production Quantity:"
        '
        'txtProdQuantity
        '
        Me.txtProdQuantity.Location = New System.Drawing.Point(115, 171)
        Me.txtProdQuantity.Name = "txtProdQuantity"
        Me.txtProdQuantity.ReadOnly = True
        Me.txtProdQuantity.Size = New System.Drawing.Size(93, 21)
        Me.txtProdQuantity.TabIndex = 21
        Me.txtProdQuantity.TabStop = False
        Me.txtProdQuantity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblBatchSizeLbl
        '
        Me.lblBatchSizeLbl.AutoSize = True
        Me.lblBatchSizeLbl.Location = New System.Drawing.Point(6, 148)
        Me.lblBatchSizeLbl.Name = "lblBatchSizeLbl"
        Me.lblBatchSizeLbl.Size = New System.Drawing.Size(60, 13)
        Me.lblBatchSizeLbl.TabIndex = 18
        Me.lblBatchSizeLbl.Text = "Batch Size:"
        '
        'gbAddResearch
        '
        Me.gbAddResearch.Controls.Add(Me.nudCopyRuns)
        Me.gbAddResearch.Controls.Add(Me.lblRunsPerCopy)
        Me.gbAddResearch.Controls.Add(Me.lblCopyTime)
        Me.gbAddResearch.Controls.Add(Me.lblBPCopyTimeLbl)
        Me.gbAddResearch.Controls.Add(Me.lblPETime)
        Me.gbAddResearch.Controls.Add(Me.lblMETime)
        Me.gbAddResearch.Controls.Add(Me.nudPELevel)
        Me.gbAddResearch.Controls.Add(Me.nudMELevel)
        Me.gbAddResearch.Controls.Add(Me.chkResearchAtPOS)
        Me.gbAddResearch.Controls.Add(Me.lblPETimeLbl)
        Me.gbAddResearch.Controls.Add(Me.lblMETimeLbl)
        Me.gbAddResearch.Controls.Add(Me.lblNewMELbl)
        Me.gbAddResearch.Controls.Add(Me.LblNewWFLbl)
        Me.gbAddResearch.Controls.Add(Me.lblNewPELbl)
        Me.gbAddResearch.Controls.Add(Me.txtNewWasteFactor)
        Me.gbAddResearch.Enabled = False
        Me.gbAddResearch.Location = New System.Drawing.Point(12, 154)
        Me.gbAddResearch.Name = "gbAddResearch"
        Me.gbAddResearch.Size = New System.Drawing.Size(200, 224)
        Me.gbAddResearch.TabIndex = 16
        Me.gbAddResearch.TabStop = False
        Me.gbAddResearch.Text = "Additional research"
        '
        'nudCopyRuns
        '
        Me.nudCopyRuns.Location = New System.Drawing.Point(115, 58)
        Me.nudCopyRuns.Maximum = New Decimal(New Integer() {1500, 0, 0, 0})
        Me.nudCopyRuns.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudCopyRuns.Name = "nudCopyRuns"
        Me.nudCopyRuns.Size = New System.Drawing.Size(79, 21)
        Me.nudCopyRuns.TabIndex = 30
        Me.nudCopyRuns.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.ToolTip1.SetToolTip(Me.nudCopyRuns, "Limited by the maximum number of Runs")
        Me.nudCopyRuns.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'lblRunsPerCopy
        '
        Me.lblRunsPerCopy.AutoSize = True
        Me.lblRunsPerCopy.Location = New System.Drawing.Point(6, 60)
        Me.lblRunsPerCopy.Name = "lblRunsPerCopy"
        Me.lblRunsPerCopy.Size = New System.Drawing.Size(82, 13)
        Me.lblRunsPerCopy.TabIndex = 29
        Me.lblRunsPerCopy.Text = "Runs Per Copy:"
        '
        'lblCopyTime
        '
        Me.lblCopyTime.AutoSize = True
        Me.lblCopyTime.Location = New System.Drawing.Point(113, 185)
        Me.lblCopyTime.Name = "lblCopyTime"
        Me.lblCopyTime.Size = New System.Drawing.Size(18, 13)
        Me.lblCopyTime.TabIndex = 28
        Me.lblCopyTime.Text = "0s"
        '
        'lblBPCopyTimeLbl
        '
        Me.lblBPCopyTimeLbl.AutoSize = True
        Me.lblBPCopyTimeLbl.Location = New System.Drawing.Point(6, 185)
        Me.lblBPCopyTimeLbl.Name = "lblBPCopyTimeLbl"
        Me.lblBPCopyTimeLbl.Size = New System.Drawing.Size(106, 13)
        Me.lblBPCopyTimeLbl.TabIndex = 27
        Me.lblBPCopyTimeLbl.Text = "Blueprint Copy Time:"
        '
        'lblPETime
        '
        Me.lblPETime.AutoSize = True
        Me.lblPETime.Location = New System.Drawing.Point(113, 165)
        Me.lblPETime.Name = "lblPETime"
        Me.lblPETime.Size = New System.Drawing.Size(18, 13)
        Me.lblPETime.TabIndex = 26
        Me.lblPETime.Text = "0s"
        '
        'lblMETime
        '
        Me.lblMETime.AutoSize = True
        Me.lblMETime.Location = New System.Drawing.Point(113, 145)
        Me.lblMETime.Name = "lblMETime"
        Me.lblMETime.Size = New System.Drawing.Size(18, 13)
        Me.lblMETime.TabIndex = 25
        Me.lblMETime.Text = "0s"
        '
        'nudPELevel
        '
        Me.nudPELevel.Location = New System.Drawing.Point(115, 36)
        Me.nudPELevel.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudPELevel.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.nudPELevel.Name = "nudPELevel"
        Me.nudPELevel.Size = New System.Drawing.Size(79, 21)
        Me.nudPELevel.TabIndex = 24
        Me.nudPELevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'nudMELevel
        '
        Me.nudMELevel.Location = New System.Drawing.Point(115, 14)
        Me.nudMELevel.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.nudMELevel.Minimum = New Decimal(New Integer() {10, 0, 0, -2147483648})
        Me.nudMELevel.Name = "nudMELevel"
        Me.nudMELevel.Size = New System.Drawing.Size(79, 21)
        Me.nudMELevel.TabIndex = 23
        Me.nudMELevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chkResearchAtPOS
        '
        Me.chkResearchAtPOS.AutoSize = True
        Me.chkResearchAtPOS.Location = New System.Drawing.Point(7, 115)
        Me.chkResearchAtPOS.Name = "chkResearchAtPOS"
        Me.chkResearchAtPOS.Size = New System.Drawing.Size(156, 17)
        Me.chkResearchAtPOS.TabIndex = 14
        Me.chkResearchAtPOS.Text = "I do my research at a POS."
        Me.chkResearchAtPOS.UseVisualStyleBackColor = True
        '
        'lblPETimeLbl
        '
        Me.lblPETimeLbl.AutoSize = True
        Me.lblPETimeLbl.Location = New System.Drawing.Point(6, 165)
        Me.lblPETimeLbl.Name = "lblPETimeLbl"
        Me.lblPETimeLbl.Size = New System.Drawing.Size(96, 13)
        Me.lblPETimeLbl.TabIndex = 13
        Me.lblPETimeLbl.Text = "PE Research Time:"
        '
        'lblMETimeLbl
        '
        Me.lblMETimeLbl.AutoSize = True
        Me.lblMETimeLbl.Location = New System.Drawing.Point(6, 145)
        Me.lblMETimeLbl.Name = "lblMETimeLbl"
        Me.lblMETimeLbl.Size = New System.Drawing.Size(98, 13)
        Me.lblMETimeLbl.TabIndex = 12
        Me.lblMETimeLbl.Text = "ME Research Time:"
        '
        'lblNewMELbl
        '
        Me.lblNewMELbl.AutoSize = True
        Me.lblNewMELbl.Location = New System.Drawing.Point(6, 16)
        Me.lblNewMELbl.Name = "lblNewMELbl"
        Me.lblNewMELbl.Size = New System.Drawing.Size(80, 13)
        Me.lblNewMELbl.TabIndex = 4
        Me.lblNewMELbl.Text = "New ME Level :"
        '
        'LblNewWFLbl
        '
        Me.LblNewWFLbl.AutoSize = True
        Me.LblNewWFLbl.Location = New System.Drawing.Point(6, 83)
        Me.LblNewWFLbl.Name = "LblNewWFLbl"
        Me.LblNewWFLbl.Size = New System.Drawing.Size(103, 13)
        Me.LblNewWFLbl.TabIndex = 8
        Me.LblNewWFLbl.Text = "New Waste Factor :"
        '
        'lblNewPELbl
        '
        Me.lblNewPELbl.AutoSize = True
        Me.lblNewPELbl.Location = New System.Drawing.Point(6, 39)
        Me.lblNewPELbl.Name = "lblNewPELbl"
        Me.lblNewPELbl.Size = New System.Drawing.Size(78, 13)
        Me.lblNewPELbl.TabIndex = 6
        Me.lblNewPELbl.Text = "New PE Level :"
        '
        'txtNewWasteFactor
        '
        Me.txtNewWasteFactor.Location = New System.Drawing.Point(115, 80)
        Me.txtNewWasteFactor.Name = "txtNewWasteFactor"
        Me.txtNewWasteFactor.ReadOnly = True
        Me.txtNewWasteFactor.Size = New System.Drawing.Size(79, 21)
        Me.txtNewWasteFactor.TabIndex = 9
        Me.txtNewWasteFactor.TabStop = False
        Me.txtNewWasteFactor.Text = "0"
        Me.txtNewWasteFactor.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'gbProduction
        '
        Me.gbProduction.Controls.Add(Me.lblUnitProfit)
        Me.gbProduction.Controls.Add(Me.lblUnitProfitlbl)
        Me.gbProduction.Controls.Add(Me.lblUnitValue)
        Me.gbProduction.Controls.Add(Me.lblUnitValuelbl)
        Me.gbProduction.Controls.Add(Me.lblUnitCost)
        Me.gbProduction.Controls.Add(Me.lblUnitCostLbl)
        Me.gbProduction.Controls.Add(Me.lblTotalWasteCost)
        Me.gbProduction.Controls.Add(Me.lblTotalWasteCostLbl)
        Me.gbProduction.Controls.Add(Me.lblUnitWasteCost)
        Me.gbProduction.Controls.Add(Me.lblUnitWasteCostLbl)
        Me.gbProduction.Controls.Add(Me.lblTotalCosts)
        Me.gbProduction.Controls.Add(Me.lblTotalCostsLbl)
        Me.gbProduction.Controls.Add(Me.lblFactoryCosts)
        Me.gbProduction.Controls.Add(Me.lblFactoryCostsLbl)
        Me.gbProduction.Controls.Add(Me.lblTotalBuildCost)
        Me.gbProduction.Controls.Add(Me.lblUnitBuildCost)
        Me.gbProduction.Controls.Add(Me.lblTotalBuildCostsLbl)
        Me.gbProduction.Controls.Add(Me.lblUnitBuildCostsLbl)
        Me.gbProduction.Controls.Add(Me.lblTotalBuildTime)
        Me.gbProduction.Controls.Add(Me.lblUnitBuildTime)
        Me.gbProduction.Controls.Add(Me.lblTotalBuildTimeLbl)
        Me.gbProduction.Controls.Add(Me.lblUnitBuildTimeLbl)
        Me.gbProduction.Controls.Add(Me.lblBPEfficiency)
        Me.gbProduction.Controls.Add(Me.lblBPEfficiencyLbl)
        Me.gbProduction.Controls.Add(Me.gbProdLocation)
        Me.gbProduction.Enabled = False
        Me.gbProduction.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbProduction.Location = New System.Drawing.Point(218, 154)
        Me.gbProduction.Name = "gbProduction"
        Me.gbProduction.Size = New System.Drawing.Size(554, 224)
        Me.gbProduction.TabIndex = 16
        Me.gbProduction.TabStop = False
        Me.gbProduction.Text = "Production"
        '
        'lblUnitProfit
        '
        Me.lblUnitProfit.AutoSize = True
        Me.lblUnitProfit.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitProfit.Location = New System.Drawing.Point(369, 195)
        Me.lblUnitProfit.Name = "lblUnitProfit"
        Me.lblUnitProfit.Size = New System.Drawing.Size(28, 13)
        Me.lblUnitProfit.TabIndex = 173
        Me.lblUnitProfit.Text = "0 isk"
        '
        'lblUnitProfitlbl
        '
        Me.lblUnitProfitlbl.AutoSize = True
        Me.lblUnitProfitlbl.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitProfitlbl.Location = New System.Drawing.Point(256, 195)
        Me.lblUnitProfitlbl.Name = "lblUnitProfitlbl"
        Me.lblUnitProfitlbl.Size = New System.Drawing.Size(92, 13)
        Me.lblUnitProfitlbl.TabIndex = 172
        Me.lblUnitProfitlbl.Text = "Unit Profit/(Loss):"
        '
        'lblUnitValue
        '
        Me.lblUnitValue.AutoSize = True
        Me.lblUnitValue.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitValue.Location = New System.Drawing.Point(369, 177)
        Me.lblUnitValue.Name = "lblUnitValue"
        Me.lblUnitValue.Size = New System.Drawing.Size(28, 13)
        Me.lblUnitValue.TabIndex = 171
        Me.lblUnitValue.Text = "0 isk"
        '
        'lblUnitValuelbl
        '
        Me.lblUnitValuelbl.AutoSize = True
        Me.lblUnitValuelbl.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitValuelbl.Location = New System.Drawing.Point(256, 177)
        Me.lblUnitValuelbl.Name = "lblUnitValuelbl"
        Me.lblUnitValuelbl.Size = New System.Drawing.Size(59, 13)
        Me.lblUnitValuelbl.TabIndex = 170
        Me.lblUnitValuelbl.Text = "Unit Value:"
        '
        'lblUnitCost
        '
        Me.lblUnitCost.AutoSize = True
        Me.lblUnitCost.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitCost.Location = New System.Drawing.Point(369, 164)
        Me.lblUnitCost.Name = "lblUnitCost"
        Me.lblUnitCost.Size = New System.Drawing.Size(28, 13)
        Me.lblUnitCost.TabIndex = 169
        Me.lblUnitCost.Text = "0 isk"
        '
        'lblUnitCostLbl
        '
        Me.lblUnitCostLbl.AutoSize = True
        Me.lblUnitCostLbl.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitCostLbl.Location = New System.Drawing.Point(256, 164)
        Me.lblUnitCostLbl.Name = "lblUnitCostLbl"
        Me.lblUnitCostLbl.Size = New System.Drawing.Size(55, 13)
        Me.lblUnitCostLbl.TabIndex = 168
        Me.lblUnitCostLbl.Text = "Unit Cost:"
        '
        'lblTotalWasteCost
        '
        Me.lblTotalWasteCost.AutoSize = True
        Me.lblTotalWasteCost.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalWasteCost.Location = New System.Drawing.Point(369, 64)
        Me.lblTotalWasteCost.Name = "lblTotalWasteCost"
        Me.lblTotalWasteCost.Size = New System.Drawing.Size(28, 13)
        Me.lblTotalWasteCost.TabIndex = 167
        Me.lblTotalWasteCost.Text = "0 isk"
        '
        'lblTotalWasteCostLbl
        '
        Me.lblTotalWasteCostLbl.AutoSize = True
        Me.lblTotalWasteCostLbl.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalWasteCostLbl.Location = New System.Drawing.Point(256, 64)
        Me.lblTotalWasteCostLbl.Name = "lblTotalWasteCostLbl"
        Me.lblTotalWasteCostLbl.Size = New System.Drawing.Size(94, 13)
        Me.lblTotalWasteCostLbl.TabIndex = 166
        Me.lblTotalWasteCostLbl.Text = "Total Waste Cost:"
        '
        'lblUnitWasteCost
        '
        Me.lblUnitWasteCost.AutoSize = True
        Me.lblUnitWasteCost.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitWasteCost.Location = New System.Drawing.Point(369, 51)
        Me.lblUnitWasteCost.Name = "lblUnitWasteCost"
        Me.lblUnitWasteCost.Size = New System.Drawing.Size(28, 13)
        Me.lblUnitWasteCost.TabIndex = 165
        Me.lblUnitWasteCost.Text = "0 isk"
        '
        'lblUnitWasteCostLbl
        '
        Me.lblUnitWasteCostLbl.AutoSize = True
        Me.lblUnitWasteCostLbl.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitWasteCostLbl.Location = New System.Drawing.Point(256, 51)
        Me.lblUnitWasteCostLbl.Name = "lblUnitWasteCostLbl"
        Me.lblUnitWasteCostLbl.Size = New System.Drawing.Size(97, 13)
        Me.lblUnitWasteCostLbl.TabIndex = 164
        Me.lblUnitWasteCostLbl.Text = "Batch Waste Cost:"
        '
        'lblTotalCosts
        '
        Me.lblTotalCosts.AutoSize = True
        Me.lblTotalCosts.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalCosts.Location = New System.Drawing.Point(369, 145)
        Me.lblTotalCosts.Name = "lblTotalCosts"
        Me.lblTotalCosts.Size = New System.Drawing.Size(28, 13)
        Me.lblTotalCosts.TabIndex = 163
        Me.lblTotalCosts.Text = "0 isk"
        '
        'lblTotalCostsLbl
        '
        Me.lblTotalCostsLbl.AutoSize = True
        Me.lblTotalCostsLbl.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalCostsLbl.Location = New System.Drawing.Point(256, 141)
        Me.lblTotalCostsLbl.Name = "lblTotalCostsLbl"
        Me.lblTotalCostsLbl.Size = New System.Drawing.Size(65, 13)
        Me.lblTotalCostsLbl.TabIndex = 162
        Me.lblTotalCostsLbl.Text = "Total Costs:"
        '
        'lblFactoryCosts
        '
        Me.lblFactoryCosts.AutoSize = True
        Me.lblFactoryCosts.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFactoryCosts.Location = New System.Drawing.Point(369, 130)
        Me.lblFactoryCosts.Name = "lblFactoryCosts"
        Me.lblFactoryCosts.Size = New System.Drawing.Size(28, 13)
        Me.lblFactoryCosts.TabIndex = 161
        Me.lblFactoryCosts.Text = "0 isk"
        '
        'lblFactoryCostsLbl
        '
        Me.lblFactoryCostsLbl.AutoSize = True
        Me.lblFactoryCostsLbl.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFactoryCostsLbl.Location = New System.Drawing.Point(256, 128)
        Me.lblFactoryCostsLbl.Name = "lblFactoryCostsLbl"
        Me.lblFactoryCostsLbl.Size = New System.Drawing.Size(78, 13)
        Me.lblFactoryCostsLbl.TabIndex = 160
        Me.lblFactoryCostsLbl.Text = "Factory Costs:"
        '
        'lblTotalBuildCost
        '
        Me.lblTotalBuildCost.AutoSize = True
        Me.lblTotalBuildCost.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalBuildCost.Location = New System.Drawing.Point(369, 115)
        Me.lblTotalBuildCost.Name = "lblTotalBuildCost"
        Me.lblTotalBuildCost.Size = New System.Drawing.Size(28, 13)
        Me.lblTotalBuildCost.TabIndex = 159
        Me.lblTotalBuildCost.Text = "0 isk"
        '
        'lblUnitBuildCost
        '
        Me.lblUnitBuildCost.AutoSize = True
        Me.lblUnitBuildCost.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitBuildCost.Location = New System.Drawing.Point(369, 103)
        Me.lblUnitBuildCost.Name = "lblUnitBuildCost"
        Me.lblUnitBuildCost.Size = New System.Drawing.Size(28, 13)
        Me.lblUnitBuildCost.TabIndex = 158
        Me.lblUnitBuildCost.Text = "0 isk"
        '
        'lblTotalBuildCostsLbl
        '
        Me.lblTotalBuildCostsLbl.AutoSize = True
        Me.lblTotalBuildCostsLbl.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalBuildCostsLbl.Location = New System.Drawing.Point(256, 115)
        Me.lblTotalBuildCostsLbl.Name = "lblTotalBuildCostsLbl"
        Me.lblTotalBuildCostsLbl.Size = New System.Drawing.Size(101, 13)
        Me.lblTotalBuildCostsLbl.TabIndex = 157
        Me.lblTotalBuildCostsLbl.Text = "Total Material Cost:"
        '
        'lblUnitBuildCostsLbl
        '
        Me.lblUnitBuildCostsLbl.AutoSize = True
        Me.lblUnitBuildCostsLbl.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitBuildCostsLbl.Location = New System.Drawing.Point(256, 102)
        Me.lblUnitBuildCostsLbl.Name = "lblUnitBuildCostsLbl"
        Me.lblUnitBuildCostsLbl.Size = New System.Drawing.Size(104, 13)
        Me.lblUnitBuildCostsLbl.TabIndex = 156
        Me.lblUnitBuildCostsLbl.Text = "Batch Material Cost:"
        '
        'lblTotalBuildTime
        '
        Me.lblTotalBuildTime.AutoSize = True
        Me.lblTotalBuildTime.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalBuildTime.Location = New System.Drawing.Point(369, 29)
        Me.lblTotalBuildTime.Name = "lblTotalBuildTime"
        Me.lblTotalBuildTime.Size = New System.Drawing.Size(18, 13)
        Me.lblTotalBuildTime.TabIndex = 155
        Me.lblTotalBuildTime.Text = "0s"
        '
        'lblUnitBuildTime
        '
        Me.lblUnitBuildTime.AutoSize = True
        Me.lblUnitBuildTime.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitBuildTime.Location = New System.Drawing.Point(369, 16)
        Me.lblUnitBuildTime.Name = "lblUnitBuildTime"
        Me.lblUnitBuildTime.Size = New System.Drawing.Size(18, 13)
        Me.lblUnitBuildTime.TabIndex = 154
        Me.lblUnitBuildTime.Text = "0s"
        '
        'lblTotalBuildTimeLbl
        '
        Me.lblTotalBuildTimeLbl.AutoSize = True
        Me.lblTotalBuildTimeLbl.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotalBuildTimeLbl.Location = New System.Drawing.Point(256, 29)
        Me.lblTotalBuildTimeLbl.Name = "lblTotalBuildTimeLbl"
        Me.lblTotalBuildTimeLbl.Size = New System.Drawing.Size(85, 13)
        Me.lblTotalBuildTimeLbl.TabIndex = 23
        Me.lblTotalBuildTimeLbl.Text = "Total Build Time:"
        '
        'lblUnitBuildTimeLbl
        '
        Me.lblUnitBuildTimeLbl.AutoSize = True
        Me.lblUnitBuildTimeLbl.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUnitBuildTimeLbl.Location = New System.Drawing.Point(256, 16)
        Me.lblUnitBuildTimeLbl.Name = "lblUnitBuildTimeLbl"
        Me.lblUnitBuildTimeLbl.Size = New System.Drawing.Size(88, 13)
        Me.lblUnitBuildTimeLbl.TabIndex = 22
        Me.lblUnitBuildTimeLbl.Text = "Batch Build Time:"
        '
        'lblBPEfficiency
        '
        Me.lblBPEfficiency.AutoSize = True
        Me.lblBPEfficiency.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBPEfficiency.Location = New System.Drawing.Point(369, 78)
        Me.lblBPEfficiency.Name = "lblBPEfficiency"
        Me.lblBPEfficiency.Size = New System.Drawing.Size(27, 13)
        Me.lblBPEfficiency.TabIndex = 150
        Me.lblBPEfficiency.Text = "0 %"
        '
        'lblBPEfficiencyLbl
        '
        Me.lblBPEfficiencyLbl.AutoSize = True
        Me.lblBPEfficiencyLbl.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBPEfficiencyLbl.Location = New System.Drawing.Point(256, 77)
        Me.lblBPEfficiencyLbl.Name = "lblBPEfficiencyLbl"
        Me.lblBPEfficiencyLbl.Size = New System.Drawing.Size(57, 13)
        Me.lblBPEfficiencyLbl.TabIndex = 24
        Me.lblBPEfficiencyLbl.Text = "Efficiency:"
        '
        'btnCopyToClipboard
        '
        Me.btnCopyToClipboard.Location = New System.Drawing.Point(671, 6)
        Me.btnCopyToClipboard.Name = "btnCopyToClipboard"
        Me.btnCopyToClipboard.Size = New System.Drawing.Size(125, 23)
        Me.btnCopyToClipboard.TabIndex = 3
        Me.btnCopyToClipboard.Text = "Copy To Clipboard"
        Me.btnCopyToClipboard.UseVisualStyleBackColor = True
        '
        'gbSkills
        '
        Me.gbSkills.Controls.Add(Me.chkOverrideSkills)
        Me.gbSkills.Controls.Add(Me.lblPilot)
        Me.gbSkills.Controls.Add(Me.cboPilot)
        Me.gbSkills.Controls.Add(Me.gbProdSkills)
        Me.gbSkills.Controls.Add(Me.gbResearchSkills)
        Me.gbSkills.Enabled = False
        Me.gbSkills.Location = New System.Drawing.Point(438, 12)
        Me.gbSkills.Name = "gbSkills"
        Me.gbSkills.Size = New System.Drawing.Size(334, 135)
        Me.gbSkills.TabIndex = 153
        Me.gbSkills.TabStop = False
        Me.gbSkills.Text = "Pilot && Skill Selection"
        '
        'chkOverrideSkills
        '
        Me.chkOverrideSkills.AutoSize = True
        Me.chkOverrideSkills.Location = New System.Drawing.Point(216, 19)
        Me.chkOverrideSkills.Name = "chkOverrideSkills"
        Me.chkOverrideSkills.Size = New System.Drawing.Size(93, 17)
        Me.chkOverrideSkills.TabIndex = 6
        Me.chkOverrideSkills.Text = "Override Skills"
        Me.chkOverrideSkills.UseVisualStyleBackColor = True
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        Me.lblPilot.Location = New System.Drawing.Point(6, 21)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(31, 13)
        Me.lblPilot.TabIndex = 5
        Me.lblPilot.Text = "Pilot:"
        '
        'cboPilot
        '
        Me.cboPilot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboPilot.FormattingEnabled = True
        Me.cboPilot.ItemHeight = 13
        Me.cboPilot.Location = New System.Drawing.Point(42, 17)
        Me.cboPilot.MaxDropDownItems = 20
        Me.cboPilot.Name = "cboPilot"
        Me.cboPilot.Size = New System.Drawing.Size(168, 21)
        Me.cboPilot.TabIndex = 2
        '
        'gbProdSkills
        '
        Me.gbProdSkills.Controls.Add(Me.cboIndustyImplant)
        Me.gbProdSkills.Controls.Add(Me.cboProdEffSkill)
        Me.gbProdSkills.Controls.Add(Me.cboIndustrySkill)
        Me.gbProdSkills.Controls.Add(Me.lblPESkill)
        Me.gbProdSkills.Controls.Add(Me.lblIndustrySkill)
        Me.gbProdSkills.Location = New System.Drawing.Point(172, 44)
        Me.gbProdSkills.Name = "gbProdSkills"
        Me.gbProdSkills.Size = New System.Drawing.Size(156, 84)
        Me.gbProdSkills.TabIndex = 1
        Me.gbProdSkills.TabStop = False
        Me.gbProdSkills.Text = "Production skills  /  implants"
        '
        'cboIndustyImplant
        '
        Me.cboIndustyImplant.FormattingEnabled = True
        Me.cboIndustyImplant.Items.AddRange(New Object() {"0%", "1%", "2%", "4%"})
        Me.cboIndustyImplant.Location = New System.Drawing.Point(105, 15)
        Me.cboIndustyImplant.Name = "cboIndustyImplant"
        Me.cboIndustyImplant.Size = New System.Drawing.Size(44, 21)
        Me.cboIndustyImplant.TabIndex = 8
        Me.cboIndustyImplant.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.cboIndustyImplant, "'Beancounter' F Series: Reduces manufacturing time.")
        '
        'cboProdEffSkill
        '
        Me.cboProdEffSkill.Enabled = False
        Me.cboProdEffSkill.FormattingEnabled = True
        Me.cboProdEffSkill.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5"})
        Me.cboProdEffSkill.Location = New System.Drawing.Point(64, 36)
        Me.cboProdEffSkill.Name = "cboProdEffSkill"
        Me.cboProdEffSkill.Size = New System.Drawing.Size(35, 21)
        Me.cboProdEffSkill.TabIndex = 7
        Me.cboProdEffSkill.Text = "0"
        Me.ToolTip1.SetToolTip(Me.cboProdEffSkill, "Reduces manufacturing waste material.")
        '
        'cboIndustrySkill
        '
        Me.cboIndustrySkill.Enabled = False
        Me.cboIndustrySkill.FormattingEnabled = True
        Me.cboIndustrySkill.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5"})
        Me.cboIndustrySkill.Location = New System.Drawing.Point(64, 15)
        Me.cboIndustrySkill.Name = "cboIndustrySkill"
        Me.cboIndustrySkill.Size = New System.Drawing.Size(35, 21)
        Me.cboIndustrySkill.TabIndex = 6
        Me.cboIndustrySkill.Text = "0"
        Me.ToolTip1.SetToolTip(Me.cboIndustrySkill, "Reduces manufacturing time.")
        '
        'lblPESkill
        '
        Me.lblPESkill.AutoSize = True
        Me.lblPESkill.Location = New System.Drawing.Point(7, 39)
        Me.lblPESkill.Name = "lblPESkill"
        Me.lblPESkill.Size = New System.Drawing.Size(54, 13)
        Me.lblPESkill.TabIndex = 5
        Me.lblPESkill.Text = "Prod. Eff:"
        Me.ToolTip1.SetToolTip(Me.lblPESkill, "Reduces manufacturing waste material")
        '
        'lblIndustrySkill
        '
        Me.lblIndustrySkill.AutoSize = True
        Me.lblIndustrySkill.Location = New System.Drawing.Point(7, 18)
        Me.lblIndustrySkill.Name = "lblIndustrySkill"
        Me.lblIndustrySkill.Size = New System.Drawing.Size(52, 13)
        Me.lblIndustrySkill.TabIndex = 4
        Me.lblIndustrySkill.Text = "Industry:"
        Me.ToolTip1.SetToolTip(Me.lblIndustrySkill, "Reduces manufacturing time.")
        '
        'gbResearchSkills
        '
        Me.gbResearchSkills.Controls.Add(Me.cboScienceImplant)
        Me.gbResearchSkills.Controls.Add(Me.cboMetallurgyImplant)
        Me.gbResearchSkills.Controls.Add(Me.cboResearchImplant)
        Me.gbResearchSkills.Controls.Add(Me.lblScienceSkill)
        Me.gbResearchSkills.Controls.Add(Me.cboScienceSkill)
        Me.gbResearchSkills.Controls.Add(Me.cboMetallurgySkill)
        Me.gbResearchSkills.Controls.Add(Me.cboResearchSkill)
        Me.gbResearchSkills.Controls.Add(Me.lblMetallurgySkill)
        Me.gbResearchSkills.Controls.Add(Me.lblResearchSkill)
        Me.gbResearchSkills.Location = New System.Drawing.Point(6, 44)
        Me.gbResearchSkills.Name = "gbResearchSkills"
        Me.gbResearchSkills.Size = New System.Drawing.Size(160, 84)
        Me.gbResearchSkills.TabIndex = 0
        Me.gbResearchSkills.TabStop = False
        Me.gbResearchSkills.Text = "Research skills  /  implants"
        '
        'cboScienceImplant
        '
        Me.cboScienceImplant.FormattingEnabled = True
        Me.cboScienceImplant.Items.AddRange(New Object() {"0%", "1%", "3%", "5%"})
        Me.cboScienceImplant.Location = New System.Drawing.Point(110, 57)
        Me.cboScienceImplant.Name = "cboScienceImplant"
        Me.cboScienceImplant.Size = New System.Drawing.Size(44, 21)
        Me.cboScienceImplant.TabIndex = 8
        Me.cboScienceImplant.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.cboScienceImplant, "'Beancounter' K Series: Reduces the time taken to make a blueprint copy.")
        '
        'cboMetallurgyImplant
        '
        Me.cboMetallurgyImplant.FormattingEnabled = True
        Me.cboMetallurgyImplant.Items.AddRange(New Object() {"0%", "1%", "3%", "5%"})
        Me.cboMetallurgyImplant.Location = New System.Drawing.Point(110, 36)
        Me.cboMetallurgyImplant.Name = "cboMetallurgyImplant"
        Me.cboMetallurgyImplant.Size = New System.Drawing.Size(44, 21)
        Me.cboMetallurgyImplant.TabIndex = 7
        Me.cboMetallurgyImplant.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.cboMetallurgyImplant, "'Beancounter' J Series: Reduces the time taken to research the ME Level.")
        '
        'cboResearchImplant
        '
        Me.cboResearchImplant.FormattingEnabled = True
        Me.cboResearchImplant.Items.AddRange(New Object() {"0%", "1%", "3%", "5%"})
        Me.cboResearchImplant.Location = New System.Drawing.Point(110, 15)
        Me.cboResearchImplant.Name = "cboResearchImplant"
        Me.cboResearchImplant.Size = New System.Drawing.Size(44, 21)
        Me.cboResearchImplant.TabIndex = 6
        Me.cboResearchImplant.Text = "0%"
        Me.ToolTip1.SetToolTip(Me.cboResearchImplant, "'Beancounter' I Series: Reduces the time taken to research the PE Level.")
        '
        'lblScienceSkill
        '
        Me.lblScienceSkill.AutoSize = True
        Me.lblScienceSkill.Location = New System.Drawing.Point(7, 60)
        Me.lblScienceSkill.Name = "lblScienceSkill"
        Me.lblScienceSkill.Size = New System.Drawing.Size(47, 13)
        Me.lblScienceSkill.TabIndex = 5
        Me.lblScienceSkill.Text = "Science:"
        Me.ToolTip1.SetToolTip(Me.lblScienceSkill, "Reduces the time taken to make a blueprint copy.")
        '
        'cboScienceSkill
        '
        Me.cboScienceSkill.Enabled = False
        Me.cboScienceSkill.FormattingEnabled = True
        Me.cboScienceSkill.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5"})
        Me.cboScienceSkill.Location = New System.Drawing.Point(69, 57)
        Me.cboScienceSkill.Name = "cboScienceSkill"
        Me.cboScienceSkill.Size = New System.Drawing.Size(35, 21)
        Me.cboScienceSkill.TabIndex = 4
        Me.cboScienceSkill.Text = "0"
        Me.ToolTip1.SetToolTip(Me.cboScienceSkill, "Reduces the time taken to make a blueprint copy.")
        '
        'cboMetallurgySkill
        '
        Me.cboMetallurgySkill.Enabled = False
        Me.cboMetallurgySkill.FormattingEnabled = True
        Me.cboMetallurgySkill.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5"})
        Me.cboMetallurgySkill.Location = New System.Drawing.Point(69, 36)
        Me.cboMetallurgySkill.Name = "cboMetallurgySkill"
        Me.cboMetallurgySkill.Size = New System.Drawing.Size(35, 21)
        Me.cboMetallurgySkill.TabIndex = 3
        Me.cboMetallurgySkill.Text = "0"
        Me.ToolTip1.SetToolTip(Me.cboMetallurgySkill, "Reduces the time taken to research the ME Level.")
        '
        'cboResearchSkill
        '
        Me.cboResearchSkill.Enabled = False
        Me.cboResearchSkill.FormattingEnabled = True
        Me.cboResearchSkill.Items.AddRange(New Object() {"0", "1", "2", "3", "4", "5"})
        Me.cboResearchSkill.Location = New System.Drawing.Point(69, 15)
        Me.cboResearchSkill.Name = "cboResearchSkill"
        Me.cboResearchSkill.Size = New System.Drawing.Size(35, 21)
        Me.cboResearchSkill.TabIndex = 2
        Me.cboResearchSkill.Text = "0"
        Me.ToolTip1.SetToolTip(Me.cboResearchSkill, "Reduces the time taken to research the PE Level.")
        '
        'lblMetallurgySkill
        '
        Me.lblMetallurgySkill.AutoSize = True
        Me.lblMetallurgySkill.Location = New System.Drawing.Point(7, 39)
        Me.lblMetallurgySkill.Name = "lblMetallurgySkill"
        Me.lblMetallurgySkill.Size = New System.Drawing.Size(61, 13)
        Me.lblMetallurgySkill.TabIndex = 1
        Me.lblMetallurgySkill.Text = "Metallurgy:"
        Me.ToolTip1.SetToolTip(Me.lblMetallurgySkill, "Reduces the time taken to research the ME Level.")
        '
        'lblResearchSkill
        '
        Me.lblResearchSkill.AutoSize = True
        Me.lblResearchSkill.Location = New System.Drawing.Point(7, 18)
        Me.lblResearchSkill.Name = "lblResearchSkill"
        Me.lblResearchSkill.Size = New System.Drawing.Size(56, 13)
        Me.lblResearchSkill.TabIndex = 0
        Me.lblResearchSkill.Text = "Research:"
        Me.ToolTip1.SetToolTip(Me.lblResearchSkill, "Reduces the time taken to research the PE Level.")
        '
        'tabBPResources
        '
        Me.tabBPResources.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tabBPResources.Controls.Add(Me.tabBPResourcesRequired)
        Me.tabBPResources.Controls.Add(Me.tabBPResourcesOwned)
        Me.tabBPResources.Enabled = False
        Me.tabBPResources.Location = New System.Drawing.Point(12, 384)
        Me.tabBPResources.Name = "tabBPResources"
        Me.tabBPResources.SelectedIndex = 0
        Me.tabBPResources.Size = New System.Drawing.Size(810, 291)
        Me.tabBPResources.TabIndex = 154
        '
        'tabBPResourcesRequired
        '
        Me.tabBPResourcesRequired.Controls.Add(Me.chkUseStandardBPCosting)
        Me.tabBPResourcesRequired.Controls.Add(Me.chkShowSkills)
        Me.tabBPResourcesRequired.Controls.Add(Me.clvResources)
        Me.tabBPResourcesRequired.Location = New System.Drawing.Point(4, 22)
        Me.tabBPResourcesRequired.Name = "tabBPResourcesRequired"
        Me.tabBPResourcesRequired.Padding = New System.Windows.Forms.Padding(3)
        Me.tabBPResourcesRequired.Size = New System.Drawing.Size(802, 265)
        Me.tabBPResourcesRequired.TabIndex = 0
        Me.tabBPResourcesRequired.Text = "Production Resources"
        Me.tabBPResourcesRequired.UseVisualStyleBackColor = True
        '
        'chkUseStandardBPCosting
        '
        Me.chkUseStandardBPCosting.AutoSize = True
        Me.chkUseStandardBPCosting.Location = New System.Drawing.Point(135, 10)
        Me.chkUseStandardBPCosting.Name = "chkUseStandardBPCosting"
        Me.chkUseStandardBPCosting.Size = New System.Drawing.Size(130, 17)
        Me.chkUseStandardBPCosting.TabIndex = 6
        Me.chkUseStandardBPCosting.Tag = ""
        Me.chkUseStandardBPCosting.Text = "Use Standard Costing"
        Me.chkUseStandardBPCosting.UseVisualStyleBackColor = True
        '
        'chkShowSkills
        '
        Me.chkShowSkills.AutoSize = True
        Me.chkShowSkills.Location = New System.Drawing.Point(6, 10)
        Me.chkShowSkills.Name = "chkShowSkills"
        Me.chkShowSkills.Size = New System.Drawing.Size(123, 17)
        Me.chkShowSkills.TabIndex = 5
        Me.chkShowSkills.Tag = ""
        Me.chkShowSkills.Text = "Show Required Skills"
        Me.chkShowSkills.UseVisualStyleBackColor = True
        '
        'clvResources
        '
        Me.clvResources.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clvResources.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colBPResMaterials, Me.colBPResPerfect, Me.colBPResWaste, Me.colBPResTotal, Me.colBPResPrice, Me.colBPResValue, Me.colBPResIdealML})
        Me.clvResources.DefaultItemHeight = 20
        Me.clvResources.Location = New System.Drawing.Point(3, 33)
        Me.clvResources.Name = "clvResources"
        Me.clvResources.ShowPlusMinus = True
        Me.clvResources.ShowRootTreeLines = True
        Me.clvResources.ShowTreeLines = True
        Me.clvResources.Size = New System.Drawing.Size(796, 229)
        Me.clvResources.TabIndex = 0
        '
        'colBPResMaterials
        '
        Me.colBPResMaterials.CustomSortTag = Nothing
        Me.colBPResMaterials.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colBPResMaterials.Tag = Nothing
        Me.colBPResMaterials.Text = "Required Material"
        Me.colBPResMaterials.Width = 225
        '
        'colBPResPerfect
        '
        Me.colBPResPerfect.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colBPResPerfect.CustomSortTag = Nothing
        Me.colBPResPerfect.DisplayIndex = 1
        Me.colBPResPerfect.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colBPResPerfect.Tag = Nothing
        Me.colBPResPerfect.Text = "Perfect Units"
        '
        'colBPResWaste
        '
        Me.colBPResWaste.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colBPResWaste.CustomSortTag = Nothing
        Me.colBPResWaste.DisplayIndex = 2
        Me.colBPResWaste.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colBPResWaste.Tag = Nothing
        Me.colBPResWaste.Text = "Waste Units"
        '
        'colBPResTotal
        '
        Me.colBPResTotal.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colBPResTotal.CustomSortTag = Nothing
        Me.colBPResTotal.DisplayIndex = 3
        Me.colBPResTotal.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colBPResTotal.Tag = Nothing
        Me.colBPResTotal.Text = "Total Units"
        '
        'colBPResPrice
        '
        Me.colBPResPrice.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colBPResPrice.CustomSortTag = Nothing
        Me.colBPResPrice.DisplayIndex = 4
        Me.colBPResPrice.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colBPResPrice.Tag = Nothing
        Me.colBPResPrice.Text = "Unit Price"
        '
        'colBPResValue
        '
        Me.colBPResValue.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colBPResValue.CustomSortTag = Nothing
        Me.colBPResValue.DisplayIndex = 5
        Me.colBPResValue.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colBPResValue.Tag = Nothing
        Me.colBPResValue.Text = "Total Price"
        '
        'colBPResIdealML
        '
        Me.colBPResIdealML.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colBPResIdealML.CustomSortTag = Nothing
        Me.colBPResIdealML.DisplayIndex = 6
        Me.colBPResIdealML.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colBPResIdealML.Tag = Nothing
        Me.colBPResIdealML.Text = "Ideal ML"
        Me.colBPResIdealML.Width = 75
        '
        'tabBPResourcesOwned
        '
        Me.tabBPResourcesOwned.Controls.Add(Me.cboAssetSelection)
        Me.tabBPResourcesOwned.Controls.Add(Me.lblAssetSelection)
        Me.tabBPResourcesOwned.Controls.Add(Me.clvOwnedResources)
        Me.tabBPResourcesOwned.Controls.Add(Me.lblMaxUnits)
        Me.tabBPResourcesOwned.Controls.Add(Me.btnCopyToClipboard)
        Me.tabBPResourcesOwned.Location = New System.Drawing.Point(4, 22)
        Me.tabBPResourcesOwned.Name = "tabBPResourcesOwned"
        Me.tabBPResourcesOwned.Padding = New System.Windows.Forms.Padding(3)
        Me.tabBPResourcesOwned.Size = New System.Drawing.Size(802, 265)
        Me.tabBPResourcesOwned.TabIndex = 1
        Me.tabBPResourcesOwned.Text = "Resources Owned"
        Me.tabBPResourcesOwned.UseVisualStyleBackColor = True
        '
        'cboAssetSelection
        '
        Me.cboAssetSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboAssetSelection.FormattingEnabled = True
        Me.cboAssetSelection.Items.AddRange(New Object() {"Owner Only", "Owner + Corp", "Corp + Members"})
        Me.cboAssetSelection.Location = New System.Drawing.Point(96, 8)
        Me.cboAssetSelection.Name = "cboAssetSelection"
        Me.cboAssetSelection.Size = New System.Drawing.Size(144, 21)
        Me.cboAssetSelection.TabIndex = 8
        '
        'lblAssetSelection
        '
        Me.lblAssetSelection.AutoSize = True
        Me.lblAssetSelection.Location = New System.Drawing.Point(6, 11)
        Me.lblAssetSelection.Name = "lblAssetSelection"
        Me.lblAssetSelection.Size = New System.Drawing.Size(84, 13)
        Me.lblAssetSelection.TabIndex = 7
        Me.lblAssetSelection.Text = "Asset Selection:"
        '
        'clvOwnedResources
        '
        Me.clvOwnedResources.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clvOwnedResources.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colBPOResMaterial, Me.colBPOResNeeded, Me.colBPOResOwned, Me.colBPOResSurplus})
        Me.clvOwnedResources.DefaultItemHeight = 20
        Me.clvOwnedResources.Location = New System.Drawing.Point(3, 33)
        Me.clvOwnedResources.Name = "clvOwnedResources"
        Me.clvOwnedResources.Size = New System.Drawing.Size(793, 231)
        Me.clvOwnedResources.TabIndex = 6
        '
        'colBPOResMaterial
        '
        Me.colBPOResMaterial.CustomSortTag = Nothing
        Me.colBPOResMaterial.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colBPOResMaterial.Tag = Nothing
        Me.colBPOResMaterial.Text = "Required Material"
        Me.colBPOResMaterial.Width = 200
        '
        'colBPOResNeeded
        '
        Me.colBPOResNeeded.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colBPOResNeeded.CustomSortTag = Nothing
        Me.colBPOResNeeded.DisplayIndex = 1
        Me.colBPOResNeeded.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colBPOResNeeded.Tag = Nothing
        Me.colBPOResNeeded.Text = "Quantity Required"
        Me.colBPOResNeeded.Width = 150
        '
        'colBPOResOwned
        '
        Me.colBPOResOwned.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colBPOResOwned.CustomSortTag = Nothing
        Me.colBPOResOwned.DisplayIndex = 2
        Me.colBPOResOwned.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colBPOResOwned.Tag = Nothing
        Me.colBPOResOwned.Text = "Quantity Owned"
        Me.colBPOResOwned.Width = 150
        '
        'colBPOResSurplus
        '
        Me.colBPOResSurplus.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colBPOResSurplus.CustomSortTag = Nothing
        Me.colBPOResSurplus.DisplayIndex = 3
        Me.colBPOResSurplus.SortDataType = DotNetLib.Windows.Forms.SortDataType.Tag
        Me.colBPOResSurplus.Tag = Nothing
        Me.colBPOResSurplus.Text = "Surplus"
        Me.colBPOResSurplus.Width = 150
        '
        'lblMaxUnits
        '
        Me.lblMaxUnits.AutoSize = True
        Me.lblMaxUnits.Location = New System.Drawing.Point(305, 11)
        Me.lblMaxUnits.Name = "lblMaxUnits"
        Me.lblMaxUnits.Size = New System.Drawing.Size(138, 13)
        Me.lblMaxUnits.TabIndex = 5
        Me.lblMaxUnits.Text = "Maximum Producable Units:"
        '
        'frmBPCalculator
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.ClientSize = New System.Drawing.Size(834, 679)
        Me.Controls.Add(Me.tabBPResources)
        Me.Controls.Add(Me.gbAddResearch)
        Me.Controls.Add(Me.gbSkills)
        Me.Controls.Add(Me.gbProduction)
        Me.Controls.Add(Me.gbBPOSelection)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimumSize = New System.Drawing.Size(850, 715)
        Me.Name = "frmBPCalculator"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blueprint Calculator"
        Me.TransparencyKey = System.Drawing.Color.Silver
        Me.gbBPOSelection.ResumeLayout(False)
        Me.gbBPOSelection.PerformLayout()
        CType(Me.pbBP, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbProdLocation.ResumeLayout(False)
        Me.gbProdLocation.PerformLayout()
        CType(Me.nudRunningCost, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudInstallCost, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudRuns, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbAddResearch.ResumeLayout(False)
        Me.gbAddResearch.PerformLayout()
        CType(Me.nudCopyRuns, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPELevel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudMELevel, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbProduction.ResumeLayout(False)
        Me.gbProduction.PerformLayout()
        Me.gbSkills.ResumeLayout(False)
        Me.gbSkills.PerformLayout()
        Me.gbProdSkills.ResumeLayout(False)
        Me.gbProdSkills.PerformLayout()
        Me.gbResearchSkills.ResumeLayout(False)
        Me.gbResearchSkills.PerformLayout()
        Me.tabBPResources.ResumeLayout(False)
        Me.tabBPResourcesRequired.ResumeLayout(False)
        Me.tabBPResourcesRequired.PerformLayout()
        Me.tabBPResourcesOwned.ResumeLayout(False)
        Me.tabBPResourcesOwned.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cboBPs As System.Windows.Forms.ComboBox
    Friend WithEvents chkOwnedBPOs As System.Windows.Forms.CheckBox
    Friend WithEvents gbBPOSelection As System.Windows.Forms.GroupBox
    Friend WithEvents lblBPMELbl As System.Windows.Forms.Label
    Friend WithEvents lblBPPELbl As System.Windows.Forms.Label
    Friend WithEvents lblBPWFLbl As System.Windows.Forms.Label
    Friend WithEvents pbBP As System.Windows.Forms.PictureBox
    Friend WithEvents gbProdLocation As System.Windows.Forms.GroupBox
    Friend WithEvents chkPOSProduction As System.Windows.Forms.CheckBox
    Friend WithEvents cboPOSArrays As System.Windows.Forms.ComboBox
    Friend WithEvents gbAddResearch As System.Windows.Forms.GroupBox
    Friend WithEvents chkResearchAtPOS As System.Windows.Forms.CheckBox
    Friend WithEvents lblPETimeLbl As System.Windows.Forms.Label
    Friend WithEvents lblMETimeLbl As System.Windows.Forms.Label
    Friend WithEvents lblNewMELbl As System.Windows.Forms.Label
    Friend WithEvents LblNewWFLbl As System.Windows.Forms.Label
    Friend WithEvents lblNewPELbl As System.Windows.Forms.Label
    Friend WithEvents txtNewWasteFactor As System.Windows.Forms.TextBox
    Friend WithEvents gbProduction As System.Windows.Forms.GroupBox
    Friend WithEvents txtProdQuantity As System.Windows.Forms.TextBox
    Friend WithEvents lblProdQuantity As System.Windows.Forms.Label
    Friend WithEvents lblBatchSizeLbl As System.Windows.Forms.Label
    Friend WithEvents lblRuns As System.Windows.Forms.Label
    Friend WithEvents lblUnitBuildTimeLbl As System.Windows.Forms.Label
    Friend WithEvents lblTotalBuildTimeLbl As System.Windows.Forms.Label
    Friend WithEvents lblBPEfficiencyLbl As System.Windows.Forms.Label
    Friend WithEvents lblBPOMarketValueLbl As System.Windows.Forms.Label
    Friend WithEvents lblBPEfficiency As System.Windows.Forms.Label
    Friend WithEvents lblBPOMarketValue As System.Windows.Forms.Label
    Friend WithEvents gbSkills As System.Windows.Forms.GroupBox
    Friend WithEvents gbResearchSkills As System.Windows.Forms.GroupBox
    Friend WithEvents gbProdSkills As System.Windows.Forms.GroupBox
    Friend WithEvents lblMetallurgySkill As System.Windows.Forms.Label
    Friend WithEvents lblResearchSkill As System.Windows.Forms.Label
    Friend WithEvents cboMetallurgySkill As System.Windows.Forms.ComboBox
    Friend WithEvents cboResearchSkill As System.Windows.Forms.ComboBox
    Friend WithEvents cboProdEffSkill As System.Windows.Forms.ComboBox
    Friend WithEvents cboIndustrySkill As System.Windows.Forms.ComboBox
    Friend WithEvents lblPESkill As System.Windows.Forms.Label
    Friend WithEvents lblIndustrySkill As System.Windows.Forms.Label
    Friend WithEvents btnCopyToClipboard As System.Windows.Forms.Button
    Friend WithEvents lblBPPE As System.Windows.Forms.Label
    Friend WithEvents lblBPWF As System.Windows.Forms.Label
    Friend WithEvents lblBPME As System.Windows.Forms.Label
    Friend WithEvents lblPilot As System.Windows.Forms.Label
    Friend WithEvents cboPilot As System.Windows.Forms.ComboBox
    Friend WithEvents chkOverrideSkills As System.Windows.Forms.CheckBox
    Friend WithEvents tabBPResources As System.Windows.Forms.TabControl
    Friend WithEvents tabBPResourcesRequired As System.Windows.Forms.TabPage
    Friend WithEvents tabBPResourcesOwned As System.Windows.Forms.TabPage
    Friend WithEvents nudPELevel As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudMELevel As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudRuns As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblPETime As System.Windows.Forms.Label
    Friend WithEvents lblMETime As System.Windows.Forms.Label
    Friend WithEvents lblCopyTime As System.Windows.Forms.Label
    Friend WithEvents lblBPCopyTimeLbl As System.Windows.Forms.Label
    Friend WithEvents lblScienceSkill As System.Windows.Forms.Label
    Friend WithEvents cboScienceSkill As System.Windows.Forms.ComboBox
    Friend WithEvents nudCopyRuns As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblRunsPerCopy As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents lblUnitBuildTime As System.Windows.Forms.Label
    Friend WithEvents lblTotalBuildTime As System.Windows.Forms.Label
    Friend WithEvents lblBPRuns As System.Windows.Forms.Label
    Friend WithEvents lblBPRunsLbl As System.Windows.Forms.Label
    Friend WithEvents lblTotalBuildCost As System.Windows.Forms.Label
    Friend WithEvents lblUnitBuildCost As System.Windows.Forms.Label
    Friend WithEvents lblTotalBuildCostsLbl As System.Windows.Forms.Label
    Friend WithEvents lblUnitBuildCostsLbl As System.Windows.Forms.Label
    Friend WithEvents nudRunningCost As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudInstallCost As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblRunningCost As System.Windows.Forms.Label
    Friend WithEvents lblInstallCost As System.Windows.Forms.Label
    Friend WithEvents lblFactoryCosts As System.Windows.Forms.Label
    Friend WithEvents lblFactoryCostsLbl As System.Windows.Forms.Label
    Friend WithEvents lblTotalCosts As System.Windows.Forms.Label
    Friend WithEvents lblTotalCostsLbl As System.Windows.Forms.Label
    Friend WithEvents lblTotalWasteCost As System.Windows.Forms.Label
    Friend WithEvents lblTotalWasteCostLbl As System.Windows.Forms.Label
    Friend WithEvents lblUnitWasteCost As System.Windows.Forms.Label
    Friend WithEvents lblUnitWasteCostLbl As System.Windows.Forms.Label
    Friend WithEvents lblUnitCost As System.Windows.Forms.Label
    Friend WithEvents lblUnitCostLbl As System.Windows.Forms.Label
    Friend WithEvents lblUnitValue As System.Windows.Forms.Label
    Friend WithEvents lblUnitValuelbl As System.Windows.Forms.Label
    Friend WithEvents lblUnitProfit As System.Windows.Forms.Label
    Friend WithEvents lblUnitProfitlbl As System.Windows.Forms.Label
    Friend WithEvents txtBatchSize As System.Windows.Forms.TextBox
    Friend WithEvents cboResearchImplant As System.Windows.Forms.ComboBox
    Friend WithEvents cboIndustyImplant As System.Windows.Forms.ComboBox
    Friend WithEvents cboScienceImplant As System.Windows.Forms.ComboBox
    Friend WithEvents cboMetallurgyImplant As System.Windows.Forms.ComboBox
    Friend WithEvents lblMaxUnits As System.Windows.Forms.Label
    Friend WithEvents clvResources As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colBPResMaterials As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBPResPerfect As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBPResWaste As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBPResTotal As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBPResPrice As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBPResValue As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBPResIdealML As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents clvOwnedResources As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colBPOResMaterial As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBPOResNeeded As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBPOResOwned As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colBPOResSurplus As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents chkShowSkills As System.Windows.Forms.CheckBox
    Friend WithEvents cboAssetSelection As System.Windows.Forms.ComboBox
    Friend WithEvents lblAssetSelection As System.Windows.Forms.Label
    Friend WithEvents chkUseStandardBPCosting As System.Windows.Forms.CheckBox

End Class
