<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmUpdater
    Inherits DevComponents.DotNetBar.Office2007Form

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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUpdater))
        Me.lblUpdateStatus = New System.Windows.Forms.Label()
        Me.btnStartUpdate = New System.Windows.Forms.Button()
        Me.tmrUpdate = New System.Windows.Forms.Timer(Me.components)
        Me.btnRecheckUpdates = New System.Windows.Forms.Button()
        Me.PanelEx1 = New DevComponents.DotNetBar.PanelEx()
        Me.btnCancelUpdate = New System.Windows.Forms.Button()
        Me.nudDownloads = New DevComponents.Editors.IntegerInput()
        Me.lblMaxConcurrentDownloads = New System.Windows.Forms.Label()
        Me.adtUpdates = New DevComponents.AdvTree.AdvTree()
        Me.colComponent = New DevComponents.AdvTree.ColumnHeader()
        Me.colFunction = New DevComponents.AdvTree.ColumnHeader()
        Me.colVersion = New DevComponents.AdvTree.ColumnHeader()
        Me.colAvailable = New DevComponents.AdvTree.ColumnHeader()
        Me.colDownload = New DevComponents.AdvTree.ColumnHeader()
        Me.colProgressMain = New DevComponents.AdvTree.ColumnHeader()
        Me.colProgressPDB = New DevComponents.AdvTree.ColumnHeader()
        Me.NodeConnector1 = New DevComponents.AdvTree.NodeConnector()
        Me.Log = New DevComponents.DotNetBar.ElementStyle()
        Me.LogCentre = New DevComponents.DotNetBar.ElementStyle()
        Me.Centre = New DevComponents.DotNetBar.ElementStyle()
        Me.Cell1 = New DevComponents.AdvTree.Cell()
        Me.CheckBoxItem1 = New DevComponents.DotNetBar.CheckBoxItem()
        Me.PanelEx1.SuspendLayout()
        CType(Me.nudDownloads, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.adtUpdates, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblUpdateStatus
        '
        Me.lblUpdateStatus.AutoSize = True
        Me.lblUpdateStatus.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUpdateStatus.Location = New System.Drawing.Point(3, 9)
        Me.lblUpdateStatus.Name = "lblUpdateStatus"
        Me.lblUpdateStatus.Size = New System.Drawing.Size(210, 13)
        Me.lblUpdateStatus.TabIndex = 0
        Me.lblUpdateStatus.Text = "Status: Attempting to obtain update file..."
        '
        'btnStartUpdate
        '
        Me.btnStartUpdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnStartUpdate.Enabled = False
        Me.btnStartUpdate.Location = New System.Drawing.Point(637, 393)
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
        Me.btnRecheckUpdates.Location = New System.Drawing.Point(531, 393)
        Me.btnRecheckUpdates.Name = "btnRecheckUpdates"
        Me.btnRecheckUpdates.Size = New System.Drawing.Size(100, 22)
        Me.btnRecheckUpdates.TabIndex = 6
        Me.btnRecheckUpdates.Text = "Check Updates"
        Me.btnRecheckUpdates.UseVisualStyleBackColor = True
        '
        'PanelEx1
        '
        Me.PanelEx1.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx1.Controls.Add(Me.btnCancelUpdate)
        Me.PanelEx1.Controls.Add(Me.nudDownloads)
        Me.PanelEx1.Controls.Add(Me.lblMaxConcurrentDownloads)
        Me.PanelEx1.Controls.Add(Me.adtUpdates)
        Me.PanelEx1.Controls.Add(Me.lblUpdateStatus)
        Me.PanelEx1.Controls.Add(Me.btnRecheckUpdates)
        Me.PanelEx1.Controls.Add(Me.btnStartUpdate)
        Me.PanelEx1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelEx1.Location = New System.Drawing.Point(0, 0)
        Me.PanelEx1.Name = "PanelEx1"
        Me.PanelEx1.Size = New System.Drawing.Size(744, 422)
        Me.PanelEx1.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx1.Style.GradientAngle = 90
        Me.PanelEx1.TabIndex = 7
        '
        'btnCancelUpdate
        '
        Me.btnCancelUpdate.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCancelUpdate.Enabled = False
        Me.btnCancelUpdate.Location = New System.Drawing.Point(425, 393)
        Me.btnCancelUpdate.Name = "btnCancelUpdate"
        Me.btnCancelUpdate.Size = New System.Drawing.Size(100, 22)
        Me.btnCancelUpdate.TabIndex = 10
        Me.btnCancelUpdate.Text = "Cancel Update"
        Me.btnCancelUpdate.UseVisualStyleBackColor = True
        '
        'nudDownloads
        '
        Me.nudDownloads.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        '
        '
        '
        Me.nudDownloads.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.nudDownloads.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.nudDownloads.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
        Me.nudDownloads.Location = New System.Drawing.Point(216, 394)
        Me.nudDownloads.MaxValue = 5
        Me.nudDownloads.MinValue = 1
        Me.nudDownloads.Name = "nudDownloads"
        Me.nudDownloads.ShowUpDown = True
        Me.nudDownloads.Size = New System.Drawing.Size(51, 21)
        Me.nudDownloads.TabIndex = 9
        Me.nudDownloads.Value = 5
        '
        'lblMaxConcurrentDownloads
        '
        Me.lblMaxConcurrentDownloads.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblMaxConcurrentDownloads.AutoSize = True
        Me.lblMaxConcurrentDownloads.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMaxConcurrentDownloads.Location = New System.Drawing.Point(3, 398)
        Me.lblMaxConcurrentDownloads.Name = "lblMaxConcurrentDownloads"
        Me.lblMaxConcurrentDownloads.Size = New System.Drawing.Size(207, 13)
        Me.lblMaxConcurrentDownloads.TabIndex = 8
        Me.lblMaxConcurrentDownloads.Text = "Maximum Concurrent Downloads (max 5):"
        '
        'adtUpdates
        '
        Me.adtUpdates.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline
        Me.adtUpdates.AllowDrop = True
        Me.adtUpdates.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.adtUpdates.BackColor = System.Drawing.SystemColors.Window
        '
        '
        '
        Me.adtUpdates.BackgroundStyle.Class = "TreeBorderKey"
        Me.adtUpdates.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.adtUpdates.Columns.Add(Me.colComponent)
        Me.adtUpdates.Columns.Add(Me.colFunction)
        Me.adtUpdates.Columns.Add(Me.colVersion)
        Me.adtUpdates.Columns.Add(Me.colAvailable)
        Me.adtUpdates.Columns.Add(Me.colDownload)
        Me.adtUpdates.Columns.Add(Me.colProgressMain)
        Me.adtUpdates.Columns.Add(Me.colProgressPDB)
        Me.adtUpdates.DoubleClickTogglesNode = False
        Me.adtUpdates.DragDropEnabled = False
        Me.adtUpdates.ExpandWidth = 0
        Me.adtUpdates.LicenseKey = "F962CEC7-CD8F-4911-A9E9-CAB39962FC1F"
        Me.adtUpdates.Location = New System.Drawing.Point(3, 26)
        Me.adtUpdates.Name = "adtUpdates"
        Me.adtUpdates.NodesConnector = Me.NodeConnector1
        Me.adtUpdates.NodeStyle = Me.Log
        Me.adtUpdates.PathSeparator = ";"
        Me.adtUpdates.Size = New System.Drawing.Size(734, 361)
        Me.adtUpdates.Styles.Add(Me.Log)
        Me.adtUpdates.Styles.Add(Me.LogCentre)
        Me.adtUpdates.Styles.Add(Me.Centre)
        Me.adtUpdates.TabIndex = 7
        Me.adtUpdates.Text = "AdvTree1"
        '
        'colComponent
        '
        Me.colComponent.Name = "colComponent"
        Me.colComponent.SortingEnabled = False
        Me.colComponent.Text = "Component"
        Me.colComponent.Width.Absolute = 175
        '
        'colFunction
        '
        Me.colFunction.Name = "colFunction"
        Me.colFunction.SortingEnabled = False
        Me.colFunction.StyleNormal = "LogCentre"
        Me.colFunction.Text = "Function"
        Me.colFunction.Width.Absolute = 75
        '
        'colVersion
        '
        Me.colVersion.Name = "colVersion"
        Me.colVersion.SortingEnabled = False
        Me.colVersion.StyleNormal = "LogCentre"
        Me.colVersion.Text = "Version"
        Me.colVersion.Width.Absolute = 75
        '
        'colAvailable
        '
        Me.colAvailable.Name = "colAvailable"
        Me.colAvailable.SortingEnabled = False
        Me.colAvailable.StyleNormal = "LogCentre"
        Me.colAvailable.Text = "Available"
        Me.colAvailable.Width.Absolute = 75
        '
        'colDownload
        '
        Me.colDownload.Name = "colDownload"
        Me.colDownload.SortingEnabled = False
        Me.colDownload.StyleNormal = "LogCentre"
        Me.colDownload.Text = "D/L?"
        Me.colDownload.Width.Absolute = 40
        '
        'colProgressMain
        '
        Me.colProgressMain.Name = "colProgressMain"
        Me.colProgressMain.SortingEnabled = False
        Me.colProgressMain.StyleNormal = "LogCentre"
        Me.colProgressMain.Text = "Main File Progress"
        Me.colProgressMain.Width.Absolute = 120
        '
        'colProgressPDB
        '
        Me.colProgressPDB.Name = "colProgressPDB"
        Me.colProgressPDB.SortingEnabled = False
        Me.colProgressPDB.StyleNormal = "LogCentre"
        Me.colProgressPDB.Text = "PDB File Progress"
        Me.colProgressPDB.Width.Absolute = 120
        '
        'NodeConnector1
        '
        Me.NodeConnector1.LineColor = System.Drawing.SystemColors.ControlText
        '
        'Log
        '
        Me.Log.Class = ""
        Me.Log.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.Log.Name = "Log"
        Me.Log.TextColor = System.Drawing.SystemColors.ControlText
        '
        'LogCentre
        '
        Me.LogCentre.Class = ""
        Me.LogCentre.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.LogCentre.Name = "LogCentre"
        Me.LogCentre.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.LogCentre.TextColor = System.Drawing.SystemColors.ControlText
        '
        'Centre
        '
        Me.Centre.Class = ""
        Me.Centre.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.Centre.Name = "Centre"
        Me.Centre.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        '
        'Cell1
        '
        Me.Cell1.HostedItem = Me.CheckBoxItem1
        Me.Cell1.Name = "Cell1"
        Me.Cell1.StyleMouseOver = Nothing
        Me.Cell1.StyleNormal = Me.LogCentre
        '
        'CheckBoxItem1
        '
        Me.CheckBoxItem1.CheckBoxPosition = DevComponents.DotNetBar.eCheckBoxPosition.Top
        Me.CheckBoxItem1.ItemAlignment = DevComponents.DotNetBar.eItemAlignment.Center
        Me.CheckBoxItem1.Name = "CheckBoxItem1"
        Me.CheckBoxItem1.Stretch = True
        '
        'frmUpdater
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(744, 422)
        Me.Controls.Add(Me.PanelEx1)
        Me.DoubleBuffered = True
        Me.EnableGlass = False
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "frmUpdater"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "EveHQ Updater"
        Me.PanelEx1.ResumeLayout(False)
        Me.PanelEx1.PerformLayout()
        CType(Me.nudDownloads, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.adtUpdates, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblUpdateStatus As System.Windows.Forms.Label
    Friend WithEvents btnStartUpdate As System.Windows.Forms.Button
    Friend WithEvents tmrUpdate As System.Windows.Forms.Timer
    Friend WithEvents btnRecheckUpdates As System.Windows.Forms.Button
    Friend WithEvents PanelEx1 As DevComponents.DotNetBar.PanelEx
    Friend WithEvents adtUpdates As DevComponents.AdvTree.AdvTree
    Friend WithEvents NodeConnector1 As DevComponents.AdvTree.NodeConnector
    Friend WithEvents Log As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents colComponent As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colFunction As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colVersion As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colAvailable As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colDownload As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents colProgressMain As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents LogCentre As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents Centre As DevComponents.DotNetBar.ElementStyle
    Friend WithEvents Cell1 As DevComponents.AdvTree.Cell
    Friend WithEvents CheckBoxItem1 As DevComponents.DotNetBar.CheckBoxItem
    Friend WithEvents colProgressPDB As DevComponents.AdvTree.ColumnHeader
    Friend WithEvents nudDownloads As DevComponents.Editors.IntegerInput
    Friend WithEvents lblMaxConcurrentDownloads As System.Windows.Forms.Label
    Friend WithEvents btnCancelUpdate As System.Windows.Forms.Button
End Class
