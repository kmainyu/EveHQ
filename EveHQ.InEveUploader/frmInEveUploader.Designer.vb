<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmInEveUploader
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmInEveUploader))
        Me.tabInEve = New System.Windows.Forms.TabControl
        Me.tabInEveSkills = New System.Windows.Forms.TabPage
        Me.Label3 = New System.Windows.Forms.Label
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel
        Me.Label2 = New System.Windows.Forms.Label
        Me.lvwPilots = New System.Windows.Forms.ListView
        Me.colPilot = New System.Windows.Forms.ColumnHeader
        Me.colStatus = New System.Windows.Forms.ColumnHeader
        Me.btnUploadSkills = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.tabInEveQueues = New System.Windows.Forms.TabPage
        Me.Label6 = New System.Windows.Forms.Label
        Me.btnUploadQueues = New System.Windows.Forms.Button
        Me.lvwQueues = New System.Windows.Forms.ListView
        Me.colSkillQueue = New System.Windows.Forms.ColumnHeader
        Me.colQueueStatus = New System.Windows.Forms.ColumnHeader
        Me.lblPilotName = New System.Windows.Forms.Label
        Me.cboPilots = New System.Windows.Forms.ComboBox
        Me.LinkLabel2 = New System.Windows.Forms.LinkLabel
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.btnClose = New System.Windows.Forms.Button
        Me.tabInEve.SuspendLayout()
        Me.tabInEveSkills.SuspendLayout()
        Me.tabInEveQueues.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabInEve
        '
        Me.tabInEve.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tabInEve.Controls.Add(Me.tabInEveSkills)
        Me.tabInEve.Controls.Add(Me.tabInEveQueues)
        Me.tabInEve.Location = New System.Drawing.Point(12, 12)
        Me.tabInEve.Name = "tabInEve"
        Me.tabInEve.SelectedIndex = 0
        Me.tabInEve.Size = New System.Drawing.Size(506, 744)
        Me.tabInEve.TabIndex = 8
        '
        'tabInEveSkills
        '
        Me.tabInEveSkills.Controls.Add(Me.Label3)
        Me.tabInEveSkills.Controls.Add(Me.LinkLabel1)
        Me.tabInEveSkills.Controls.Add(Me.Label2)
        Me.tabInEveSkills.Controls.Add(Me.lvwPilots)
        Me.tabInEveSkills.Controls.Add(Me.btnUploadSkills)
        Me.tabInEveSkills.Controls.Add(Me.Label1)
        Me.tabInEveSkills.Location = New System.Drawing.Point(4, 22)
        Me.tabInEveSkills.Name = "tabInEveSkills"
        Me.tabInEveSkills.Padding = New System.Windows.Forms.Padding(3)
        Me.tabInEveSkills.Size = New System.Drawing.Size(498, 718)
        Me.tabInEveSkills.TabIndex = 0
        Me.tabInEveSkills.Text = "Skills"
        Me.tabInEveSkills.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.Location = New System.Drawing.Point(3, 672)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(481, 43)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Special Thanks to Sara Dawn at inEve.net for the assistance in developing this pl" & _
            "ug-in."
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LinkLabel1
        '
        Me.LinkLabel1.Location = New System.Drawing.Point(9, 77)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(478, 23)
        Me.LinkLabel1.TabIndex = 13
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "http://ineve.net"
        Me.LinkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(6, 110)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(481, 43)
        Me.Label2.TabIndex = 12
        Me.Label2.Text = "To upload your skills to the InEve website, please ensure the characters you want" & _
            " to upload are checked, then click the 'Upload' button."
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lvwPilots
        '
        Me.lvwPilots.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lvwPilots.CheckBoxes = True
        Me.lvwPilots.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colPilot, Me.colStatus})
        Me.lvwPilots.FullRowSelect = True
        Me.lvwPilots.GridLines = True
        Me.lvwPilots.HideSelection = False
        Me.lvwPilots.Location = New System.Drawing.Point(6, 156)
        Me.lvwPilots.MultiSelect = False
        Me.lvwPilots.Name = "lvwPilots"
        Me.lvwPilots.Size = New System.Drawing.Size(481, 484)
        Me.lvwPilots.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwPilots.TabIndex = 11
        Me.lvwPilots.UseCompatibleStateImageBehavior = False
        Me.lvwPilots.View = System.Windows.Forms.View.Details
        '
        'colPilot
        '
        Me.colPilot.Text = "Character Name"
        Me.colPilot.Width = 150
        '
        'colStatus
        '
        Me.colStatus.Text = "Upload Status"
        Me.colStatus.Width = 300
        '
        'btnUploadSkills
        '
        Me.btnUploadSkills.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnUploadSkills.Location = New System.Drawing.Point(412, 646)
        Me.btnUploadSkills.Name = "btnUploadSkills"
        Me.btnUploadSkills.Size = New System.Drawing.Size(75, 23)
        Me.btnUploadSkills.TabIndex = 9
        Me.btnUploadSkills.Text = "Upload"
        Me.btnUploadSkills.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(6, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(481, 63)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = resources.GetString("Label1.Text")
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tabInEveQueues
        '
        Me.tabInEveQueues.Controls.Add(Me.Label6)
        Me.tabInEveQueues.Controls.Add(Me.btnUploadQueues)
        Me.tabInEveQueues.Controls.Add(Me.lvwQueues)
        Me.tabInEveQueues.Controls.Add(Me.lblPilotName)
        Me.tabInEveQueues.Controls.Add(Me.cboPilots)
        Me.tabInEveQueues.Controls.Add(Me.LinkLabel2)
        Me.tabInEveQueues.Controls.Add(Me.Label4)
        Me.tabInEveQueues.Controls.Add(Me.Label5)
        Me.tabInEveQueues.Location = New System.Drawing.Point(4, 22)
        Me.tabInEveQueues.Name = "tabInEveQueues"
        Me.tabInEveQueues.Padding = New System.Windows.Forms.Padding(3)
        Me.tabInEveQueues.Size = New System.Drawing.Size(498, 718)
        Me.tabInEveQueues.TabIndex = 1
        Me.tabInEveQueues.Text = "Skill Queues"
        Me.tabInEveQueues.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label6.Location = New System.Drawing.Point(9, 672)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(481, 43)
        Me.Label6.TabIndex = 22
        Me.Label6.Text = "Special Thanks to Sara Dawn at inEve.net for the assistance in developing this pl" & _
            "ug-in."
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnUploadQueues
        '
        Me.btnUploadQueues.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnUploadQueues.Location = New System.Drawing.Point(415, 646)
        Me.btnUploadQueues.Name = "btnUploadQueues"
        Me.btnUploadQueues.Size = New System.Drawing.Size(75, 23)
        Me.btnUploadQueues.TabIndex = 20
        Me.btnUploadQueues.Text = "Upload"
        Me.btnUploadQueues.UseVisualStyleBackColor = True
        '
        'lvwQueues
        '
        Me.lvwQueues.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lvwQueues.CheckBoxes = True
        Me.lvwQueues.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colSkillQueue, Me.colQueueStatus})
        Me.lvwQueues.FullRowSelect = True
        Me.lvwQueues.GridLines = True
        Me.lvwQueues.HideSelection = False
        Me.lvwQueues.Location = New System.Drawing.Point(9, 196)
        Me.lvwQueues.MultiSelect = False
        Me.lvwQueues.Name = "lvwQueues"
        Me.lvwQueues.Size = New System.Drawing.Size(481, 444)
        Me.lvwQueues.Sorting = System.Windows.Forms.SortOrder.Ascending
        Me.lvwQueues.TabIndex = 19
        Me.lvwQueues.UseCompatibleStateImageBehavior = False
        Me.lvwQueues.View = System.Windows.Forms.View.Details
        '
        'colSkillQueue
        '
        Me.colSkillQueue.Text = "Queue Name"
        Me.colSkillQueue.Width = 150
        '
        'colQueueStatus
        '
        Me.colQueueStatus.Text = "Upload Status"
        Me.colQueueStatus.Width = 300
        '
        'lblPilotName
        '
        Me.lblPilotName.AutoSize = True
        Me.lblPilotName.Location = New System.Drawing.Point(53, 172)
        Me.lblPilotName.Name = "lblPilotName"
        Me.lblPilotName.Size = New System.Drawing.Size(61, 13)
        Me.lblPilotName.TabIndex = 18
        Me.lblPilotName.Text = "Pilot Name:"
        '
        'cboPilots
        '
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.Location = New System.Drawing.Point(120, 169)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(273, 21)
        Me.cboPilots.Sorted = True
        Me.cboPilots.TabIndex = 17
        '
        'LinkLabel2
        '
        Me.LinkLabel2.Location = New System.Drawing.Point(9, 77)
        Me.LinkLabel2.Name = "LinkLabel2"
        Me.LinkLabel2.Size = New System.Drawing.Size(478, 23)
        Me.LinkLabel2.TabIndex = 16
        Me.LinkLabel2.TabStop = True
        Me.LinkLabel2.Text = "http://ineve.net"
        Me.LinkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(6, 110)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(481, 43)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "To upload your skill queues to the InEve website, please select a character, ensu" & _
            "re the queues you want to upload are checked, then click the 'Upload' button."
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(6, 3)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(481, 63)
        Me.Label5.TabIndex = 14
        Me.Label5.Text = resources.GetString("Label5.Text")
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(12, 770)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 10
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmInEveUploader
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1175, 805)
        Me.Controls.Add(Me.tabInEve)
        Me.Controls.Add(Me.btnClose)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Name = "frmInEveUploader"
        Me.Text = "InEve Uploader"
        Me.tabInEve.ResumeLayout(False)
        Me.tabInEveSkills.ResumeLayout(False)
        Me.tabInEveQueues.ResumeLayout(False)
        Me.tabInEveQueues.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents tabInEve As System.Windows.Forms.TabControl
    Friend WithEvents tabInEveSkills As System.Windows.Forms.TabPage
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lvwPilots As System.Windows.Forms.ListView
    Friend WithEvents colPilot As System.Windows.Forms.ColumnHeader
    Friend WithEvents colStatus As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnUploadSkills As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tabInEveQueues As System.Windows.Forms.TabPage
    Friend WithEvents LinkLabel2 As System.Windows.Forms.LinkLabel
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnUploadQueues As System.Windows.Forms.Button
    Friend WithEvents lvwQueues As System.Windows.Forms.ListView
    Friend WithEvents colSkillQueue As System.Windows.Forms.ColumnHeader
    Friend WithEvents colQueueStatus As System.Windows.Forms.ColumnHeader
    Friend WithEvents lblPilotName As System.Windows.Forms.Label
    Friend WithEvents cboPilots As System.Windows.Forms.ComboBox
End Class
