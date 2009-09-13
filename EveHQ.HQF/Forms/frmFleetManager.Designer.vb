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
        Me.ctxFleetStructure = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.clvFleetList = New DotNetLib.Windows.Forms.ContainerListView
        Me.colFleetList = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.btnNewFleet = New System.Windows.Forms.Button
        Me.lblViewingFleet = New System.Windows.Forms.Label
        Me.clvPilotList = New DotNetLib.Windows.Forms.ContainerListView
        Me.ContainerListViewColumnHeader1 = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.btnSaveFleet = New System.Windows.Forms.Button
        Me.btnClearFleet = New System.Windows.Forms.Button
        Me.btnLoadFleet = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'clvFleetStructure
        '
        Me.clvFleetStructure.AllowDrop = True
        Me.clvFleetStructure.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colFleetStructure})
        Me.clvFleetStructure.DefaultItemHeight = 20
        Me.clvFleetStructure.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.clvFleetStructure.ItemContextMenu = Me.ctxFleetStructure
        Me.clvFleetStructure.Location = New System.Drawing.Point(12, 193)
        Me.clvFleetStructure.Name = "clvFleetStructure"
        Me.clvFleetStructure.ShowPlusMinus = True
        Me.clvFleetStructure.ShowRootTreeLines = True
        Me.clvFleetStructure.ShowTreeLines = True
        Me.clvFleetStructure.Size = New System.Drawing.Size(334, 432)
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
        Me.clvFleetList.Size = New System.Drawing.Size(334, 123)
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
        Me.clvPilotList.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.ContainerListViewColumnHeader1})
        Me.clvPilotList.DefaultItemHeight = 20
        Me.clvPilotList.Location = New System.Drawing.Point(352, 12)
        Me.clvPilotList.Name = "clvPilotList"
        Me.clvPilotList.Size = New System.Drawing.Size(329, 613)
        Me.clvPilotList.TabIndex = 4
        '
        'ContainerListViewColumnHeader1
        '
        Me.ContainerListViewColumnHeader1.CustomSortTag = Nothing
        Me.ContainerListViewColumnHeader1.Tag = Nothing
        Me.ContainerListViewColumnHeader1.Text = "Pilot Name"
        Me.ContainerListViewColumnHeader1.Width = 300
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
        'frmFleetManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1059, 637)
        Me.Controls.Add(Me.btnLoadFleet)
        Me.Controls.Add(Me.btnClearFleet)
        Me.Controls.Add(Me.btnSaveFleet)
        Me.Controls.Add(Me.clvPilotList)
        Me.Controls.Add(Me.lblViewingFleet)
        Me.Controls.Add(Me.btnNewFleet)
        Me.Controls.Add(Me.clvFleetList)
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
    Friend WithEvents ContainerListViewColumnHeader1 As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colFleetList As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents ctxFleetStructure As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents btnSaveFleet As System.Windows.Forms.Button
    Friend WithEvents btnClearFleet As System.Windows.Forms.Button
    Friend WithEvents btnLoadFleet As System.Windows.Forms.Button
End Class
