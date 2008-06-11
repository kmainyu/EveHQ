<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmRequiredSkills
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
        Me.clvSkills = New DotNetLib.Windows.Forms.ContainerListView
        Me.colSkillName = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colReqLevel = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colActLevel = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colHQFLevel = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colRequiredFor = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.btnClose = New System.Windows.Forms.Button
        Me.btnAddToQueue = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'clvSkills
        '
        Me.clvSkills.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colSkillName, Me.colReqLevel, Me.colActLevel, Me.colHQFLevel, Me.colRequiredFor})
        Me.clvSkills.DefaultItemHeight = 20
        Me.clvSkills.Location = New System.Drawing.Point(13, 13)
        Me.clvSkills.Name = "clvSkills"
        Me.clvSkills.ShowPlusMinus = True
        Me.clvSkills.ShowRootTreeLines = True
        Me.clvSkills.ShowTreeLines = True
        Me.clvSkills.Size = New System.Drawing.Size(765, 516)
        Me.clvSkills.TabIndex = 0
        '
        'colSkillName
        '
        Me.colSkillName.CustomSortTag = Nothing
        Me.colSkillName.Tag = Nothing
        Me.colSkillName.Text = "SkillName"
        Me.colSkillName.Width = 225
        '
        'colReqLevel
        '
        Me.colReqLevel.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colReqLevel.CustomSortTag = Nothing
        Me.colReqLevel.DisplayIndex = 1
        Me.colReqLevel.Tag = Nothing
        Me.colReqLevel.Text = "Req Level"
        Me.colReqLevel.Width = 70
        '
        'colActLevel
        '
        Me.colActLevel.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colActLevel.CustomSortTag = Nothing
        Me.colActLevel.DisplayIndex = 2
        Me.colActLevel.Tag = Nothing
        Me.colActLevel.Text = "Act Level"
        Me.colActLevel.Width = 70
        '
        'colHQFLevel
        '
        Me.colHQFLevel.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colHQFLevel.CustomSortTag = Nothing
        Me.colHQFLevel.DisplayIndex = 3
        Me.colHQFLevel.Tag = Nothing
        Me.colHQFLevel.Text = "HQF Level"
        Me.colHQFLevel.Width = 70
        '
        'colRequiredFor
        '
        Me.colRequiredFor.CustomSortTag = Nothing
        Me.colRequiredFor.DisplayIndex = 4
        Me.colRequiredFor.Tag = Nothing
        Me.colRequiredFor.Text = "Required For"
        Me.colRequiredFor.Width = 300
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(678, 535)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(100, 23)
        Me.btnClose.TabIndex = 1
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'btnAddToQueue
        '
        Me.btnAddToQueue.Location = New System.Drawing.Point(572, 535)
        Me.btnAddToQueue.Name = "btnAddToQueue"
        Me.btnAddToQueue.Size = New System.Drawing.Size(100, 23)
        Me.btnAddToQueue.TabIndex = 2
        Me.btnAddToQueue.Text = "Add To Queue"
        Me.btnAddToQueue.UseVisualStyleBackColor = True
        '
        'frmRequiredSkills
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(790, 564)
        Me.Controls.Add(Me.btnAddToQueue)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.clvSkills)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRequiredSkills"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Required Skills"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents clvSkills As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents btnAddToQueue As System.Windows.Forms.Button
    Friend WithEvents colSkillName As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colReqLevel As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colHQFLevel As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colActLevel As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colRequiredFor As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
End Class
