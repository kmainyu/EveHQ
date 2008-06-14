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
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization

<Serializable()> Public Class ShipModule

    Public Shared Event ShowModuleMarketGroup(ByVal path As String)
    Shared WriteOnly Property DisplayedMarketGroup() As String
        Set(ByVal value As String)
            RaiseEvent ShowModuleMarketGroup(value)
        End Set
    End Property

#Region "Property Variables"

    ' Name and Classification
    Private cName As String
    Private cID As String
    Private cMarketGroup As String
    Private cDatabaseGroup As String
    Private cDatabaseCategory As String
    Private cDescription As String
    Private cBasePrice As Double
    Private cMarketPrice As Double
    Private cMetaType As Integer
    Private cMetaLevel As Integer
    Private cIcon As String

    ' Fitting Details
    Private cSlot As Integer ' 1=Rig, 2=Low, 4=Mid, 8=High
    Private cVolume As Double
    Private cCPU As Double
    Private cPG As Double
    Private cCalibration As Integer
    Private cCapUsage As Double
    Private cActivationTime As Double
    Private cIsLauncher As Boolean
    Private cIsTurret As Boolean
    Private cIsDrone As Boolean
    Private cIsCharge As Boolean

    ' Skills
    Private cRequiredSkills As New SortedList
    Private cRequiredSkillList As New SortedList

    ' Named Attributes
    Private cChargeSize As Integer

    ' Attributes
    Private cAttributes As New SortedList

    ' Bonuses
    Private cBonuses As New ArrayList

    ' Charges
    Private cCharges As New ArrayList
    Private cLoadedCharge As ShipModule

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
    Public Property DatabaseCategory() As String
        Get
            Return cDatabaseCategory
        End Get
        Set(ByVal value As String)
            cDatabaseCategory = value
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
    Public Property MetaLevel() As Integer
        Get
            Return cMetaLevel
        End Get
        Set(ByVal value As Integer)
            cMetaLevel = value
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

    ' Fitting Details
    Public Property Slot() As Integer
        Get
            Return cSlot
        End Get
        Set(ByVal value As Integer)
            cSlot = value
        End Set
    End Property
    Public Property Volume() As Double
        Get
            Return cVolume
        End Get
        Set(ByVal value As Double)
            cVolume = value
        End Set
    End Property
    Public Property CPU() As Double
        Get
            Return cCPU
        End Get
        Set(ByVal value As Double)
            cCPU = value
        End Set
    End Property
    Public Property PG() As Double
        Get
            Return cPG
        End Get
        Set(ByVal value As Double)
            cPG = value
        End Set
    End Property
    Public Property Calibration() As Integer
        Get
            Return cCalibration
        End Get
        Set(ByVal value As Integer)
            cCalibration = value
        End Set
    End Property
    Public Property CapUsage() As Double
        Get
            Return cCapUsage
        End Get
        Set(ByVal value As Double)
            cCapUsage = value
        End Set
    End Property
    Public Property ActivationTime() As Double
        Get
            Return cActivationTime
        End Get
        Set(ByVal value As Double)
            cActivationTime = value
        End Set
    End Property
    Public Property IsLauncher() As Boolean
        Get
            Return cIsLauncher
        End Get
        Set(ByVal value As Boolean)
            cIsLauncher = value
        End Set
    End Property
    Public Property IsTurret() As Boolean
        Get
            Return cIsTurret
        End Get
        Set(ByVal value As Boolean)
            cIsTurret = value
        End Set
    End Property
    Public Property IsDrone() As Boolean
        Get
            Return cIsDrone
        End Get
        Set(ByVal value As Boolean)
            cIsDrone = value
        End Set
    End Property
    Public Property IsCharge() As Boolean
        Get
            Return cIsCharge
        End Get
        Set(ByVal value As Boolean)
            cIsCharge = value
        End Set
    End Property

    ' Skills
    Public Property RequiredSkills() As SortedList
        Get
            Return cRequiredSkills
        End Get
        Set(ByVal value As SortedList)
            cRequiredSkills = value
        End Set
    End Property
    Public Property RequiredSkillList() As SortedList
        Get
            Return cRequiredSkillList
        End Get
        Set(ByVal value As SortedList)
            cRequiredSkillList = value
        End Set
    End Property

    ' Named Attributes
    Public Property ChargeSize() As Integer
        Get
            Return cChargeSize
        End Get
        Set(ByVal value As Integer)
            cChargeSize = value
        End Set
    End Property

    ' Attributes
    Public Property Attributes() As SortedList
        Get
            Return cAttributes
        End Get
        Set(ByVal value As SortedList)
            cAttributes = value
        End Set
    End Property

    ' Bonuses/Roles
    Public Property Bonuses() As ArrayList
        Get
            Return cBonuses
        End Get
        Set(ByVal value As ArrayList)
            cBonuses = value
        End Set
    End Property

    ' Charges
    Public Property Charges() As ArrayList
        Get
            Return cCharges
        End Get
        Set(ByVal value As ArrayList)
            cCharges = value
        End Set
    End Property
    Public Property LoadedCharge() As ShipModule
        Get
            Return cLoadedCharge
        End Get
        Set(ByVal value As ShipModule)
            cLoadedCharge = value
        End Set
    End Property

#End Region

#Region "Cloning"
    Public Function Clone() As ShipModule
        Dim ShipMemoryStream As New MemoryStream(10000)
        Dim objBinaryFormatter As New BinaryFormatter(Nothing, New StreamingContext(StreamingContextStates.Clone))
        objBinaryFormatter.Serialize(ShipMemoryStream, Me)
        ShipMemoryStream.Seek(0, SeekOrigin.Begin)
        Dim newModule As ShipModule = CType(objBinaryFormatter.Deserialize(ShipMemoryStream), ShipModule)
        ShipMemoryStream.Close()
        Return newModule
    End Function
#End Region

#Region "Map Attributes to Properties"
    Public Shared Sub MapModuleAttributes(ByVal newModule As ShipModule)
        Dim attValue As Double = 0
        For Each att As String In newModule.Attributes.Keys
            attValue = CDbl(newModule.Attributes(att))
            Select Case CInt(att)
                Case 6
                    newModule.CapUsage = attValue
                Case 30
                    newModule.PG = attValue
                Case 50
                    newModule.CPU = attValue
                Case 73
                    newModule.ActivationTime = attValue
                Case 1153
                    newModule.Calibration = CInt(attValue)
            End Select
        Next
    End Sub
#End Region

End Class

<Serializable()> Public Class ModuleLists
    Public Shared moduleMetaTypes As New SortedList
    Public Shared moduleMetaGroups As New SortedList
    Public Shared moduleList As New SortedList   ' Key = module ID
    Public Shared moduleListName As New SortedList ' Key = moduleName (for quick name to ID conversions)
End Class

