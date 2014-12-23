Namespace Controls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class BlueprintMEControl
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
            Me.nudME = New DevComponents.Editors.IntegerInput()
            CType(Me.nudME, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'nudME
            '
            '
            '
            '
            Me.nudME.BackgroundStyle.Class = "DateTimeInputBackground"
            Me.nudME.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square
            Me.nudME.ButtonCustom.Checked = True
            Me.nudME.ButtonCustom.Text = "Set"
            Me.nudME.ButtonCustom.Visible = True
            Me.nudME.ButtonFreeText.Shortcut = DevComponents.DotNetBar.eShortcut.F2
            Me.nudME.Dock = System.Windows.Forms.DockStyle.Fill
            Me.nudME.Location = New System.Drawing.Point(0, 0)
            Me.nudME.LockUpdateChecked = False
            Me.nudME.MaxValue = 10
            Me.nudME.MinValue = 0
            Me.nudME.Name = "nudME"
            Me.nudME.ShowCheckBox = True
            Me.nudME.ShowUpDown = True
            Me.nudME.Size = New System.Drawing.Size(100, 21)
            Me.nudME.TabIndex = 0
            '
            'BlueprintMEControl
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.nudME)
            Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Name = "BlueprintMEControl"
            Me.Size = New System.Drawing.Size(100, 21)
            CType(Me.nudME, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents nudME As DevComponents.Editors.IntegerInput

    End Class
End NameSpace