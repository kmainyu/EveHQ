Public Class HQFEvents
    Public Shared Event FindModule(ByVal modData As ArrayList)
    Public Shared Event UpdateFitting()
    Public Shared Event UpdateFittingList()
    Public Shared Event UpdateModuleList()
    Public Shared Event UpdateShipInfo(ByVal pilotName As String)
    Public Shared Event UpdateAllImplantLists()

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
    Shared WriteOnly Property StartUpdateFittingList() As Boolean
        Set(ByVal value As Boolean)
            If value = True Then
                RaiseEvent UpdateFittingList()
            End If
        End Set
    End Property
    Shared WriteOnly Property StartUpdateShipInfo() As String
        Set(ByVal value As String)
            If value <> "" Then
                RaiseEvent UpdateShipInfo(value)
            End If
        End Set
    End Property
    Shared WriteOnly Property StartUpdateModuleList() As Boolean
        Set(ByVal value As Boolean)
            If value = True Then
                RaiseEvent UpdateModuleList()
            End If
        End Set
    End Property
    Shared WriteOnly Property StartUpdateAllImplantLists() As Boolean
        Set(ByVal value As Boolean)
            If value = True Then
                RaiseEvent UpdateAllImplantLists()
            End If
        End Set
    End Property

End Class
