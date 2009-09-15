<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmFleetManager
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmFleetManager))
        Me.clvFleetStructure = New DotNetLib.Windows.Forms.ContainerListView
        Me.colFleetStructure = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colFleetBooster = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colFleetEHP = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colDPS = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colTank = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colShieldHP = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colArmorHP = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colVelocity = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colCap = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ctxFleetStructure = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.clvFleetList = New DotNetLib.Windows.Forms.ContainerListView
        Me.colFleetList = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.btnNewFleet = New System.Windows.Forms.Button
        Me.lblViewingFleet = New System.Windows.Forms.Label
        Me.colPilot = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colFitting = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.btnSaveFleet = New System.Windows.Forms.Button
        Me.btnClearFleet = New System.Windows.Forms.Button
        Me.btnLoadFleet = New System.Windows.Forms.Button
        Me.btnAddPilot = New System.Windows.Forms.Button
        Me.btnEditPilot = New System.Windows.Forms.Button
        Me.btnDeletePilot = New System.Windows.Forms.Button
        Me.btnUpdateFleet = New System.Windows.Forms.Button
        Me.lblWHClass = New System.Windows.Forms.Label
        Me.cboWHClass = New System.Windows.Forms.ComboBox
        Me.lblWHEffect = New System.Windows.Forms.Label
        Me.cboWHEffect = New System.Windows.Forms.ComboBox
        Me.ContainerListViewColumnHeader1 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader2 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader3 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.ContainerListViewColumnHeader4 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.btnShipAudit = New System.Windows.Forms.Button
        Me.clvPilots = New DotNetLib.Windows.Forms.ContainerListView
        Me.colRemotePilot = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colPilotFitting = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colRemoteModGroup = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colRemoteMod = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colRemoteAssign = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.btnClearAssignments = New System.Windows.Forms.Button
        Me.tabFM = New System.Windows.Forms.TabControl
        Me.tabFleetSettings = New System.Windows.Forms.TabPage
        Me.panelFleetSettings = New System.Windows.Forms.Panel
        Me.gbFleetSettings = New System.Windows.Forms.GroupBox
        Me.tabFleetStructure = New System.Windows.Forms.TabPage
        Me.panelFleetStructure = New System.Windows.Forms.Panel
        Me.cboFleet = New System.Windows.Forms.ComboBox
        Me.lblFleet = New System.Windows.Forms.Label
        Me.ctxPilotList = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RemoveAssignmnetToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.tabFM.SuspendLayout()
        Me.tabFleetSettings.SuspendLayout()
        Me.panelFleetSettings.SuspendLayout()
        Me.gbFleetSettings.SuspendLayout()
        Me.tabFleetStructure.SuspendLayout()
        Me.panelFleetStructure.SuspendLayout()
        Me.ctxPilotList.SuspendLayout()
        Me.SuspendLayout()
        '
        'clvFleetStructure
        '
        Me.clvFleetStructure.AllowDrop = True
        Me.clvFleetStructure.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clvFleetStructure.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colFleetStructure, Me.colFleetBooster, Me.colFleetEHP, Me.colDPS, Me.colTank, Me.colShieldHP, Me.colArmorHP, Me.colVelocity, Me.colCap})
        Me.clvFleetStructure.DefaultItemHeight = 20
        Me.clvFleetStructure.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.clvFleetStructure.ItemContextMenu = Me.ctxFleetStructure
        Me.clvFleetStructure.Location = New System.Drawing.Point(8, 389)
        Me.clvFleetStructure.Name = "clvFleetStructure"
        Me.clvFleetStructure.ShowPlusMinus = True
        Me.clvFleetStructure.ShowRootTreeLines = True
        Me.clvFleetStructure.ShowTreeLines = True
        Me.clvFleetStructure.Size = New System.Drawing.Size(1066, 234)
        Me.clvFleetStructure.TabIndex = 0
        '
        'colFleetStructure
        '
        Me.colFleetStructure.CustomSortTag = Nothing
        Me.colFleetStructure.Tag = Nothing
        Me.colFleetStructure.Text = "Fleet Structure"
        Me.colFleetStructure.Width = 225
        Me.colFleetStructure.WidthBehavior = DotNetLib.Windows.Forms.ColumnWidthBehavior.Fill
        '
        'colFleetBooster
        '
        Me.colFleetBooster.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colFleetBooster.CustomSortTag = Nothing
        Me.colFleetBooster.DisplayIndex = 1
        Me.colFleetBooster.Tag = Nothing
        Me.colFleetBooster.Text = "Boost"
        Me.colFleetBooster.Width = 50
        '
        'colFleetEHP
        '
        Me.colFleetEHP.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colFleetEHP.CustomSortTag = Nothing
        Me.colFleetEHP.DisplayIndex = 2
        Me.colFleetEHP.Tag = Nothing
        Me.colFleetEHP.Text = "EHP"
        Me.colFleetEHP.Width = 75
        '
        'colDPS
        '
        Me.colDPS.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colDPS.CustomSortTag = Nothing
        Me.colDPS.DisplayIndex = 3
        Me.colDPS.Tag = Nothing
        Me.colDPS.Text = "DPS"
        Me.colDPS.Width = 75
        '
        'colTank
        '
        Me.colTank.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colTank.CustomSortTag = Nothing
        Me.colTank.DisplayIndex = 4
        Me.colTank.Tag = Nothing
        Me.colTank.Text = "Tank"
        Me.colTank.Width = 75
        '
        'colShieldHP
        '
        Me.colShieldHP.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colShieldHP.CustomSortTag = Nothing
        Me.colShieldHP.DisplayIndex = 5
        Me.colShieldHP.Tag = Nothing
        Me.colShieldHP.Text = "Shield"
        Me.colShieldHP.Width = 75
        '
        'colArmorHP
        '
        Me.colArmorHP.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colArmorHP.CustomSortTag = Nothing
        Me.colArmorHP.DisplayIndex = 6
        Me.colArmorHP.Tag = Nothing
        Me.colArmorHP.Text = "Armor"
        Me.colArmorHP.Width = 75
        '
        'colVelocity
        '
        Me.colVelocity.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colVelocity.CustomSortTag = Nothing
        Me.colVelocity.DisplayIndex = 7
        Me.colVelocity.Tag = Nothing
        Me.colVelocity.Text = "Max Vel"
        Me.colVelocity.Width = 75
        '
        'colCap
        '
        Me.colCap.ContentAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.colCap.CustomSortTag = Nothing
        Me.colCap.DisplayIndex = 8
        Me.colCap.Tag = Nothing
        Me.colCap.Text = "Cap Stability"
        Me.colCap.Width = 110
        '
        'ctxFleetStructure
        '
        Me.ctxFleetStructure.Name = "ctxFleetStructure"
        Me.ctxFleetStructure.Size = New System.Drawing.Size(61, 4)
        '
        'clvFleetList
        '
        Me.clvFleetList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.clvFleetList.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colFleetList})
        Me.clvFleetList.DefaultItemHeight = 20
        Me.clvFleetList.Location = New System.Drawing.Point(3, 3)
        Me.clvFleetList.Name = "clvFleetList"
        Me.clvFleetList.Size = New System.Drawing.Size(233, 620)
        Me.clvFleetList.TabIndex = 1
        '
        'colFleetList
        '
        Me.colFleetList.CustomSortTag = Nothing
        Me.colFleetList.Tag = Nothing
        Me.colFleetList.Text = "Fleet List"
        Me.colFleetList.Width = 200
        '
        'btnNewFleet
        '
        Me.btnNewFleet.Location = New System.Drawing.Point(242, 32)
        Me.btnNewFleet.Name = "btnNewFleet"
        Me.btnNewFleet.Size = New System.Drawing.Size(75, 23)
        Me.btnNewFleet.TabIndex = 2
        Me.btnNewFleet.Text = "New Fleet"
        Me.btnNewFleet.UseVisualStyleBackColor = True
        '
        'lblViewingFleet
        '
        Me.lblViewingFleet.AutoSize = True
        Me.lblViewingFleet.Location = New System.Drawing.Point(5, 373)
        Me.lblViewingFleet.Name = "lblViewingFleet"
        Me.lblViewingFleet.Size = New System.Drawing.Size(102, 13)
        Me.lblViewingFleet.TabIndex = 3
        Me.lblViewingFleet.Text = "Viewing Fleet: None"
        '
        'colPilot
        '
        Me.colPilot.CustomSortTag = Nothing
        Me.colPilot.Tag = Nothing
        Me.colPilot.Text = "Pilot Name"
        Me.colPilot.Width = 150
        '
        'colFitting
        '
        Me.colFitting.ContentAlign = System.Drawing.ContentAlignment.TopLeft
        Me.colFitting.CustomSortTag = Nothing
        Me.colFitting.Tag = Nothing
        Me.colFitting.Text = "Fitting"
        Me.colFitting.Width = 250
        '
        'btnSaveFleet
        '
        Me.btnSaveFleet.Location = New System.Drawing.Point(242, 61)
        Me.btnSaveFleet.Name = "btnSaveFleet"
        Me.btnSaveFleet.Size = New System.Drawing.Size(75, 23)
        Me.btnSaveFleet.TabIndex = 5
        Me.btnSaveFleet.Text = "Save Fleet"
        Me.btnSaveFleet.UseVisualStyleBackColor = True
        '
        'btnClearFleet
        '
        Me.btnClearFleet.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClearFleet.Location = New System.Drawing.Point(242, 600)
        Me.btnClearFleet.Name = "btnClearFleet"
        Me.btnClearFleet.Size = New System.Drawing.Size(75, 23)
        Me.btnClearFleet.TabIndex = 6
        Me.btnClearFleet.Text = "Clear Fleet"
        Me.btnClearFleet.UseVisualStyleBackColor = True
        '
        'btnLoadFleet
        '
        Me.btnLoadFleet.Location = New System.Drawing.Point(242, 90)
        Me.btnLoadFleet.Name = "btnLoadFleet"
        Me.btnLoadFleet.Size = New System.Drawing.Size(75, 23)
        Me.btnLoadFleet.TabIndex = 7
        Me.btnLoadFleet.Text = "Load Fleet"
        Me.btnLoadFleet.UseVisualStyleBackColor = True
        '
        'btnAddPilot
        '
        Me.btnAddPilot.Location = New System.Drawing.Point(5, 337)
        Me.btnAddPilot.Name = "btnAddPilot"
        Me.btnAddPilot.Size = New System.Drawing.Size(75, 23)
        Me.btnAddPilot.TabIndex = 8
        Me.btnAddPilot.Text = "Add Pilot"
        Me.btnAddPilot.UseVisualStyleBackColor = True
        '
        'btnEditPilot
        '
        Me.btnEditPilot.Location = New System.Drawing.Point(86, 337)
        Me.btnEditPilot.Name = "btnEditPilot"
        Me.btnEditPilot.Size = New System.Drawing.Size(75, 23)
        Me.btnEditPilot.TabIndex = 9
        Me.btnEditPilot.Text = "Edit Pilot"
        Me.btnEditPilot.UseVisualStyleBackColor = True
        '
        'btnDeletePilot
        '
        Me.btnDeletePilot.Location = New System.Drawing.Point(167, 337)
        Me.btnDeletePilot.Name = "btnDeletePilot"
        Me.btnDeletePilot.Size = New System.Drawing.Size(75, 23)
        Me.btnDeletePilot.TabIndex = 10
        Me.btnDeletePilot.Text = "Delete Pilot"
        Me.btnDeletePilot.UseVisualStyleBackColor = True
        '
        'btnUpdateFleet
        '
        Me.btnUpdateFleet.Location = New System.Drawing.Point(443, 337)
        Me.btnUpdateFleet.Name = "btnUpdateFleet"
        Me.btnUpdateFleet.Size = New System.Drawing.Size(156, 23)
        Me.btnUpdateFleet.TabIndex = 11
        Me.btnUpdateFleet.Text = "Update Fleet"
        Me.btnUpdateFleet.UseVisualStyleBackColor = True
        '
        'lblWHClass
        '
        Me.lblWHClass.AutoSize = True
        Me.lblWHClass.Location = New System.Drawing.Point(17, 58)
        Me.lblWHClass.Name = "lblWHClass"
        Me.lblWHClass.Size = New System.Drawing.Size(87, 13)
        Me.lblWHClass.TabIndex = 18
        Me.lblWHClass.Text = "Wormhole Class:"
        '
        'cboWHClass
        '
        Me.cboWHClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWHClass.FormattingEnabled = True
        Me.cboWHClass.Items.AddRange(New Object() {"1", "2", "3", "4", "5", "6"})
        Me.cboWHClass.Location = New System.Drawing.Point(114, 55)
        Me.cboWHClass.Name = "cboWHClass"
        Me.cboWHClass.Size = New System.Drawing.Size(59, 21)
        Me.cboWHClass.TabIndex = 17
        '
        'lblWHEffect
        '
        Me.lblWHEffect.AutoSize = True
        Me.lblWHEffect.Location = New System.Drawing.Point(17, 29)
        Me.lblWHEffect.Name = "lblWHEffect"
        Me.lblWHEffect.Size = New System.Drawing.Size(91, 13)
        Me.lblWHEffect.TabIndex = 16
        Me.lblWHEffect.Text = "Wormhole Effect:"
        '
        'cboWHEffect
        '
        Me.cboWHEffect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboWHEffect.FormattingEnabled = True
        Me.cboWHEffect.Items.AddRange(New Object() {"<None>", "Black Hole", "Cataclysmic Variable", "Magnetar", "Pulsar", "Red Giant", "Wolf Rayet"})
        Me.cboWHEffect.Location = New System.Drawing.Point(114, 26)
        Me.cboWHEffect.Name = "cboWHEffect"
        Me.cboWHEffect.Size = New System.Drawing.Size(148, 21)
        Me.cboWHEffect.TabIndex = 15
        '
        'ContainerListViewColumnHeader1
        '
        Me.ContainerListViewColumnHeader1.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader1.Tag = Nothing
        Me.ContainerListViewColumnHeader1.Text = "Attribute"
        Me.ContainerListViewColumnHeader1.Width = 290
        '
        'ContainerListViewColumnHeader2
        '
        Me.ContainerListViewColumnHeader2.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader2.Tag = Nothing
        Me.ContainerListViewColumnHeader2.Text = "Effect"
        Me.ContainerListViewColumnHeader2.Width = 290
        '
        'ContainerListViewColumnHeader3
        '
        Me.ContainerListViewColumnHeader3.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader3.Tag = Nothing
        Me.ContainerListViewColumnHeader3.Text = "Old Value"
        '
        'ContainerListViewColumnHeader4
        '
        Me.ContainerListViewColumnHeader4.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader4.Tag = Nothing
        Me.ContainerListViewColumnHeader4.Text = "New Value"
        '
        'btnShipAudit
        '
        Me.btnShipAudit.Location = New System.Drawing.Point(248, 337)
        Me.btnShipAudit.Name = "btnShipAudit"
        Me.btnShipAudit.Size = New System.Drawing.Size(75, 23)
        Me.btnShipAudit.TabIndex = 20
        Me.btnShipAudit.Text = "Ship Audit"
        Me.btnShipAudit.UseVisualStyleBackColor = True
        '
        'clvPilots
        '
        Me.clvPilots.AllowDrop = True
        Me.clvPilots.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clvPilots.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colRemotePilot, Me.colPilotFitting, Me.colRemoteModGroup, Me.colRemoteMod, Me.colRemoteAssign})
        Me.clvPilots.DefaultItemHeight = 20
        Me.clvPilots.Location = New System.Drawing.Point(3, 33)
        Me.clvPilots.Name = "clvPilots"
        Me.clvPilots.ShowPlusMinus = True
        Me.clvPilots.ShowRootTreeLines = True
        Me.clvPilots.ShowTreeLines = True
        Me.clvPilots.Size = New System.Drawing.Size(1071, 298)
        Me.clvPilots.TabIndex = 21
        '
        'colRemotePilot
        '
        Me.colRemotePilot.CustomSortTag = Nothing
        Me.colRemotePilot.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colRemotePilot.Tag = Nothing
        Me.colRemotePilot.Text = "Pilot"
        Me.colRemotePilot.Width = 150
        '
        'colPilotFitting
        '
        Me.colPilotFitting.CustomSortTag = Nothing
        Me.colPilotFitting.DisplayIndex = 1
        Me.colPilotFitting.Tag = Nothing
        Me.colPilotFitting.Text = "Fitting"
        Me.colPilotFitting.Width = 250
        '
        'colRemoteModGroup
        '
        Me.colRemoteModGroup.CustomSortTag = Nothing
        Me.colRemoteModGroup.DisplayIndex = 2
        Me.colRemoteModGroup.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colRemoteModGroup.Tag = Nothing
        Me.colRemoteModGroup.Text = "Module Group"
        Me.colRemoteModGroup.Width = 150
        '
        'colRemoteMod
        '
        Me.colRemoteMod.CustomSortTag = Nothing
        Me.colRemoteMod.DisplayIndex = 3
        Me.colRemoteMod.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colRemoteMod.Tag = Nothing
        Me.colRemoteMod.Text = "Module"
        Me.colRemoteMod.Width = 200
        '
        'colRemoteAssign
        '
        Me.colRemoteAssign.CustomSortTag = Nothing
        Me.colRemoteAssign.DisplayIndex = 4
        Me.colRemoteAssign.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colRemoteAssign.Tag = Nothing
        Me.colRemoteAssign.Text = "Assigned To"
        Me.colRemoteAssign.Width = 150
        '
        'btnClearAssignments
        '
        Me.btnClearAssignments.Location = New System.Drawing.Point(329, 337)
        Me.btnClearAssignments.Name = "btnClearAssignments"
        Me.btnClearAssignments.Size = New System.Drawing.Size(108, 23)
        Me.btnClearAssignments.TabIndex = 22
        Me.btnClearAssignments.Text = "Clear Remotes"
        Me.btnClearAssignments.UseVisualStyleBackColor = True
        '
        'tabFM
        '
        Me.tabFM.Controls.Add(Me.tabFleetSettings)
        Me.tabFM.Controls.Add(Me.tabFleetStructure)
        Me.tabFM.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tabFM.Location = New System.Drawing.Point(0, 0)
        Me.tabFM.Name = "tabFM"
        Me.tabFM.SelectedIndex = 0
        Me.tabFM.Size = New System.Drawing.Size(1093, 660)
        Me.tabFM.TabIndex = 23
        '
        'tabFleetSettings
        '
        Me.tabFleetSettings.BackColor = System.Drawing.SystemColors.Control
        Me.tabFleetSettings.Controls.Add(Me.panelFleetSettings)
        Me.tabFleetSettings.Location = New System.Drawing.Point(4, 22)
        Me.tabFleetSettings.Name = "tabFleetSettings"
        Me.tabFleetSettings.Padding = New System.Windows.Forms.Padding(3)
        Me.tabFleetSettings.Size = New System.Drawing.Size(1085, 634)
        Me.tabFleetSettings.TabIndex = 0
        Me.tabFleetSettings.Text = "Fleet Settings"
        '
        'panelFleetSettings
        '
        Me.panelFleetSettings.Controls.Add(Me.gbFleetSettings)
        Me.panelFleetSettings.Controls.Add(Me.clvFleetList)
        Me.panelFleetSettings.Controls.Add(Me.btnNewFleet)
        Me.panelFleetSettings.Controls.Add(Me.btnSaveFleet)
        Me.panelFleetSettings.Controls.Add(Me.btnClearFleet)
        Me.panelFleetSettings.Controls.Add(Me.btnLoadFleet)
        Me.panelFleetSettings.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelFleetSettings.Location = New System.Drawing.Point(3, 3)
        Me.panelFleetSettings.Name = "panelFleetSettings"
        Me.panelFleetSettings.Size = New System.Drawing.Size(1079, 628)
        Me.panelFleetSettings.TabIndex = 0
        '
        'gbFleetSettings
        '
        Me.gbFleetSettings.Controls.Add(Me.lblWHEffect)
        Me.gbFleetSettings.Controls.Add(Me.cboWHEffect)
        Me.gbFleetSettings.Controls.Add(Me.lblWHClass)
        Me.gbFleetSettings.Controls.Add(Me.cboWHClass)
        Me.gbFleetSettings.Enabled = False
        Me.gbFleetSettings.Location = New System.Drawing.Point(348, 32)
        Me.gbFleetSettings.Name = "gbFleetSettings"
        Me.gbFleetSettings.Size = New System.Drawing.Size(283, 169)
        Me.gbFleetSettings.TabIndex = 19
        Me.gbFleetSettings.TabStop = False
        Me.gbFleetSettings.Text = "Fleet Settings"
        '
        'tabFleetStructure
        '
        Me.tabFleetStructure.BackColor = System.Drawing.SystemColors.Control
        Me.tabFleetStructure.Controls.Add(Me.panelFleetStructure)
        Me.tabFleetStructure.Location = New System.Drawing.Point(4, 22)
        Me.tabFleetStructure.Name = "tabFleetStructure"
        Me.tabFleetStructure.Padding = New System.Windows.Forms.Padding(3)
        Me.tabFleetStructure.Size = New System.Drawing.Size(1085, 634)
        Me.tabFleetStructure.TabIndex = 1
        Me.tabFleetStructure.Text = "Fleet Structure"
        '
        'panelFleetStructure
        '
        Me.panelFleetStructure.Controls.Add(Me.cboFleet)
        Me.panelFleetStructure.Controls.Add(Me.lblFleet)
        Me.panelFleetStructure.Controls.Add(Me.clvPilots)
        Me.panelFleetStructure.Controls.Add(Me.lblViewingFleet)
        Me.panelFleetStructure.Controls.Add(Me.btnDeletePilot)
        Me.panelFleetStructure.Controls.Add(Me.clvFleetStructure)
        Me.panelFleetStructure.Controls.Add(Me.btnUpdateFleet)
        Me.panelFleetStructure.Controls.Add(Me.btnAddPilot)
        Me.panelFleetStructure.Controls.Add(Me.btnClearAssignments)
        Me.panelFleetStructure.Controls.Add(Me.btnEditPilot)
        Me.panelFleetStructure.Controls.Add(Me.btnShipAudit)
        Me.panelFleetStructure.Dock = System.Windows.Forms.DockStyle.Fill
        Me.panelFleetStructure.Location = New System.Drawing.Point(3, 3)
        Me.panelFleetStructure.Name = "panelFleetStructure"
        Me.panelFleetStructure.Size = New System.Drawing.Size(1079, 628)
        Me.panelFleetStructure.TabIndex = 0
        '
        'cboFleet
        '
        Me.cboFleet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboFleet.FormattingEnabled = True
        Me.cboFleet.Location = New System.Drawing.Point(46, 6)
        Me.cboFleet.Name = "cboFleet"
        Me.cboFleet.Size = New System.Drawing.Size(193, 21)
        Me.cboFleet.Sorted = True
        Me.cboFleet.TabIndex = 24
        '
        'lblFleet
        '
        Me.lblFleet.AutoSize = True
        Me.lblFleet.Location = New System.Drawing.Point(5, 9)
        Me.lblFleet.Name = "lblFleet"
        Me.lblFleet.Size = New System.Drawing.Size(35, 13)
        Me.lblFleet.TabIndex = 23
        Me.lblFleet.Text = "Fleet:"
        '
        'ctxPilotList
        '
        Me.ctxPilotList.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RemoveAssignmnetToolStripMenuItem})
        Me.ctxPilotList.Name = "ctxPilotList"
        Me.ctxPilotList.Size = New System.Drawing.Size(184, 48)
        '
        'RemoveAssignmnetToolStripMenuItem
        '
        Me.RemoveAssignmnetToolStripMenuItem.Name = "RemoveAssignmnetToolStripMenuItem"
        Me.RemoveAssignmnetToolStripMenuItem.Size = New System.Drawing.Size(183, 22)
        Me.RemoveAssignmnetToolStripMenuItem.Text = "Remove Assignment"
        '
        'frmFleetManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1093, 660)
        Me.Controls.Add(Me.tabFM)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmFleetManager"
        Me.Text = "HQF Fleet Manager"
        Me.tabFM.ResumeLayout(False)
        Me.tabFleetSettings.ResumeLayout(False)
        Me.panelFleetSettings.ResumeLayout(False)
        Me.gbFleetSettings.ResumeLayout(False)
        Me.gbFleetSettings.PerformLayout()
        Me.tabFleetStructure.ResumeLayout(False)
        Me.panelFleetStructure.ResumeLayout(False)
        Me.panelFleetStructure.PerformLayout()
        Me.ctxPilotList.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents clvFleetStructure As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents clvFleetList As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colFleetStructure As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents btnNewFleet As System.Windows.Forms.Button
    Friend WithEvents lblViewingFleet As System.Windows.Forms.Label
    Friend WithEvents colPilot As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colFleetList As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ctxFleetStructure As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents btnSaveFleet As System.Windows.Forms.Button
    Friend WithEvents btnClearFleet As System.Windows.Forms.Button
    Friend WithEvents btnLoadFleet As System.Windows.Forms.Button
    Friend WithEvents colFitting As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents btnAddPilot As System.Windows.Forms.Button
    Friend WithEvents btnEditPilot As System.Windows.Forms.Button
    Friend WithEvents btnDeletePilot As System.Windows.Forms.Button
    Friend WithEvents colFleetBooster As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents btnUpdateFleet As System.Windows.Forms.Button
    Friend WithEvents lblWHClass As System.Windows.Forms.Label
    Friend WithEvents cboWHClass As System.Windows.Forms.ComboBox
    Friend WithEvents lblWHEffect As System.Windows.Forms.Label
    Friend WithEvents cboWHEffect As System.Windows.Forms.ComboBox
    Friend WithEvents ContainerListViewColumnHeader1 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader2 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader3 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ContainerListViewColumnHeader4 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents btnShipAudit As System.Windows.Forms.Button
    Friend WithEvents clvPilots As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colRemotePilot As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colRemoteModGroup As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colRemoteMod As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colRemoteAssign As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents btnClearAssignments As System.Windows.Forms.Button
    Friend WithEvents colFleetEHP As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colDPS As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colTank As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colShieldHP As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colArmorHP As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colVelocity As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colCap As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents tabFM As System.Windows.Forms.TabControl
    Friend WithEvents tabFleetSettings As System.Windows.Forms.TabPage
    Friend WithEvents panelFleetSettings As System.Windows.Forms.Panel
    Friend WithEvents tabFleetStructure As System.Windows.Forms.TabPage
    Friend WithEvents panelFleetStructure As System.Windows.Forms.Panel
    Friend WithEvents cboFleet As System.Windows.Forms.ComboBox
    Friend WithEvents lblFleet As System.Windows.Forms.Label
    Friend WithEvents gbFleetSettings As System.Windows.Forms.GroupBox
    Friend WithEvents colPilotFitting As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ctxPilotList As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents RemoveAssignmnetToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class
