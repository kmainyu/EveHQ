<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DBCEveSkillQueueInfoConfig
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DBCEveSkillQueueInfoConfig))
        Me.lblDefaultPilot = New DevComponents.DotNetBar.LabelX
        Me.cboPilots = New DevComponents.DotNetBar.Controls.ComboBoxEx
        Me.btnAccept = New DevComponents.DotNetBar.ButtonX
        Me.btnCancel = New DevComponents.DotNetBar.ButtonX
        Me.SuspendLayout()
        '
        'lblDefaultPilot
        '
        Me.lblDefaultPilot.AutoSize = True
        '
        '
        '
        Me.lblDefaultPilot.BackgroundStyle.Class = ""
        Me.lblDefaultPilot.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
        Me.lblDefaultPilot.Location = New System.Drawing.Point(12, 17)
        Me.lblDefaultPilot.Name = "lblDefaultPilot"
        Me.lblDefaultPilot.Size = New System.Drawing.Size(65, 16)
        Me.lblDefaultPilot.TabIndex = 8
        Me.lblDefaultPilot.Text = "Default Pilot:"
        '
        'cboPilots
        '
        Me.cboPilots.DisplayMember = "Text"
        Me.cboPilots.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.ItemHeight = 15
        Me.cboPilots.Location = New System.Drawing.Point(84, 12)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(198, 21)
        Me.cboPilots.Sorted = True
        Me.cboPilots.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.cboPilots.TabIndex = 9
        '
        'btnAccept
        '
        Me.btnAccept.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnAccept.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnAccept.Location = New System.Drawing.Point(126, 48)
        Me.btnAccept.Name = "btnAccept"
        Me.btnAccept.Size = New System.Drawing.Size(75, 23)
        Me.btnAccept.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnAccept.TabIndex = 10
        Me.btnAccept.Text = "Accept"
        '
        'btnCancel
        '
        Me.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton
        Me.btnCancel.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground
        Me.btnCancel.Location = New System.Drawing.Point(207, 48)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
        Me.btnCancel.TabIndex = 0
        Me.btnCancel.Text = "Cancel"
        '
        'DBCEveSkillQueueInfoConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(294, 81)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnAccept)
        Me.Controls.Add(Me.cboPilots)
        Me.Controls.Add(Me.lblDefaultPilot)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "DBCEveSkillQueueInfoConfig"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Eve Skill Queue Info Configuration"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ' Load the combo box with the pilot info
        cboPilots.BeginUpdate()
        cboPilots.Items.Clear()
        For Each pilot As EveHQ.Core.EveHQPilot In EveHQ.Core.HQ.Settings.Pilots.Values
            If pilot.Active = True Then
                cboPilots.Items.Add(pilot.Name)
            End If
        Next
        cboPilots.EndUpdate()

    End Sub
    Friend WithEvents lblDefaultPilot As DevComponents.DotNetBar.LabelX
    Friend WithEvents cboPilots As DevComponents.DotNetBar.Controls.ComboBoxEx
    Friend WithEvents btnAccept As DevComponents.DotNetBar.ButtonX
    Friend WithEvents btnCancel As DevComponents.DotNetBar.ButtonX
End Class
