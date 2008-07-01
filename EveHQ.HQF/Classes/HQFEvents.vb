Public Class HQFEvents
    Public Shared Event FindModule(ByVal modData As ArrayList)

    Shared WriteOnly Property StartFindModule() As ArrayList
        Set(ByVal value As ArrayList)
            If value IsNot Nothing Then
                RaiseEvent FindModule(value)
            End If
        End Set
    End Property
End Class
