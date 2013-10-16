' ========================================================================
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

' ReSharper disable once CheckNamespace - for MS serialization compatability
<Serializable()> Public Class DefenceProfile

    ' ReSharper disable InconsistentNaming - for MS serialization compatability
    Public Name As String
    Public Type As Integer ' = DefenceProfileTypes
    Public SEM As Double
    Public SExplosive As Double
    Public SKinetic As Double
    Public SThermal As Double
    Public AEM As Double
    Public AExplosive As Double
    Public AKinetic As Double
    Public AThermal As Double
    Public HEM As Double
    Public HExplosive As Double
    Public HKinetic As Double
    Public HThermal As Double
    Public DPS As Double
    Public Fitting As String
    Public Pilot As String
    ' ReSharper restore InconsistentNaming

End Class
