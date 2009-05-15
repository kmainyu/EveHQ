Public Class frmDashboard

#Region "Form Loading Routines"
    Private Sub frmDashboard_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        For Each control As Control In FLP1.Controls
            AddHandler control.MouseDown, AddressOf MyMouseDown
        Next
    End Sub
#End Region

#Region "Panel Drag/Drop Routines"
    Private Sub MyMouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
        Dim source As Control = CType(sender, Control)
        source.DoDragDrop(New MyWrapper(source), DragDropEffects.Move)
    End Sub

    Private Sub FLP1_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles FLP1.DragDrop
        Dim wrapper As MyWrapper = CType(e.Data.GetData(GetType(MyWrapper)), MyWrapper)
        Dim source As Control = wrapper.Control

        Dim mousePosition As Point = FLP1.PointToClient(New Point(e.X, e.Y))
        Dim destination As Control = FLP1.GetChildAtPoint(mousePosition)

        Dim indexDestination As Integer = FLP1.Controls.IndexOf(destination)
        FLP1.Controls.SetChildIndex(source, indexDestination)
    End Sub

    Private Sub FLP1_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles FLP1.DragEnter
        If (e.Data.GetDataPresent(GetType(MyWrapper))) Then
            e.Effect = DragDropEffects.Move
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub
#End Region

End Class

Public Class MyWrapper
    Dim cControl As New Control

    Public Sub New(ByVal control As Control)
        Me.Control = control
    End Sub

    Public Property Control() As Control
        Get
            Return cControl
        End Get
        Set(ByVal value As Control)
            cControl = value
        End Set
    End Property

End Class