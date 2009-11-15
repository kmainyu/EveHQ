<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmShipComparison
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
        Me.lblPilot = New System.Windows.Forms.Label
        Me.lblDamageProfile = New System.Windows.Forms.Label
        Me.cboPilots = New System.Windows.Forms.ComboBox
        Me.cboProfiles = New System.Windows.Forms.ComboBox
        Me.clvShips = New DotNetLib.Windows.Forms.ContainerListView
        Me.colQuery = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colFitting = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colEHP = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colTank = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colCapacitor = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colVolley = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colDPS = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colSEM = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colSEx = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colSKi = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colSTh = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colAEM = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colAEx = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colAKi = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.colATh = New DotNetLib.Windows.Forms.ContainerListViewColumnHeader
        Me.btnCopy = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        'lblPilot
        '
        Me.lblPilot.AutoSize = True
        Me.lblPilot.Location = New System.Drawing.Point(12, 15)
        Me.lblPilot.Name = "lblPilot"
        Me.lblPilot.Size = New System.Drawing.Size(31, 13)
        Me.lblPilot.TabIndex = 0
        Me.lblPilot.Text = "Pilot:"
        '
        'lblDamageProfile
        '
        Me.lblDamageProfile.AutoSize = True
        Me.lblDamageProfile.Location = New System.Drawing.Point(12, 42)
        Me.lblDamageProfile.Name = "lblDamageProfile"
        Me.lblDamageProfile.Size = New System.Drawing.Size(83, 13)
        Me.lblDamageProfile.TabIndex = 1
        Me.lblDamageProfile.Text = "Damage Profile:"
        '
        'cboPilots
        '
        Me.cboPilots.FormattingEnabled = True
        Me.cboPilots.Location = New System.Drawing.Point(100, 12)
        Me.cboPilots.Name = "cboPilots"
        Me.cboPilots.Size = New System.Drawing.Size(251, 21)
        Me.cboPilots.TabIndex = 2
        '
        'cboProfiles
        '
        Me.cboProfiles.FormattingEnabled = True
        Me.cboProfiles.Location = New System.Drawing.Point(100, 39)
        Me.cboProfiles.Name = "cboProfiles"
        Me.cboProfiles.Size = New System.Drawing.Size(251, 21)
        Me.cboProfiles.TabIndex = 3
        '
        'clvShips
        '
        Me.clvShips.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clvShips.Columns.AddRange(New DotNetLib.Windows.Forms.ContainerListViewColumnHeader() {Me.colQuery, Me.colFitting, Me.colEHP, Me.colTank, Me.colCapacitor, Me.colVolley, Me.colDPS, Me.colSEM, Me.colSEx, Me.colSKi, Me.colSTh, Me.colAEM, Me.colAEx, Me.colAKi, Me.colATh})
        Me.clvShips.DefaultItemHeight = 20
        Me.clvShips.HeaderHeight = 32
        Me.clvShips.Location = New System.Drawing.Point(12, 68)
        Me.clvShips.MultipleColumnSort = True
        Me.clvShips.Name = "clvShips"
        Me.clvShips.Size = New System.Drawing.Size(788, 494)
        Me.clvShips.TabIndex = 4
        '
        'colQuery
        '
        Me.colQuery.ContentAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.colQuery.CustomSortTag = Nothing
        Me.colQuery.Tag = Nothing
        Me.colQuery.Width = 25
        '
        'colFitting
        '
        Me.colFitting.CustomSortTag = Nothing
        Me.colFitting.DisplayIndex = 1
        Me.colFitting.SortDataType = DotNetLib.Windows.Forms.SortDataType.[String]
        Me.colFitting.Tag = Nothing
        Me.colFitting.Text = "Fitting"
        Me.colFitting.Width = 200
        '
        'colEHP
        '
        Me.colEHP.CustomSortTag = Nothing
        Me.colEHP.DisplayIndex = 2
        Me.colEHP.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colEHP.Tag = Nothing
        Me.colEHP.Text = "EHP"
        Me.colEHP.Width = 75
        '
        'colTank
        '
        Me.colTank.CustomSortTag = Nothing
        Me.colTank.DisplayIndex = 3
        Me.colTank.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colTank.Tag = Nothing
        Me.colTank.Text = "Tank"
        Me.colTank.Width = 75
        '
        'colCapacitor
        '
        Me.colCapacitor.CustomSortTag = Nothing
        Me.colCapacitor.DisplayIndex = 4
        Me.colCapacitor.SortDataType = DotNetLib.Windows.Forms.SortDataType.Tag
        Me.colCapacitor.Tag = Nothing
        Me.colCapacitor.Text = "Capacitor"
        Me.colCapacitor.Width = 100
        '
        'colVolley
        '
        Me.colVolley.CustomSortTag = Nothing
        Me.colVolley.DisplayIndex = 5
        Me.colVolley.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colVolley.Tag = Nothing
        Me.colVolley.Text = "Volley"
        Me.colVolley.Width = 75
        '
        'colDPS
        '
        Me.colDPS.CustomSortTag = Nothing
        Me.colDPS.DisplayIndex = 6
        Me.colDPS.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colDPS.Tag = Nothing
        Me.colDPS.Text = "DPS"
        Me.colDPS.Width = 50
        '
        'colSEM
        '
        Me.colSEM.CustomSortTag = Nothing
        Me.colSEM.DisplayIndex = 7
        Me.colSEM.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colSEM.Tag = Nothing
        Me.colSEM.Text = "S EM"
        Me.colSEM.Width = 50
        '
        'colSEx
        '
        Me.colSEx.CustomSortTag = Nothing
        Me.colSEx.DisplayIndex = 8
        Me.colSEx.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colSEx.Tag = Nothing
        Me.colSEx.Text = "S Ex"
        Me.colSEx.Width = 50
        '
        'colSKi
        '
        Me.colSKi.CustomSortTag = Nothing
        Me.colSKi.DisplayIndex = 9
        Me.colSKi.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colSKi.Tag = Nothing
        Me.colSKi.Text = "S Ki"
        Me.colSKi.Width = 50
        '
        'colSTh
        '
        Me.colSTh.CustomSortTag = Nothing
        Me.colSTh.DisplayIndex = 10
        Me.colSTh.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colSTh.Tag = Nothing
        Me.colSTh.Text = "S Th"
        Me.colSTh.Width = 50
        '
        'colAEM
        '
        Me.colAEM.CustomSortTag = Nothing
        Me.colAEM.DisplayIndex = 11
        Me.colAEM.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colAEM.Tag = Nothing
        Me.colAEM.Text = "A EM"
        Me.colAEM.Width = 50
        '
        'colAEx
        '
        Me.colAEx.CustomSortTag = Nothing
        Me.colAEx.DisplayIndex = 12
        Me.colAEx.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colAEx.Tag = Nothing
        Me.colAEx.Text = "A Ex"
        Me.colAEx.Width = 50
        '
        'colAKi
        '
        Me.colAKi.CustomSortTag = Nothing
        Me.colAKi.DisplayIndex = 13
        Me.colAKi.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colAKi.Tag = Nothing
        Me.colAKi.Text = "A Ki"
        Me.colAKi.Width = 50
        '
        'colATh
        '
        Me.colATh.CustomSortTag = Nothing
        Me.colATh.DisplayIndex = 14
        Me.colATh.SortDataType = DotNetLib.Windows.Forms.SortDataType.[Double]
        Me.colATh.Tag = Nothing
        Me.colATh.Text = "A Th"
        Me.colATh.Width = 50
        '
        'btnCopy
        '
        Me.btnCopy.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnCopy.Location = New System.Drawing.Point(692, 39)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(108, 23)
        Me.btnCopy.TabIndex = 5
        Me.btnCopy.Text = "Copy To Clipboard"
        Me.btnCopy.UseVisualStyleBackColor = True
        '
        'frmShipComparison
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(812, 574)
        Me.Controls.Add(Me.btnCopy)
        Me.Controls.Add(Me.clvShips)
        Me.Controls.Add(Me.cboProfiles)
        Me.Controls.Add(Me.cboPilots)
        Me.Controls.Add(Me.lblDamageProfile)
        Me.Controls.Add(Me.lblPilot)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow
        Me.Name = "frmShipComparison"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "HQF Ship Comparison"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblPilot As System.Windows.Forms.Label
    Friend WithEvents lblDamageProfile As System.Windows.Forms.Label
    Friend WithEvents cboPilots As System.Windows.Forms.ComboBox
    Friend WithEvents cboProfiles As System.Windows.Forms.ComboBox
    Friend WithEvents clvShips As DotNetLib.Windows.Forms.ContainerListView
    Friend WithEvents colFitting As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colEHP As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colTank As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colCapacitor As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colVolley As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colDPS As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colSEM As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colSEx As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colSKi As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colSTh As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colAEM As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colAEx As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colAKi As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents colATh As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
    Friend WithEvents btnCopy As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents colQuery As DotNetLib.Windows.Forms.ContainerListViewColumnHeader
End Class
