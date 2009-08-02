Public Class TrainingQueue
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private _OldParent As Control

    Private Sub TrainingQueue_ParentChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ParentChanged
        If _OldParent IsNot Nothing Then
            RemoveHandler _OldParent.Disposed, AddressOf Parent_Disposed
        End If

        AddHandler Me.Parent.Disposed, AddressOf Parent_Disposed

        _OldParent = Me.Parent
    End Sub

    Private Sub Parent_Disposed(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Dispose()
    End Sub
End Class
