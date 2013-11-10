Namespace Controls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class PrismSelectionHostControl
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
            Me.cboHost = New DevComponents.DotNetBar.Controls.TextBoxDropDown()
            Me.SuspendLayout()
            '
            'cboHost
            '
            Me.cboHost.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None
            Me.cboHost.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None
            '
            '
            '
            Me.cboHost.BackgroundStyle.Class = "TextBoxBorder"
            Me.cboHost.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.cboHost.ButtonDropDown.Visible = True
            Me.cboHost.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal
            Me.cboHost.Dock = System.Windows.Forms.DockStyle.Fill
            Me.cboHost.Location = New System.Drawing.Point(0, 0)
            Me.cboHost.Name = "cboHost"
            Me.cboHost.Size = New System.Drawing.Size(250, 21)
            Me.cboHost.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled
            Me.cboHost.TabIndex = 13
            Me.cboHost.Text = ""
            Me.cboHost.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
            Me.cboHost.WatermarkColor = System.Drawing.Color.Silver
            Me.cboHost.WatermarkText = "Select owners..."
            '
            'PrismSelectionHostControl
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.cboHost)
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Name = "PrismSelectionHostControl"
            Me.Size = New System.Drawing.Size(250, 21)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents cboHost As DevComponents.DotNetBar.Controls.TextBoxDropDown

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class
End NameSpace