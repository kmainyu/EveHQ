<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUpdater
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUpdater))
        Me.lblUpdateStatus = New System.Windows.Forms.Label
        Me.clvUpdates = New DotNetLib.Windows.Forms.ContainerListView
        Me.colComponent = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colFunction = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colVersion = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colAvailable = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colDownload = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colProgress = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.btnStartUpdate = New System.Windows.Forms.Button
        Me.tmrUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.btnRecheckUpdates = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lblUpdateStatus
        '
        Me.lblUpdateStatus.AutoSize = True
        Me.lblUpdateStatus.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUpdateStatus.Location = New System.Drawing.Point(13, 13)
        Me.lblUpdateStatus.Name = "lblUpdateStatus"
        Me.lblUpdateStatus.Size = New System.Drawing.Size(210, 13)
        Me.lblUpdateStatus.TabIndex = 0
        Me.lblUpdateStatus.Text = "Status: Attempting to obtain update file..."
        '
        'clvUpdates
        '
        Me.clvUpdates.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clvUpdates.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colComponent, Me.colFunction, Me.colVersion, Me.colAvailable, Me.colDownload, Me.colProgress})
        Me.clvUpdates.DefaultItemHeight = 20
        Me.clvUpdates.Location = New System.Drawing.Point(12, 30)
        Me.clvUpdates.Name = "clvUpdates"
        Me.clvUpdates.Size = New System.Drawing.Size(770, 554)
        Me.clvUpdates.TabIndex = 4
        '
        'colComponent
        '
        Me.colComponent.CustomSortTag = Nothing
        Me.colComponent.Tag = Nothing
        Me.colComponent.Text = "Component"
        Me.colComponent.Width = 200
        '
        'colFunction
        '
        Me.colFunction.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colFunction.CustomSortTag = Nothing
        Me.colFunction.DisplayIndex = 1
        Me.colFunction.Tag = Nothing
        Me.colFunction.Text = "Function"
        '
        'colVersion
        '
        Me.colVersion.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colVersion.CustomSortTag = Nothing
        Me.colVersion.DisplayIndex = 2
        Me.colVersion.Tag = Nothing
        Me.colVersion.Text = "Version"
        '
        'colAvailable
        '
        Me.colAvailable.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colAvailable.CustomSortTag = Nothing
        Me.colAvailable.DisplayIndex = 3
        Me.colAvailable.Tag = Nothing
        Me.colAvailable.Text = "Available"
        '
        'colDownload
        '
        Me.colDownload.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colDownload.CustomSortTag = Nothing
        Me.colDownload.DisplayIndex = 4
        Me.colDownload.Tag = Nothing
        Me.colDownload.Text = "Download?"
        Me.colDownload.Width = 70
        '
        'colProgress
        '
        Me.colProgress.CustomSortTag = Nothing
        Me.colProgress.DisplayIndex = 5
        Me.colProgress.Tag = Nothing
        Me.colProgress.Text = "Progress"
        Me.colProgress.Width = 200
        '
        'btnStartUpdate
        '
        Me.btnStartUpdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStartUpdate.Enabled = False
        Me.btnStartUpdate.Location = New System.Drawing.Point(682, 590)
        Me.btnStartUpdate.Name = "btnStartUpdate"
        Me.btnStartUpdate.Size = New System.Drawing.Size(100, 22)
        Me.btnStartUpdate.TabIndex = 5
        Me.btnStartUpdate.Text = "Start Update"
        Me.btnStartUpdate.UseVisualStyleBackColor = True
        '
        'tmrUpdate
        '
        '
        'btnRecheckUpdates
        '
        Me.btnRecheckUpdates.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRecheckUpdates.Enabled = False
        Me.btnRecheckUpdates.Location = New System.Drawing.Point(576, 590)
        Me.btnRecheckUpdates.Name = "btnRecheckUpdates"
        Me.btnRecheckUpdates.Size = New System.Drawing.Size(100, 22)
        Me.btnRecheckUpdates.TabIndex = 6
        Me.btnRecheckUpdates.Text = "Check Updates"
        Me.btnRecheckUpdates.UseVisualStyleBackColor = True
        '
        'frmUpdater
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(794, 624)
        Me.Controls.Add(Me.btnRecheckUpdates)
        Me.Controls.Add(Me.btnStartUpdate)
        Me.Controls.Add(Me.clvUpdates)
        Me.Controls.Add(Me.lblUpdateStatus)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmUpdater"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "EveHQ Updater"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblUpdateStatus As System.Windows.Forms.Label
    Friend WithEvents clvUpdates As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colComponent As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colFunction As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colVersion As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colAvailable As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colProgress As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents btnStartUpdate As System.Windows.Forms.Button
    Friend WithEvents tmrUpdate As System.Windows.Forms.Timer
    Friend WithEvents btnRecheckUpdates As System.Windows.Forms.Button
    Friend WithEvents colDownload As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
End Class
