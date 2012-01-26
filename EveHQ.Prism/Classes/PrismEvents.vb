﻿' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2012  EveHQ Development Team
' 
' This file is part of EveHQ.
'
' EveHQ is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' EveHQ is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
'
' You should have received a copy of the GNU General Public License
' along with EveHQ.  If not, see <http://www.gnu.org/licenses/>.
'=========================================================================

Public Class PrismEvents

    Public Shared Event UpdateProductionJobs()
    Public Shared Event UpdateInventionJobs()
    Public Shared Event UpdateBatchJobs()
    Public Shared Event RecyclingInfoAvailable()

    Public Shared Sub StartUpdateProductionJobs()
        RaiseEvent UpdateProductionJobs()
        RaiseEvent UpdateInventionJobs()
    End Sub

    Public Shared Sub StartUpdateBatchJobs()
        RaiseEvent UpdateBatchJobs()
    End Sub

    Public Shared Sub StartRecyclingInfoAvailable()
        RaiseEvent RecyclingInfoAvailable()
    End Sub

End Class
