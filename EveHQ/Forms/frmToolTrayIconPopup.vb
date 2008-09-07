Public Class frmToolTrayIconPopup

    Private Sub frmToolTrayIconPopup_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        'tmrClose.Stop()
    End Sub

    Private Sub frmToolTrayIconPopup_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim workingRectangle As System.Drawing.Rectangle = Screen.PrimaryScreen.WorkingArea
        Me.Location = New System.Drawing.Point(workingRectangle.Width - Me.Width - 5, workingRectangle.Height - Me.Height - 5)
    End Sub

    Private Sub frmToolTrayIconPopup_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        'tmrClose.Start()
    End Sub

    Private Sub tmrClose_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrClose.Tick
        'tmrClose.Stop()
        Me.Hide()
    End Sub
End Class