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
        Me.pnlBottom = New System.Windows.Forms.Panel
        Me.lblQueueTime = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lvQueue = New EveHQ.DragAndDropListView
        Me.pnlBottom.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlBottom
        '
        Me.pnlBottom.Controls.Add(Me.lblQueueTime)
        Me.pnlBottom.Controls.Add(Me.Label1)
        Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlBottom.Location = New System.Drawing.Point(0, 374)
        Me.pnlBottom.Name = "pnlBottom"
        Me.pnlBottom.Size = New System.Drawing.Size(766, 40)
        Me.pnlBottom.TabIndex = 0
        '
        'lblQueueTime
        '
        Me.lblQueueTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblQueueTime.Location = New System.Drawing.Point(116, 14)
        Me.lblQueueTime.Name = "lblQueueTime"
        Me.lblQueueTime.Size = New System.Drawing.Size(110, 15)
        Me.lblQueueTime.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(6, 14)
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
        Me.lvQueue.Size = New System.Drawing.Size(766, 374)
        Me.lvQueue.TabIndex = 1
        Me.lvQueue.UseCompatibleStateImageBehavior = False
        Me.lvQueue.View = System.Windows.Forms.View.Details
        '
        'TrainingQueue
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.lvQueue)
        Me.Controls.Add(Me.pnlBottom)
        Me.Name = "TrainingQueue"
        Me.Size = New System.Drawing.Size(766, 414)
        Me.pnlBottom.ResumeLayout(False)
        Me.pnlBottom.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents pnlBottom As System.Windows.Forms.Panel
    Public WithEvents lvQueue As EveHQ.DragAndDropListView
    Friend WithEvents lblQueueTime As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label

End Class
