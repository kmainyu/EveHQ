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
<Serializable()> Public Class Implant

#Region "Property Variables"

    ' Name and Classification
    Private cName As String
    Private cID As String
    Private cMarketGroup As String
    Private cDatabaseGroup As String
    Private cDescription As String
    Private cBasePrice As Double
    Private cMarketPrice As Double
    Private cMetaType As Integer
    Private cIcon As String

    ' Other Details
    Private cSlot As Integer ' 1 - 10
    Private cImplantGroups As New ArrayList
    Private cImplantBonuses As New ArrayList

#End Region

#Region "Properties"

    ' Name and Classification
    Public Property Name() As String
        Get
            Return cName
        End Get
        Set(ByVal value As String)
            cName = value
        End Set
    End Property
    Public Property ID() As String
        Get
            Return cID
        End Get
        Set(ByVal value As String)
            cID = value
        End Set
    End Property
    Public Property MarketGroup() As String
        Get
            Return cMarketGroup
        End Get
        Set(ByVal value As String)
            cMarketGroup = value
        End Set
    End Property
    Public Property DatabaseGroup() As String
        Get
            Return cDatabaseGroup
        End Get
        Set(ByVal value As String)
            cDatabaseGroup = value
        End Set
    End Property
    Public Property Description() As String
        Get
            Return cDescription
        End Get
        Set(ByVal value As String)
            cDescription = value
        End Set
    End Property
    Public Property BasePrice() As Double
        Get
            Return cBasePrice
        End Get
        Set(ByVal value As Double)
            cBasePrice = value
        End Set
    End Property
    Public Property MarketPrice() As Double
        Get
            Return cMarketPrice
        End Get
        Set(ByVal value As Double)
            cMarketPrice = value
        End Set
    End Property
    Public Property MetaType() As Integer
        Get
            Return cMetaType
        End Get
        Set(ByVal value As Integer)
            cMetaType = value
        End Set
    End Property
    Public Property Icon() As String
        Get
            Return cIcon
        End Get
        Set(ByVal value As String)
            cIcon = value
        End Set
    End Property

    ' Other Details
    Public Property Slot() As Integer
        Get
            Return cSlot
        End Get
        Set(ByVal value As Integer)
            cSlot = value
        End Set
    End Property
    Public Property ImplantGroups() As ArrayList
        Get
            Return cImplantGroups
        End Get
        Set(ByVal value As ArrayList)
            cImplantGroups = value
        End Set
    End Property
    Public Property ImplantBonuses() As ArrayList
        Get
            Return cImplantBonuses
        End Get
        Set(ByVal value As ArrayList)
            cImplantBonuses = value
        End Set
    End Property
#End Region

End Class

<Serializable()> Public Class ImplantBonus

#Region "Property Variables"

    Private cBonusName As String
    Private cBonusValue As Double

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
#End Region

End Class

<Serializable()> Public Class Implants

    Public Shared implantList As New SortedList   ' Key = Name
    Public Shared implantGroups As New SortedList   ' Key = Name

End Class

<Serializable()> Public Class ImplantGroup

#Region "Property Variables"

    Private cGroupName As String
    Private cImplantName(10) As String

#End Region

#Region "Properties"

    Public Property GroupName() As String
        Get
            Return cGroupName
        End Get
        Set(ByVal value As String)
            cGroupName = value
        End Set
    End Property
    Public Property ImplantName(ByVal index As Integer) As String
        Get
            Return cImplantName(index)
        End Get
        Set(ByVal value As String)
            cImplantName(index) = value
        End Set
    End Property

#End Region

End Class
