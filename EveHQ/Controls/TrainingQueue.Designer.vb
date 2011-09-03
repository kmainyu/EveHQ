<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TrainingQueue
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.lblSkillCount = New System.Windows.Forms.Label
        Me.lblNumberOfSkills = New System.Windows.Forms.Label
        Me.lblQueueTime = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lvQueue = New EveHQ.DragAndDropListView
        Me.panelInfo = New DevComponents.DotNetBar.PanelEx
        Me.panelInfo.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblSkillCount
        '
        Me.lblSkillCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblSkillCount.Location = New System.Drawing.Point(325, 13)
        Me.lblSkillCount.Name = "lblSkillCount"
        Me.lblSkillCount.Size = New System.Drawing.Size(43, 17)
        Me.lblSkillCount.TabIndex = 5
        '
        'lblNumberOfSkills
        '
        Me.lblNumberOfSkills.AutoSize = True
        Me.lblNumberOfSkills.Location = New System.Drawing.Point(233, 14)
        Me.lblNumberOfSkills.Name = "lblNumberOfSkills"
        Me.lblNumberOfSkills.Size = New System.Drawing.Size(86, 13)
        Me.lblNumberOfSkills.TabIndex = 4
        Me.lblNumberOfSkills.Text = "Number of Skills:"
        '
        'lblQueueTime
        '
        Me.lblQueueTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblQueueTime.Location = New System.Drawing.Point(113, 14)
        Me.lblQueueTime.Name = "lblQueueTime"
        Me.lblQueueTime.Size = New System.Drawing.Size(110, 15)
        Me.lblQueueTime.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 14)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(101, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Total Training Time:"
        '
        'lvQueue
        '
        Me.lvQueue.AllowColumnReorder = True
        Me.lvQueue.AllowDrop = True
        Me.lvQueue.AllowReorder = True
        Me.lvQueue.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvQueue.FullRowSelect = True
        Me.lvQueue.HideSelection = False
        Me.lvQueue.IncludeCurrentTraining = False
        Me.lvQueue.LineColor = System.Drawing.Color.Red
        Me.lvQueue.Location = New System.Drawing.Point(0, 0)
        Me.lvQueue.Name = "lvQueue"
        Me.lvQueue.ShowItemToolTips = True
        Me.lvQueue.Size = New System.Drawing.Size(766, 373)
        Me.lvQueue.TabIndex = 1
        Me.lvQueue.UseCompatibleStateImageBehavior = False
        Me.lvQueue.View = System.Windows.Forms.View.Details
        '
        'panelInfo
        '
        Me.panelInfo.CanvasColor = System.Drawing.SystemColors.Control
        Me.panelInfo.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.panelInfo.Controls.Add(Me.lblSkillCount)
        Me.panelInfo.Controls.Add(Me.Label1)
        Me.panelInfo.Controls.Add(Me.lblNumberOfSkills)
        Me.panelInfo.Controls.Add(Me.lblQueueTime)
        Me.panelInfo.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.panelInfo.Location = New System.Drawing.Point(0, 373)
        Me.panelInfo.Name = "panelInfo"
        Me.panelInfo.Size = New System.Drawing.Size(766, 41)
        Me.panelInfo.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.panelInfo.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.panelInfo.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.panelInfo.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.panelInfo.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.panelInfo.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.panelInfo.Style.GradientAngle = 90
        Me.panelInfo.TabIndex = 2
        '
        'TrainingQueue
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.lvQueue)
        Me.Controls.Add(Me.panelInfo)
        Me.Name = "TrainingQueue"
        Me.Size = New System.Drawing.Size(766, 414)
        Me.panelInfo.ResumeLayout(False)
        Me.panelInfo.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents lvQueue As EveHQ.DragAndDropListView
    Friend WithEvents lblQueueTime As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblSkillCount As System.Windows.Forms.Label
    Friend WithEvents lblNumberOfSkills As System.Windows.Forms.Label
    Friend WithEvents panelInfo As DevComponents.DotNetBar.PanelEx

End Class
