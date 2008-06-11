Imports System.Windows.Forms

Public Class ListViewNoFlicker
    Inherits ListView

    Public Sub New()
        MyBase.New()
        InitializeComponent()
        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer Or ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.EnableNotifyMessage, True)
    End Sub

    Protected Overrides Sub OnNotifyMessage(ByVal m As Message)
        'Filter out the WM_ERASEBKGND message
        If (m.Msg <> 14) Then
            MyBase.OnNotifyMessage(m)
        End If
    End Sub

End Class
