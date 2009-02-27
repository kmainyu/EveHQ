Imports System.Windows.Forms

Public Class frmSelectQuantity

    Private cQuantity As Long

    Public Property Quantity() As Long
        Get
            Return cQuantity
        End Get
        Set(ByVal value As Long)
            cQuantity=value
        End Set
    End Property

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub frmSelectQuantity_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        nudQuantity.Value = cQuantity
        nudQuantity.Select(0, Len(nudQuantity.Value.ToString))
    End Sub

    Private Sub btnAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAccept.Click
        cQuantity = CLng(nudQuantity.Value)
        Me.Close()
    End Sub

End Class