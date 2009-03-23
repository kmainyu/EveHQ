<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmHQF
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmHQF))
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip
        Me.btnShipPanel = New System.Windows.Forms.ToolStripButton
        Me.btnItemPanel = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator
        Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton
        Me.tsbOptions = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator
        Me.btnPilotManager = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator
        Me.btnCopy = New System.Windows.Forms.ToolStripSplitButton
        Me.mnuCopyForHQF = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuCopyForEFT = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuCopyForForums = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem7 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuShipStats = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator
        Me.btnScreenshot = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator
        Me.btnImportFittings = New System.Windows.Forms.ToolStripSplitButton
        Me.mnuImportEveFittings = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuImportEFTFittings = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator
        Me.btnClipboardPaste = New System.Windows.Forms.ToolStripButton
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator
        Me.cboFittings = New System.Windows.Forms.ToolStripComboBox
        Me.SplitContainerShip = New System.Windows.Forms.SplitContainer
        Me.clvFittings = New DotNetLib.Windows.Forms.ContainerListView
        Me.ContainerListViewColumnHeader1 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ctxFittings = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuFittingsFittingName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuPreviewShip2 = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFittingsShowFitting = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuFittingsRenameFitting = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFittingsCopyFitting = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFittingsDeleteFitting = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuFittingsCreateFitting = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuFittingsBCBrowser = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuCompareFittings = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuExportToEve = New System.Windows.Forms.ToolStripMenuItem
        Me.lblFlyable = New System.Windows.Forms.Label
        Me.cboFlyable = New System.Windows.Forms.ComboBox
        Me.pbSearchShips = New System.Windows.Forms.PictureBox
        Me.txtShipSearch = New System.Windows.Forms.TextBox
        Me.btnResetShips = New System.Windows.Forms.Button
        Me.tvwShips = New System.Windows.Forms.TreeView
        Me.ctxShipBrowser = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuShipBrowserShipName = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuPreviewShip = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuCreateNewFitting = New System.Windows.Forms.ToolStripMenuItem
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuBattleClinicBrowser = New System.Windows.Forms.ToolStripMenuItem
        Me.SplitContainerMod = New System.Windows.Forms.SplitContainer
        Me.chkFilter8192 = New System.Windows.Forms.CheckBox
        Me.chkOnlyShowFittable = New System.Windows.Forms.CheckBox
        Me.chkOnlyShowUsable = New System.Windows.Forms.CheckBox
        Me.chkApplySkills = New System.Windows.Forms.CheckBox
        Me.chkFilter32 = New System.Windows.Forms.CheckBox
        Me.chkFilter16 = New System.Windows.Forms.CheckBox
        Me.chkFilter8 = New System.Windows.Forms.CheckBox
        Me.chkFilter4 = New System.Windows.Forms.CheckBox
        Me.chkFilter2 = New System.Windows.Forms.CheckBox
        Me.chkFilter1 = New System.Windows.Forms.CheckBox
        Me.tvwItems = New System.Windows.Forms.TreeView
        Me.lblModuleDisplayType = New System.Windows.Forms.Label
        Me.txtSearchModules = New System.Windows.Forms.TextBox
        Me.lblSearchModules = New System.Windows.Forms.Label
        Me.lvwItems = New EveHQ.HQF.ListViewNoFlicker
        Me.colModuleName = New System.Windows.Forms.ColumnHeader
        Me.colModuleMetaType = New System.Windows.Forms.ColumnHeader
        Me.colModuleCPU = New System.Windows.Forms.ColumnHeader
        Me.colModulePG = New System.Windows.Forms.ColumnHeader
        Me.ctxModuleList = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuShowModuleInfo = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSep1 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuAddToFavourites_List = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuRemoveFromFavourites = New System.Windows.Forms.ToolStripMenuItem
        Me.mnuSep2 = New System.Windows.Forms.ToolStripSeparator
        Me.mnuShowModuleMarketGroup = New System.Windows.Forms.ToolStripMenuItem
        Me.imgAttributes = New System.Windows.Forms.ImageList(Me.components)
        Me.ctxTabHQF = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.mnuCloseHQFTab = New System.Windows.Forms.ToolStripMenuItem
        Me.tmrClipboard = New System.Windows.Forms.Timer(Me.components)
        Me.tabHQF = New System.Windows.Forms.TabControl
        Me.tabShipPreview = New System.Windows.Forms.TabPage
        Me.panelPreview = New System.Windows.Forms.Panel
        Me.lblShipType = New System.Windows.Forms.Label
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lblLauncherSlots = New System.Windows.Forms.Label
        Me.lblTurretSlots = New System.Windows.Forms.Label
        Me.lblRigSlots = New System.Windows.Forms.Label
        Me.pbLauncherSlot = New System.Windows.Forms.PictureBox
        Me.pbTurretSlot = New System.Windows.Forms.PictureBox
        Me.pbRigSlot = New System.Windows.Forms.PictureBox
        Me.lblHiSlots = New System.Windows.Forms.Label
        Me.lblMedSlots = New System.Windows.Forms.Label
        Me.lblLowSlots = New System.Windows.Forms.Label
        Me.pbHiSlot = New System.Windows.Forms.PictureBox
        Me.pbMedSlot = New System.Windows.Forms.PictureBox
        Me.pbLowSlot = New System.Windows.Forms.PictureBox
        Me.gbFitting = New System.Windows.Forms.GroupBox
        Me.lblCalibration = New System.Windows.Forms.Label
        Me.lblPG = New System.Windows.Forms.Label
        Me.lblCPU = New System.Windows.Forms.Label
        Me.pbCalibration = New System.Windows.Forms.PictureBox
        Me.pbPG = New System.Windows.Forms.PictureBox
        Me.pbCPU = New System.Windows.Forms.PictureBox
        Me.gbShield = New System.Windows.Forms.GroupBox
        Me.lblShieldRecharge = New System.Windows.Forms.Label
        Me.lblShieldHP = New System.Windows.Forms.Label
        Me.lblShieldKi = New System.Windows.Forms.Label
        Me.lblShieldTh = New System.Windows.Forms.Label
        Me.lblShieldEx = New System.Windows.Forms.Label
        Me.lblShieldEM = New System.Windows.Forms.Label
        Me.pbShieldTh = New System.Windows.Forms.PictureBox
        Me.pbShieldKi = New System.Windows.Forms.PictureBox
        Me.pbShieldEx = New System.Windows.Forms.PictureBox
        Me.pbShieldEM = New System.Windows.Forms.PictureBox
        Me.pbShield = New System.Windows.Forms.PictureBox
        Me.gbArmor = New System.Windows.Forms.GroupBox
        Me.lblArmorHP = New System.Windows.Forms.Label
        Me.lblArmorKi = New System.Windows.Forms.Label
        Me.lblArmorTh = New System.Windows.Forms.Label
        Me.lblArmorEx = New System.Windows.Forms.Label
        Me.lblArmorEM = New System.Windows.Forms.Label
        Me.pbArmorTh = New System.Windows.Forms.PictureBox
        Me.pbArmorKi = New System.Windows.Forms.PictureBox
        Me.pbArmorEx = New System.Windows.Forms.PictureBox
        Me.pbArmorEM = New System.Windows.Forms.PictureBox
        Me.pbArmor = New System.Windows.Forms.PictureBox
        Me.gbStructure = New System.Windows.Forms.GroupBox
        Me.lblStructureHP = New System.Windows.Forms.Label
        Me.lblStructureKi = New System.Windows.Forms.Label
        Me.lblStructureTh = New System.Windows.Forms.Label
        Me.lblStructureEx = New System.Windows.Forms.Label
        Me.lblStructureEM = New System.Windows.Forms.Label
        Me.pbStructureTh = New System.Windows.Forms.PictureBox
        Me.pbStructureKi = New System.Windows.Forms.PictureBox
        Me.pbStructureEx = New System.Windows.Forms.PictureBox
        Me.pbStructureEM = New System.Windows.Forms.PictureBox
        Me.pbStructure = New System.Windows.Forms.PictureBox
        Me.gbStorage = New System.Windows.Forms.GroupBox
        Me.lblDroneBandwidth = New System.Windows.Forms.Label
        Me.lblDroneBay = New System.Windows.Forms.Label
        Me.lblCargohold = New System.Windows.Forms.Label
        Me.pbStorage = New System.Windows.Forms.PictureBox
        Me.gbCapacitor = New System.Windows.Forms.GroupBox
        Me.lblCapRecharge = New System.Windows.Forms.Label
        Me.lblCapacitor = New System.Windows.Forms.Label
        Me.pbCapacitor = New System.Windows.Forms.PictureBox
        Me.gbSpeed = New System.Windows.Forms.GroupBox
        Me.lblWarpSpeed = New System.Windows.Forms.Label
        Me.lblInertia = New System.Windows.Forms.Label
        Me.lblSpeed = New System.Windows.Forms.Label
        Me.pbSpeed = New System.Windows.Forms.PictureBox
        Me.pbShip = New System.Windows.Forms.PictureBox
        Me.txtShipDescription = New System.Windows.Forms.RichTextBox
        Me.tabFit = New System.Windows.Forms.TabPage
        Me.panelShipSlot = New System.Windows.Forms.Panel
        Me.panelShipInfo = New System.Windows.Forms.Panel
        Me.Splitter1 = New System.Windows.Forms.Splitter
        Me.Splitter2 = New System.Windows.Forms.Splitter
        Me.ToolStrip1.SuspendLayout()
        Me.SplitContainerShip.Panel1.SuspendLayout()
        Me.SplitContainerShip.Panel2.SuspendLayout()
        Me.SplitContainerShip.SuspendLayout()
        Me.ctxFittings.SuspendLayout()
        CType(Me.pbSearchShips, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ctxShipBrowser.SuspendLayout()
        Me.SplitContainerMod.Panel1.SuspendLayout()
        Me.SplitContainerMod.Panel2.SuspendLayout()
        Me.SplitContainerMod.SuspendLayout()
        Me.ctxModuleList.SuspendLayout()
        Me.ctxTabHQF.SuspendLayout()
        Me.tabHQF.SuspendLayout()
        Me.tabShipPreview.SuspendLayout()
        Me.panelPreview.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.pbLauncherSlot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbTurretSlot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbRigSlot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbHiSlot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbMedSlot, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbLowSlot, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbFitting.SuspendLayout()
        CType(Me.pbCalibration, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbPG, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCPU, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbShield.SuspendLayout()
        CType(Me.pbShieldTh, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbShieldKi, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbShieldEx, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbShieldEM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbShield, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbArmor.SuspendLayout()
        CType(Me.pbArmorTh, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbArmorKi, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbArmorEx, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbArmorEM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbArmor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbStructure.SuspendLayout()
        CType(Me.pbStructureTh, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbStructureKi, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbStructureEx, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbStructureEM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbStructure, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbStorage.SuspendLayout()
        CType(Me.pbStorage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbCapacitor.SuspendLayout()
        CType(Me.pbCapacitor, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbSpeed.SuspendLayout()
        CType(Me.pbSpeed, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbShip, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tabFit.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnShipPanel, Me.btnItemPanel, Me.ToolStripSeparator5, Me.ToolStripButton3, Me.tsbOptions, Me.ToolStripSeparator3, Me.btnPilotManager, Me.ToolStripSeparator1, Me.btnCopy, Me.ToolStripSeparator6, Me.btnScreenshot, Me.ToolStripSeparator2, Me.btnImportFittings, Me.ToolStripSeparator7, Me.btnClipboardPaste, Me.ToolStripSeparator4, Me.cboFittings})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1099, 25)
        Me.ToolStrip1.TabIndex = 2
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'btnShipPanel
        '
        Me.btnShipPanel.Checked = True
        Me.btnShipPanel.CheckOnClick = True
        Me.btnShipPanel.CheckState = System.Windows.Forms.CheckState.Checked
        Me.btnShipPanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnShipPanel.Image = CType(resources.GetObject("btnShipPanel.Image"), System.Drawing.Image)
        Me.btnShipPanel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnShipPanel.Name = "btnShipPanel"
        Me.btnShipPanel.Size = New System.Drawing.Size(23, 22)
        Me.btnShipPanel.Text = "ToolStripButton1"
        Me.btnShipPanel.ToolTipText = "Toggle Ship Panels"
        '
        'btnItemPanel
        '
        Me.btnItemPanel.Checked = True
        Me.btnItemPanel.CheckOnClick = True
        Me.btnItemPanel.CheckState = System.Windows.Forms.CheckState.Checked
        Me.btnItemPanel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnItemPanel.Image = CType(resources.GetObject("btnItemPanel.Image"), System.Drawing.Image)
        Me.btnItemPanel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnItemPanel.Name = "btnItemPanel"
        Me.btnItemPanel.Size = New System.Drawing.Size(23, 22)
        Me.btnItemPanel.Text = "ToolStripButton2"
        Me.btnItemPanel.ToolTipText = "Toggle Item Panels"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 25)
        '
        'ToolStripButton3
        '
        Me.ToolStripButton3.Image = CType(resources.GetObject("ToolStripButton3.Image"), System.Drawing.Image)
        Me.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton3.Name = "ToolStripButton3"
        Me.ToolStripButton3.Size = New System.Drawing.Size(82, 22)
        Me.ToolStripButton3.Text = "Check Data"
        Me.ToolStripButton3.Visible = False
        '
        'tsbOptions
        '
        Me.tsbOptions.Image = CType(resources.GetObject("tsbOptions.Image"), System.Drawing.Image)
        Me.tsbOptions.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbOptions.Name = "tsbOptions"
        Me.tsbOptions.Size = New System.Drawing.Size(64, 22)
        Me.tsbOptions.Text = "Options"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 25)
        '
        'btnPilotManager
        '
        Me.btnPilotManager.Enabled = False
        Me.btnPilotManager.Image = CType(resources.GetObject("btnPilotManager.Image"), System.Drawing.Image)
        Me.btnPilotManager.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnPilotManager.Name = "btnPilotManager"
        Me.btnPilotManager.Size = New System.Drawing.Size(92, 22)
        Me.btnPilotManager.Text = "Pilot Manager"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 25)
        '
        'btnCopy
        '
        Me.btnCopy.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCopyForHQF, Me.mnuCopyForEFT, Me.mnuCopyForForums, Me.ToolStripMenuItem7, Me.mnuShipStats})
        Me.btnCopy.Enabled = False
        Me.btnCopy.Image = CType(resources.GetObject("btnCopy.Image"), System.Drawing.Image)
        Me.btnCopy.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(104, 22)
        Me.btnCopy.Text = "Export Fitting"
        '
        'mnuCopyForHQF
        '
        Me.mnuCopyForHQF.Name = "mnuCopyForHQF"
        Me.mnuCopyForHQF.Size = New System.Drawing.Size(179, 22)
        Me.mnuCopyForHQF.Text = "Copy For HQF"
        '
        'mnuCopyForEFT
        '
        Me.mnuCopyForEFT.Name = "mnuCopyForEFT"
        Me.mnuCopyForEFT.Size = New System.Drawing.Size(179, 22)
        Me.mnuCopyForEFT.Text = "Copy For EFT"
        '
        'mnuCopyForForums
        '
        Me.mnuCopyForForums.Name = "mnuCopyForForums"
        Me.mnuCopyForForums.Size = New System.Drawing.Size(179, 22)
        Me.mnuCopyForForums.Text = "Copy For Forums"
        '
        'ToolStripMenuItem7
        '
        Me.ToolStripMenuItem7.Name = "ToolStripMenuItem7"
        Me.ToolStripMenuItem7.Size = New System.Drawing.Size(176, 6)
        '
        'mnuShipStats
        '
        Me.mnuShipStats.Name = "mnuShipStats"
        Me.mnuShipStats.Size = New System.Drawing.Size(179, 22)
        Me.mnuShipStats.Text = "Copy Ship Statistics"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(6, 25)
        '
        'btnScreenshot
        '
        Me.btnScreenshot.AutoToolTip = False
        Me.btnScreenshot.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.btnScreenshot.Enabled = False
        Me.btnScreenshot.Image = CType(resources.GetObject("btnScreenshot.Image"), System.Drawing.Image)
        Me.btnScreenshot.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnScreenshot.Name = "btnScreenshot"
        Me.btnScreenshot.Size = New System.Drawing.Size(23, 22)
        Me.btnScreenshot.Text = "Screenshot"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 25)
        '
        'btnImportFittings
        '
        Me.btnImportFittings.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuImportEveFittings, Me.mnuImportEFTFittings})
        Me.btnImportFittings.Image = CType(resources.GetObject("btnImportFittings.Image"), System.Drawing.Image)
        Me.btnImportFittings.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnImportFittings.Name = "btnImportFittings"
        Me.btnImportFittings.Size = New System.Drawing.Size(109, 22)
        Me.btnImportFittings.Text = "Import Fittings"
        '
        'mnuImportEveFittings
        '
        Me.mnuImportEveFittings.Name = "mnuImportEveFittings"
        Me.mnuImportEveFittings.Size = New System.Drawing.Size(176, 22)
        Me.mnuImportEveFittings.Text = "Import Eve Fittings"
        '
        'mnuImportEFTFittings
        '
        Me.mnuImportEFTFittings.Name = "mnuImportEFTFittings"
        Me.mnuImportEFTFittings.Size = New System.Drawing.Size(176, 22)
        Me.mnuImportEFTFittings.Text = "Import EFT Fittings"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(6, 25)
        '
        'btnClipboardPaste
        '
        Me.btnClipboardPaste.Enabled = False
        Me.btnClipboardPaste.Image = CType(resources.GetObject("btnClipboardPaste.Image"), System.Drawing.Image)
        Me.btnClipboardPaste.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.btnClipboardPaste.Name = "btnClipboardPaste"
        Me.btnClipboardPaste.Size = New System.Drawing.Size(132, 22)
        Me.btnClipboardPaste.Text = "Import from Clipboard"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(6, 25)
        '
        'cboFittings
        '
        Me.cboFittings.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFittings.FlatStyle = System.Windows.Forms.FlatStyle.Standard
        Me.cboFittings.Name = "cboFittings"
        Me.cboFittings.Size = New System.Drawing.Size(250, 25)
        Me.cboFittings.Sorted = True
        Me.cboFittings.Visible = False
        '
        'SplitContainerShip
        '
        Me.SplitContainerShip.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerShip.Dock = System.Windows.Forms.DockStyle.Left
        Me.SplitContainerShip.Location = New System.Drawing.Point(0, 25)
        Me.SplitContainerShip.Name = "SplitContainerShip"
        Me.SplitContainerShip.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerShip.Panel1
        '
        Me.SplitContainerShip.Panel1.Controls.Add(Me.clvFittings)
        '
        'SplitContainerShip.Panel2
        '
        Me.SplitContainerShip.Panel2.Controls.Add(Me.lblFlyable)
        Me.SplitContainerShip.Panel2.Controls.Add(Me.cboFlyable)
        Me.SplitContainerShip.Panel2.Controls.Add(Me.pbSearchShips)
        Me.SplitContainerShip.Panel2.Controls.Add(Me.txtShipSearch)
        Me.SplitContainerShip.Panel2.Controls.Add(Me.btnResetShips)
        Me.SplitContainerShip.Panel2.Controls.Add(Me.tvwShips)
        Me.SplitContainerShip.Size = New System.Drawing.Size(200, 696)
        Me.SplitContainerShip.SplitterDistance = 298
        Me.SplitContainerShip.SplitterWidth = 2
        Me.SplitContainerShip.TabIndex = 3
        '
        'clvFittings
        '
        Me.clvFittings.AllowMultiSelect = True
        Me.clvFittings.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.ContainerListViewColumnHeader1})
        Me.clvFittings.DefaultItemHeight = 16
        Me.clvFittings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.clvFittings.ForeColor = System.Drawing.SystemColors.WindowText
        Me.clvFittings.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.clvFittings.ItemContextMenu = Me.ctxFittings
        Me.clvFittings.Location = New System.Drawing.Point(0, 0)
        Me.clvFittings.Name = "clvFittings"
        Me.clvFittings.ShowPlusMinus = True
        Me.clvFittings.ShowRootTreeLines = True
        Me.clvFittings.ShowTreeLines = True
        Me.clvFittings.Size = New System.Drawing.Size(196, 294)
        Me.clvFittings.TabIndex = 0
        '
        'ContainerListViewColumnHeader1
        '
        Me.ContainerListViewColumnHeader1.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader1.Tag = Nothing
        Me.ContainerListViewColumnHeader1.Text = "Available Fittings"
        Me.ContainerListViewColumnHeader1.Width = 150
        '
        'ctxFittings
        '
        Me.ctxFittings.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuFittingsFittingName, Me.ToolStripMenuItem4, Me.mnuPreviewShip2, Me.mnuFittingsShowFitting, Me.ToolStripMenuItem1, Me.mnuFittingsRenameFitting, Me.mnuFittingsCopyFitting, Me.mnuFittingsDeleteFitting, Me.ToolStripMenuItem3, Me.mnuFittingsCreateFitting, Me.mnuFittingsBCBrowser, Me.ToolStripMenuItem5, Me.mnuCompareFittings, Me.mnuExportToEve})
        Me.ctxFittings.Name = "ctxFittings"
        Me.ctxFittings.Size = New System.Drawing.Size(182, 248)
        '
        'mnuFittingsFittingName
        '
        Me.mnuFittingsFittingName.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.mnuFittingsFittingName.Name = "mnuFittingsFittingName"
        Me.mnuFittingsFittingName.Size = New System.Drawing.Size(181, 22)
        Me.mnuFittingsFittingName.Text = "Fitting Name"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(178, 6)
        '
        'mnuPreviewShip2
        '
        Me.mnuPreviewShip2.Name = "mnuPreviewShip2"
        Me.mnuPreviewShip2.Size = New System.Drawing.Size(181, 22)
        Me.mnuPreviewShip2.Text = "Preview Ship Details"
        '
        'mnuFittingsShowFitting
        '
        Me.mnuFittingsShowFitting.Name = "mnuFittingsShowFitting"
        Me.mnuFittingsShowFitting.Size = New System.Drawing.Size(181, 22)
        Me.mnuFittingsShowFitting.Text = "Show Fitting"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(178, 6)
        '
        'mnuFittingsRenameFitting
        '
        Me.mnuFittingsRenameFitting.Name = "mnuFittingsRenameFitting"
        Me.mnuFittingsRenameFitting.Size = New System.Drawing.Size(181, 22)
        Me.mnuFittingsRenameFitting.Text = "Rename Fitting"
        '
        'mnuFittingsCopyFitting
        '
        Me.mnuFittingsCopyFitting.Name = "mnuFittingsCopyFitting"
        Me.mnuFittingsCopyFitting.Size = New System.Drawing.Size(181, 22)
        Me.mnuFittingsCopyFitting.Text = "Copy Fitting"
        '
        'mnuFittingsDeleteFitting
        '
        Me.mnuFittingsDeleteFitting.Name = "mnuFittingsDeleteFitting"
        Me.mnuFittingsDeleteFitting.Size = New System.Drawing.Size(181, 22)
        Me.mnuFittingsDeleteFitting.Text = "Delete Fitting"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(178, 6)
        '
        'mnuFittingsCreateFitting
        '
        Me.mnuFittingsCreateFitting.Name = "mnuFittingsCreateFitting"
        Me.mnuFittingsCreateFitting.Size = New System.Drawing.Size(181, 22)
        Me.mnuFittingsCreateFitting.Text = "Create New Fitting"
        '
        'mnuFittingsBCBrowser
        '
        Me.mnuFittingsBCBrowser.Name = "mnuFittingsBCBrowser"
        Me.mnuFittingsBCBrowser.Size = New System.Drawing.Size(181, 22)
        Me.mnuFittingsBCBrowser.Text = "BattleClinic Browser"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(178, 6)
        '
        'mnuCompareFittings
        '
        Me.mnuCompareFittings.Name = "mnuCompareFittings"
        Me.mnuCompareFittings.Size = New System.Drawing.Size(181, 22)
        Me.mnuCompareFittings.Text = "Compare Fittings"
        '
        'mnuExportToEve
        '
        Me.mnuExportToEve.Name = "mnuExportToEve"
        Me.mnuExportToEve.Size = New System.Drawing.Size(181, 22)
        Me.mnuExportToEve.Text = "Export To Eve"
        '
        'lblFlyable
        '
        Me.lblFlyable.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblFlyable.AutoSize = True
        Me.lblFlyable.Location = New System.Drawing.Point(3, 31)
        Me.lblFlyable.Name = "lblFlyable"
        Me.lblFlyable.Size = New System.Drawing.Size(45, 13)
        Me.lblFlyable.TabIndex = 6
        Me.lblFlyable.Text = "Flyable:"
        '
        'cboFlyable
        '
        Me.cboFlyable.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboFlyable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFlyable.FormattingEnabled = True
        Me.cboFlyable.Location = New System.Drawing.Point(51, 28)
        Me.cboFlyable.Name = "cboFlyable"
        Me.cboFlyable.Size = New System.Drawing.Size(144, 21)
        Me.cboFlyable.TabIndex = 5
        '
        'pbSearchShips
        '
        Me.pbSearchShips.Image = CType(resources.GetObject("pbSearchShips.Image"), System.Drawing.Image)
        Me.pbSearchShips.Location = New System.Drawing.Point(3, 3)
        Me.pbSearchShips.Name = "pbSearchShips"
        Me.pbSearchShips.Size = New System.Drawing.Size(20, 20)
        Me.pbSearchShips.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbSearchShips.TabIndex = 4
        Me.pbSearchShips.TabStop = False
        '
        'txtShipSearch
        '
        Me.txtShipSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtShipSearch.Location = New System.Drawing.Point(29, 1)
        Me.txtShipSearch.Name = "txtShipSearch"
        Me.txtShipSearch.Size = New System.Drawing.Size(120, 21)
        Me.txtShipSearch.TabIndex = 1
        '
        'btnResetShips
        '
        Me.btnResetShips.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnResetShips.Location = New System.Drawing.Point(152, 0)
        Me.btnResetShips.Name = "btnResetShips"
        Me.btnResetShips.Size = New System.Drawing.Size(44, 23)
        Me.btnResetShips.TabIndex = 3
        Me.btnResetShips.Text = "Reset"
        Me.btnResetShips.UseVisualStyleBackColor = True
        '
        'tvwShips
        '
        Me.tvwShips.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvwShips.ContextMenuStrip = Me.ctxShipBrowser
        Me.tvwShips.FullRowSelect = True
        Me.tvwShips.Location = New System.Drawing.Point(0, 55)
        Me.tvwShips.Name = "tvwShips"
        Me.tvwShips.Size = New System.Drawing.Size(196, 339)
        Me.tvwShips.TabIndex = 0
        '
        'ctxShipBrowser
        '
        Me.ctxShipBrowser.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuShipBrowserShipName, Me.ToolStripMenuItem2, Me.mnuPreviewShip, Me.mnuCreateNewFitting, Me.ToolStripMenuItem6, Me.mnuBattleClinicBrowser})
        Me.ctxShipBrowser.Name = "ctxShipBrowser"
        Me.ctxShipBrowser.Size = New System.Drawing.Size(182, 104)
        '
        'mnuShipBrowserShipName
        '
        Me.mnuShipBrowserShipName.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.mnuShipBrowserShipName.Name = "mnuShipBrowserShipName"
        Me.mnuShipBrowserShipName.Size = New System.Drawing.Size(181, 22)
        Me.mnuShipBrowserShipName.Text = "Ship Name"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(178, 6)
        '
        'mnuPreviewShip
        '
        Me.mnuPreviewShip.Name = "mnuPreviewShip"
        Me.mnuPreviewShip.Size = New System.Drawing.Size(181, 22)
        Me.mnuPreviewShip.Text = "Preview Ship Details"
        Me.mnuPreviewShip.ToolTipText = "View Ship Stats and Attributes"
        '
        'mnuCreateNewFitting
        '
        Me.mnuCreateNewFitting.Name = "mnuCreateNewFitting"
        Me.mnuCreateNewFitting.Size = New System.Drawing.Size(181, 22)
        Me.mnuCreateNewFitting.Text = "Create New Fitting"
        Me.mnuCreateNewFitting.ToolTipText = "Create a new fitting for this ship type"
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        Me.ToolStripMenuItem6.Size = New System.Drawing.Size(178, 6)
        '
        'mnuBattleClinicBrowser
        '
        Me.mnuBattleClinicBrowser.Name = "mnuBattleClinicBrowser"
        Me.mnuBattleClinicBrowser.Size = New System.Drawing.Size(181, 22)
        Me.mnuBattleClinicBrowser.Text = "Battleclinic Browser"
        Me.mnuBattleClinicBrowser.ToolTipText = "View Ship Loadouts from BattleClinic"
        '
        'SplitContainerMod
        '
        Me.SplitContainerMod.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.SplitContainerMod.Dock = System.Windows.Forms.DockStyle.Right
        Me.SplitContainerMod.Location = New System.Drawing.Point(808, 25)
        Me.SplitContainerMod.Name = "SplitContainerMod"
        Me.SplitContainerMod.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainerMod.Panel1
        '
        Me.SplitContainerMod.Panel1.Controls.Add(Me.chkFilter8192)
        Me.SplitContainerMod.Panel1.Controls.Add(Me.chkOnlyShowFittable)
        Me.SplitContainerMod.Panel1.Controls.Add(Me.chkOnlyShowUsable)
        Me.SplitContainerMod.Panel1.Controls.Add(Me.chkApplySkills)
        Me.SplitContainerMod.Panel1.Controls.Add(Me.chkFilter32)
        Me.SplitContainerMod.Panel1.Controls.Add(Me.chkFilter16)
        Me.SplitContainerMod.Panel1.Controls.Add(Me.chkFilter8)
        Me.SplitContainerMod.Panel1.Controls.Add(Me.chkFilter4)
        Me.SplitContainerMod.Panel1.Controls.Add(Me.chkFilter2)
        Me.SplitContainerMod.Panel1.Controls.Add(Me.chkFilter1)
        Me.SplitContainerMod.Panel1.Controls.Add(Me.tvwItems)
        '
        'SplitContainerMod.Panel2
        '
        Me.SplitContainerMod.Panel2.Controls.Add(Me.lblModuleDisplayType)
        Me.SplitContainerMod.Panel2.Controls.Add(Me.txtSearchModules)
        Me.SplitContainerMod.Panel2.Controls.Add(Me.lblSearchModules)
        Me.SplitContainerMod.Panel2.Controls.Add(Me.lvwItems)
        Me.SplitContainerMod.Size = New System.Drawing.Size(291, 696)
        Me.SplitContainerMod.SplitterDistance = 298
        Me.SplitContainerMod.SplitterWidth = 2
        Me.SplitContainerMod.TabIndex = 4
        '
        'chkFilter8192
        '
        Me.chkFilter8192.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkFilter8192.AutoSize = True
        Me.chkFilter8192.Checked = True
        Me.chkFilter8192.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkFilter8192.Location = New System.Drawing.Point(138, 226)
        Me.chkFilter8192.Name = "chkFilter8192"
        Me.chkFilter8192.Size = New System.Drawing.Size(58, 17)
        Me.chkFilter8192.TabIndex = 18
        Me.chkFilter8192.Tag = "8192"
        Me.chkFilter8192.Text = "Tech 3"
        Me.chkFilter8192.UseVisualStyleBackColor = True
        '
        'chkOnlyShowFittable
        '
        Me.chkOnlyShowFittable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkOnlyShowFittable.AutoSize = True
        Me.chkOnlyShowFittable.Location = New System.Drawing.Point(154, 274)
        Me.chkOnlyShowFittable.Name = "chkOnlyShowFittable"
        Me.chkOnlyShowFittable.Size = New System.Drawing.Size(62, 17)
        Me.chkOnlyShowFittable.TabIndex = 17
        Me.chkOnlyShowFittable.Tag = "1"
        Me.chkOnlyShowFittable.Text = "Fittable"
        Me.chkOnlyShowFittable.UseVisualStyleBackColor = True
        '
        'chkOnlyShowUsable
        '
        Me.chkOnlyShowUsable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkOnlyShowUsable.AutoSize = True
        Me.chkOnlyShowUsable.Location = New System.Drawing.Point(90, 274)
        Me.chkOnlyShowUsable.Name = "chkOnlyShowUsable"
        Me.chkOnlyShowUsable.Size = New System.Drawing.Size(58, 17)
        Me.chkOnlyShowUsable.TabIndex = 16
        Me.chkOnlyShowUsable.Tag = "1"
        Me.chkOnlyShowUsable.Text = "Usable"
        Me.chkOnlyShowUsable.UseVisualStyleBackColor = True
        '
        'chkApplySkills
        '
        Me.chkApplySkills.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkApplySkills.AutoSize = True
        Me.chkApplySkills.Location = New System.Drawing.Point(6, 274)
        Me.chkApplySkills.Name = "chkApplySkills"
        Me.chkApplySkills.Size = New System.Drawing.Size(78, 17)
        Me.chkApplySkills.TabIndex = 15
        Me.chkApplySkills.Tag = "1"
        Me.chkApplySkills.Text = "Apply Skills"
        Me.chkApplySkills.UseVisualStyleBackColor = True
        '
        'chkFilter32
        '
        Me.chkFilter32.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkFilter32.AutoSize = True
        Me.chkFilter32.BackColor = System.Drawing.SystemColors.Control
        Me.chkFilter32.Checked = True
        Me.chkFilter32.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkFilter32.Location = New System.Drawing.Point(72, 258)
        Me.chkFilter32.Name = "chkFilter32"
        Me.chkFilter32.Size = New System.Drawing.Size(79, 17)
        Me.chkFilter32.TabIndex = 14
        Me.chkFilter32.Tag = "32"
        Me.chkFilter32.Text = "Deadspace"
        Me.chkFilter32.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.chkFilter32.UseVisualStyleBackColor = False
        '
        'chkFilter16
        '
        Me.chkFilter16.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkFilter16.AutoSize = True
        Me.chkFilter16.Checked = True
        Me.chkFilter16.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkFilter16.Location = New System.Drawing.Point(72, 242)
        Me.chkFilter16.Name = "chkFilter16"
        Me.chkFilter16.Size = New System.Drawing.Size(59, 17)
        Me.chkFilter16.TabIndex = 13
        Me.chkFilter16.Tag = "16"
        Me.chkFilter16.Text = "Officer"
        Me.chkFilter16.UseVisualStyleBackColor = True
        '
        'chkFilter8
        '
        Me.chkFilter8.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkFilter8.AutoSize = True
        Me.chkFilter8.Checked = True
        Me.chkFilter8.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkFilter8.Location = New System.Drawing.Point(6, 242)
        Me.chkFilter8.Name = "chkFilter8"
        Me.chkFilter8.Size = New System.Drawing.Size(61, 17)
        Me.chkFilter8.TabIndex = 12
        Me.chkFilter8.Tag = "8"
        Me.chkFilter8.Text = "Faction"
        Me.chkFilter8.UseVisualStyleBackColor = True
        '
        'chkFilter4
        '
        Me.chkFilter4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkFilter4.AutoSize = True
        Me.chkFilter4.Checked = True
        Me.chkFilter4.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkFilter4.Location = New System.Drawing.Point(6, 258)
        Me.chkFilter4.Name = "chkFilter4"
        Me.chkFilter4.Size = New System.Drawing.Size(68, 17)
        Me.chkFilter4.TabIndex = 11
        Me.chkFilter4.Tag = "4"
        Me.chkFilter4.Text = "Storyline"
        Me.chkFilter4.UseVisualStyleBackColor = True
        '
        'chkFilter2
        '
        Me.chkFilter2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkFilter2.AutoSize = True
        Me.chkFilter2.Checked = True
        Me.chkFilter2.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkFilter2.Location = New System.Drawing.Point(72, 226)
        Me.chkFilter2.Name = "chkFilter2"
        Me.chkFilter2.Size = New System.Drawing.Size(58, 17)
        Me.chkFilter2.TabIndex = 10
        Me.chkFilter2.Tag = "2"
        Me.chkFilter2.Text = "Tech 2"
        Me.chkFilter2.UseVisualStyleBackColor = True
        '
        'chkFilter1
        '
        Me.chkFilter1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.chkFilter1.AutoSize = True
        Me.chkFilter1.Checked = True
        Me.chkFilter1.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkFilter1.Location = New System.Drawing.Point(6, 226)
        Me.chkFilter1.Name = "chkFilter1"
        Me.chkFilter1.Size = New System.Drawing.Size(58, 17)
        Me.chkFilter1.TabIndex = 9
        Me.chkFilter1.Tag = "1"
        Me.chkFilter1.Text = "Tech 1"
        Me.chkFilter1.UseVisualStyleBackColor = True
        '
        'tvwItems
        '
        Me.tvwItems.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tvwItems.FullRowSelect = True
        Me.tvwItems.HideSelection = False
        Me.tvwItems.Location = New System.Drawing.Point(0, -2)
        Me.tvwItems.Name = "tvwItems"
        Me.tvwItems.Size = New System.Drawing.Size(287, 222)
        Me.tvwItems.TabIndex = 1
        '
        'lblModuleDisplayType
        '
        Me.lblModuleDisplayType.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblModuleDisplayType.AutoSize = True
        Me.lblModuleDisplayType.Location = New System.Drawing.Point(3, 375)
        Me.lblModuleDisplayType.Name = "lblModuleDisplayType"
        Me.lblModuleDisplayType.Size = New System.Drawing.Size(87, 13)
        Me.lblModuleDisplayType.TabIndex = 20
        Me.lblModuleDisplayType.Text = "Displaying: None"
        '
        'txtSearchModules
        '
        Me.txtSearchModules.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearchModules.Location = New System.Drawing.Point(54, 3)
        Me.txtSearchModules.Name = "txtSearchModules"
        Me.txtSearchModules.Size = New System.Drawing.Size(230, 21)
        Me.txtSearchModules.TabIndex = 19
        '
        'lblSearchModules
        '
        Me.lblSearchModules.AutoSize = True
        Me.lblSearchModules.Location = New System.Drawing.Point(4, 6)
        Me.lblSearchModules.Name = "lblSearchModules"
        Me.lblSearchModules.Size = New System.Drawing.Size(44, 13)
        Me.lblSearchModules.TabIndex = 18
        Me.lblSearchModules.Text = "Search:"
        '
        'lvwItems
        '
        Me.lvwItems.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lvwItems.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colModuleName, Me.colModuleMetaType, Me.colModuleCPU, Me.colModulePG})
        Me.lvwItems.ContextMenuStrip = Me.ctxModuleList
        Me.lvwItems.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lvwItems.FullRowSelect = True
        Me.lvwItems.Location = New System.Drawing.Point(0, 29)
        Me.lvwItems.MultiSelect = False
        Me.lvwItems.Name = "lvwItems"
        Me.lvwItems.ShowItemToolTips = True
        Me.lvwItems.Size = New System.Drawing.Size(287, 343)
        Me.lvwItems.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwItems.TabIndex = 0
        Me.lvwItems.UseCompatibleStateImageBehavior = False
        Me.lvwItems.View = System.Windows.Forms.View.Details
        '
        'colModuleName
        '
        Me.colModuleName.Text = "Module"
        Me.colModuleName.Width = 150
        '
        'colModuleMetaType
        '
        Me.colModuleMetaType.Text = "Meta"
        Me.colModuleMetaType.Width = 40
        '
        'colModuleCPU
        '
        Me.colModuleCPU.Text = "CPU"
        Me.colModuleCPU.Width = 40
        '
        'colModulePG
        '
        Me.colModulePG.Text = "PG"
        Me.colModulePG.Width = 40
        '
        'ctxModuleList
        '
        Me.ctxModuleList.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuShowModuleInfo, Me.mnuSep1, Me.mnuAddToFavourites_List, Me.mnuRemoveFromFavourites, Me.mnuSep2, Me.mnuShowModuleMarketGroup})
        Me.ctxModuleList.Name = "ctxModuleList"
        Me.ctxModuleList.Size = New System.Drawing.Size(217, 104)
        '
        'mnuShowModuleInfo
        '
        Me.mnuShowModuleInfo.Name = "mnuShowModuleInfo"
        Me.mnuShowModuleInfo.Size = New System.Drawing.Size(216, 22)
        Me.mnuShowModuleInfo.Text = "Show Info"
        '
        'mnuSep1
        '
        Me.mnuSep1.Name = "mnuSep1"
        Me.mnuSep1.Size = New System.Drawing.Size(213, 6)
        '
        'mnuAddToFavourites_List
        '
        Me.mnuAddToFavourites_List.Name = "mnuAddToFavourites_List"
        Me.mnuAddToFavourites_List.Size = New System.Drawing.Size(216, 22)
        Me.mnuAddToFavourites_List.Text = "Add To Favourites"
        '
        'mnuRemoveFromFavourites
        '
        Me.mnuRemoveFromFavourites.Name = "mnuRemoveFromFavourites"
        Me.mnuRemoveFromFavourites.Size = New System.Drawing.Size(216, 22)
        Me.mnuRemoveFromFavourites.Text = "Remove From Favourites"
        '
        'mnuSep2
        '
        Me.mnuSep2.Name = "mnuSep2"
        Me.mnuSep2.Size = New System.Drawing.Size(213, 6)
        '
        'mnuShowModuleMarketGroup
        '
        Me.mnuShowModuleMarketGroup.Name = "mnuShowModuleMarketGroup"
        Me.mnuShowModuleMarketGroup.Size = New System.Drawing.Size(216, 22)
        Me.mnuShowModuleMarketGroup.Text = "Show Module Market Group"
        '
        'imgAttributes
        '
        Me.imgAttributes.ImageStream = CType(resources.GetObject("imgAttributes.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imgAttributes.TransparentColor = System.Drawing.Color.Transparent
        Me.imgAttributes.Images.SetKeyName(0, "lowSlot")
        Me.imgAttributes.Images.SetKeyName(1, "midSlot")
        Me.imgAttributes.Images.SetKeyName(2, "hiSlot")
        Me.imgAttributes.Images.SetKeyName(3, "rigSlot")
        '
        'ctxTabHQF
        '
        Me.ctxTabHQF.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mnuCloseHQFTab})
        Me.ctxTabHQF.Name = "ctxTabbedMDI"
        Me.ctxTabHQF.Size = New System.Drawing.Size(128, 26)
        '
        'mnuCloseHQFTab
        '
        Me.mnuCloseHQFTab.Name = "mnuCloseHQFTab"
        Me.mnuCloseHQFTab.Size = New System.Drawing.Size(127, 22)
        Me.mnuCloseHQFTab.Text = "Not Valid"
        '
        'tmrClipboard
        '
        Me.tmrClipboard.Enabled = True
        Me.tmrClipboard.Interval = 1000
        '
        'tabHQF
        '
        Me.tabHQF.ContextMenuStrip = Me.ctxTabHQF
        Me.tabHQF.Controls.Add(Me.tabShipPreview)
        Me.tabHQF.Controls.Add(Me.tabFit)
        Me.tabHQF.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabHQF.Location = New System.Drawing.Point(203, 25)
        Me.tabHQF.Multiline = True
        Me.tabHQF.Name = "tabHQF"
        Me.tabHQF.SelectedIndex = 0
        Me.tabHQF.Size = New System.Drawing.Size(602, 696)
        Me.tabHQF.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.tabHQF.TabIndex = 0
        '
        'tabShipPreview
        '
        Me.tabShipPreview.Controls.Add(Me.panelPreview)
        Me.tabShipPreview.Location = New System.Drawing.Point(4, 22)
        Me.tabShipPreview.Name = "tabShipPreview"
        Me.tabShipPreview.Padding = New System.Windows.Forms.Padding(3)
        Me.tabShipPreview.Size = New System.Drawing.Size(594, 670)
        Me.tabShipPreview.TabIndex = 0
        Me.tabShipPreview.Text = "Ship Preview"
        Me.tabShipPreview.UseVisualStyleBackColor = True
        '
        'panelPreview
        '
        Me.panelPreview.BackColor = System.Drawing.SystemColors.Control
        Me.panelPreview.Controls.Add(Me.lblShipType)
        Me.panelPreview.Controls.Add(Me.FlowLayoutPanel1)
        Me.panelPreview.Controls.Add(Me.pbShip)
        Me.panelPreview.Controls.Add(Me.txtShipDescription)
        Me.panelPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelPreview.Location = New System.Drawing.Point(3, 3)
        Me.panelPreview.Name = "panelPreview"
        Me.panelPreview.Size = New System.Drawing.Size(588, 664)
        Me.panelPreview.TabIndex = 19
        '
        'lblShipType
        '
        Me.lblShipType.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblShipType.Font = New System.Drawing.Font("Arial", 20.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShipType.Location = New System.Drawing.Point(4, 4)
        Me.lblShipType.Name = "lblShipType"
        Me.lblShipType.Size = New System.Drawing.Size(581, 33)
        Me.lblShipType.TabIndex = 2
        Me.lblShipType.Text = "Ship Type"
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.FlowLayoutPanel1.Controls.Add(Me.GroupBox1)
        Me.FlowLayoutPanel1.Controls.Add(Me.gbFitting)
        Me.FlowLayoutPanel1.Controls.Add(Me.gbShield)
        Me.FlowLayoutPanel1.Controls.Add(Me.gbArmor)
        Me.FlowLayoutPanel1.Controls.Add(Me.gbStructure)
        Me.FlowLayoutPanel1.Controls.Add(Me.gbStorage)
        Me.FlowLayoutPanel1.Controls.Add(Me.gbCapacitor)
        Me.FlowLayoutPanel1.Controls.Add(Me.gbSpeed)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(3, 174)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(582, 485)
        Me.FlowLayoutPanel1.TabIndex = 18
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblLauncherSlots)
        Me.GroupBox1.Controls.Add(Me.lblTurretSlots)
        Me.GroupBox1.Controls.Add(Me.lblRigSlots)
        Me.GroupBox1.Controls.Add(Me.pbLauncherSlot)
        Me.GroupBox1.Controls.Add(Me.pbTurretSlot)
        Me.GroupBox1.Controls.Add(Me.pbRigSlot)
        Me.GroupBox1.Controls.Add(Me.lblHiSlots)
        Me.GroupBox1.Controls.Add(Me.lblMedSlots)
        Me.GroupBox1.Controls.Add(Me.lblLowSlots)
        Me.GroupBox1.Controls.Add(Me.pbHiSlot)
        Me.GroupBox1.Controls.Add(Me.pbMedSlot)
        Me.GroupBox1.Controls.Add(Me.pbLowSlot)
        Me.GroupBox1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox1.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(251, 126)
        Me.GroupBox1.TabIndex = 16
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Slot Layout"
        '
        'lblLauncherSlots
        '
        Me.lblLauncherSlots.AutoSize = True
        Me.lblLauncherSlots.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLauncherSlots.Location = New System.Drawing.Point(153, 93)
        Me.lblLauncherSlots.Name = "lblLauncherSlots"
        Me.lblLauncherSlots.Size = New System.Drawing.Size(90, 13)
        Me.lblLauncherSlots.TabIndex = 14
        Me.lblLauncherSlots.Text = "Launcher Slots: 0"
        '
        'lblTurretSlots
        '
        Me.lblTurretSlots.AutoSize = True
        Me.lblTurretSlots.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTurretSlots.Location = New System.Drawing.Point(153, 61)
        Me.lblTurretSlots.Name = "lblTurretSlots"
        Me.lblTurretSlots.Size = New System.Drawing.Size(76, 13)
        Me.lblTurretSlots.TabIndex = 13
        Me.lblTurretSlots.Text = "Turret Slots: 0"
        '
        'lblRigSlots
        '
        Me.lblRigSlots.AutoSize = True
        Me.lblRigSlots.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRigSlots.Location = New System.Drawing.Point(153, 29)
        Me.lblRigSlots.Name = "lblRigSlots"
        Me.lblRigSlots.Size = New System.Drawing.Size(61, 13)
        Me.lblRigSlots.TabIndex = 12
        Me.lblRigSlots.Text = "Rig Slots: 0"
        '
        'pbLauncherSlot
        '
        Me.pbLauncherSlot.Image = CType(resources.GetObject("pbLauncherSlot.Image"), System.Drawing.Image)
        Me.pbLauncherSlot.Location = New System.Drawing.Point(115, 84)
        Me.pbLauncherSlot.Name = "pbLauncherSlot"
        Me.pbLauncherSlot.Size = New System.Drawing.Size(32, 32)
        Me.pbLauncherSlot.TabIndex = 11
        Me.pbLauncherSlot.TabStop = False
        '
        'pbTurretSlot
        '
        Me.pbTurretSlot.Image = CType(resources.GetObject("pbTurretSlot.Image"), System.Drawing.Image)
        Me.pbTurretSlot.Location = New System.Drawing.Point(115, 52)
        Me.pbTurretSlot.Name = "pbTurretSlot"
        Me.pbTurretSlot.Size = New System.Drawing.Size(32, 32)
        Me.pbTurretSlot.TabIndex = 10
        Me.pbTurretSlot.TabStop = False
        '
        'pbRigSlot
        '
        Me.pbRigSlot.Image = CType(resources.GetObject("pbRigSlot.Image"), System.Drawing.Image)
        Me.pbRigSlot.Location = New System.Drawing.Point(115, 20)
        Me.pbRigSlot.Name = "pbRigSlot"
        Me.pbRigSlot.Size = New System.Drawing.Size(32, 32)
        Me.pbRigSlot.TabIndex = 9
        Me.pbRigSlot.TabStop = False
        '
        'lblHiSlots
        '
        Me.lblHiSlots.AutoSize = True
        Me.lblHiSlots.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblHiSlots.Location = New System.Drawing.Point(44, 94)
        Me.lblHiSlots.Name = "lblHiSlots"
        Me.lblHiSlots.Size = New System.Drawing.Size(67, 13)
        Me.lblHiSlots.TabIndex = 8
        Me.lblHiSlots.Text = "High Slots: 0"
        '
        'lblMedSlots
        '
        Me.lblMedSlots.AutoSize = True
        Me.lblMedSlots.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMedSlots.Location = New System.Drawing.Point(44, 61)
        Me.lblMedSlots.Name = "lblMedSlots"
        Me.lblMedSlots.Size = New System.Drawing.Size(66, 13)
        Me.lblMedSlots.TabIndex = 7
        Me.lblMedSlots.Text = "Med Slots: 0"
        '
        'lblLowSlots
        '
        Me.lblLowSlots.AutoSize = True
        Me.lblLowSlots.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLowSlots.Location = New System.Drawing.Point(44, 29)
        Me.lblLowSlots.Name = "lblLowSlots"
        Me.lblLowSlots.Size = New System.Drawing.Size(65, 13)
        Me.lblLowSlots.TabIndex = 6
        Me.lblLowSlots.Text = "Low Slots: 0"
        '
        'pbHiSlot
        '
        Me.pbHiSlot.Image = CType(resources.GetObject("pbHiSlot.Image"), System.Drawing.Image)
        Me.pbHiSlot.Location = New System.Drawing.Point(6, 84)
        Me.pbHiSlot.Name = "pbHiSlot"
        Me.pbHiSlot.Size = New System.Drawing.Size(32, 32)
        Me.pbHiSlot.TabIndex = 3
        Me.pbHiSlot.TabStop = False
        '
        'pbMedSlot
        '
        Me.pbMedSlot.Image = CType(resources.GetObject("pbMedSlot.Image"), System.Drawing.Image)
        Me.pbMedSlot.Location = New System.Drawing.Point(6, 52)
        Me.pbMedSlot.Name = "pbMedSlot"
        Me.pbMedSlot.Size = New System.Drawing.Size(32, 32)
        Me.pbMedSlot.TabIndex = 2
        Me.pbMedSlot.TabStop = False
        '
        'pbLowSlot
        '
        Me.pbLowSlot.Image = CType(resources.GetObject("pbLowSlot.Image"), System.Drawing.Image)
        Me.pbLowSlot.Location = New System.Drawing.Point(6, 20)
        Me.pbLowSlot.Name = "pbLowSlot"
        Me.pbLowSlot.Size = New System.Drawing.Size(32, 32)
        Me.pbLowSlot.TabIndex = 1
        Me.pbLowSlot.TabStop = False
        '
        'gbFitting
        '
        Me.gbFitting.Controls.Add(Me.lblCalibration)
        Me.gbFitting.Controls.Add(Me.lblPG)
        Me.gbFitting.Controls.Add(Me.lblCPU)
        Me.gbFitting.Controls.Add(Me.pbCalibration)
        Me.gbFitting.Controls.Add(Me.pbPG)
        Me.gbFitting.Controls.Add(Me.pbCPU)
        Me.gbFitting.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbFitting.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbFitting.Location = New System.Drawing.Point(260, 3)
        Me.gbFitting.Name = "gbFitting"
        Me.gbFitting.Size = New System.Drawing.Size(147, 126)
        Me.gbFitting.TabIndex = 15
        Me.gbFitting.TabStop = False
        Me.gbFitting.Text = "Fitting"
        '
        'lblCalibration
        '
        Me.lblCalibration.AutoSize = True
        Me.lblCalibration.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCalibration.Location = New System.Drawing.Point(44, 89)
        Me.lblCalibration.Name = "lblCalibration"
        Me.lblCalibration.Size = New System.Drawing.Size(71, 13)
        Me.lblCalibration.TabIndex = 8
        Me.lblCalibration.Text = "Calibration: 0"
        '
        'lblPG
        '
        Me.lblPG.AutoSize = True
        Me.lblPG.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPG.Location = New System.Drawing.Point(44, 57)
        Me.lblPG.Name = "lblPG"
        Me.lblPG.Size = New System.Drawing.Size(68, 13)
        Me.lblPG.TabIndex = 7
        Me.lblPG.Text = "Powergrid: 0"
        '
        'lblCPU
        '
        Me.lblCPU.AutoSize = True
        Me.lblCPU.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCPU.Location = New System.Drawing.Point(44, 29)
        Me.lblCPU.Name = "lblCPU"
        Me.lblCPU.Size = New System.Drawing.Size(40, 13)
        Me.lblCPU.TabIndex = 6
        Me.lblCPU.Text = "CPU: 0"
        '
        'pbCalibration
        '
        Me.pbCalibration.Image = CType(resources.GetObject("pbCalibration.Image"), System.Drawing.Image)
        Me.pbCalibration.Location = New System.Drawing.Point(6, 80)
        Me.pbCalibration.Name = "pbCalibration"
        Me.pbCalibration.Size = New System.Drawing.Size(32, 32)
        Me.pbCalibration.TabIndex = 3
        Me.pbCalibration.TabStop = False
        '
        'pbPG
        '
        Me.pbPG.Image = CType(resources.GetObject("pbPG.Image"), System.Drawing.Image)
        Me.pbPG.Location = New System.Drawing.Point(6, 51)
        Me.pbPG.Name = "pbPG"
        Me.pbPG.Size = New System.Drawing.Size(32, 32)
        Me.pbPG.TabIndex = 2
        Me.pbPG.TabStop = False
        '
        'pbCPU
        '
        Me.pbCPU.Image = CType(resources.GetObject("pbCPU.Image"), System.Drawing.Image)
        Me.pbCPU.Location = New System.Drawing.Point(6, 20)
        Me.pbCPU.Name = "pbCPU"
        Me.pbCPU.Size = New System.Drawing.Size(32, 32)
        Me.pbCPU.TabIndex = 1
        Me.pbCPU.TabStop = False
        '
        'gbShield
        '
        Me.gbShield.Controls.Add(Me.lblShieldRecharge)
        Me.gbShield.Controls.Add(Me.lblShieldHP)
        Me.gbShield.Controls.Add(Me.lblShieldKi)
        Me.gbShield.Controls.Add(Me.lblShieldTh)
        Me.gbShield.Controls.Add(Me.lblShieldEx)
        Me.gbShield.Controls.Add(Me.lblShieldEM)
        Me.gbShield.Controls.Add(Me.pbShieldTh)
        Me.gbShield.Controls.Add(Me.pbShieldKi)
        Me.gbShield.Controls.Add(Me.pbShieldEx)
        Me.gbShield.Controls.Add(Me.pbShieldEM)
        Me.gbShield.Controls.Add(Me.pbShield)
        Me.gbShield.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbShield.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbShield.Location = New System.Drawing.Point(3, 135)
        Me.gbShield.Name = "gbShield"
        Me.gbShield.Size = New System.Drawing.Size(191, 113)
        Me.gbShield.TabIndex = 9
        Me.gbShield.TabStop = False
        Me.gbShield.Text = "Shield"
        '
        'lblShieldRecharge
        '
        Me.lblShieldRecharge.AutoSize = True
        Me.lblShieldRecharge.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShieldRecharge.Location = New System.Drawing.Point(7, 88)
        Me.lblShieldRecharge.Name = "lblShieldRecharge"
        Me.lblShieldRecharge.Size = New System.Drawing.Size(97, 13)
        Me.lblShieldRecharge.TabIndex = 10
        Me.lblShieldRecharge.Text = "Recharge Rate: 0s"
        '
        'lblShieldHP
        '
        Me.lblShieldHP.AutoSize = True
        Me.lblShieldHP.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShieldHP.Location = New System.Drawing.Point(7, 70)
        Me.lblShieldHP.Name = "lblShieldHP"
        Me.lblShieldHP.Size = New System.Drawing.Size(28, 13)
        Me.lblShieldHP.TabIndex = 9
        Me.lblShieldHP.Text = "0 hp"
        '
        'lblShieldKi
        '
        Me.lblShieldKi.AutoSize = True
        Me.lblShieldKi.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShieldKi.Location = New System.Drawing.Point(146, 25)
        Me.lblShieldKi.Name = "lblShieldKi"
        Me.lblShieldKi.Size = New System.Drawing.Size(24, 13)
        Me.lblShieldKi.TabIndex = 8
        Me.lblShieldKi.Text = "0%"
        '
        'lblShieldTh
        '
        Me.lblShieldTh.AutoSize = True
        Me.lblShieldTh.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShieldTh.Location = New System.Drawing.Point(146, 57)
        Me.lblShieldTh.Name = "lblShieldTh"
        Me.lblShieldTh.Size = New System.Drawing.Size(24, 13)
        Me.lblShieldTh.TabIndex = 7
        Me.lblShieldTh.Text = "0%"
        '
        'lblShieldEx
        '
        Me.lblShieldEx.AutoSize = True
        Me.lblShieldEx.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShieldEx.Location = New System.Drawing.Point(80, 57)
        Me.lblShieldEx.Name = "lblShieldEx"
        Me.lblShieldEx.Size = New System.Drawing.Size(24, 13)
        Me.lblShieldEx.TabIndex = 6
        Me.lblShieldEx.Text = "0%"
        '
        'lblShieldEM
        '
        Me.lblShieldEM.AutoSize = True
        Me.lblShieldEM.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShieldEM.Location = New System.Drawing.Point(80, 25)
        Me.lblShieldEM.Name = "lblShieldEM"
        Me.lblShieldEM.Size = New System.Drawing.Size(24, 13)
        Me.lblShieldEM.TabIndex = 5
        Me.lblShieldEM.Text = "0%"
        '
        'pbShieldTh
        '
        Me.pbShieldTh.Image = CType(resources.GetObject("pbShieldTh.Image"), System.Drawing.Image)
        Me.pbShieldTh.Location = New System.Drawing.Point(117, 48)
        Me.pbShieldTh.Name = "pbShieldTh"
        Me.pbShieldTh.Size = New System.Drawing.Size(32, 32)
        Me.pbShieldTh.TabIndex = 4
        Me.pbShieldTh.TabStop = False
        '
        'pbShieldKi
        '
        Me.pbShieldKi.Image = CType(resources.GetObject("pbShieldKi.Image"), System.Drawing.Image)
        Me.pbShieldKi.Location = New System.Drawing.Point(117, 19)
        Me.pbShieldKi.Name = "pbShieldKi"
        Me.pbShieldKi.Size = New System.Drawing.Size(32, 32)
        Me.pbShieldKi.TabIndex = 3
        Me.pbShieldKi.TabStop = False
        '
        'pbShieldEx
        '
        Me.pbShieldEx.Image = CType(resources.GetObject("pbShieldEx.Image"), System.Drawing.Image)
        Me.pbShieldEx.Location = New System.Drawing.Point(53, 48)
        Me.pbShieldEx.Name = "pbShieldEx"
        Me.pbShieldEx.Size = New System.Drawing.Size(32, 32)
        Me.pbShieldEx.TabIndex = 2
        Me.pbShieldEx.TabStop = False
        '
        'pbShieldEM
        '
        Me.pbShieldEM.Image = CType(resources.GetObject("pbShieldEM.Image"), System.Drawing.Image)
        Me.pbShieldEM.Location = New System.Drawing.Point(53, 19)
        Me.pbShieldEM.Name = "pbShieldEM"
        Me.pbShieldEM.Size = New System.Drawing.Size(32, 32)
        Me.pbShieldEM.TabIndex = 1
        Me.pbShieldEM.TabStop = False
        '
        'pbShield
        '
        Me.pbShield.Image = CType(resources.GetObject("pbShield.Image"), System.Drawing.Image)
        Me.pbShield.Location = New System.Drawing.Point(6, 19)
        Me.pbShield.Name = "pbShield"
        Me.pbShield.Size = New System.Drawing.Size(48, 48)
        Me.pbShield.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbShield.TabIndex = 0
        Me.pbShield.TabStop = False
        '
        'gbArmor
        '
        Me.gbArmor.Controls.Add(Me.lblArmorHP)
        Me.gbArmor.Controls.Add(Me.lblArmorKi)
        Me.gbArmor.Controls.Add(Me.lblArmorTh)
        Me.gbArmor.Controls.Add(Me.lblArmorEx)
        Me.gbArmor.Controls.Add(Me.lblArmorEM)
        Me.gbArmor.Controls.Add(Me.pbArmorTh)
        Me.gbArmor.Controls.Add(Me.pbArmorKi)
        Me.gbArmor.Controls.Add(Me.pbArmorEx)
        Me.gbArmor.Controls.Add(Me.pbArmorEM)
        Me.gbArmor.Controls.Add(Me.pbArmor)
        Me.gbArmor.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbArmor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbArmor.Location = New System.Drawing.Point(200, 135)
        Me.gbArmor.Name = "gbArmor"
        Me.gbArmor.Size = New System.Drawing.Size(191, 96)
        Me.gbArmor.TabIndex = 10
        Me.gbArmor.TabStop = False
        Me.gbArmor.Text = "Armor"
        '
        'lblArmorHP
        '
        Me.lblArmorHP.AutoSize = True
        Me.lblArmorHP.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArmorHP.Location = New System.Drawing.Point(7, 70)
        Me.lblArmorHP.Name = "lblArmorHP"
        Me.lblArmorHP.Size = New System.Drawing.Size(28, 13)
        Me.lblArmorHP.TabIndex = 9
        Me.lblArmorHP.Text = "0 hp"
        '
        'lblArmorKi
        '
        Me.lblArmorKi.AutoSize = True
        Me.lblArmorKi.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArmorKi.Location = New System.Drawing.Point(146, 29)
        Me.lblArmorKi.Name = "lblArmorKi"
        Me.lblArmorKi.Size = New System.Drawing.Size(24, 13)
        Me.lblArmorKi.TabIndex = 8
        Me.lblArmorKi.Text = "0%"
        '
        'lblArmorTh
        '
        Me.lblArmorTh.AutoSize = True
        Me.lblArmorTh.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArmorTh.Location = New System.Drawing.Point(146, 54)
        Me.lblArmorTh.Name = "lblArmorTh"
        Me.lblArmorTh.Size = New System.Drawing.Size(24, 13)
        Me.lblArmorTh.TabIndex = 7
        Me.lblArmorTh.Text = "0%"
        '
        'lblArmorEx
        '
        Me.lblArmorEx.AutoSize = True
        Me.lblArmorEx.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArmorEx.Location = New System.Drawing.Point(80, 54)
        Me.lblArmorEx.Name = "lblArmorEx"
        Me.lblArmorEx.Size = New System.Drawing.Size(24, 13)
        Me.lblArmorEx.TabIndex = 6
        Me.lblArmorEx.Text = "0%"
        '
        'lblArmorEM
        '
        Me.lblArmorEM.AutoSize = True
        Me.lblArmorEM.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblArmorEM.Location = New System.Drawing.Point(80, 29)
        Me.lblArmorEM.Name = "lblArmorEM"
        Me.lblArmorEM.Size = New System.Drawing.Size(24, 13)
        Me.lblArmorEM.TabIndex = 5
        Me.lblArmorEM.Text = "0%"
        '
        'pbArmorTh
        '
        Me.pbArmorTh.Image = CType(resources.GetObject("pbArmorTh.Image"), System.Drawing.Image)
        Me.pbArmorTh.Location = New System.Drawing.Point(117, 48)
        Me.pbArmorTh.Name = "pbArmorTh"
        Me.pbArmorTh.Size = New System.Drawing.Size(32, 32)
        Me.pbArmorTh.TabIndex = 4
        Me.pbArmorTh.TabStop = False
        '
        'pbArmorKi
        '
        Me.pbArmorKi.Image = CType(resources.GetObject("pbArmorKi.Image"), System.Drawing.Image)
        Me.pbArmorKi.Location = New System.Drawing.Point(117, 19)
        Me.pbArmorKi.Name = "pbArmorKi"
        Me.pbArmorKi.Size = New System.Drawing.Size(32, 32)
        Me.pbArmorKi.TabIndex = 3
        Me.pbArmorKi.TabStop = False
        '
        'pbArmorEx
        '
        Me.pbArmorEx.Image = CType(resources.GetObject("pbArmorEx.Image"), System.Drawing.Image)
        Me.pbArmorEx.Location = New System.Drawing.Point(53, 48)
        Me.pbArmorEx.Name = "pbArmorEx"
        Me.pbArmorEx.Size = New System.Drawing.Size(32, 32)
        Me.pbArmorEx.TabIndex = 2
        Me.pbArmorEx.TabStop = False
        '
        'pbArmorEM
        '
        Me.pbArmorEM.Image = CType(resources.GetObject("pbArmorEM.Image"), System.Drawing.Image)
        Me.pbArmorEM.Location = New System.Drawing.Point(53, 17)
        Me.pbArmorEM.Name = "pbArmorEM"
        Me.pbArmorEM.Size = New System.Drawing.Size(32, 32)
        Me.pbArmorEM.TabIndex = 1
        Me.pbArmorEM.TabStop = False
        '
        'pbArmor
        '
        Me.pbArmor.Image = CType(resources.GetObject("pbArmor.Image"), System.Drawing.Image)
        Me.pbArmor.Location = New System.Drawing.Point(6, 19)
        Me.pbArmor.Name = "pbArmor"
        Me.pbArmor.Size = New System.Drawing.Size(48, 48)
        Me.pbArmor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbArmor.TabIndex = 0
        Me.pbArmor.TabStop = False
        '
        'gbStructure
        '
        Me.gbStructure.Controls.Add(Me.lblStructureHP)
        Me.gbStructure.Controls.Add(Me.lblStructureKi)
        Me.gbStructure.Controls.Add(Me.lblStructureTh)
        Me.gbStructure.Controls.Add(Me.lblStructureEx)
        Me.gbStructure.Controls.Add(Me.lblStructureEM)
        Me.gbStructure.Controls.Add(Me.pbStructureTh)
        Me.gbStructure.Controls.Add(Me.pbStructureKi)
        Me.gbStructure.Controls.Add(Me.pbStructureEx)
        Me.gbStructure.Controls.Add(Me.pbStructureEM)
        Me.gbStructure.Controls.Add(Me.pbStructure)
        Me.gbStructure.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbStructure.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbStructure.Location = New System.Drawing.Point(3, 254)
        Me.gbStructure.Name = "gbStructure"
        Me.gbStructure.Size = New System.Drawing.Size(191, 96)
        Me.gbStructure.TabIndex = 11
        Me.gbStructure.TabStop = False
        Me.gbStructure.Text = "Structure"
        '
        'lblStructureHP
        '
        Me.lblStructureHP.AutoSize = True
        Me.lblStructureHP.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStructureHP.Location = New System.Drawing.Point(7, 70)
        Me.lblStructureHP.Name = "lblStructureHP"
        Me.lblStructureHP.Size = New System.Drawing.Size(28, 13)
        Me.lblStructureHP.TabIndex = 9
        Me.lblStructureHP.Text = "0 hp"
        '
        'lblStructureKi
        '
        Me.lblStructureKi.AutoSize = True
        Me.lblStructureKi.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStructureKi.Location = New System.Drawing.Point(146, 29)
        Me.lblStructureKi.Name = "lblStructureKi"
        Me.lblStructureKi.Size = New System.Drawing.Size(24, 13)
        Me.lblStructureKi.TabIndex = 8
        Me.lblStructureKi.Text = "0%"
        '
        'lblStructureTh
        '
        Me.lblStructureTh.AutoSize = True
        Me.lblStructureTh.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStructureTh.Location = New System.Drawing.Point(146, 57)
        Me.lblStructureTh.Name = "lblStructureTh"
        Me.lblStructureTh.Size = New System.Drawing.Size(24, 13)
        Me.lblStructureTh.TabIndex = 7
        Me.lblStructureTh.Text = "0%"
        '
        'lblStructureEx
        '
        Me.lblStructureEx.AutoSize = True
        Me.lblStructureEx.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStructureEx.Location = New System.Drawing.Point(80, 57)
        Me.lblStructureEx.Name = "lblStructureEx"
        Me.lblStructureEx.Size = New System.Drawing.Size(24, 13)
        Me.lblStructureEx.TabIndex = 6
        Me.lblStructureEx.Text = "0%"
        '
        'lblStructureEM
        '
        Me.lblStructureEM.AutoSize = True
        Me.lblStructureEM.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStructureEM.Location = New System.Drawing.Point(80, 29)
        Me.lblStructureEM.Name = "lblStructureEM"
        Me.lblStructureEM.Size = New System.Drawing.Size(24, 13)
        Me.lblStructureEM.TabIndex = 5
        Me.lblStructureEM.Text = "0%"
        '
        'pbStructureTh
        '
        Me.pbStructureTh.Image = CType(resources.GetObject("pbStructureTh.Image"), System.Drawing.Image)
        Me.pbStructureTh.Location = New System.Drawing.Point(117, 51)
        Me.pbStructureTh.Name = "pbStructureTh"
        Me.pbStructureTh.Size = New System.Drawing.Size(32, 32)
        Me.pbStructureTh.TabIndex = 4
        Me.pbStructureTh.TabStop = False
        '
        'pbStructureKi
        '
        Me.pbStructureKi.Image = CType(resources.GetObject("pbStructureKi.Image"), System.Drawing.Image)
        Me.pbStructureKi.Location = New System.Drawing.Point(117, 20)
        Me.pbStructureKi.Name = "pbStructureKi"
        Me.pbStructureKi.Size = New System.Drawing.Size(32, 32)
        Me.pbStructureKi.TabIndex = 3
        Me.pbStructureKi.TabStop = False
        '
        'pbStructureEx
        '
        Me.pbStructureEx.Image = CType(resources.GetObject("pbStructureEx.Image"), System.Drawing.Image)
        Me.pbStructureEx.Location = New System.Drawing.Point(53, 51)
        Me.pbStructureEx.Name = "pbStructureEx"
        Me.pbStructureEx.Size = New System.Drawing.Size(32, 32)
        Me.pbStructureEx.TabIndex = 2
        Me.pbStructureEx.TabStop = False
        '
        'pbStructureEM
        '
        Me.pbStructureEM.Image = CType(resources.GetObject("pbStructureEM.Image"), System.Drawing.Image)
        Me.pbStructureEM.Location = New System.Drawing.Point(53, 19)
        Me.pbStructureEM.Name = "pbStructureEM"
        Me.pbStructureEM.Size = New System.Drawing.Size(32, 32)
        Me.pbStructureEM.TabIndex = 1
        Me.pbStructureEM.TabStop = False
        '
        'pbStructure
        '
        Me.pbStructure.Image = CType(resources.GetObject("pbStructure.Image"), System.Drawing.Image)
        Me.pbStructure.Location = New System.Drawing.Point(6, 19)
        Me.pbStructure.Name = "pbStructure"
        Me.pbStructure.Size = New System.Drawing.Size(48, 48)
        Me.pbStructure.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbStructure.TabIndex = 0
        Me.pbStructure.TabStop = False
        '
        'gbStorage
        '
        Me.gbStorage.Controls.Add(Me.lblDroneBandwidth)
        Me.gbStorage.Controls.Add(Me.lblDroneBay)
        Me.gbStorage.Controls.Add(Me.lblCargohold)
        Me.gbStorage.Controls.Add(Me.pbStorage)
        Me.gbStorage.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbStorage.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbStorage.Location = New System.Drawing.Point(200, 254)
        Me.gbStorage.Name = "gbStorage"
        Me.gbStorage.Size = New System.Drawing.Size(194, 75)
        Me.gbStorage.TabIndex = 14
        Me.gbStorage.TabStop = False
        Me.gbStorage.Text = "Storage"
        '
        'lblDroneBandwidth
        '
        Me.lblDroneBandwidth.AutoSize = True
        Me.lblDroneBandwidth.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDroneBandwidth.Location = New System.Drawing.Point(57, 54)
        Me.lblDroneBandwidth.Name = "lblDroneBandwidth"
        Me.lblDroneBandwidth.Size = New System.Drawing.Size(96, 13)
        Me.lblDroneBandwidth.TabIndex = 7
        Me.lblDroneBandwidth.Text = "Bandwidth: 0 Mb/s"
        '
        'lblDroneBay
        '
        Me.lblDroneBay.AutoSize = True
        Me.lblDroneBay.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDroneBay.Location = New System.Drawing.Point(57, 36)
        Me.lblDroneBay.Name = "lblDroneBay"
        Me.lblDroneBay.Size = New System.Drawing.Size(87, 13)
        Me.lblDroneBay.TabIndex = 6
        Me.lblDroneBay.Text = "Drone Bay: 0 m3"
        '
        'lblCargohold
        '
        Me.lblCargohold.AutoSize = True
        Me.lblCargohold.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCargohold.Location = New System.Drawing.Point(57, 18)
        Me.lblCargohold.Name = "lblCargohold"
        Me.lblCargohold.Size = New System.Drawing.Size(66, 13)
        Me.lblCargohold.TabIndex = 5
        Me.lblCargohold.Text = "Cargo: 0 m3"
        '
        'pbStorage
        '
        Me.pbStorage.Image = CType(resources.GetObject("pbStorage.Image"), System.Drawing.Image)
        Me.pbStorage.Location = New System.Drawing.Point(6, 19)
        Me.pbStorage.Name = "pbStorage"
        Me.pbStorage.Size = New System.Drawing.Size(48, 48)
        Me.pbStorage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbStorage.TabIndex = 0
        Me.pbStorage.TabStop = False
        '
        'gbCapacitor
        '
        Me.gbCapacitor.Controls.Add(Me.lblCapRecharge)
        Me.gbCapacitor.Controls.Add(Me.lblCapacitor)
        Me.gbCapacitor.Controls.Add(Me.pbCapacitor)
        Me.gbCapacitor.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbCapacitor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbCapacitor.Location = New System.Drawing.Point(3, 356)
        Me.gbCapacitor.Name = "gbCapacitor"
        Me.gbCapacitor.Size = New System.Drawing.Size(191, 75)
        Me.gbCapacitor.TabIndex = 12
        Me.gbCapacitor.TabStop = False
        Me.gbCapacitor.Text = "Capacitor"
        '
        'lblCapRecharge
        '
        Me.lblCapRecharge.AutoSize = True
        Me.lblCapRecharge.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCapRecharge.Location = New System.Drawing.Point(60, 45)
        Me.lblCapRecharge.Name = "lblCapRecharge"
        Me.lblCapRecharge.Size = New System.Drawing.Size(97, 13)
        Me.lblCapRecharge.TabIndex = 6
        Me.lblCapRecharge.Text = "Recharge Rate: 0s"
        '
        'lblCapacitor
        '
        Me.lblCapacitor.AutoSize = True
        Me.lblCapacitor.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCapacitor.Location = New System.Drawing.Point(60, 25)
        Me.lblCapacitor.Name = "lblCapacitor"
        Me.lblCapacitor.Size = New System.Drawing.Size(66, 13)
        Me.lblCapacitor.TabIndex = 5
        Me.lblCapacitor.Text = "Capacitor: 0"
        '
        'pbCapacitor
        '
        Me.pbCapacitor.Image = CType(resources.GetObject("pbCapacitor.Image"), System.Drawing.Image)
        Me.pbCapacitor.Location = New System.Drawing.Point(6, 19)
        Me.pbCapacitor.Name = "pbCapacitor"
        Me.pbCapacitor.Size = New System.Drawing.Size(48, 48)
        Me.pbCapacitor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbCapacitor.TabIndex = 0
        Me.pbCapacitor.TabStop = False
        '
        'gbSpeed
        '
        Me.gbSpeed.Controls.Add(Me.lblWarpSpeed)
        Me.gbSpeed.Controls.Add(Me.lblInertia)
        Me.gbSpeed.Controls.Add(Me.lblSpeed)
        Me.gbSpeed.Controls.Add(Me.pbSpeed)
        Me.gbSpeed.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.gbSpeed.ForeColor = System.Drawing.SystemColors.ControlText
        Me.gbSpeed.Location = New System.Drawing.Point(200, 356)
        Me.gbSpeed.Name = "gbSpeed"
        Me.gbSpeed.Size = New System.Drawing.Size(222, 75)
        Me.gbSpeed.TabIndex = 13
        Me.gbSpeed.TabStop = False
        Me.gbSpeed.Text = "Speed && Inertia"
        '
        'lblWarpSpeed
        '
        Me.lblWarpSpeed.AutoSize = True
        Me.lblWarpSpeed.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWarpSpeed.Location = New System.Drawing.Point(60, 54)
        Me.lblWarpSpeed.Name = "lblWarpSpeed"
        Me.lblWarpSpeed.Size = New System.Drawing.Size(79, 13)
        Me.lblWarpSpeed.TabIndex = 7
        Me.lblWarpSpeed.Text = "Warp Speed: 0"
        '
        'lblInertia
        '
        Me.lblInertia.AutoSize = True
        Me.lblInertia.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInertia.Location = New System.Drawing.Point(60, 36)
        Me.lblInertia.Name = "lblInertia"
        Me.lblInertia.Size = New System.Drawing.Size(52, 13)
        Me.lblInertia.TabIndex = 6
        Me.lblInertia.Text = "Inertia: 0"
        '
        'lblSpeed
        '
        Me.lblSpeed.AutoSize = True
        Me.lblSpeed.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSpeed.Location = New System.Drawing.Point(60, 19)
        Me.lblSpeed.Name = "lblSpeed"
        Me.lblSpeed.Size = New System.Drawing.Size(100, 13)
        Me.lblSpeed.TabIndex = 5
        Me.lblSpeed.Text = "Max Velocity: 0 m/s"
        '
        'pbSpeed
        '
        Me.pbSpeed.Image = CType(resources.GetObject("pbSpeed.Image"), System.Drawing.Image)
        Me.pbSpeed.Location = New System.Drawing.Point(6, 19)
        Me.pbSpeed.Name = "pbSpeed"
        Me.pbSpeed.Size = New System.Drawing.Size(48, 48)
        Me.pbSpeed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.pbSpeed.TabIndex = 0
        Me.pbSpeed.TabStop = False
        '
        'pbShip
        '
        Me.pbShip.Location = New System.Drawing.Point(0, 40)
        Me.pbShip.Name = "pbShip"
        Me.pbShip.Size = New System.Drawing.Size(128, 128)
        Me.pbShip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbShip.TabIndex = 0
        Me.pbShip.TabStop = False
        '
        'txtShipDescription
        '
        Me.txtShipDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtShipDescription.BackColor = System.Drawing.SystemColors.Control
        Me.txtShipDescription.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtShipDescription.Cursor = System.Windows.Forms.Cursors.Default
        Me.txtShipDescription.Location = New System.Drawing.Point(134, 40)
        Me.txtShipDescription.Name = "txtShipDescription"
        Me.txtShipDescription.ReadOnly = True
        Me.txtShipDescription.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical
        Me.txtShipDescription.Size = New System.Drawing.Size(451, 128)
        Me.txtShipDescription.TabIndex = 17
        Me.txtShipDescription.Text = "Ship Description"
        '
        'tabFit
        '
        Me.tabFit.Controls.Add(Me.panelShipSlot)
        Me.tabFit.Controls.Add(Me.panelShipInfo)
        Me.tabFit.Location = New System.Drawing.Point(4, 22)
        Me.tabFit.Name = "tabFit"
        Me.tabFit.Size = New System.Drawing.Size(594, 670)
        Me.tabFit.TabIndex = 2
        Me.tabFit.Text = "Fitting"
        Me.tabFit.UseVisualStyleBackColor = True
        '
        'panelShipSlot
        '
        Me.panelShipSlot.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelShipSlot.Location = New System.Drawing.Point(0, 0)
        Me.panelShipSlot.Name = "panelShipSlot"
        Me.panelShipSlot.Size = New System.Drawing.Size(344, 670)
        Me.panelShipSlot.TabIndex = 1
        '
        'panelShipInfo
        '
        Me.panelShipInfo.Dock = System.Windows.Forms.DockStyle.Right
        Me.panelShipInfo.Location = New System.Drawing.Point(344, 0)
        Me.panelShipInfo.Name = "panelShipInfo"
        Me.panelShipInfo.Size = New System.Drawing.Size(250, 670)
        Me.panelShipInfo.TabIndex = 0
        '
        'Splitter1
        '
        Me.Splitter1.Location = New System.Drawing.Point(200, 25)
        Me.Splitter1.Name = "Splitter1"
        Me.Splitter1.Size = New System.Drawing.Size(3, 696)
        Me.Splitter1.TabIndex = 7
        Me.Splitter1.TabStop = False
        '
        'Splitter2
        '
        Me.Splitter2.Dock = System.Windows.Forms.DockStyle.Right
        Me.Splitter2.Location = New System.Drawing.Point(805, 25)
        Me.Splitter2.Name = "Splitter2"
        Me.Splitter2.Size = New System.Drawing.Size(3, 696)
        Me.Splitter2.TabIndex = 8
        Me.Splitter2.TabStop = False
        '
        'frmHQF
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1099, 721)
        Me.Controls.Add(Me.tabHQF)
        Me.Controls.Add(Me.Splitter2)
        Me.Controls.Add(Me.Splitter1)
        Me.Controls.Add(Me.SplitContainerMod)
        Me.Controls.Add(Me.SplitContainerShip)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmHQF"
        Me.Text = "EveHQ Fitter"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.SplitContainerShip.Panel1.ResumeLayout(False)
        Me.SplitContainerShip.Panel2.ResumeLayout(False)
        Me.SplitContainerShip.Panel2.PerformLayout()
        Me.SplitContainerShip.ResumeLayout(False)
        Me.ctxFittings.ResumeLayout(False)
        CType(Me.pbSearchShips, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ctxShipBrowser.ResumeLayout(False)
        Me.SplitContainerMod.Panel1.ResumeLayout(False)
        Me.SplitContainerMod.Panel1.PerformLayout()
        Me.SplitContainerMod.Panel2.ResumeLayout(False)
        Me.SplitContainerMod.Panel2.PerformLayout()
        Me.SplitContainerMod.ResumeLayout(False)
        Me.ctxModuleList.ResumeLayout(False)
        Me.ctxTabHQF.ResumeLayout(False)
        Me.tabHQF.ResumeLayout(False)
        Me.tabShipPreview.ResumeLayout(False)
        Me.panelPreview.ResumeLayout(False)
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.pbLauncherSlot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbTurretSlot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbRigSlot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbHiSlot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbMedSlot, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbLowSlot, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbFitting.ResumeLayout(False)
        Me.gbFitting.PerformLayout()
        CType(Me.pbCalibration, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbPG, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCPU, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbShield.ResumeLayout(False)
        Me.gbShield.PerformLayout()
        CType(Me.pbShieldTh, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbShieldKi, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbShieldEx, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbShieldEM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbShield, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbArmor.ResumeLayout(False)
        Me.gbArmor.PerformLayout()
        CType(Me.pbArmorTh, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbArmorKi, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbArmorEx, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbArmorEM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbArmor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbStructure.ResumeLayout(False)
        Me.gbStructure.PerformLayout()
        CType(Me.pbStructureTh, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbStructureKi, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbStructureEx, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbStructureEM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbStructure, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbStorage.ResumeLayout(False)
        Me.gbStorage.PerformLayout()
        CType(Me.pbStorage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbCapacitor.ResumeLayout(False)
        Me.gbCapacitor.PerformLayout()
        CType(Me.pbCapacitor, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbSpeed.ResumeLayout(False)
        Me.gbSpeed.PerformLayout()
        CType(Me.pbSpeed, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbShip, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tabFit.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents SplitContainerShip As System.Windows.Forms.SplitContainer
    Friend WithEvents SplitContainerMod As System.Windows.Forms.SplitContainer
    Friend WithEvents tvwShips As System.Windows.Forms.TreeView
    Friend WithEvents tsbOptions As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnShipPanel As System.Windows.Forms.ToolStripButton
    Friend WithEvents btnItemPanel As System.Windows.Forms.ToolStripButton
    Friend WithEvents tvwItems As System.Windows.Forms.TreeView
    Friend WithEvents lvwItems As EveHQ.HQF.ListViewNoFlicker
    Friend WithEvents ToolStripButton3 As System.Windows.Forms.ToolStripButton
    Friend WithEvents chkFilter1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkFilter32 As System.Windows.Forms.CheckBox
    Friend WithEvents chkFilter16 As System.Windows.Forms.CheckBox
    Friend WithEvents chkFilter8 As System.Windows.Forms.CheckBox
    Friend WithEvents chkFilter4 As System.Windows.Forms.CheckBox
    Friend WithEvents chkFilter2 As System.Windows.Forms.CheckBox
    Friend WithEvents colModuleName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colModuleCPU As System.Windows.Forms.ColumnHeader
    Friend WithEvents colModulePG As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblSearchModules As System.Windows.Forms.Label
    Friend WithEvents txtSearchModules As System.Windows.Forms.TextBox
    Friend WithEvents ctxShipBrowser As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuCreateNewFitting As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuShipBrowserShipName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents imgAttributes As System.Windows.Forms.ImageList
    Friend WithEvents mnuPreviewShip As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxModuleList As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuShowModuleInfo As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ctxTabHQF As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuCloseHQFTab As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnClipboardPaste As System.Windows.Forms.ToolStripButton
    Friend WithEvents tmrClipboard As System.Windows.Forms.Timer
    Friend WithEvents ctxFittings As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents mnuFittingsShowFitting As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuFittingsRenameFitting As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFittingsCopyFitting As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFittingsDeleteFitting As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuFittingsCreateFitting As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFittingsFittingName As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuPreviewShip2 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnPilotManager As System.Windows.Forms.ToolStripButton
    Friend WithEvents colModuleMetaType As System.Windows.Forms.ColumnHeader
    Friend WithEvents chkOnlyShowUsable As System.Windows.Forms.CheckBox
    Friend WithEvents chkApplySkills As System.Windows.Forms.CheckBox
    Friend WithEvents mnuSep1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuAddToFavourites_List As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblModuleDisplayType As System.Windows.Forms.Label
    Friend WithEvents mnuRemoveFromFavourites As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSep2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuShowModuleMarketGroup As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents txtShipSearch As System.Windows.Forms.TextBox
    Friend WithEvents btnResetShips As System.Windows.Forms.Button
    Friend WithEvents cboFittings As System.Windows.Forms.ToolStripComboBox
    Friend WithEvents btnCopy As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents mnuCopyForHQF As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCopyForEFT As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuCopyForForums As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tabHQF As System.Windows.Forms.TabControl
    Friend WithEvents tabShipPreview As System.Windows.Forms.TabPage
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblLauncherSlots As System.Windows.Forms.Label
    Friend WithEvents lblTurretSlots As System.Windows.Forms.Label
    Friend WithEvents lblRigSlots As System.Windows.Forms.Label
    Friend WithEvents pbLauncherSlot As System.Windows.Forms.PictureBox
    Friend WithEvents pbTurretSlot As System.Windows.Forms.PictureBox
    Friend WithEvents pbRigSlot As System.Windows.Forms.PictureBox
    Friend WithEvents lblHiSlots As System.Windows.Forms.Label
    Friend WithEvents lblMedSlots As System.Windows.Forms.Label
    Friend WithEvents lblLowSlots As System.Windows.Forms.Label
    Friend WithEvents pbHiSlot As System.Windows.Forms.PictureBox
    Friend WithEvents pbMedSlot As System.Windows.Forms.PictureBox
    Friend WithEvents pbLowSlot As System.Windows.Forms.PictureBox
    Friend WithEvents gbShield As System.Windows.Forms.GroupBox
    Friend WithEvents lblShieldRecharge As System.Windows.Forms.Label
    Friend WithEvents lblShieldHP As System.Windows.Forms.Label
    Friend WithEvents lblShieldKi As System.Windows.Forms.Label
    Friend WithEvents lblShieldTh As System.Windows.Forms.Label
    Friend WithEvents lblShieldEx As System.Windows.Forms.Label
    Friend WithEvents lblShieldEM As System.Windows.Forms.Label
    Friend WithEvents pbShieldTh As System.Windows.Forms.PictureBox
    Friend WithEvents pbShieldKi As System.Windows.Forms.PictureBox
    Friend WithEvents pbShieldEx As System.Windows.Forms.PictureBox
    Friend WithEvents pbShieldEM As System.Windows.Forms.PictureBox
    Friend WithEvents pbShield As System.Windows.Forms.PictureBox
    Friend WithEvents gbArmor As System.Windows.Forms.GroupBox
    Friend WithEvents lblArmorHP As System.Windows.Forms.Label
    Friend WithEvents lblArmorKi As System.Windows.Forms.Label
    Friend WithEvents lblArmorTh As System.Windows.Forms.Label
    Friend WithEvents lblArmorEx As System.Windows.Forms.Label
    Friend WithEvents lblArmorEM As System.Windows.Forms.Label
    Friend WithEvents pbArmorTh As System.Windows.Forms.PictureBox
    Friend WithEvents pbArmorKi As System.Windows.Forms.PictureBox
    Friend WithEvents pbArmorEx As System.Windows.Forms.PictureBox
    Friend WithEvents pbArmorEM As System.Windows.Forms.PictureBox
    Friend WithEvents pbArmor As System.Windows.Forms.PictureBox
    Friend WithEvents gbStructure As System.Windows.Forms.GroupBox
    Friend WithEvents lblStructureHP As System.Windows.Forms.Label
    Friend WithEvents lblStructureKi As System.Windows.Forms.Label
    Friend WithEvents lblStructureTh As System.Windows.Forms.Label
    Friend WithEvents lblStructureEx As System.Windows.Forms.Label
    Friend WithEvents lblStructureEM As System.Windows.Forms.Label
    Friend WithEvents pbStructureTh As System.Windows.Forms.PictureBox
    Friend WithEvents pbStructureKi As System.Windows.Forms.PictureBox
    Friend WithEvents pbStructureEx As System.Windows.Forms.PictureBox
    Friend WithEvents pbStructureEM As System.Windows.Forms.PictureBox
    Friend WithEvents pbStructure As System.Windows.Forms.PictureBox
    Friend WithEvents gbStorage As System.Windows.Forms.GroupBox
    Friend WithEvents lblDroneBay As System.Windows.Forms.Label
    Friend WithEvents lblCargohold As System.Windows.Forms.Label
    Friend WithEvents pbStorage As System.Windows.Forms.PictureBox
    Friend WithEvents gbFitting As System.Windows.Forms.GroupBox
    Friend WithEvents lblCalibration As System.Windows.Forms.Label
    Friend WithEvents lblPG As System.Windows.Forms.Label
    Friend WithEvents lblCPU As System.Windows.Forms.Label
    Friend WithEvents pbCalibration As System.Windows.Forms.PictureBox
    Friend WithEvents pbPG As System.Windows.Forms.PictureBox
    Friend WithEvents pbCPU As System.Windows.Forms.PictureBox
    Friend WithEvents gbCapacitor As System.Windows.Forms.GroupBox
    Friend WithEvents lblCapRecharge As System.Windows.Forms.Label
    Friend WithEvents lblCapacitor As System.Windows.Forms.Label
    Friend WithEvents pbCapacitor As System.Windows.Forms.PictureBox
    Friend WithEvents gbSpeed As System.Windows.Forms.GroupBox
    Friend WithEvents lblWarpSpeed As System.Windows.Forms.Label
    Friend WithEvents lblInertia As System.Windows.Forms.Label
    Friend WithEvents lblSpeed As System.Windows.Forms.Label
    Friend WithEvents pbSpeed As System.Windows.Forms.PictureBox
    Friend WithEvents txtShipDescription As System.Windows.Forms.RichTextBox
    Friend WithEvents lblShipType As System.Windows.Forms.Label
    Friend WithEvents pbShip As System.Windows.Forms.PictureBox
    Friend WithEvents tabFit As System.Windows.Forms.TabPage
    Friend WithEvents panelShipSlot As System.Windows.Forms.Panel
    Friend WithEvents panelShipInfo As System.Windows.Forms.Panel
    Friend WithEvents Splitter1 As System.Windows.Forms.Splitter
    Friend WithEvents Splitter2 As System.Windows.Forms.Splitter
    Friend WithEvents chkOnlyShowFittable As System.Windows.Forms.CheckBox
    Friend WithEvents btnScreenshot As System.Windows.Forms.ToolStripButton
    Friend WithEvents ToolStripSeparator3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents pbSearchShips As System.Windows.Forms.PictureBox
    Friend WithEvents ToolStripSeparator5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuShipStats As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents clvFittings As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents ContainerListViewColumnHeader1 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents mnuCompareFittings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents panelPreview As System.Windows.Forms.Panel
    Friend WithEvents lblDroneBandwidth As System.Windows.Forms.Label
    Friend WithEvents mnuBattleClinicBrowser As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuFittingsBCBrowser As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents mnuExportToEve As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnImportFittings As System.Windows.Forms.ToolStripSplitButton
    Friend WithEvents mnuImportEveFittings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuImportEFTFittings As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents lblFlyable As System.Windows.Forms.Label
    Friend WithEvents cboFlyable As System.Windows.Forms.ComboBox
    Friend WithEvents chkFilter8192 As System.Windows.Forms.CheckBox
End Class
