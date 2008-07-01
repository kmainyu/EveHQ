Public Class HQFEvents
    Public Shared Event FindModule(ByVal modData As ArrayList)
    Public Shared Event UpdateFitting()

    Shared WriteOnly Property StartFindModule() As ArrayList
        Set(ByVal value As ArrayList)
            If value IsNot Nothing Then
                RaiseEvent FindModule(value)
            End If
        End Set
    End Property
    Shared WriteOnly Property StartUpdateFitting() As Boolean
        Set(ByVal value As Boolean)
            If value = True Then
                RaiseEvent UpdateFitting()
            End If
        End Set
    End Property
End Class
