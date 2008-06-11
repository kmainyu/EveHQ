' ========================================================================
' EveHQ - An Eve-Online™ character assistance application
' Copyright © 2005-2008  Lee Vessey
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
<Serializable()> Public Class Bonuses

    Public Shared BonusGroups As New SortedList
    Public Shared BonusNames As New SortedList

End Class

<Serializable()> Public Class ItemBonus
#Region "Property Variables"

    Private cBonusName As String
    Private cBonusValue As Double
    Private cBonusIsPerLevel As Boolean
    Private cBonusSkillName As String

#End Region

#Region "Properties"

    Public Property BonusName() As String
        Get
            Return cBonusName
        End Get
        Set(ByVal value As String)
            cBonusName = value
        End Set
    End Property
    Public Property BonusValue() As Double
        Get
            Return cBonusValue
        End Get
        Set(ByVal value As Double)
            cBonusValue = value
        End Set
    End Property
    Public Property BonusIsPerLevel() As Boolean
        Get
            Return cBonusIsPerLevel
        End Get
        Set(ByVal value As Boolean)
            cBonusIsPerLevel = value
        End Set
    End Property
    Public Property BonusSkillName() As String
        Get
            Return cBonusSkillName
        End Get
        Set(ByVal value As String)
            cBonusSkillName = value
        End Set
    End Property

#End Region

End Class
