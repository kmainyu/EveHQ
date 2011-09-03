<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCapSim
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
        Me.components = New System.ComponentModel.Container
        Me.PanelEx1 = New DevComponents.DotNetBar.PanelEx
        Me.btnExport = New DevComponents.DotNetBar.ButtonX
        Me.btnReset = New DevComponents.DotNetBar.ButtonX
        Me.zgcCapacitor = New ZedGraph.ZedGraphControl
        Me.btnUpdateEvents = New DevComponents.DotNetBar.ButtonX
        Me.iiEndTime = New DevComponents.Editors.IntegerInput
        Me.iiStartTime = New DevComponents.Editors.IntegerInput
        Me.lblEndTimeOffset = New DevComponents.DotNetBar.LabelX
        Me.lblStartTimeOffset = New DevComponents.DotNetBar.LabelX
        Me.gpResults = New DevComponents.DotNetBar.Controls.GroupPanel
        Me.lvwResults = New DevComponents.DotNetBar.Controls.ListViewEx
        Me.colTime = New System.Windows.Forms.ColumnHeader
        Me.colEvent = New System.Windows.Forms.ColumnHeader
        Me.colStartCap = New System.Windows.Forms.ColumnHeader
        Me.colCapCost = New System.Windows.Forms.ColumnHeader
        Me.colEndCap = New System.Windows.Forms.ColumnHeader
        Me.colCapRatio = New System.Windows.Forms.ColumnHeader
        Me.colCapRate = New System.Windows.Forms.ColumnHeader
        Me.gpModuleList = New DevComponents.DotNetBar.Controls.GroupPanel
        Me.lvwModules = New DevComponents.DotNetBar.Controls.ListViewEx
        Me.colModuleName = New System.Windows.Forms.ColumnHeader
        Me.colModuleTime = New System.Windows.Forms.ColumnHeader
        Me.colModuleCost = New System.Windows.Forms.ColumnHeader
        Me.colModuleRate = New System.Windows.Forms.ColumnHeader
        Me.gpCapStats = New DevComponents.DotNetBar.Controls.GroupPanel
        Me.lblStability = New DevComponents.DotNetBar.LabelX
        Me.lblPeakDelta = New DevComponents.DotNetBar.LabelX
        Me.lblPeakRate = New DevComponents.DotNetBar.LabelX
        Me.lblPeakIn = New DevComponents.DotNetBar.LabelX
        Me.lblPeakOut = New DevComponents.DotNetBar.LabelX
        Me.lblRecharge = New DevComponents.DotNetBar.LabelX
        Me.lblCapacity = New DevComponents.DotNetBar.LabelX
        Me.PanelEx1.SuspendLayout()
        CType(Me.iiEndTime, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.iiStartTime, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gpResults.SuspendLayout()
        Me.gpModuleList.SuspendLayout()
        Me.gpCapStats.SuspendLayout()
        Me.SuspendLayout()
        '
        'PanelEx1
        '
        Me.PanelEx1.CanvasColor = System.Drawing.SystemColors.Control
        Me.PanelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.PanelEx1.Controls.Add(Me.btnExport)
        Me.PanelEx1.Controls.Add(Me.btnReset)
        Me.PanelEx1.Controls.Add(Me.zgcCapacitor)
        Me.PanelEx1.Controls.Add(Me.btnUpdateEvents)
        Me.PanelEx1.Controls.Add(Me.iiEndTime)
        Me.PanelEx1.Controls.Add(Me.iiStartTime)
        Me.PanelEx1.Controls.Add(Me.lblEndTimeOffset)
        Me.PanelEx1.Controls.Add(Me.lblStartTimeOffset)
        Me.PanelEx1.Controls.Add(Me.gpResults)
        Me.PanelEx1.Controls.Add(Me.gpModuleList)
        Me.PanelEx1.Controls.Add(Me.gpCapStats)
        Me.PanelEx1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PanelEx1.Location = New System.Drawing.Point(0, 0)
        Me.PanelEx1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.PanelEx1.Name = "PanelEx1"
        Me.PanelEx1.Size = New System.Drawing.Size(794, 576)
        Me.PanelEx1.Style.Alignment = System.Drawing.StringAlignment.Center
        Me.PanelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.PanelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.PanelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine
        Me.PanelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.PanelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.PanelEx1.Style.GradientAngle = 90
        Me.PanelEx1.TabIndex = 0
        '
        'btnExport
        '
        Me.btnExport.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnExport.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnExport.Location = New System.Drawing.Point(681, 318)
        Me.btnExport.Name = "btnExport"
        Me.btnExport.Size = New System.Drawing.Size(101, 21)
        Me.btnExport.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnExport.TabIndex = 10
        Me.btnExport.Text = "Export to Clipboard"
        '
        'btnReset
        '
        Me.btnReset.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnReset.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnReset.Location = New System.Drawing.Point(734, 291)
        Me.btnReset.Name = "btnReset"
        Me.btnReset.Size = New System.Drawing.Size(48, 21)
        Me.btnReset.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnReset.TabIndex = 9
        Me.btnReset.Text = "Reset"
        '
        'zgcCapacitor
        '
        Me.zgcCapacitor.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.zgcCapacitor.Location = New System.Drawing.Point(0, 352)
        Me.zgcCapacitor.Name = "zgcCapacitor"
        Me.zgcCapacitor.ScrollGrace = 0
        Me.zgcCapacitor.ScrollMaxX = 0
        Me.zgcCapacitor.ScrollMaxY = 0
        Me.zgcCapacitor.ScrollMaxY2 = 0
        Me.zgcCapacitor.ScrollMinX = 0
        Me.zgcCapacitor.ScrollMinY = 0
        Me.zgcCapacitor.ScrollMinY2 = 0
        Me.zgcCapacitor.Size = New System.Drawing.Size(794, 224)
        Me.zgcCapacitor.TabIndex = 8
        '
        'btnUpdateEvents
        '
        Me.btnUpdateEvents.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnUpdateEvents.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnUpdateEvents.Location = New System.Drawing.Point(681, 291)
        Me.btnUpdateEvents.Name = "btnUpdateEvents"
        Me.btnUpdateEvents.Size = New System.Drawing.Size(48, 21)
        Me.btnUpdateEvents.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnUpdateEvents.TabIndex = 7
        Me.btnUpdateEvents.Text = "Update"
        '
        'iiEndTime
        '
        '
        '
        '
        Me.iiEndTime.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.iiEndTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.iiEndTime.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
        Me.iiEndTime.Location = New System.Drawing.Point(681, 264)
        Me.iiEndTime.Name = "iiEndTime"
        Me.iiEndTime.ShowUpDown = True
        Me.iiEndTime.Size = New System.Drawing.Size(101, 21)
        Me.iiEndTime.TabIndex = 6
        '
        'iiStartTime
        '
        '
        '
        '
        Me.iiStartTime.BackgroundStyle.Class = "DateTimeInputBackground"
        Me.iiStartTime.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.iiStartTime.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
        Me.iiStartTime.Location = New System.Drawing.Point(681, 220)
        Me.iiStartTime.Name = "iiStartTime"
        Me.iiStartTime.ShowUpDown = True
        Me.iiStartTime.Size = New System.Drawing.Size(101, 21)
        Me.iiStartTime.TabIndex = 5
        '
        'lblEndTimeOffset
        '
        Me.lblEndTimeOffset.AutoSize = True
        '
        '
        '
        Me.lblEndTimeOffset.BackgroundStyle.Class = ""
        Me.lblEndTimeOffset.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblEndTimeOffset.Location = New System.Drawing.Point(681, 247)
        Me.lblEndTimeOffset.Name = "lblEndTimeOffset"
        Me.lblEndTimeOffset.Size = New System.Drawing.Size(85, 16)
        Me.lblEndTimeOffset.TabIndex = 4
        Me.lblEndTimeOffset.Text = "End Time Offset:"
        '
        'lblStartTimeOffset
        '
        Me.lblStartTimeOffset.AutoSize = True
        '
        '
        '
        Me.lblStartTimeOffset.BackgroundStyle.Class = ""
        Me.lblStartTimeOffset.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblStartTimeOffset.Location = New System.Drawing.Point(681, 203)
        Me.lblStartTimeOffset.Name = "lblStartTimeOffset"
        Me.lblStartTimeOffset.Size = New System.Drawing.Size(90, 16)
        Me.lblStartTimeOffset.TabIndex = 3
        Me.lblStartTimeOffset.Text = "Start Time Offset:"
        '
        'gpResults
        '
        Me.gpResults.CanvasColor = System.Drawing.SystemColors.Control
        Me.gpResults.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.gpResults.Controls.Add(Me.lvwResults)
        Me.gpResults.Location = New System.Drawing.Point(12, 193)
        Me.gpResults.Name = "gpResults"
        Me.gpResults.Size = New System.Drawing.Size(663, 153)
        '
        '
        '
        Me.gpResults.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.gpResults.Style.BackColorGradientAngle = 90
        Me.gpResults.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.gpResults.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpResults.Style.BorderBottomWidth = 1
        Me.gpResults.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.gpResults.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpResults.Style.BorderLeftWidth = 1
        Me.gpResults.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpResults.Style.BorderRightWidth = 1
        Me.gpResults.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpResults.Style.BorderTopWidth = 1
        Me.gpResults.Style.Class = ""
        Me.gpResults.Style.CornerDiameter = 4
        Me.gpResults.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpResults.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpResults.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpResults.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpResults.StyleMouseDown.Class = ""
        Me.gpResults.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.gpResults.StyleMouseOver.Class = ""
        Me.gpResults.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.gpResults.TabIndex = 2
        Me.gpResults.Text = "Simulation Results"
        '
        'lvwResults
        '
        Me.lvwResults.Activation = System.Windows.Forms.ItemActivation.OneClick
        '
        '
        '
        Me.lvwResults.Border.Class = "ListViewBorder"
        Me.lvwResults.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lvwResults.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colTime, Me.colEvent, Me.colStartCap, Me.colCapCost, Me.colEndCap, Me.colCapRatio, Me.colCapRate})
        Me.lvwResults.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwResults.FullRowSelect = True
        Me.lvwResults.GridLines = True
        Me.lvwResults.Location = New System.Drawing.Point(0, 0)
        Me.lvwResults.Name = "lvwResults"
        Me.lvwResults.Size = New System.Drawing.Size(657, 131)
        Me.lvwResults.TabIndex = 0
        Me.lvwResults.UseCompatibleStateImageBehavior = False
        Me.lvwResults.View = System.Windows.Forms.View.Details
        '
        'colTime
        '
        Me.colTime.Text = "Time Offset"
        Me.colTime.Width = 80
        '
        'colEvent
        '
        Me.colEvent.Text = "Event Type"
        Me.colEvent.Width = 200
        '
        'colStartCap
        '
        Me.colStartCap.Text = "Start Cap"
        Me.colStartCap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colStartCap.Width = 80
        '
        'colCapCost
        '
        Me.colCapCost.Text = "Cap Amount"
        Me.colCapCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colCapCost.Width = 80
        '
        'colEndCap
        '
        Me.colEndCap.Text = "End Cap"
        Me.colEndCap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colEndCap.Width = 80
        '
        'colCapRatio
        '
        Me.colCapRatio.Text = "Cap %"
        Me.colCapRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colCapRatio.Width = 50
        '
        'colCapRate
        '
        Me.colCapRate.Text = "Rechg"
        Me.colCapRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'gpModuleList
        '
        Me.gpModuleList.CanvasColor = System.Drawing.SystemColors.Control
        Me.gpModuleList.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.gpModuleList.Controls.Add(Me.lvwModules)
        Me.gpModuleList.Location = New System.Drawing.Point(240, 12)
        Me.gpModuleList.Name = "gpModuleList"
        Me.gpModuleList.Size = New System.Drawing.Size(542, 176)
        '
        '
        '
        Me.gpModuleList.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.gpModuleList.Style.BackColorGradientAngle = 90
        Me.gpModuleList.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.gpModuleList.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpModuleList.Style.BorderBottomWidth = 1
        Me.gpModuleList.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.gpModuleList.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpModuleList.Style.BorderLeftWidth = 1
        Me.gpModuleList.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpModuleList.Style.BorderRightWidth = 1
        Me.gpModuleList.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpModuleList.Style.BorderTopWidth = 1
        Me.gpModuleList.Style.Class = ""
        Me.gpModuleList.Style.CornerDiameter = 4
        Me.gpModuleList.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpModuleList.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpModuleList.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpModuleList.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpModuleList.StyleMouseDown.Class = ""
        Me.gpModuleList.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.gpModuleList.StyleMouseOver.Class = ""
        Me.gpModuleList.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.gpModuleList.TabIndex = 1
        Me.gpModuleList.Text = "Modules Affecting Capacitor"
        '
        'lvwModules
        '
        Me.lvwModules.Activation = System.Windows.Forms.ItemActivation.OneClick
        '
        '
        '
        Me.lvwModules.Border.Class = "ListViewBorder"
        Me.lvwModules.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lvwModules.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colModuleName, Me.colModuleTime, Me.colModuleCost, Me.colModuleRate})
        Me.lvwModules.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvwModules.FullRowSelect = True
        Me.lvwModules.GridLines = True
        Me.lvwModules.Location = New System.Drawing.Point(0, 0)
        Me.lvwModules.Name = "lvwModules"
        Me.lvwModules.Size = New System.Drawing.Size(536, 154)
        Me.lvwModules.TabIndex = 0
        Me.lvwModules.UseCompatibleStateImageBehavior = False
        Me.lvwModules.View = System.Windows.Forms.View.Details
        '
        'colModuleName
        '
        Me.colModuleName.Text = "Module Name"
        Me.colModuleName.Width = 200
        '
        'colModuleTime
        '
        Me.colModuleTime.Text = "Cycle Time"
        Me.colModuleTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colModuleTime.Width = 100
        '
        'colModuleCost
        '
        Me.colModuleCost.Text = "Activation Cost"
        Me.colModuleCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colModuleCost.Width = 100
        '
        'colModuleRate
        '
        Me.colModuleRate.Text = "Rate (GJ/s)"
        Me.colModuleRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.colModuleRate.Width = 100
        '
        'gpCapStats
        '
        Me.gpCapStats.CanvasColor = System.Drawing.SystemColors.Control
        Me.gpCapStats.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007
        Me.gpCapStats.Controls.Add(Me.lblStability)
        Me.gpCapStats.Controls.Add(Me.lblPeakDelta)
        Me.gpCapStats.Controls.Add(Me.lblPeakRate)
        Me.gpCapStats.Controls.Add(Me.lblPeakIn)
        Me.gpCapStats.Controls.Add(Me.lblPeakOut)
        Me.gpCapStats.Controls.Add(Me.lblRecharge)
        Me.gpCapStats.Controls.Add(Me.lblCapacity)
        Me.gpCapStats.Location = New System.Drawing.Point(12, 11)
        Me.gpCapStats.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.gpCapStats.Name = "gpCapStats"
        Me.gpCapStats.Size = New System.Drawing.Size(222, 177)
        '
        '
        '
        Me.gpCapStats.Style.BackColor2SchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground2
        Me.gpCapStats.Style.BackColorGradientAngle = 90
        Me.gpCapStats.Style.BackColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground
        Me.gpCapStats.Style.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpCapStats.Style.BorderBottomWidth = 1
        Me.gpCapStats.Style.BorderColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder
        Me.gpCapStats.Style.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpCapStats.Style.BorderLeftWidth = 1
        Me.gpCapStats.Style.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpCapStats.Style.BorderRightWidth = 1
        Me.gpCapStats.Style.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid
        Me.gpCapStats.Style.BorderTopWidth = 1
        Me.gpCapStats.Style.Class = ""
        Me.gpCapStats.Style.CornerDiameter = 4
        Me.gpCapStats.Style.CornerType = DevComponents.DotNetBar.eCornerType.Rounded
        Me.gpCapStats.Style.TextAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Center
        Me.gpCapStats.Style.TextColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText
        Me.gpCapStats.Style.TextLineAlignment = DevComponents.DotNetBar.eStyleTextAlignment.Near
        '
        '
        '
        Me.gpCapStats.StyleMouseDown.Class = ""
        Me.gpCapStats.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square
        '
        '
        '
        Me.gpCapStats.StyleMouseOver.Class = ""
        Me.gpCapStats.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.gpCapStats.TabIndex = 0
        Me.gpCapStats.Text = "Capacitor Summary Statistics"
        '
        'lblStability
        '
        Me.lblStability.AutoSize = True
        '
        '
        '
        Me.lblStability.BackgroundStyle.Class = ""
        Me.lblStability.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblStability.Location = New System.Drawing.Point(3, 132)
        Me.lblStability.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.lblStability.Name = "lblStability"
        Me.lblStability.Size = New System.Drawing.Size(45, 16)
        Me.lblStability.TabIndex = 6
        Me.lblStability.Text = "Stability:"
        '
        'lblPeakDelta
        '
        Me.lblPeakDelta.AutoSize = True
        '
        '
        '
        Me.lblPeakDelta.BackgroundStyle.Class = ""
        Me.lblPeakDelta.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblPeakDelta.Location = New System.Drawing.Point(3, 112)
        Me.lblPeakDelta.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.lblPeakDelta.Name = "lblPeakDelta"
        Me.lblPeakDelta.Size = New System.Drawing.Size(58, 16)
        Me.lblPeakDelta.TabIndex = 5
        Me.lblPeakDelta.Text = "Peak Delta:"
        '
        'lblPeakRate
        '
        Me.lblPeakRate.AutoSize = True
        '
        '
        '
        Me.lblPeakRate.BackgroundStyle.Class = ""
        Me.lblPeakRate.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblPeakRate.Location = New System.Drawing.Point(3, 52)
        Me.lblPeakRate.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.lblPeakRate.Name = "lblPeakRate"
        Me.lblPeakRate.Size = New System.Drawing.Size(104, 16)
        Me.lblPeakRate.TabIndex = 4
        Me.lblPeakRate.Text = "Peak Recharge Rate:"
        '
        'lblPeakIn
        '
        Me.lblPeakIn.AutoSize = True
        '
        '
        '
        Me.lblPeakIn.BackgroundStyle.Class = ""
        Me.lblPeakIn.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblPeakIn.Location = New System.Drawing.Point(3, 72)
        Me.lblPeakIn.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.lblPeakIn.Name = "lblPeakIn"
        Me.lblPeakIn.Size = New System.Drawing.Size(43, 16)
        Me.lblPeakIn.TabIndex = 3
        Me.lblPeakIn.Text = "Peak In:"
        '
        'lblPeakOut
        '
        Me.lblPeakOut.AutoSize = True
        '
        '
        '
        Me.lblPeakOut.BackgroundStyle.Class = ""
        Me.lblPeakOut.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblPeakOut.Location = New System.Drawing.Point(3, 92)
        Me.lblPeakOut.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.lblPeakOut.Name = "lblPeakOut"
        Me.lblPeakOut.Size = New System.Drawing.Size(51, 16)
        Me.lblPeakOut.TabIndex = 2
        Me.lblPeakOut.Text = "Peak Out:"
        '
        'lblRecharge
        '
        Me.lblRecharge.AutoSize = True
        '
        '
        '
        Me.lblRecharge.BackgroundStyle.Class = ""
        Me.lblRecharge.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblRecharge.Location = New System.Drawing.Point(3, 32)
        Me.lblRecharge.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.lblRecharge.Name = "lblRecharge"
        Me.lblRecharge.Size = New System.Drawing.Size(79, 16)
        Me.lblRecharge.TabIndex = 1
        Me.lblRecharge.Text = "Recharge Time:"
        '
        'lblCapacity
        '
        Me.lblCapacity.AutoSize = True
        '
        '
        '
        Me.lblCapacity.BackgroundStyle.Class = ""
        Me.lblCapacity.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblCapacity.Location = New System.Drawing.Point(3, 12)
        Me.lblCapacity.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.lblCapacity.Name = "lblCapacity"
        Me.lblCapacity.Size = New System.Drawing.Size(47, 16)
        Me.lblCapacity.TabIndex = 0
        Me.lblCapacity.Text = "Capacity:"
        '
        'frmCapSim
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(794, 576)
        Me.Controls.Add(Me.PanelEx1)
        Me.DoubleBuffered = True
        Me.EnableGlass = False
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "frmCapSim"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Capacitor Simulation Results"
        Me.PanelEx1.ResumeLayout(False)
        Me.PanelEx1.PerformLayout()
        CType(Me.iiEndTime, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.iiStartTime, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gpResults.ResumeLayout(False)
        Me.gpModuleList.ResumeLayout(False)
        Me.gpCapStats.ResumeLayout(False)
        Me.gpCapStats.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PanelEx1 As DevComponents.DotNetBar.PanelEx
    Friend WithEvents gpCapStats As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents lblPeakDelta As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblPeakRate As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblPeakIn As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblPeakOut As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblRecharge As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblCapacity As DevComponents.DotNetBar.LabelX
    Friend WithEvents gpModuleList As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents lvwModules As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents colModuleName As System.Windows.Forms.ColumnHeader
    Friend WithEvents colModuleTime As System.Windows.Forms.ColumnHeader
    Friend WithEvents colModuleCost As System.Windows.Forms.ColumnHeader
    Friend WithEvents colModuleRate As System.Windows.Forms.ColumnHeader
    Friend WithEvents gpResults As DevComponents.DotNetBar.Controls.GroupPanel
    Friend WithEvents lvwResults As DevComponents.DotNetBar.Controls.ListViewEx
    Friend WithEvents colTime As System.Windows.Forms.ColumnHeader
    Friend WithEvents colEvent As System.Windows.Forms.ColumnHeader
    Friend WithEvents colStartCap As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCapCost As System.Windows.Forms.ColumnHeader
    Friend WithEvents colEndCap As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCapRatio As System.Windows.Forms.ColumnHeader
    Friend WithEvents colCapRate As System.Windows.Forms.ColumnHeader
    Friend WithEvents iiEndTime As DevComponents.Editors.IntegerInput
    Friend WithEvents iiStartTime As DevComponents.Editors.IntegerInput
    Friend WithEvents lblEndTimeOffset As DevComponents.DotNetBar.LabelX
    Friend WithEvents lblStartTimeOffset As DevComponents.DotNetBar.LabelX
    Friend WithEvents btnUpdateEvents As DevComponents.DotNetBar.ButtonX
    Friend WithEvents lblStability As DevComponents.DotNetBar.LabelX
    Friend WithEvents zgcCapacitor As ZedGraph.ZedGraphControl
    Friend WithEvents btnReset As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnExport As DevComponents.DotNetBar.ButtonX
End Class
