﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
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
        Me.ctxFleetStructure = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.clvFleetList = New DotNetLib.Windows.Forms.ContainerListView
        Me.colFleetList = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.btnNewFleet = New System.Windows.Forms.Button
        Me.lblViewingFleet = New System.Windows.Forms.Label
        Me.clvPilotList = New DotNetLib.Windows.Forms.ContainerListView
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
        Me.SuspendLayout()
        '
        'clvFleetStructure
        '
        Me.clvFleetStructure.AllowDrop = True
        Me.clvFleetStructure.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colFleetStructure, Me.colFleetBooster})
        Me.clvFleetStructure.DefaultItemHeight = 20
        Me.clvFleetStructure.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.clvFleetStructure.ItemContextMenu = Me.ctxFleetStructure
        Me.clvFleetStructure.Location = New System.Drawing.Point(12, 193)
        Me.clvFleetStructure.Name = "clvFleetStructure"
        Me.clvFleetStructure.ShowPlusMinus = True
        Me.clvFleetStructure.ShowRootTreeLines = True
        Me.clvFleetStructure.ShowTreeLines = True
        Me.clvFleetStructure.Size = New System.Drawing.Size(318, 432)
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
        Me.colFleetBooster.CustomSortTag = Nothing
        Me.colFleetBooster.DisplayIndex = 1
        Me.colFleetBooster.Tag = Nothing
        Me.colFleetBooster.Text = "Booster"
        Me.colFleetBooster.Width = 60
        '
        'ctxFleetStructure
        '
        Me.ctxFleetStructure.Name = "ctxFleetStructure"
        Me.ctxFleetStructure.Size = New System.Drawing.Size(61, 4)
        '
        'clvFleetList
        '
        Me.clvFleetList.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colFleetList})
        Me.clvFleetList.DefaultItemHeight = 20
        Me.clvFleetList.Location = New System.Drawing.Point(12, 12)
        Me.clvFleetList.Name = "clvFleetList"
        Me.clvFleetList.Size = New System.Drawing.Size(318, 123)
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
        Me.btnNewFleet.Location = New System.Drawing.Point(12, 141)
        Me.btnNewFleet.Name = "btnNewFleet"
        Me.btnNewFleet.Size = New System.Drawing.Size(75, 23)
        Me.btnNewFleet.TabIndex = 2
        Me.btnNewFleet.Text = "New Fleet"
        Me.btnNewFleet.UseVisualStyleBackColor = True
        '
        'lblViewingFleet
        '
        Me.lblViewingFleet.AutoSize = True
        Me.lblViewingFleet.Location = New System.Drawing.Point(9, 177)
        Me.lblViewingFleet.Name = "lblViewingFleet"
        Me.lblViewingFleet.Size = New System.Drawing.Size(102, 13)
        Me.lblViewingFleet.TabIndex = 3
        Me.lblViewingFleet.Text = "Viewing Fleet: None"
        '
        'clvPilotList
        '
        Me.clvPilotList.AllowDrop = True
        Me.clvPilotList.AllowMultiSelect = True
        Me.clvPilotList.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colPilot, Me.colFitting})
        Me.clvPilotList.DefaultItemHeight = 20
        Me.clvPilotList.Location = New System.Drawing.Point(336, 12)
        Me.clvPilotList.Name = "clvPilotList"
        Me.clvPilotList.Size = New System.Drawing.Size(432, 325)
        Me.clvPilotList.TabIndex = 4
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
        Me.colFitting.DisplayIndex = 1
        Me.colFitting.Tag = Nothing
        Me.colFitting.Text = "Fitting"
        Me.colFitting.Width = 250
        '
        'btnSaveFleet
        '
        Me.btnSaveFleet.Location = New System.Drawing.Point(93, 141)
        Me.btnSaveFleet.Name = "btnSaveFleet"
        Me.btnSaveFleet.Size = New System.Drawing.Size(75, 23)
        Me.btnSaveFleet.TabIndex = 5
        Me.btnSaveFleet.Text = "Save Fleet"
        Me.btnSaveFleet.UseVisualStyleBackColor = True
        '
        'btnClearFleet
        '
        Me.btnClearFleet.Location = New System.Drawing.Point(255, 141)
        Me.btnClearFleet.Name = "btnClearFleet"
        Me.btnClearFleet.Size = New System.Drawing.Size(75, 23)
        Me.btnClearFleet.TabIndex = 6
        Me.btnClearFleet.Text = "Clear Fleet"
        Me.btnClearFleet.UseVisualStyleBackColor = True
        '
        'btnLoadFleet
        '
        Me.btnLoadFleet.Location = New System.Drawing.Point(174, 141)
        Me.btnLoadFleet.Name = "btnLoadFleet"
        Me.btnLoadFleet.Size = New System.Drawing.Size(75, 23)
        Me.btnLoadFleet.TabIndex = 7
        Me.btnLoadFleet.Text = "Load Fleet"
        Me.btnLoadFleet.UseVisualStyleBackColor = True
        '
        'btnAddPilot
        '
        Me.btnAddPilot.Location = New System.Drawing.Point(774, 51)
        Me.btnAddPilot.Name = "btnAddPilot"
        Me.btnAddPilot.Size = New System.Drawing.Size(75, 23)
        Me.btnAddPilot.TabIndex = 8
        Me.btnAddPilot.Text = "Add Pilot"
        Me.btnAddPilot.UseVisualStyleBackColor = True
        '
        'btnEditPilot
        '
        Me.btnEditPilot.Location = New System.Drawing.Point(855, 51)
        Me.btnEditPilot.Name = "btnEditPilot"
        Me.btnEditPilot.Size = New System.Drawing.Size(75, 23)
        Me.btnEditPilot.TabIndex = 9
        Me.btnEditPilot.Text = "Edit Pilot"
        Me.btnEditPilot.UseVisualStyleBackColor = True
        '
        'btnDeletePilot
        '
        Me.btnDeletePilot.Location = New System.Drawing.Point(936, 51)
        Me.btnDeletePilot.Name = "btnDeletePilot"
        Me.btnDeletePilot.Size = New System.Drawing.Size(75, 23)
        Me.btnDeletePilot.TabIndex = 10
        Me.btnDeletePilot.Text = "Delete Pilot"
        Me.btnDeletePilot.UseVisualStyleBackColor = True
        '
        'btnUpdateFleet
        '
        Me.btnUpdateFleet.Location = New System.Drawing.Point(774, 12)
        Me.btnUpdateFleet.Name = "btnUpdateFleet"
        Me.btnUpdateFleet.Size = New System.Drawing.Size(156, 23)
        Me.btnUpdateFleet.TabIndex = 11
        Me.btnUpdateFleet.Text = "Update Fleet"
        Me.btnUpdateFleet.UseVisualStyleBackColor = True
        '
        'lblWHClass
        '
        Me.lblWHClass.AutoSize = True
        Me.lblWHClass.Location = New System.Drawing.Point(774, 122)
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
        Me.cboWHClass.Location = New System.Drawing.Point(862, 119)
        Me.cboWHClass.Name = "cboWHClass"
        Me.cboWHClass.Size = New System.Drawing.Size(59, 21)
        Me.cboWHClass.TabIndex = 17
        '
        'lblWHEffect
        '
        Me.lblWHEffect.AutoSize = True
        Me.lblWHEffect.Location = New System.Drawing.Point(774, 87)
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
        Me.cboWHEffect.Location = New System.Drawing.Point(869, 84)
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
        Me.ContainerListViewColumnHeader2.DisplayIndex = 1
        Me.ContainerListViewColumnHeader2.Tag = Nothing
        Me.ContainerListViewColumnHeader2.Text = "Effect"
        Me.ContainerListViewColumnHeader2.Width = 290
        '
        'ContainerListViewColumnHeader3
        '
        Me.ContainerListViewColumnHeader3.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader3.DisplayIndex = 2
        Me.ContainerListViewColumnHeader3.Tag = Nothing
        Me.ContainerListViewColumnHeader3.Text = "Old Value"
        '
        'ContainerListViewColumnHeader4
        '
        Me.ContainerListViewColumnHeader4.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader4.DisplayIndex = 3
        Me.ContainerListViewColumnHeader4.Tag = Nothing
        Me.ContainerListViewColumnHeader4.Text = "New Value"
        '
        'btnShipAudit
        '
        Me.btnShipAudit.Location = New System.Drawing.Point(936, 12)
        Me.btnShipAudit.Name = "btnShipAudit"
        Me.btnShipAudit.Size = New System.Drawing.Size(75, 23)
        Me.btnShipAudit.TabIndex = 20
        Me.btnShipAudit.Text = "Ship Audit"
        Me.btnShipAudit.UseVisualStyleBackColor = True
        '
        'frmFleetManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1028, 637)
        Me.Controls.Add(Me.btnShipAudit)
        Me.Controls.Add(Me.lblWHClass)
        Me.Controls.Add(Me.cboWHClass)
        Me.Controls.Add(Me.lblWHEffect)
        Me.Controls.Add(Me.cboWHEffect)
        Me.Controls.Add(Me.btnLoadFleet)
        Me.Controls.Add(Me.btnDeletePilot)
        Me.Controls.Add(Me.btnEditPilot)
        Me.Controls.Add(Me.btnAddPilot)
        Me.Controls.Add(Me.btnClearFleet)
        Me.Controls.Add(Me.btnUpdateFleet)
        Me.Controls.Add(Me.btnSaveFleet)
        Me.Controls.Add(Me.lblViewingFleet)
        Me.Controls.Add(Me.btnNewFleet)
        Me.Controls.Add(Me.clvFleetList)
        Me.Controls.Add(Me.clvPilotList)
        Me.Controls.Add(Me.clvFleetStructure)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmFleetManager"
        Me.Text = "HQF Fleet Manager"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents clvFleetStructure As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents clvFleetList As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colFleetStructure As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents btnNewFleet As System.Windows.Forms.Button
    Friend WithEvents lblViewingFleet As System.Windows.Forms.Label
    Friend WithEvents clvPilotList As DotNetLib.Windows.Forms.ContainerListView
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
End Class
